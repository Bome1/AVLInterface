using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace AVL_Interface
{
    public class Aircraft
    {
        public class AircraftUpdateEventArgs : EventArgs
        {
            public enum Update_Type
            {
                File_Load,
                File_Save,
                Added_Case,
                Removed_Case,
                Started_Case,
                Ended_Case,
                New_AVLInstance,
                Closed
            }
            public Update_Type UpdateType;
            public object Message;

            public AircraftUpdateEventArgs(Update_Type cmd, object msg)
            {
                UpdateType = cmd;
                Message = msg;
            }
        }

        public delegate void UpdateEventHandler(Aircraft sender, AircraftUpdateEventArgs e);
        public static event UpdateEventHandler OnUpdateAircraft = null;

        private string Initial_File_Path;

        private AVL_File m_work_File;
        private string m_backup_location;

        public bool HasBackup { get { return !string.IsNullOrEmpty(m_backup_location); } }

        private List<AVL_Instance> instances = new List<AVL_Instance>();

        private int CaseIndex = 0;
        public List<Run_Case> Analyze_Cases { get; private set; }

        public string Configuration_Name
        {
            get 
            {
                if (m_work_File != null)
                    return m_work_File.Title;
                else return "No Name";
            }
        }
        
        public int Surface_Count
        {
            get { return m_work_File.Surface_Count; }
        }

        public AVL_Instance PrimaryInstance
        {
            get
            {
                if (instances.Count > 0)
                    return instances[0];
                else return null;
            }
        }

        public AVL_File Initial_AVL_File
        {
            get { return m_work_File; }
        }

        public Aircraft()
        {
            Analyze_Cases = new List<Run_Case>();
            m_work_File = new AVL_File();
            AVL_Instance.OnUpdateMessages += new MessageEventHandler(AVL_Instance_OnUpdateMessages);
        }

        public Aircraft(string initial_file):this()
        {
            Initial_File_Path = initial_file;
            if (m_work_File.ReadFile(Initial_File_Path))
            {
                SaveBackup(m_work_File);
                //tell everyone who cares that we have loaded a file
                OnUpdate(this, new AircraftUpdateEventArgs(AircraftUpdateEventArgs.Update_Type.File_Load, initial_file));
                Spawn_Instance(Initial_File_Path, "Primary Instance");
                PrimaryInstance.GenerateGeomImage();
            }
        }

        private void AVL_Instance_OnUpdateMessages(AVL_Instance sender, AVLEventArgs e)
        {
            //if the AVL isntance is part of this aircraft's collection
            if (instances.Contains(sender))
            {
                switch (e.DataSource)
                {
                    case AVLEventArgs.AVLData_Source.Case_Started: OnUpdate(this, new AircraftUpdateEventArgs(AircraftUpdateEventArgs.Update_Type.Started_Case, e.Message)); break;
                    case AVLEventArgs.AVLData_Source.Case_Ended:
                        {
                            OnUpdate(this, new AircraftUpdateEventArgs(AircraftUpdateEventArgs.Update_Type.Ended_Case, e.Message));
                            BeginNextCase(sender);
                        }; break;
                }
            }
        }

        private static void OnUpdate(Aircraft ac, AircraftUpdateEventArgs e)
        {
            if (OnUpdateAircraft != null)
                OnUpdateAircraft(ac, e);
        }

        public void Close()
        {
            foreach (AVL_Instance inst in instances)
                inst.Close();

            OnUpdate(this, new AircraftUpdateEventArgs(AircraftUpdateEventArgs.Update_Type.Closed, null));
        }

        private void SaveBackup(AVL_File afile)
        {
            FileInfo finfo = new FileInfo(Initial_File_Path);
            if(finfo.Exists)
            {
                string fileName = finfo.Name;
                m_backup_location = finfo.DirectoryName + "\\" + fileName + ".bak";
                afile.WriteFile(m_backup_location);
            }
        }

        public void RevertToBackup()
        {
            if (HasBackup)
            {
                m_work_File.ReadFile(m_backup_location);
                SaveFile();
                OnUpdate(this, new AircraftUpdateEventArgs(AircraftUpdateEventArgs.Update_Type.File_Load, null));
            }
        }

        /// <summary>
        /// Save the AVL file that represents this aircraft. This will overwrite the previous file.
        /// </summary>
        public void SaveFile()
        {
            if (!String.IsNullOrEmpty(Initial_File_Path))
                SaveFile(Initial_File_Path);
            else
            {
                var sfd = new System.Windows.Forms.SaveFileDialog();
                sfd.Title = "AVL Vehicle File";
                sfd.Filter = "AVL Files (*.avl)|*.avl";
                sfd.InitialDirectory = Properties.Settings.Default.AVL_Location;
                sfd.FileName = this.Initial_AVL_File.Title + ".avl";
                if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    SaveFile(sfd.FileName);
                }
            }
        }

        /// <summary>
        /// Save the AVL file that represents this aircraft to the specified filepath. This will create a new file
        /// if one does not exist with the specified name, otherwise overwrites. The newly saved file then becomes the
        /// primary file for this Aircraft.
        /// </summary>
        /// <param name="filepath"></param>
        public void SaveFile(string filepath)
        {
            foreach (AVL_Instance inst in this.instances)
                inst.Close();
            this.instances.Clear();

            m_work_File.WriteFile(filepath);

            this.Initial_File_Path = filepath;

            Spawn_Instance(filepath, "Primary Instance");
            PrimaryInstance.GenerateGeomImage();
            OnUpdate(this, new AircraftUpdateEventArgs(AircraftUpdateEventArgs.Update_Type.File_Save, null));
        }

        private AVL_Instance Spawn_Instance(string FilePath)
        {
            return this.Spawn_Instance(FilePath, string.Empty);
        }

        private AVL_Instance Spawn_Instance(string FilePath, string Notes)
        {
            if (FilePath != string.Empty)
            {
                AVL_Instance instance = new AVL_Instance(FilePath, this);
                instance.Instance_Notes = Notes;
                instances.Add(instance);
                instance.Load();

                OnUpdate(this, new AircraftUpdateEventArgs(AircraftUpdateEventArgs.Update_Type.New_AVLInstance, instance));
                return instance;
            }
            return null;
        }

        public void Add_Run_Case(Run_Case rc)
        {
            this.Analyze_Cases.Add(rc);
            rc.Current_Status = Run_Case.Case_Status.Ready;

            rc.Case_Index = this.CaseIndex;
            this.CaseIndex++;

            OnUpdate(this, new AircraftUpdateEventArgs(AircraftUpdateEventArgs.Update_Type.Added_Case, rc));
        }

        public void Clear_Run_Cases()
        {
            while (Analyze_Cases.Count > 0)
                Remove_Run_Case(Analyze_Cases[0].Case_Index);//we just have to keep calling it at 0 till they are all gone
            this.CaseIndex = 0;
        }

        public void Remove_Run_Case(int caseIndex)
        {
            OnUpdate(this, new AircraftUpdateEventArgs(AircraftUpdateEventArgs.Update_Type.Removed_Case, caseIndex));
            for (int i = 0; i < Analyze_Cases.Count; i++)
                if (Analyze_Cases[i].Case_Index == caseIndex)
                    this.Analyze_Cases.RemoveAt(i);
        }

        public void Run_All_Cases(int copies)
        {
            foreach (Run_Case rc in Analyze_Cases)
            {
                //If the case has already been run (we are rerunning) wipe all old data
                if (rc.Current_Status == Run_Case.Case_Status.Finished)
                    rc.Reset();
                //set the case status to pending, indicating that we plan to evaluate
                rc.Current_Status = Run_Case.Case_Status.Pending;
            }

            if (copies <= 1)
            {
                BeginNextCase(PrimaryInstance);
            }
            else
            {
                if (copies > Analyze_Cases.Count)
                    copies = Analyze_Cases.Count;

                //spawn all the instances of AVL
                if (instances.Count < copies)
                    for (int i = 1; i < copies; i++)
                        Spawn_Instance(Initial_File_Path, i.ToString());

                for (int i = 0; i < instances.Count; i++)
                    BeginNextCase(instances[i]);
            }
        }

        private void BeginNextCase(AVL_Instance inst)
        {
            if (inst == null)
                return;

            lock (Analyze_Cases)
            {
                foreach (Run_Case rc in Analyze_Cases)
                {
                    //If it isnt marked pending, dont touch it
                    if (rc.Current_Status == Run_Case.Case_Status.Pending)
                    {
                        inst.ExecuteCase(rc);
                        return;
                    }
                }
            }

            if (PrimaryInstance != inst)
            {
                inst.Close();
                this.instances.Remove(inst);
            }
        }
    }
}
