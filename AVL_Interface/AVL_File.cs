using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AVL_Interface
{
    public class AVL_File
    {
        /// <summary>
        /// Surface Class, storing all the information about the surface
        /// itself, along with all the component sections
        /// </summary>
        public class Surface
        {
            /// <summary>
            /// Section class, storing information about the location/chord/etc
            /// and the collection of control surfaces it owns
            /// </summary>
            public class Section
            {
                /// <summary>
                /// Nested to the Extreme. Control surface information
                /// </summary>
                public class Control
                {
                    /*name     name of control variable
                      gain     control deflection gain, units:  degrees deflection / control variable
                      Xhinge   x/c location of hinge.  
                                If positive, control surface extent is Xhinge..1  (TE surface)
                                If negative, control surface extent is 0..-Xhinge (LE surface)
                      XYZhvec  vector giving hinge axis about which surface rotates 
                                + deflection is + rotation about hinge by righthand rule
                                Specifying XYZhvec = 0. 0. 0. puts the hinge vector along the hinge
                      SgnDup   sign of deflection for duplicated surface
                                An elevator would have SgnDup = +1
                                An aileron  would have SgnDup = -1*/
                    public string Name;

                    public double gain = 1;
                    public double Xhinge = 0.7;
                    public double SgnDup = 1;
                    public double[] HVec = new double[3];

                    public Control(string name)
                    {
                        Name = name;
                    }

                    public Control(string name, double[] args) : this(name)
                    {
                        if (args.Length != 6)
                            return;

                        gain = args[0];
                        Xhinge = args[1];
                        HVec[0] = args[2];
                        HVec[1] = args[3];
                        HVec[2] = args[4];
                        SgnDup = args[5];
                    }

                    public override string ToString()
                    {
                        return Name;
                    }

                    public string TextSerialize()
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine("CONTROL");
                        sb.AppendLine("#NAME \tGAIN \tXHinge \tXHVEC \tYHVEC \tZHVEC \tSGNDUP");
                        sb.AppendFormat("{0} \t{1} \t{2} \t{3} \t{4} \t{5} \t{6}\r\n", Name, gain, Xhinge, HVec[0], HVec[1], HVec[2], SgnDup);
                        return sb.ToString();
                    }
                }

                private Surface m_parentSurf;

                public double X_LeadingEdge;
                public double Y_LeadingEdge;
                public double Z_LeadingEdge;
                public double Chord;
                public double Angle_Incidence;
                public int Nspanwise;
                public int Sspace;

                public bool UseAirfoilFile = false;
                public string AirfoilFile = string.Empty;
                public string NACA = string.Empty;

                public List<Control> control_surfaces { get; private set; }

                public Section(Surface parentSurf)
                {
                    control_surfaces = new List<Control>();
                    m_parentSurf = parentSurf;
                }

                public void ParseContent(string[] Lines)
                {
                    string sectLine = Lines[1];
                    double[] args = PullArgs(sectLine);
                    if (args.Length < 5)
                        return;

                    X_LeadingEdge = args[0];
                    Y_LeadingEdge = args[1];
                    Z_LeadingEdge = args[2];
                    Chord = args[3];
                    Angle_Incidence = args[4];

                    if (args.Length == 7)
                    {
                        Nspanwise = (int)args[5];
                        Sspace = (int)args[6];
                    }

                    for (int i = 2; i < Lines.Length; i++)
                    {
                        switch (Lines[i].Substring(0, 4).ToUpperInvariant())
                        {
                            case "AFIL": AirfoilFile = Lines[i + 1]; UseAirfoilFile = true; i++; break;
                            case "NACA": NACA = Lines[i + 1]; i++; break;
                            case "CONT":
                                {
                                    string contLine = Lines[i + 1];
                                    string[] contsplits = contLine.Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries);
                                    contLine = "";
                                    for (int j = 1; j < contsplits.Length; j++)
                                        contLine += contsplits[j] + " ";
                                    double[] contArgs = PullArgs(contLine);
                                    Control newCont = new Control(contsplits[0], contArgs);
                                    control_surfaces.Add(newCont);
                                    i++;
                                }; break;
                        }
                    }
                }

                public string TextSerialize()
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("#----------------------------------------------");
                    sb.AppendLine("SECTION");
                    sb.AppendLine("#Xle \tYle \tZle \tChord \tAinc \tNspan \tSspace");
                    sb.AppendFormat("{0} \t{1} \t{2} \t{3} \t{4} \t{5} \t{6}\r\n", X_LeadingEdge, Y_LeadingEdge, Z_LeadingEdge, Chord, Angle_Incidence, Nspanwise, Sspace);
                    if (UseAirfoilFile && !string.IsNullOrEmpty(AirfoilFile))
                    {
                        sb.AppendLine("AFILE");
                        sb.AppendLine(AirfoilFile);
                    }
                    else if (!string.IsNullOrEmpty(NACA))
                    {
                        sb.AppendLine("NACA");
                        sb.AppendLine(NACA.ToString());
                    }

                    foreach (Control c in control_surfaces)
                        sb.Append(c.TextSerialize());

                    return sb.ToString();
                }
            }

            public string Name{get; private set;}
            public AVL_File Parent { get; private set; }

            public int Nchordwise = 14;
            public int Cspace = 1;
            public int Nspanwise = 20;
            public int Sspace = 1;

            public int COMPONENT = -1;
            public double YDUPLICATE = 0;
            public double[] SCALE = new double[3];
            public double[] TRANSLATE = new double[3];
            public double ANGLE = 0;

            public bool NOWAKE = false;
            public bool NOALBE = false;
            public bool NOLOAD = false;

            /// <summary>
            /// Span of the entire control surface.
            /// </summary>
            public double Span
            {
                get
                {
                    if (Sections.Count >= 2)
                        return Sections[Sections.Count - 1].Y_LeadingEdge * 2;
                    else return 0;
                }
            }

            /// <summary>
            /// Area of the entire control surface. Might not be correct for wierd y-dup values
            /// </summary>
            public double Area
            {
                get
                {
                    double area = 0;
                    for (int i = 0; i < Sections.Count - 1; i++)
                    {
                        double single_span = Sections[i + 1].Y_LeadingEdge - Sections[i].Y_LeadingEdge;
                        area += Sections[i].Chord * single_span + (Sections[i + 1].Chord - Sections[i].Chord) * (single_span) / 2;
                    }
                    if (area != 0)
                        return area *= 2;
                    else
                        return -1;
                }
            }

            /// <summary>
            /// Aspect ratio of the surface
            /// </summary>
            public double AspectRatio
            {
                get { return Math.Pow(Span, 2) / Area; }
            }

            /// <summary>
            /// Mean chord of the surface, calculated from total area / total span
            /// </summary>
            public double MeanChord
            {
                get
                {
                    return Area / Span;
                }
            }

            public List<Section> Sections = new List<Section>();

            public Surface(string name, AVL_File parent)
            {
                Name = name;
                Parent = parent;
            }

            public void ParseContent(string[] Lines)
            {
                Name = Lines[1];

                string sectLine = Lines[2];
                double[] args = PullArgs(sectLine);
                if (args.Length < 2)
                    return;

                Nchordwise = (int)args[0];
                Cspace = (int)args[1];

                if (args.Length == 4)
                {
                    Nspanwise = (int)args[2];
                    Sspace = (int)args[3];
                }

                for (int i = 2; i < Lines.Length; i++)
                {
                    //only first 4 letters becuase they are all that AVL looks for
                    switch (Lines[i].Substring(0, 4).ToUpperInvariant())
                    {
                        case "INDE"://added for compatibility, INDEX and COMPONENT are same thing
                        case "COMP": int.TryParse(Lines[i + 1], out COMPONENT); i++; break;
                        case "YDUP": double.TryParse(Lines[i + 1], out YDUPLICATE); i++; break;
                        case "SCAL":
                            {
                                string scaleLine = Lines[i + 1];
                                double[] scaleArgs = PullArgs(scaleLine);
                                if (scaleArgs.Length == 3)
                                    SCALE = scaleArgs;
                                i++;
                            }; break;
                        case "TRAN":
                            {
                                string transLine = Lines[i + 1];
                                double[] transArgs = PullArgs(transLine);
                                if (transArgs.Length == 3)
                                    TRANSLATE = transArgs;
                                i++;
                            } break;
                        case "ANGL": double.TryParse(Lines[i + 1], out ANGLE); i++; break;
                        case "NOWA": NOWAKE = true; break;
                        case "NOAL": NOALBE = true; break;
                        case "NOLO": NOLOAD = true; break;
                        default: break;
                    }
                }
            }

            public string TextSerialize()
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("\r\n#==============================================");
                sb.AppendLine("SURFACE");
                sb.AppendLine(this.Name);
                sb.AppendLine("#Nchord \tCspace \tNspan \tSspace");
                sb.AppendFormat("{0} \t{1} \t{2} \t{3}\r\n", Nchordwise, Cspace, Nspanwise, Sspace);
                //do we duplicating this surface?
                if (!double.IsNaN(YDUPLICATE))
                {
                    sb.AppendLine("YDUPLICATE");
                    sb.AppendLine(YDUPLICATE.ToString());
                }
                //is it at an angle other than 0?
                if (ANGLE != 0)
                {
                    sb.AppendLine("ANGLE");
                    sb.AppendLine(ANGLE.ToString());
                }
                //Do we scale this surface?
                if (SCALE[0] != 0 || SCALE[1] != 0 || SCALE[2] != 0)
                {
                    sb.AppendLine("SCALE");
                    sb.AppendLine("#Xscale \tYscale \tZscale");
                    sb.AppendFormat("{0} \t{1} \t{2}\r\n", SCALE[0], SCALE[1], SCALE[2]);
                }
                //Do we translate this surface?
                if (TRANSLATE[0] != 0 || TRANSLATE[1] != 0 || TRANSLATE[2] != 0)
                {
                    sb.AppendLine("TRANSLATE");
                    sb.AppendLine("#dX \tdY \tdZ");
                    sb.AppendFormat("{0} \t{1} \t{2}\r\n", TRANSLATE[0], TRANSLATE[1], TRANSLATE[2]);
                }
                //other options
                if(NOWAKE)
                    sb.AppendLine("NOWAKE");
                if (NOLOAD)
                    sb.AppendLine("NOLOAD");
                if (NOALBE)
                    sb.AppendLine("NOALBE");

                //finally add in all the sections, and their control surfaces
                foreach (Section sec in Sections)
                    sb.Append(sec.TextSerialize());

                return sb.ToString();
            }
        }

        private string m_fileName;

        public string Title { get; set; }

        //Mach number, keep below 0.6 for accuracy
        public double Mach = 0.0;

        //symmetry
        double IYSym = 0, IZSym = 0, ZSym = 0;

        //reference size vars
        public double Sref, Cref, Bref;

        //CG Location
        public double Xref, Yref, Zref;

        //optional, profile drag
        public double CDp = 0;

        public List<Surface> Surfaces{get; private set;}

        private string[] m_controls = null;

        //rewrite or maybe just recalculate every call...
        public string[] Controls
        {
            get
            {
                if (m_controls == null && Surfaces.Count > 0)
                {
                    List<string> all_controls = new List<string>();
                    foreach (Surface surf in Surfaces)
                        foreach (Surface.Section sec in surf.Sections)
                            foreach (Surface.Section.Control ctrl in sec.control_surfaces)
                                if (!all_controls.Contains(ctrl.Name))
                                    all_controls.Add(ctrl.Name);

                    m_controls = all_controls.ToArray();
                }
                return m_controls;
            }
        }

        public int Surface_Count
        {
            get { return Surfaces.Count; }
        }

        public AVL_File()
        {
            Surfaces = new List<Surface>();
        }

#region Text Read/Write Methods
        public bool ReadFile(string FileName)
        {
            m_fileName = FileName;
            FileInfo finfo = new FileInfo(m_fileName);

            //is the file really there?
            if (!finfo.Exists)
                return false;

            Surfaces = new List<Surface>();

            string[] FileLines;
            string fullFile;

            //Use a read-only filestream in case we have any write conflicts
            using (var fs = new FileStream(m_fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (var sr = new StreamReader(fs))
                    fullFile = sr.ReadToEnd();
            
            FileLines = fullFile.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            FileLines = StripComments(FileLines);

            int lineNum = 0;

            this.Title = FileLines[0];//pull the title line, always the first
            double.TryParse(FileLines[1], out Mach);//mach number is the second line

            for( int i=2; i<5;i++)
            {
                double[] headerArgs = PullArgs(FileLines[i]);

                if (headerArgs.Length == 3)
                {
                    if (i == 2)//third line is the symmetry
                    {
                        IYSym = headerArgs[0];
                        IZSym = headerArgs[1];
                        ZSym = headerArgs[2];
                    }
                    else if (i == 3)//forth line is the reference sizes
                    {
                        Sref = headerArgs[0];
                        Cref = headerArgs[1];
                        Bref = headerArgs[2];
                    }
                    else if (i == 4)//fifth non-blank/non-comment line is C.G. locations
                    {
                        Xref = headerArgs[0];
                        Yref = headerArgs[1];
                        Zref = headerArgs[2];
                    }
                }
            }
            //special for the optional CDp parameter, if we can parse it as a number, good, otherwise continue down to the surfaces
            if (double.TryParse(FileLines[5], out CDp))
                lineNum = 6;
            else lineNum = 5;

            List<int> KeyLines = new List<int>();
            List<string> KeyWords = new List<string>();
            for (int i = lineNum; i < FileLines.Length; i++)
            {
                if (FileLines[i].ToUpperInvariant().StartsWith("SURF"))
                {
                    KeyLines.Add(i);
                    KeyWords.Add("SURF");
                }
                else if (FileLines[i].ToUpperInvariant().StartsWith("BODY"))
                {
                    KeyLines.Add(i);
                    KeyWords.Add("BODY");
                }
                else if (FileLines[i].ToUpperInvariant().StartsWith("SECT"))
                {
                    KeyLines.Add(i);
                    KeyWords.Add("SECT");
                }
            }

            for( int i=0; i<KeyLines.Count; i++)
            {
                int length = i == KeyLines.Count-1 ? FileLines.Length - KeyLines[i] : KeyLines[i+1] - KeyLines[i];

                string[] content = new string[length];
                for( int j=0; j<content.Length; j++)
                    content[j] = FileLines[KeyLines[i]+j];

                if(KeyWords[i] == "SURF")
                {
                    //should have code somewhere to handle if the surfaces have the same name.
                    Surface surf = new Surface(content[1], this);
                    surf.ParseContent(content);
                    this.Surfaces.Add(surf);
                }
                else if(KeyWords[i] == "SECT")
                {
                    Surface.Section sec = new Surface.Section(this.Surfaces[Surfaces.Count - 1]);
                    sec.ParseContent(content);
                    this.Surfaces[Surfaces.Count - 1].Sections.Add(sec);
                }
            }

            return true;
        }

        private static string[] StripComments(string[] lines)
        {
            List<string> CleanedLines = new List<string>();

            for (int i = 0; i < lines.Length; i++)
                if (lines[i].Trim().Length > 0 && !lines[i].StartsWith("#"))
                    CleanedLines.Add(lines[i]);

            return CleanedLines.ToArray();
        }

        private static double[] PullArgs(string line)
        {
            string[] StArgs = line.Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries);
            double[] ArgVals = new double[StArgs.Length];
            try
            {
                for (int i = 0; i < ArgVals.Length; i++)
                    ArgVals[i] = double.Parse(StArgs[i]);
            }//needs a more graceful fail
            catch { throw new Exception("Error Parsing Arguments"); }

            return ArgVals;
        }

        public string TextSerialize()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("#This file generated by AVL_INTERFACE on " + DateTime.Now.ToShortDateString() + "at " + DateTime.Now.ToShortTimeString());
            sb.AppendLine(Title);
            sb.AppendLine("#MACH");
            sb.AppendLine(Mach.ToString());
            sb.AppendLine("#IYsym \tIZsym \tZsym \tVehicle Symmetry");
            sb.AppendFormat("{0} \t{1} \t{2}\r\n", IYSym, IZSym, ZSym);
            sb.AppendLine("#Sref \tCref \tBref \tReference Area and Lengths");
            sb.AppendFormat("{0} \t{1} \t{2}\r\n", Sref, Cref, Bref);
            sb.AppendLine("#Xref \tYref \tZref \tCenter of Gravity Location");
            sb.AppendFormat("{0} \t{1} \t{2}\r\n", Xref, Yref, Zref);
            if (CDp != 0)
            {
                sb.AppendLine("#Profile Drag");
                sb.AppendLine(CDp.ToString());
            }

            foreach (Surface surf in Surfaces)
                sb.Append(surf.TextSerialize());

            return sb.ToString();
        }

        public void WriteFile(string FileName)
        {
            FileInfo finfo = new FileInfo(FileName);
            using (StreamWriter sw = new StreamWriter(finfo.Open(FileMode.Create, FileAccess.ReadWrite)))
            {
                sw.Write(TextSerialize());
                sw.Flush();
                sw.Close();
            }
        }
#endregion
    }
}
