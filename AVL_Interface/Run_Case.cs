using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace AVL_Interface
{
    public class LabelAttribute : Attribute
    {
        public string Label { get; private set; }

        public LabelAttribute(string label) { Label = label; }
    }

    public class ColumnLabelAttribute : Attribute
    {
        public string[] Column_Labels { get; private set; }

        public ColumnLabelAttribute(string[] col_label) { Column_Labels = col_label; }
    }

    public class MenuCodeAttribute : Attribute
    {
        public string MenuCode { get; private set; }
        public Run_Case.Constraint MatchingCon { get; private set; }

        public MenuCodeAttribute(string menuCode)
        {
            MenuCode = menuCode;
        }
        public MenuCodeAttribute(string menuCode, Run_Case.Constraint matchingC)
            : this(menuCode)
        {
            MatchingCon = matchingC;
        }
    }

    public static class LabelExtension
    {
        public static string GetLabel(this Enum @enum)
        {
            string value = null;
            var fieldInfo = @enum.GetType().GetField(@enum.ToString());
            var attrs = fieldInfo.GetCustomAttributes(typeof(LabelAttribute), false) as LabelAttribute[];
            if (attrs != null) value = attrs.Length > 0 ? attrs[0].Label : null;
            return value;
        }

        public static string GetMenuCode(this Enum @enum)
        {
            string value = null;
            var fieldInfo = @enum.GetType().GetField(@enum.ToString());
            var attrs = fieldInfo.GetCustomAttributes(typeof(MenuCodeAttribute), false) as MenuCodeAttribute[];
            if (attrs != null) value = attrs.Length > 0 ? attrs[0].MenuCode : null;
            return value;
        }

        public static Run_Case.Constraint GetMatchingCon(this Enum @enum)
        {
            Run_Case.Constraint value = Run_Case.Constraint.Value;
            var fieldInfo = @enum.GetType().GetField(@enum.ToString());
            var attrs = fieldInfo.GetCustomAttributes(typeof(MenuCodeAttribute), false) as MenuCodeAttribute[];
            if (attrs != null) value = attrs.Length > 0 ? attrs[0].MatchingCon : Run_Case.Constraint.Value;
            return value;
        }
    }

    public class Run_Case
    {
        public enum Case_Status
        {
            Unassigned,
            Ready,
            Pending,
            Working,
            Finished,
            Error
        }

        public enum Independant_Vars
        {
            [Label("Angle of Attack"), MenuCode( "a", Constraint.Angle_of_attack)]
            Angle_of_attack,
            [Label("Sideslip"), MenuCode("b", Constraint.SideSlip)]
            SideSlip,
            [Label("Roll Rate"), MenuCode("r", Constraint.pb_2V)]
            Roll_Rate,
            [Label("Pitch Rate"), MenuCode("p", Constraint.qc_2V)]
            Pitch_Rate,
            [Label("Yaw Rate"), MenuCode("y", Constraint.rb_2V)]
            Yaw_Rate,
            [Label("X C.G. Location"), MenuCode("x", Constraint.Value)]
            X_cg,
            [Label("Y C.G. Location"), MenuCode("y", Constraint.Value)]
            Y_cg,
            [Label("Z C.G. Location"), MenuCode("z", Constraint.Value)]
            Z_cg,
            [Label("Control Surface"), MenuCode("d", Constraint.Value)]
            Control_Surface,
            [MenuCode("d1", Constraint.D1)]
            D1,
            [MenuCode("d2", Constraint.D2)]
            D2,
            [MenuCode("d3", Constraint.D3)]
            D3,
            [MenuCode("d4", Constraint.D4)]
            D4,
            [MenuCode("d5", Constraint.D5)]
            D5,
            [MenuCode("d6", Constraint.D6)]
            D6,
            [MenuCode("d7", Constraint.D7)]
            D7,
            [MenuCode("d8", Constraint.D8)]
            D8,
            [MenuCode("d9", Constraint.D9)]
            D9,
            [MenuCode("d10", Constraint.D10)]
            D10,
            Section_Span,
            Section_Chord
        }

        public enum Constraint
        {
            [Label("Value"), MenuCode("")]
            Value,
            [Label("Angle of Attack"), MenuCode("a")]
            Angle_of_attack,
            [Label("Sideslip"), MenuCode("b")]
            SideSlip,
            [Label("Roll Rate"), MenuCode("r")]
            pb_2V,
            [Label("Pitch Rate"), MenuCode("p")]
            qc_2V,
            [Label("Yaw Rate"), MenuCode("y")]
            rb_2V,
            [Label("Lift Coefficient"), MenuCode("c")]
            CL,
            [Label("Side-Force Coefficient"), MenuCode("s")]
            CY,
            [Label("Roll Moment"), MenuCode("rm")]
            Roll_Moment,
            [Label("Pitch Moment"), MenuCode("pm")]
            Pitching_Moment,
            [Label("Yaw Moment"), MenuCode("ym")]
            Yaw_Moment,
            [MenuCode("d1")]
            D1,
            [MenuCode("d2")]
            D2,
            [MenuCode("d3")]
            D3,
            [MenuCode("d4")]
            D4,
            [MenuCode("d5")]
            D5,
            [MenuCode("d6")]
            D6,
            [MenuCode("d7")]
            D7,
            [MenuCode("d8")]
            D8,
            [MenuCode("d9")]
            D9,
            [MenuCode("d10")]
            D10
        }

        public class TotalForces
        {
            public double Sref { get; private set; }
            public double Bref { get; private set; }
            public double Cref { get; private set; }

            [Label("Angle of Attack")]
            public double Alpha { get; private set; }
            [Label("Sideslip")]
            public double Beta { get; private set; }
            [Label("cL Total")]
            public double CL_tot { get; private set; }
            [Label("cD Total")]
            public double CD_tot { get; private set; }
            [Label("cD Far Field")]
            public double CD_ff { get; private set; }
            [Label("Spanwise Efficiency")]
            public double e { get; private set; }

            public double[] controls { get; private set; }

            private StringBuilder m_fullText;

            public string FullText
            {
                get 
                {
                    if (m_fullText != null)
                        return m_fullText.ToString();
                    else return string.Empty;
                }
            }

            public TotalForces() { }

            public void ParseTotalForces(string[] FullMsg)
            {
                m_fullText = new StringBuilder();
                for (int i = 0; i < FullMsg.Length; i++)
                    m_fullText.AppendLine(FullMsg[i]);

                int line = 0;
                //I dont actually expect this loop to do anything, its just for robustness
                //since all the values here are pulled by specific line numbers rather than matching
                while (!FullMsg[line].StartsWith("Vortex Lattice Output"))
                    line++;

                //sref parsing
                Sref = ParseLine(FullMsg[line + 5], 2);
                //cref parsing
                Cref = ParseLine(FullMsg[line + 5], 5);
                //bref parsing
                Bref = ParseLine(FullMsg[line + 5], 8);

                //Alpha parsing
                Alpha = ParseLine(FullMsg[line + 9], 2);

                //Beta parsing
                Beta = ParseLine(FullMsg[line + 10], 2);

                //CL parsing
                CL_tot = ParseLine(FullMsg[line + 15], 2);

                //CD parsing
                CD_tot = ParseLine(FullMsg[line + 16], 2);

                //CD_farfield parsing
                CD_ff = ParseLine(FullMsg[line + 18], 5);

                //spawise efficiency parsing
                e = ParseLine(FullMsg[line + 19], 5);

                //now do all the control surfaces
                line +=20;
                int controlNum = 0;
                List<double> control_deflections = new List<double>();
                while (line < FullMsg.Length)
                {
                    control_deflections.Add(ParseLine(FullMsg[line], 2));
                    line++;
                    controlNum++;
                }
                controls = control_deflections.ToArray();
            }
        }

        public class Stability
        {
            public enum Motion_Vars
            {
                [Label("Angle of Attack"), MenuCode("a")]
                Alpha = 0,
                [Label("Sideslip"), MenuCode("b")]
                Beta = 1,
                [Label("Roll Rate"), MenuCode("p")]
                Roll_rate = 2,
                [Label("Pitch Rate"), MenuCode("q")]
                Pitch_rate = 3,
                [Label("Yaw Rate"), MenuCode("r")]
                Yaw_rate = 4
            }

            public enum Forces_Moments
            {
                [Label("z' Force CL"), MenuCode("CL")]
                CL = 0,
                [Label("y Force CY"), MenuCode("CY")]
                CY = 1,
                [Label("x' moment Cl'"), MenuCode("Cl")]
                Cl = 2,
                [Label("y moment Cm'"), MenuCode("Cm")]
                Cm = 3,
                [Label("z' moment Cn'"), MenuCode("Cn")]
                Cn = 4
            }

            [Label("Neutral Point")]
            public double Neutral_Point { get; private set; }

            [Label("Static Margin")]
            public double Static_Margin
            {
                get { return -GetStabDerivative(Motion_Vars.Alpha, Forces_Moments.Cm) / GetStabDerivative(Motion_Vars.Alpha, Forces_Moments.CL); }
            }

            private double[] derivatives = new double[25];

            [Label("Stability Derivatives")]
            [ColumnLabel(new string[] { "CLa", "CYa", "Cla", "Cma", "Cna", "CLb", "CYb", "Clb", "Cmb", "Cnb", "CLp", "CYp", "Clp", "Cmp", "Cnp", "CLq", "CYq", "Clq", "Cmq", "Cnq", "CLr", "CYr", "Clr", "Cmr", "Cnr" })]
            public double[] Stability_Derivatives { get { return derivatives; } }

            private StringBuilder m_fullText;

            public string FullText
            {
                get
                {
                    if (m_fullText != null)
                        return m_fullText.ToString();
                    else return string.Empty;
                }
            }

            public Stability() { }

            public void SetStability(double[] stability)
            {
                derivatives = stability;
            }

            public double GetStabDerivative(Motion_Vars something, Forces_Moments other)
            {
                return derivatives[(int)something * Enum.GetNames(typeof(Motion_Vars)).Length + (int)other];
            }

            public void ParseStability(string[] FullMsg)
            {
                m_fullText = new StringBuilder();
                for (int i = 0; i < FullMsg.Length; i++)
                    m_fullText.AppendLine(FullMsg[i]);

                int line = 0;
                while (!FullMsg[line].StartsWith("Stability-axis"))
                    line++;

                line += 3;
                for (int i = 0; i < 5; i++, line++)
                {
                    int startPos = FullMsg[line].IndexOf('|');
                    derivatives[i] = ParseLine(FullMsg[line].Substring(startPos), 3);
                    derivatives[Enum.GetNames(typeof(Motion_Vars)).Length + i] = ParseLine(FullMsg[line].Substring(startPos), 6);
                }
                line += 2;
                for (int i = 0; i < 5; i++, line++)
                {
                    int startPos = FullMsg[line].IndexOf('|');
                    derivatives[Enum.GetNames(typeof(Motion_Vars)).Length * 2 + i] = ParseLine(FullMsg[line].Substring(startPos), 3);
                    derivatives[Enum.GetNames(typeof(Motion_Vars)).Length * 3 + i] = ParseLine(FullMsg[line].Substring(startPos), 6);
                    derivatives[Enum.GetNames(typeof(Motion_Vars)).Length * 4 + i] = ParseLine(FullMsg[line].Substring(startPos), 7);
                }

                while (!FullMsg[line].StartsWith("Neutral point"))
                    line++;

                Neutral_Point = ParseLine(FullMsg[line], 4);
            }
        }

        public string CaseNotes;

        public Case_Status Current_Status { get; set; }
        public int Case_Index = -1;

        [Label("Total Forces")]
        public TotalForces Forces { get; private set; }
        [Label("Stability")]
        public Stability StabDerivs { get; private set; }

        private StringBuilder m_stripfullText;

        public string StripFullText
        {
            get
            {
                if (m_stripfullText != null)
                    return m_stripfullText.ToString();
                else return string.Empty;
            }
        }

        [Label("Strip Forces")]
        [ColumnLabel(new string[] { "Normalized Span", "chord*cl/cref", "Strip Index", "Y-Leading Edge", "Chord", "Area", "Chord*cl", "Incidence Angle", "Normal cl", "cl", "cd", "cdv", "cm_c/4", "cm_LE", "C.P.x/c" })]
        public Dictionary<string, double[,]> Strip_Forces { get; private set; }

        public Dictionary<string, double[,]> Load_Forces { get; private set; }

        public Dictionary<Independant_Vars, Tuple<Constraint, double>> Parameters { get; private set; }
        
        static string[] splitChars = new string[] { " ", "\t" };//split on spaces and tabs

        public Run_Case()
        {
            Strip_Forces = new Dictionary<string, double[,]>();
            Load_Forces = new Dictionary<string, double[,]>();
            StabDerivs = new Stability();
            Forces = new TotalForces();
            Parameters = new Dictionary<Independant_Vars, Tuple<Constraint, double>>();
            Current_Status = Case_Status.Unassigned;
        }

        public void Reset()
        {
            Strip_Forces = new Dictionary<string, double[,]>();
            Load_Forces = new Dictionary<string, double[,]>();
            StabDerivs = new Stability();
            Forces = new TotalForces();
        }

        public void SetParameters(Independant_Vars Ivar, Constraint c, double value)
        {
            Parameters[Ivar] = new Tuple<Constraint, double>(c, value);
        }

        public void ParseStability(string[] FullMsg)
        {
            StabDerivs.ParseStability(FullMsg);
        }

        public void ParseTotalForces(string[] FullMsg)
        {
            Forces.ParseTotalForces(FullMsg);
        }

        public void ParseSurfaceStripForces(string[] FullMsg)
        {
            m_stripfullText = new StringBuilder();
            for (int i = 0; i < FullMsg.Length; i++)
                m_stripfullText.AppendLine(FullMsg[i]);

            //find the Surface # line
            int line = 2;
            //this may be a mistake... should probably become an error result
            if (Strip_Forces.Count > 0)
                Strip_Forces = new Dictionary<string, double[,]>();

            while (line < FullMsg.Length)
            {
                if (FullMsg[line].StartsWith("Surface #"))
                {
                    //error here if you have spacex in the name of your surfaces
                    string surfName = FullMsg[line].Split(splitChars, StringSplitOptions.RemoveEmptyEntries)[3];
                    line++;

                    if (!FullMsg[line-1].Contains("YDUP"))//if this is the original section
                        Strip_Forces.Add(surfName, ParseSurfaceStripData(FullMsg, ref line));
                    else//if this is the duplicate section
                    {
                        if (Strip_Forces.ContainsKey(surfName))
                        {
                            double[,] NormData = Strip_Forces[surfName];
                            double[,] DupData = ParseSurfaceStripData(FullMsg, ref line);
                            double[,] FullData = new double[DupData.GetLength(0) + NormData.GetLength(0), NormData.GetLength(1)];
                            if (DupData[DupData.GetLength(0)-1, 0] < DupData[0, 0] && DupData[DupData.GetLength(0)-1, 0] < NormData[0, 0])
                            {
                                for (int i=0, j = DupData.GetLength(0) - 1; j >= 0; j--, i++)
                                {
                                    for (int k = 0; k < FullData.GetLength(1); k++)
                                    {
                                        FullData[i, k] = DupData[j, k];
                                        FullData[i + FullData.GetLength(0)/2, k] = NormData[i, k];
                                    }
                                }
                            }
                            Strip_Forces[surfName] = FullData;
                        }
                    }
                }
                line++;
            }
        }

        private double[,] ParseSurfaceStripData(string[] fullMsg, ref int line)
        {
            double[,] SectionData = new double[0, 0];

            //redo this so we check # of items since there is a possibility that the last section does =# not = #
            int sectioncount = (int)ParseLine(fullMsg[line], 7);
            //this is when you have more than 100 sections in a surface... because you think more sections is always better.
            if (sectioncount == 0)
            {
                int secondEquals = fullMsg[line].IndexOf('=', 14);
                if (!int.TryParse(fullMsg[line].Substring(secondEquals, 3), out sectioncount))
                    return SectionData;//failed to get number of sections, stop parsing
            }

            SectionData = new double[sectioncount, 15];

            //find the start of the data
            while (!fullMsg[line].StartsWith("j"))
                line++;

            for (int i = 0; i < sectioncount; i++)
            {
                string[] dataColumns = fullMsg[line + i + 1].Split(splitChars, StringSplitOptions.RemoveEmptyEntries);

                if (dataColumns.Length < 12)
                    continue;//actually 13 columns, but sometimes AVL decides not to give you the last one...
                
                for (int j = 0; j < dataColumns.Length; j++)
                {
                    if (!double.TryParse(dataColumns[j], out SectionData[i, j+2]))
                        break;//if we run into a problem, break loop, skip this line, more graceful needed
                }
                SectionData[i, 0] = SectionData[i, 3] / (Forces.Bref / 2);
                SectionData[i, 1] = SectionData[i, 6] / Forces.Cref;
            }
            line += sectioncount;
            return SectionData;
        }

        private static double ParseLine(string line, int splitIndex)
        {
            double val;
            string[] splits = line.Split(splitChars, StringSplitOptions.RemoveEmptyEntries);
            if (splitIndex < splits.Length)
                if (double.TryParse(splits[splitIndex], out val))
                    return val;

            return 0;
        }
    }
}
