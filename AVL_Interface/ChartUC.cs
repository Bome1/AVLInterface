using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Reflection;

namespace AVL_Interface
{
    public partial class ChartUC : UserControl
    {
        private Form1 m_form
        {
            get { return (Form1)this.ParentForm; }
        }

        private Control m_menuSource;
        private Dictionary<Chart, ChartDisplay> m_charts = new Dictionary<Chart,ChartDisplay>();

        private int main_Lines = 0;

        private class ChartDisplay
        {
            public Chart c { get; set; }
            public Tuple<PropertyInfo, object> PrimaryX { get; set; }
            public Tuple<PropertyInfo, object> PrimaryY { get; set; }
            public Tuple<PropertyInfo, object> SecondaryX { get; set; }
            public Tuple<PropertyInfo, object> SecondaryY { get; set; }

            public ChartDisplay(Chart c)
            {
                this.c = c;
            }
        }

        public ChartUC()
        {
            InitializeComponent();
            Aircraft.OnUpdateAircraft += new Aircraft.UpdateEventHandler(Aircraft_OnUpdateAircraft);
            this.subChart1.Series.Clear();
            this.subChart2.Series.Clear();
            this.mainChart.Series.Clear();

            m_charts.Add(mainChart, new ChartDisplay(mainChart));
            m_charts.Add(subChart1, new ChartDisplay(subChart1));
            m_charts.Add(subChart2, new ChartDisplay(subChart2));

            //set defaults
            plotAlphaSweep(subChart1);
            plotCl_CD(subChart2);
            plotTrefftz2(mainChart);
        }

        private void ChartUC_Load(object sender, EventArgs e)
        {
            PopulateContextMenu();
        }

        private void Aircraft_OnUpdateAircraft(Aircraft sender, Aircraft.AircraftUpdateEventArgs e)
        {
            if (this.InvokeRequired)
                this.Invoke((MethodInvoker)delegate { Aircraft_OnUpdateAircraft(sender, e); });
            else
            {
                switch (e.UpdateType)
                {
                    case Aircraft.AircraftUpdateEventArgs.Update_Type.File_Load:
                    case Aircraft.AircraftUpdateEventArgs.Update_Type.File_Save:
                        {
                            foreach (ListViewGroup lvg in listView1.Groups)
                                if (lvg.Header == sender.Configuration_Name)
                                    return;

                            this.listView1.Groups.Add(sender.Configuration_Name, sender.Configuration_Name);
                        }; break;
                    case Aircraft.AircraftUpdateEventArgs.Update_Type.Closed:
                        {
                            if (listView1.Groups.Count > 0)
                            {
                                if (listView1.Groups[sender.Configuration_Name] != null)
                                {
                                    for (int i = listView1.Groups[sender.Configuration_Name].Items.Count; i > 0; i--)
                                        listView1.Items.Remove(listView1.Groups[sender.Configuration_Name].Items[i - 1]);

                                    listView1.Groups[sender.Configuration_Name].Items.Clear();
                                    listView1.Groups.Remove(listView1.Groups[sender.Configuration_Name]);
                                    RefreshAllCharts();
                                }
                            }
                        }; break;
                    case Aircraft.AircraftUpdateEventArgs.Update_Type.Ended_Case:
                        {
                            int runIndex = (int)e.Message;
                            string key = sender.Configuration_Name + "~~~" + runIndex.ToString();
                            if (!listView1.Items.ContainsKey(key))
                            {
                                ListViewItem lvi = listView1.Items.Add(key, sender.Analyze_Cases[runIndex].CaseNotes, 0);
                                lvi.Group = listView1.Groups[sender.Configuration_Name];
                            }
                            else
                            {
                                ListViewItem lvi = listView1.Items[key];
                                lvi.Text = sender.Analyze_Cases[runIndex].CaseNotes;
                            }
                            RefreshAllCharts();
                        }; break;
                    case Aircraft.AircraftUpdateEventArgs.Update_Type.Removed_Case:
                        {
                            int runIndex = (int)e.Message;
                            string key = sender.Configuration_Name + "~~~" + runIndex.ToString();
                            listView1.Items.RemoveByKey(key);
                        }; break;
                }
            }
        }

        private void PopulateContextMenu()
        {
            primaryXAxisMenuItem.DropDownItems.AddRange(GetProperties(typeof(Run_Case)));
            primaryYAxisMenuItem.DropDownItems.AddRange(GetProperties(typeof(Run_Case)));
            //secondaryXAxisMenuItem.DropDownItems.AddRange(GetProperties(typeof(Run_Case)));
            secondaryYAxisMenuItem.DropDownItems.AddRange(GetProperties(typeof(Run_Case)));
        }

        private Run_Case GetRepresentedCase(ListViewItem lvi)
        {
            string ac_name = lvi.Name.Split(new string[] { "~~~" }, StringSplitOptions.RemoveEmptyEntries)[0];
            string caseNum = lvi.Name.Split(new string[] { "~~~" }, StringSplitOptions.RemoveEmptyEntries)[1];
            foreach (Aircraft ac in m_form.designs)
            {
                if (ac.Configuration_Name == ac_name && ac.Analyze_Cases.Count > int.Parse(caseNum))
                {
                    return ac.Analyze_Cases[int.Parse(caseNum)];
                }
            }
            return null;
        }

        private void StandardMenuItem_Click(object sender, EventArgs e)
        {
            var stripItem = sender as ToolStripMenuItem;

            if (stripItem == null)
                return;

            if (stripItem.Tag != null && stripItem.Tag.GetType() == typeof(Tuple<PropertyInfo, object>))
            {
                if (stripItem.OwnerItem == primaryXAxisMenuItem || (stripItem.OwnerItem.OwnerItem != null && stripItem.OwnerItem.OwnerItem == primaryXAxisMenuItem))
                    m_charts[(Chart)m_menuSource].PrimaryX = (stripItem.Tag as Tuple<PropertyInfo, object>);
                else if (stripItem.OwnerItem == primaryYAxisMenuItem || (stripItem.OwnerItem.OwnerItem != null && stripItem.OwnerItem.OwnerItem == primaryYAxisMenuItem))
                    m_charts[(Chart)m_menuSource].PrimaryY = (stripItem.Tag as Tuple<PropertyInfo, object>);
                else if (stripItem.OwnerItem == secondaryYAxisMenuItem || (stripItem.OwnerItem.OwnerItem != null && stripItem.OwnerItem.OwnerItem == secondaryYAxisMenuItem))
                    m_charts[(Chart)m_menuSource].SecondaryY = (stripItem.Tag as Tuple<PropertyInfo, object>);
            }
            PlotChart(m_charts[(Chart)m_menuSource]);
        }

        private ToolStripMenuItem[] GetProperties(Type t)
        {
            var toolItems = new List<ToolStripMenuItem>();

            foreach (PropertyInfo info in t.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var latt = info.GetCustomAttributes(false);
                var tsi = new ToolStripMenuItem();
                for (int i = 0; i < latt.Length; i++)
                {
                    if (latt[i].GetType() == typeof(LabelAttribute))
                    {
                        //we add the property to the menu
                        tsi.Text = ((LabelAttribute)latt[i]).Label;
                        toolItems.Add(tsi);

                        //if the type of this property is not a basic class, but our custom ones, go deeper
                        if (info.PropertyType.Module.ScopeName != "CommonLanguageRuntimeLibrary")
                            tsi.DropDownItems.AddRange(GetProperties(info.PropertyType));
                        //otherwise this is a basic data type and we treat it like the end of the line
                        else
                        {
                            tsi.CheckOnClick = true;
                            tsi.Click += new EventHandler(StandardMenuItem_Click);
                            tsi.Tag = new Tuple<PropertyInfo, object>(info, null);
                        }
                    }
                    else if (latt[i].GetType() == typeof(ColumnLabelAttribute))
                    {
                        var colAttr = (latt[i] as ColumnLabelAttribute);
                        for (int j = 0; j < colAttr.Column_Labels.Length; j++)
                        {
                            var sub_tsi = new ToolStripMenuItem(colAttr.Column_Labels[j]);
                            sub_tsi.Click += new EventHandler(StandardMenuItem_Click);
                            sub_tsi.Tag = new Tuple<PropertyInfo, object>(info, j);
                            tsi.DropDownItems.Add(sub_tsi);
                        }
                    }
                }
            }
            return toolItems.ToArray();
        }

        private void RefreshAllCharts()
        {
            foreach (var kvp in m_charts)
                PlotChart(kvp.Value);
        }

        private void PlotChart(ChartDisplay cd)
        {
            var primXResults = new List<double[]>();
            var primYResults = new List<double[]>();
            var secXResults = new List<double[]>();
            var secYResults = new List<double[]>();

            if (cd.PrimaryX != null && cd.PrimaryX.Item1 != null)
                primXResults = GetRelevantData(cd.PrimaryX.Item1, cd.PrimaryX.Item2);

            if (cd.PrimaryY != null && cd.PrimaryY.Item1 != null)
                primYResults = GetRelevantData(cd.PrimaryY.Item1, cd.PrimaryY.Item2);

            if (cd.SecondaryY != null && cd.SecondaryY.Item1 != null)
                secYResults = GetRelevantData(cd.SecondaryY.Item1, cd.SecondaryY.Item2);

            if (primXResults.Count > 0 && (primYResults.Count > 0 || secYResults.Count > 0))
            {
                cd.c.Series.Clear();
                cd.c.ChartAreas[0].AxisX.Title = ((LabelAttribute)cd.PrimaryX.Item1.GetCustomAttributes(typeof(LabelAttribute), false)[0]).Label;
                if (primYResults.Count > 0)
                    cd.c.ChartAreas[0].AxisY.Title = ((LabelAttribute)cd.PrimaryY.Item1.GetCustomAttributes(typeof(LabelAttribute), false)[0]).Label;
                else if (secYResults.Count > 0)
                    cd.c.ChartAreas[0].AxisY.Title = ((LabelAttribute)cd.SecondaryY.Item1.GetCustomAttributes(typeof(LabelAttribute), false)[0]).Label;

                if (primYResults.Count > 0 && secYResults.Count > 0)
                    cd.c.ChartAreas[0].AxisY2.Title = ((LabelAttribute)cd.SecondaryY.Item1.GetCustomAttributes(typeof(LabelAttribute), false)[0]).Label;

                for (int i = 0; i < Math.Min(primYResults.Count, primXResults.Count); i++)
                    PlotSpline(cd.c, primXResults[i], primYResults[i], cd.c.ChartAreas[0].AxisY.Title, Color.Blue, i == 0, true);

                //if we are suing the same X-axis for both Y-graphs
                if (secXResults.Count == 0)
                    for (int i = 0; i < Math.Min(secYResults.Count, primXResults.Count); i++)
                        PlotSpline(cd.c, primXResults[i], secYResults[i], cd.c.ChartAreas[0].AxisY.Title, Color.Blue, i == 0, true);

                SetAxis(cd.c);
            }
        }

        private void SetAxis(Chart c)
        {
            double minX = double.MaxValue;
            double maxX = double.MinValue;
            double minY = double.MaxValue;
            double maxY = double.MinValue;

            for (int i = 0; i < c.Series.Count; i++)
            {
                if (c.Series[i].Points.Count == 0)
                    continue;

                //if (c.Series[i].YAxisType == AxisType.Primary)
                {
                    maxY = Math.Max(maxY, c.Series[i].Points.FindMaxByValue("Y1").YValues[0]);
                    minY = Math.Min(minY, c.Series[i].Points.FindMinByValue("Y1").YValues[0]);
                } 
                //if (c.Series[i].XAxisType == AxisType.Primary)
                {
                    maxX = Math.Max(maxX, c.Series[i].Points.FindMaxByValue("X").XValue);
                    minX = Math.Min(minX, c.Series[i].Points.FindMinByValue("X").XValue);
                }
            }

            minX = Math.Round(minX, 2, MidpointRounding.AwayFromZero);
            maxX = Math.Round(maxX, 2, MidpointRounding.AwayFromZero);
            minY = Math.Round(minY, 2, MidpointRounding.AwayFromZero);
            maxY = Math.Round(maxY, 2, MidpointRounding.AwayFromZero);

            if (minY == maxY)
            {
                if (minY == 0)
                    maxY = 0.1;
                else
                {
                    maxY = Math.Round(minY * 1.5, 1, MidpointRounding.AwayFromZero);
                    minY = Math.Round(minY * .5, 1, MidpointRounding.AwayFromZero);
                }
            }
            if (minX == maxX)
            {
                if (minX == 0)
                    maxX = 1;
                else
                {
                    maxX = Math.Round(minX * 1.5, 1, MidpointRounding.AwayFromZero);
                    minX = Math.Round(minX * .5, 1, MidpointRounding.AwayFromZero);
                }
            }
            c.ChartAreas[0].AxisX.Interval = CalcStepSize(maxX - minX, 5);
            c.ChartAreas[0].AxisY.Interval = CalcStepSize(maxY - minY, 5);
            c.ChartAreas[0].AxisX.Minimum = c.ChartAreas[0].AxisX.Interval * -2.5 + ((maxX + minX) / 2);
            c.ChartAreas[0].AxisX.Maximum = c.ChartAreas[0].AxisX.Interval * 2.5 + ((maxX + minX) / 2);
            c.ChartAreas[0].AxisY.Minimum = c.ChartAreas[0].AxisY.Interval * -2.5 + ((maxY + minY) / 2);
            c.ChartAreas[0].AxisY.Maximum = c.ChartAreas[0].AxisY.Interval * 2.5 + ((maxY + minY) / 2);
        }

        public static double CalcStepSize(double range, float targetSteps)
        {
            // calculate an initial guess at step size
            double tempStep = Math.Abs(range) / targetSteps;

            // get the magnitude of the step size
            double mag = Math.Floor(Math.Log10(tempStep));
            double magPow = Math.Pow(10, mag);

            // calculate most significant digit of the new step size
            double magMsd = (int)Math.Round(tempStep / magPow + 0.5,0);

            // promote the MSD to either 1, 2, or 5
            if (magMsd > 5.0)
                magMsd = 10.0f;
            else if (magMsd > 4.0)
                magMsd = 5.0f;
            else if (magMsd > 2.0)
                magMsd = 4.0f;
            else if (magMsd > 1.0)
                magMsd = 2.0f;

            return magMsd * magPow;
        }
        
        private List<double[]> GetRelevantData(PropertyInfo info, object data)
        {
            var values = new List<double[]>();

            if (info == null)
                return values;

            if (info.PropertyType == typeof(double))
            {
                for( int i=0; i<m_form.designs.Count; i++)
                {
                    var results = GetValueAllRuns(m_form.designs[i], info);
                    var dresults = new double[results.Length];
                    Array.Copy(results, dresults, results.Length);
                    values.Add(dresults);
                }
            }
            else if ((info.PropertyType == typeof(double[]) && data != null && data.GetType() == typeof(int)))
            {
                for (int i = 0; i < m_form.designs.Count; i++)
                {
                    var results = GetValueAllRuns(m_form.designs[i], info);
                    var dresults = new double[results.Length];
                    for (int j = 0; j < results.Length; j++)
                        dresults[j] = ((double[])results[j])[(int)data];
                    values.Add(dresults);
                }
            }
            else if ((info.PropertyType == typeof(double[,]) && data != null && data.GetType() == typeof(int)))
            {
                for (int i = 0; i < listView1.SelectedItems.Count; i++)
                {
                    Run_Case rc = GetRepresentedCase(listView1.SelectedItems[i]);
                    if (rc != null)
                    {
                        var results = (double[,])GetValueSingleRun(rc, info);
                        var dresults = new double[results.GetLength(0)];
                        for (int j = 0; j < results.GetLength(0); j++)
                            dresults[j] = ((double)results[j, (int)data]);
                        values.Add(dresults);
                    }
                }
            }
            else if ((info.PropertyType == typeof(Dictionary<string,double[,]>) && data != null && data.GetType() == typeof(int)))
            {
                for (int i = 0; i < listView1.SelectedItems.Count; i++)
                {
                    Run_Case rc = GetRepresentedCase(listView1.SelectedItems[i]);
                    if (rc != null)
                    {
                        var results = (Dictionary<string, double[,]>)GetValueSingleRun(rc, info);
                        foreach (var kvp in results)
                        {
                            var dresults = new double[kvp.Value.GetLength(0)];
                            for (int j = 0; j < dresults.GetLength(0); j++)
                                dresults[j] = ((double)kvp.Value[j, (int)data]);
                            values.Add(dresults);
                        }
                    }
                }
            }
            return values;
        }

        private static object GetValueSingleRun(Run_Case rc, PropertyInfo info)
        {
            return GetPropertyValue(info, rc);
        }

        /// <summary>
        /// For each run analyzed, get the value of the desired property
        /// </summary>
        /// <param name="ac"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        private static object[] GetValueAllRuns(Aircraft ac, PropertyInfo info)
        {
            List<object> values = new List<object>();
            foreach (Run_Case rc in ac.Analyze_Cases)
            {
                //skip cases that are not finished
                if (rc.Current_Status != Run_Case.Case_Status.Finished)
                    continue;
                var val = GetPropertyValue(info, rc);
                if (val != null)//should we strip out the nulls? I think yes
                    values.Add(val);
            }
            return values.ToArray();
        }

        /// <summary>
        /// Gets the value of the desired property from object o
        /// </summary>
        /// <param name="prop">Property who's value we want</param>
        /// <param name="o">object to get property from</param>
        /// <returns>Returns value as object. If property does not exist, returns null</returns>
        private static object GetPropertyValue(PropertyInfo prop, object o)
        {
            var locations = new List<object>();
            locations.Add(o);

            foreach (PropertyInfo info in o.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (info.PropertyType.Module.ScopeName != "CommonLanguageRuntimeLibrary")
                    locations.Add(info.GetValue(o, null));
            }
            for (int i = 0; i < locations.Count; i++)
            {
                if (locations[i].GetType().GetProperty(prop.Name) != null)
                {
                    var results = prop.GetValue(locations[i], null);
                        return results;//could be null...
                }
            }
            return null;
        }

        private void plotTrefftz2(Chart c)
        {
            c.Series.Clear();
            m_charts[c].PrimaryX = new Tuple<PropertyInfo, object>(typeof(Run_Case).GetProperty("Strip_Forces"), 0);
            m_charts[c].PrimaryY = new Tuple<PropertyInfo, object>(typeof(Run_Case).GetProperty("Strip_Forces"), 1);
            m_charts[c].SecondaryY = new Tuple<PropertyInfo, object>(typeof(Run_Case).GetProperty("Strip_Forces"), 9);
            PlotChart(m_charts[c]);

            c.ChartAreas[0].AxisX.Minimum = -1;
            c.ChartAreas[0].AxisX.Maximum = 1;
            c.ChartAreas[0].AxisY.Minimum = -1;
            c.ChartAreas[0].AxisY.Maximum = Math.Max(c.ChartAreas[0].AxisY.Maximum, 2.5);//just in case a design goes higher than 2.5...
            c.ChartAreas[0].AxisX.Title = "Spanwise Location";
            c.ChartAreas[0].AxisY.Title = "Section Lift Coefficient";
        }

        private void plotTrefftz(Aircraft ac, Run_Case rc, Chart c)
        {
            //if there is a mixup between the aircraft and run case, stop
            if (!ac.Analyze_Cases.Contains(rc))
                return;

            if(rc.Strip_Forces.Count > 0)
            {
                //this does it for each surface
                foreach (string s in rc.Strip_Forces.Keys)
                {
                    double[] Yle = new double[rc.Strip_Forces[s].GetLength(0)];
                    double[] cl = new double[rc.Strip_Forces[s].GetLength(0)];
                    double[] cl_c_cref = new double[rc.Strip_Forces[s].GetLength(0)];
                    for (int i = 0; i < Yle.Length; i++)
                    {
                        Yle[i] = (rc.Strip_Forces[s])[i, 0];
                        cl[i] = (rc.Strip_Forces[s])[i, 9];
                        cl_c_cref[i] = (rc.Strip_Forces[s])[i, 1];
                    }
                    PlotSpline(c, Yle, cl, main_Lines == 0 ? "cl" : "cl " + s + main_Lines.ToString(), Color.Green, main_Lines == 0, true);
                    PlotSpline(c, Yle, cl_c_cref, main_Lines == 0 ? "cl*(c/cref)" : "cl*(c/cref) " + s + main_Lines.ToString(), Color.Orange, main_Lines == 0, true);
                    main_Lines++;
                }
                c.ChartAreas[0].AxisX.Minimum = -1;
                c.ChartAreas[0].AxisX.Maximum = 1;
                c.ChartAreas[0].AxisX.Title = "Spanwise Location";
                c.ChartAreas[0].AxisY.Title = "Section Lift Coefficient";
            }
        }

        private void plotAlphaSweep(Chart c)
        {
            m_charts[c].PrimaryX = new Tuple<PropertyInfo, object>(typeof(Run_Case.TotalForces).GetProperty("Alpha"), null);
            m_charts[c].PrimaryY = new Tuple<PropertyInfo, object>(typeof(Run_Case.TotalForces).GetProperty("CL_tot"), null);
            //m_charts[subChart1].SecondaryY = new Tuple<PropertyInfo, object>(typeof(Run_Case.TotalForces).GetProperty("elevator"), null);
        }

        private void plotCl_CD(Chart c)
        {
            m_charts[c].PrimaryX = new Tuple<PropertyInfo, object>(typeof(Run_Case.TotalForces).GetProperty("CD_tot"), null);
            m_charts[c].PrimaryY = new Tuple<PropertyInfo, object>(typeof(Run_Case.TotalForces).GetProperty("CL_tot"), null);
        }

        private void PlotSpline(Chart c, double[] xvals, double[] yvals, string label, Color color, bool legend, bool primary)
        {
            if (xvals.Length != yvals.Length)
                return;

            Series temps = null;
            //if name already exists, clear it out
            for (int i = 0; i < c.Series.Count; i++)
            {
                if (c.Series[i].Name == label)
                {
                    //temps = c.Series[i];
                    label = label + " " + c.Series.Count.ToString();
                    break;
                }
            }

            if( temps == null)
                temps = c.Series.Add(label);

            if (!primary)
                temps.YAxisType = AxisType.Secondary;

            temps.ToolTip = "#VALY, #VALX";
            temps.Points.DataBindXY(xvals, yvals);
            temps.IsVisibleInLegend = legend;
            temps.ChartType = SeriesChartType.Spline;

            //c.ChartAreas[0].AxisX.Minimum = Math.Min(c.ChartAreas[0].AxisX.Minimum, ((int)Math.Round(temps.Points.FindMinByValue("X").XValue / 10.0 - 0.5)) * 10);
            //c.ChartAreas[0].AxisX.Maximum = Math.Max(c.ChartAreas[0].AxisX.Maximum, ((int)Math.Round(temps.Points.FindMaxByValue("X").XValue / 10.0 + 0.5)) * 10);
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            /*if (listView1.SelectedItems.Count > 0)
            {
                mainChart.Series.Clear();
                main_Lines = 0;
            }

            for (int i = 0; i < listView1.SelectedItems.Count; i++)
            {
                string ac_name = listView1.SelectedItems[i].Name.Split(new string[] { "~~~" }, StringSplitOptions.RemoveEmptyEntries)[0];
                string caseNum = listView1.SelectedItems[i].Name.Split(new string[]{"~~~"}, StringSplitOptions.RemoveEmptyEntries)[1];
                foreach (Aircraft ac in m_form.designs)
                {
                    if (ac.Configuration_Name == ac_name && ac.Analyze_Cases.Count > int.Parse(caseNum))
                        plotTrefftz(ac, ac.Analyze_Cases[int.Parse(caseNum)], mainChart);
                }
            }*/
            RefreshAllCharts();
        }

        private void trefftzPlotToolStripMenuItem_Click(object sender, EventArgs e)
        {
            plotTrefftz2((Chart)m_menuSource);
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            m_menuSource = ((ContextMenuStrip)sender).SourceControl;
        }

        private void saveAsImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_menuSource == null)
                return;

            var SD = new SaveFileDialog();
            SD.Title = "Save Image to file";
            SD.OverwritePrompt = true;
            SD.Filter = "PNG (*.png)|*.png|JPEG (*.jpg)|*.jpg";
            if (SD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (SD.FilterIndex == 1)
                    (m_menuSource as Chart).SaveImage(SD.FileName, ChartImageFormat.Png);
                else if (SD.FilterIndex == 2)
                    (m_menuSource as Chart).SaveImage(SD.FileName, ChartImageFormat.Jpeg);
            }
        }

        private void noneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_charts[(Chart)m_menuSource].PrimaryX = null;
            RefreshAllCharts();
        }

        private void primYNoneItem_Click(object sender, EventArgs e)
        {
            m_charts[(Chart)m_menuSource].PrimaryY = null;
            RefreshAllCharts();
        }

        private void secYNoneItem_Click(object sender, EventArgs e)
        {
            m_charts[(Chart)m_menuSource].SecondaryY = null; 
            RefreshAllCharts();
        }

        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (listView1.FocusedItem.Bounds.Contains(e.Location) == true)
                {
                    runMenuStrip.Tag = listView1.FocusedItem;
                    runMenuStrip.Show(MousePosition);
                }
            }
        }

        private void rawTotalForcesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (runMenuStrip.Tag == null)
                return;

            if (runMenuStrip.Tag.GetType() == typeof(ListViewItem))
            {
                Run_Case rc = GetRepresentedCase(runMenuStrip.Tag as ListViewItem);
                if (rc != null)
                {
                    RawDataForm rdf = new RawDataForm((runMenuStrip.Tag as ListViewItem).Group.Header, "Total Forces", rc.Forces.FullText);
                    rdf.Show();
                }
            }
        }

        private void rawStripForcesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (runMenuStrip.Tag == null)
                return;

            if (runMenuStrip.Tag.GetType() == typeof(ListViewItem))
            {
                Run_Case rc = GetRepresentedCase(runMenuStrip.Tag as ListViewItem);
                if (rc != null)
                {
                    RawDataForm rdf = new RawDataForm((runMenuStrip.Tag as ListViewItem).Group.Header, "Strip Forces", rc.StripFullText);
                    rdf.Show();
                }
            }
        }

        private void rawStabilityToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (runMenuStrip.Tag == null)
                return;

            if (runMenuStrip.Tag.GetType() == typeof(ListViewItem))
            {
                Run_Case rc = GetRepresentedCase(runMenuStrip.Tag as ListViewItem);
                if (rc != null)
                {
                    RawDataForm rdf = new RawDataForm((runMenuStrip.Tag as ListViewItem).Group.Header, "Stability", rc.StabDerivs.FullText);
                    rdf.Show();
                }
            }
        }

        private void showRawDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (runMenuStrip.Tag == null)
                return;

            if (runMenuStrip.Tag.GetType() == typeof(ListViewItem))
            {
                Run_Case rc = GetRepresentedCase(runMenuStrip.Tag as ListViewItem);
                if (rc != null)
                {
                    RawDataForm rdf = new RawDataForm((runMenuStrip.Tag as ListViewItem).Group.Header, "All Raw Data", rc.Forces.FullText + "\r\n" + rc.StabDerivs.FullText + "\r\n" + rc.StripFullText);
                    rdf.Show();
                }
            }
        }

        private void deleteRunCaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (runMenuStrip.Tag == null)
                return;

            if (runMenuStrip.Tag.GetType() == typeof(ListViewItem))
            {
                listView1.Items.Remove(runMenuStrip.Tag as ListViewItem);
                RefreshAllCharts();
            }
        }
    }
}
