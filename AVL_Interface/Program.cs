using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Reflection;
using System.IO;

namespace AVL_Interface
{
    static class Program
    {
        public static Assembly ribbon = null;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            string resource = "AVL_Interface.System.Windows.Forms.Ribbon35.dll";
            using (Stream stm = Assembly.GetExecutingAssembly().GetManifestResourceStream(resource))
            {
                byte[] ba = new byte[(int)stm.Length];
                stm.Read(ba, 0, (int)stm.Length);
                ribbon = Assembly.Load(ba);
            }
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

            //var v1 = CalcStepSize(0.5, 5);
            //var v2 = CalcStepSize(0.083, 5);
            //var v3 = CalcStepSize(99, 5);
        }

        static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            if (args.Name.StartsWith("System.Windows.Forms.Ribbon35"))
                return ribbon;
            else return null;
        }

        public static double CalcStepSize(double range, float targetSteps)
        {
            // calculate an initial guess at step size
            double tempStep = range / targetSteps;

            // get the magnitude of the step size
            double mag = Math.Floor(Math.Log10(tempStep));
            double magPow = Math.Pow(10, mag);

            // calculate most significant digit of the new step size
            double magMsd = (int)(tempStep / magPow + 0.5);

            // promote the MSD to either 1, 2, or 5
            if (magMsd > 5.0)
                magMsd = 10.0f;
            else if (magMsd > 2.0)
                magMsd = 5.0f;
            else if (magMsd > 1.0)
                magMsd = 2.0f;

            return magMsd * magPow;
        }
    }
}
