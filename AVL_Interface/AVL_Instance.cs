using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace AVL_Interface
{
#region events
    //possible event data sources
    //Geom Pict, Traff Pict, control deflections, Cl_tot, stability derivatives, section lift information, 
    public class AVLEventArgs : EventArgs
    {
        public enum AVLData_Source
        {
            Geometry_Pict,
            Traff_Pict,
            Case_Started,
            Case_Ended,
            Closed
        }
        public AVLData_Source DataSource;
        public object Message;

        public AVLEventArgs(AVLData_Source cmd, object msg)
        {
            DataSource = cmd;
            Message = msg;
        }
    }

    public class AVL_PictureEventArgs : AVLEventArgs
    {
        public System.Drawing.Image Img
        {
            get { return (System.Drawing.Image)base.Message; }
        }

        public AVL_PictureEventArgs(AVLData_Source cmd, System.Drawing.Image img):base(cmd, img)
        {
        }
    }

    public delegate void MessageEventHandler(AVL_Instance sender, AVLEventArgs e);
#endregion

    public class AVL_Instance
    {
        public enum Menu
        {
            NONE,
            AVL,
            OPER,
            GEOM
        }

        public Process AVL_Process{ get; private set;}

        private Aircraft m_aircraft;
        private Menu current_Menu = Menu.NONE;

        public System.Drawing.Image Geometry_Image { get; private set; }
        public string Vehicle_File;
        public string Instance_Notes;

        private StringBuilder m_outputBuffer;

        private bool m_running;
        private Run_Case m_currentCase = null;

        public static event MessageEventHandler OnUpdateMessages = null;

        public bool Ready
        {
            get
            {
                /*if (AVL_Process == null || AVL_Process.StartTime == null)
                    return false;
                else
                    return AVL_Process.StandardInput.BaseStream.CanWrite;*/

                /*foreach (ProcessThread thread in AVL_Process.Threads)
                    if (thread.ThreadState == System.Diagnostics.ThreadState.Wait && thread.WaitReason == ThreadWaitReason.LpcReceive)
                        return false;
                return true;*/
                return !m_running;
            }
        }

        public Aircraft AVL_aircraft
        {
            get { return m_aircraft; }
        }

        public Menu AVL_Menu
        {
            get { return current_Menu; }
        }

        public AVL_Instance(Aircraft aircraft)
        {
            m_aircraft = aircraft;

            AVL_Process = new Process();
            AVL_Process.StartInfo.FileName = Properties.Settings.Default.AVL_Location;
            AVL_Process.StartInfo.UseShellExecute = false;
            AVL_Process.StartInfo.CreateNoWindow = true;
            AVL_Process.StartInfo.RedirectStandardOutput = true;
            AVL_Process.StartInfo.RedirectStandardInput = true;
            AVL_Process.OutputDataReceived += new DataReceivedEventHandler(p_OutputDataReceived);
        }

        /// <summary>
        /// Creates a new instance of AVL_Instance, and specifing the *.avl file to load
        /// </summary>
        /// <param name="FullFileName">Full path and name of the file to open</param>
        public AVL_Instance(string FullFileName, Aircraft aircraft): this(aircraft)
        {
            Vehicle_File = FullFileName;
            FileInfo finfo = new FileInfo(Vehicle_File);
            AVL_Process.StartInfo.WorkingDirectory = finfo.DirectoryName;
        }

        private void p_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            //Debug.Print(e.Data);
            SortMessage(e.Data);
        }

        public void Load()
        {
            try
            {
                if (m_aircraft == null)
                    return;

                FileInfo finfo = new FileInfo(this.Vehicle_File);
                if (!finfo.Exists)
                    return;

                this.AVL_Process.StartInfo.Arguments = finfo.Name;
                this.AVL_Process.Start();
                this.AVL_Process.BeginOutputReadLine();
                AVL_Process.StandardInput.AutoFlush = true;

                Thread.Sleep(100);
                //disable graphics, enable color, enable individual PS files
                this.Write(new string[] { "PLOP", "G", "I\n" });
                //go to OPER menu
                this.Write("OPER", true);
            }
            catch (Exception e)
            {
                throw new Exception("OS error: " + e.Message, e);
            }
        }

        public void ExecuteCase(Run_Case rc)
        {
            m_currentCase = rc;
            //now that we have set all parameters, change status and execute.
            rc.Current_Status = Run_Case.Case_Status.Working;

            Task.Factory.StartNew(() =>
            {
                while (!Ready)
                    System.Threading.Thread.Sleep(50);

                AVLUpdateMessage(this, new AVLEventArgs(AVLEventArgs.AVLData_Source.Case_Started, m_currentCase.Case_Index));

                RunCase(rc);
                System.Threading.Thread.Sleep(2000);

                while (!Ready)
                    System.Threading.Thread.Sleep(50);

                if (rc.Current_Status != Run_Case.Case_Status.Error)
                    rc.Current_Status = Run_Case.Case_Status.Finished;

                AVLUpdateMessage(this, new AVLEventArgs(AVLEventArgs.AVLData_Source.Case_Ended, m_currentCase.Case_Index));
                //System.Threading.Thread.Sleep(200);
            });
        }

        private void RunCase(Run_Case rc)
        {
            if (rc.Parameters.Count == 0)
                return;

            foreach (var kvp in rc.Parameters)
            {
                switch (kvp.Key)
                {
                    case Run_Case.Independant_Vars.Section_Span: break;
                    case Run_Case.Independant_Vars.Section_Chord: break;
                    case Run_Case.Independant_Vars.Control_Surface: break;
                    //the C.G. edits are done through the 'M'odify menu, and only to a value
                    case Run_Case.Independant_Vars.X_cg:
                    case Run_Case.Independant_Vars.Y_cg:
                    case Run_Case.Independant_Vars.Z_cg:
                        {
                            if (kvp.Value.Item1 != Run_Case.Constraint.Value)
                                break;
                            ChangeMenu(Menu.OPER);
                            Write("M", true);
                            Write(kvp.Key.GetMenuCode() + " " + kvp.Value.Item2.ToString(), true);
                            Write(" ", true);

                        }; break;
                        //this is the case where we can translate the variable to the character
                        //that AVL is looking for, for instance a a # to set alpha => alpha = #
                    default:
                        {
                            switch (kvp.Value.Item1)
                            {
                                //This is the case where 90% of the time you ment to say a a #, but the other 10% is the new
                                //factors like section chord and section lift
                                case Run_Case.Constraint.Value:
                                    {
                                        if (kvp.Key == Run_Case.Independant_Vars.Section_Chord || kvp.Key == Run_Case.Independant_Vars.Section_Span)
                                            return;
                                        else
                                        {
                                            ChangeMenu(Menu.OPER);
                                            Write(kvp.Key.GetMenuCode() + " " + kvp.Key.GetMenuCode() + " " + kvp.Value.Item2.ToString(), true);
                                        }
                                    }; break;
                                //these are all the cases where we have a menu code for the constraint
                                default:
                                    {
                                        ChangeMenu(Menu.OPER);
                                        Write(kvp.Key.GetMenuCode() + " " + kvp.Value.Item1.GetMenuCode() + " " + kvp.Value.Item2.ToString(), true);
                                    }; break;
                            }
                        }; break;
                }
            } 
            ChangeMenu(Menu.OPER);
            Write("x", true);
            System.Threading.Thread.Sleep(50);

            //write for stability
            Write("st\n", true);

            //write for surface strip forces
            Write("FS\n", true);
            //strip shear/moment
            //Write("VM\n");

            //flush for good luck
            Write("?", true);
        }

        /// <summary>
        /// Changes the menu and tells AVL to generate the .PS file for the geometry
        /// </summary>
        public void GenerateGeomImage()
        {
            ChangeMenu(Menu.GEOM);
            //Thread.Sleep(2000);
            //jumping back into the geom window apparently flushes the hardcopy buffer
            this.Write(new string[]{"h","g"});
        }

        /// <summary>
        /// Changes to the desired menu from whereever we are now
        /// </summary>
        /// <param name="desiredMenu">Desired AVL Menu</param>
        private void ChangeMenu(Menu desiredMenu)
        {
            while (!this.Ready)
                System.Threading.Thread.Sleep(20);

            if (!Ready)
                return;

            if( current_Menu == desiredMenu)
                return;

            switch (desiredMenu)
            {
                case Menu.AVL:
                    this.Write(new string[]{" "," "," "}); break;
                case Menu.OPER:
                    {
                        if (current_Menu == Menu.AVL)
                            this.Write("OPER", true);
                        else
                            this.Write(new string[] { " ", " ", " ", "OPER" });
                    } break;
                case Menu.GEOM:
                    {
                        if (current_Menu == Menu.OPER)
                            this.Write("G", true);
                        else if (current_Menu == Menu.AVL)
                            this.Write(new string[] { "OPER", "G" });
                        else
                            this.Write(new string[] { " ", " ", " ", "OPER", "G" });
                    } break;
                default: break;
            }
        }

        public void Write(string[] msg)
        {
            string fullLine = "";
            for (int i = 0; i < msg.Length; i++)
                fullLine += msg[i] + "\n";

            Write(fullLine, true);
        }

        public void Write(string msg, bool flush)
        {
            while (!this.Ready)
                System.Threading.Thread.Sleep(20);
            m_running = true;
            AVL_Process.StandardInput.WriteLine(msg);
            if (flush)
                AVL_Process.StandardInput.WriteLine("?");
        }

        public void Close()
        {
            AVL_Process.Close();
            AVLUpdateMessage(this, new AVLEventArgs(AVLEventArgs.AVLData_Source.Closed, null));
        }

        private void SortMessage(string msg)
        {
            if (msg == null)
                return;

            msg = msg.Trim();

            //this is a wierd one for now. When there is a ton of results being pushed
            //into the window by the stripforce or whatever output, 
            //it starts and ends with a bunch of ------. So when we see this, fill a buffer
            //then we can parse that buffer once the ----- shows up to signify the end
            if( m_outputBuffer == null && (msg.StartsWith("Enter filename, or <return> for screen output") || msg.StartsWith("Stability-axis derivatives...")))
            {
                if (m_outputBuffer == null)
                    m_outputBuffer = new StringBuilder();
            }

            //looking for when to stop collecting lines into the buffer. All outputs EXCEPT
            //the stability ones end with the -----, but Consistancy is clearly to much to ask,
            //so that last part there is a specific check for the end of the stability output
            if (m_outputBuffer != null && (msg == "---------------------------------------------------------------"|| msg.StartsWith("Clb Cnr / Clr Cnb  =" )))
            {
                string fullMsg = m_outputBuffer.ToString();
                m_outputBuffer = null;
                ParseOutputSections(fullMsg);//signifies end of buffer, call parsing method
                return;
            }

            //if we are not collecting and just looking at one-off messages
            if (m_outputBuffer == null)
            {
                if (msg.StartsWith("AVL"))
                    current_Menu = Menu.AVL;
                else if (msg.StartsWith(".OPER (case"))
                    current_Menu = Menu.OPER;
                else if (msg.StartsWith("Geometry plot command:"))
                {
                    current_Menu = Menu.GEOM;
                    if (msg.Contains("PostScript to file"))
                    {
                        string[] words = msg.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                        string Geometry_Image_File = words[7];
                        //System.Threading.Thread.Sleep(200);
                        PostScript_Interp psi = new PostScript_Interp();
                        Geometry_Image = (System.Drawing.Bitmap)psi.Load(Path.Combine(AVL_Process.StartInfo.WorkingDirectory, Geometry_Image_File));
                        psi = null;
                        AVLUpdateMessage(this, new AVL_PictureEventArgs(AVLEventArgs.AVLData_Source.Geometry_Pict, Geometry_Image));
                    }
                }

                if(msg.EndsWith("c>"))
                    m_running = false;
            }
            else//otherwise keep compiling
                m_outputBuffer.AppendLine(msg);
        }

        private void ParseOutputSections(string fullMsg)
        {
            m_outputBuffer = null;
            string[] splits = fullMsg.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            if (splits.Length <= 1)
                return;

            switch (splits[1])//0th line is ------, check 1st for words
            {
                case "Surface and Strip Forces by surface":
                    {
                        if (m_currentCase != null)
                            m_currentCase.ParseSurfaceStripForces(splits);
                    }; break;
                case "Vortex Lattice Output -- Total Forces":
                    {
                        if (m_currentCase != null)
                            m_currentCase.ParseTotalForces(splits);
                    }; break;
                case "alpha                beta":
                    {
                        if (m_currentCase != null)
                            m_currentCase.ParseStability(splits);
                    }; break;
            }
        }

        private static void AVLUpdateMessage(AVL_Instance inst, AVLEventArgs e)
        {
            if (OnUpdateMessages != null)
                OnUpdateMessages(inst, e);
        }
    }
}
