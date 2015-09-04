using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace AVL_Interface
{
    public partial class ConstraintsUC : UserControl
    {
        public Aircraft DispAircraft { get; private set; }

        public ConstraintsUC()
        {
            InitializeComponent();
        }

        public ConstraintsUC(Aircraft ac):this()
        {
            DispAircraft = ac;
            if (DispAircraft.PrimaryInstance != null)
                this.pictureBox1.Image = DispAircraft.PrimaryInstance.Geometry_Image;
            Aircraft.OnUpdateAircraft += new Aircraft.UpdateEventHandler(Aircraft_OnUpdateAircraft);
            AVL_Instance.OnUpdateMessages += new MessageEventHandler(AVL_Instance_OnUpdateMessages);
            PopulateData();
        }

        private void Aircraft_OnUpdateAircraft(Aircraft sender, Aircraft.AircraftUpdateEventArgs e)
        {
            if (sender != DispAircraft)
                return;

            if (this.InvokeRequired)
                this.Invoke((MethodInvoker)delegate { Aircraft_OnUpdateAircraft(sender, e); });
            else
            {
                //if we add a new case, repopulate the lists
                if (e.UpdateType == Aircraft.AircraftUpdateEventArgs.Update_Type.Added_Case)
                    PopulateData();
                //if a case has ended, find that listview item and change picture and status
                else if (e.UpdateType == Aircraft.AircraftUpdateEventArgs.Update_Type.Ended_Case)
                {
                    this.statusLabel.Text = "Executing Cases, Finished Case " + ((int)e.Message).ToString();

                    for (int i = 0; i < this.DispAircraft.Analyze_Cases.Count; i++)
                    {
                        if (this.DispAircraft.Analyze_Cases[i].Case_Index != (int)e.Message)
                            continue;
                        ListViewItem lvi = listView1.Groups[0].Items[i];
                        lvi.ImageIndex = 1;
                        lvi.SubItems[2].Text = "Finished";
                        break;
                    }
                    if ((int)e.Message == listView1.Groups[0].Items.Count - 1)
                    {
                        this.statusLabel.Text = "Finished executing all cases";
                        runCaseButton.Enabled = true;
                    }
                }
                //if a case has just started, find that listview item and change picture and status
                else if (e.UpdateType == Aircraft.AircraftUpdateEventArgs.Update_Type.Started_Case)
                {
                    for (int i = 0; i < this.DispAircraft.Analyze_Cases.Count; i++)
                    {
                        if (this.DispAircraft.Analyze_Cases[i].Case_Index != (int)e.Message)
                            continue;
                        ListViewItem lvi = listView1.Groups[0].Items[i];
                        lvi.ImageIndex = 4;//replace this with a 'pending' image
                        lvi.SubItems[2].Text = "Working";
                        break;
                    }
                }
            }
        }

        private void AVL_Instance_OnUpdateMessages(AVL_Instance sender, AVLEventArgs e)
        {
            if (this.InvokeRequired)
                this.Invoke((MethodInvoker)delegate { AVL_Instance_OnUpdateMessages(sender, e); });
            else
            {
                //if the AVL_Instance's airplane is our airplane
                if (sender.AVL_aircraft == DispAircraft)
                {
                    //if this is a geom pict update
                    if (e.DataSource == AVLEventArgs.AVLData_Source.Geometry_Pict)
                        this.pictureBox1.Image = ((AVL_PictureEventArgs)e).Img;
                }
            }
        }

        private void PopulateData()
        {
            if (DispAircraft == null)
                return;

            this.listView1.SuspendLayout();
            this.listView1.Items.Clear();
            this.listView1.Groups.Clear();

            var group = this.listView1.Groups.Add("Status", "Status");

            for (int i = 0; i < DispAircraft.Analyze_Cases.Count; i++)
            {
                //add the item for the specific case
                Run_Case rc = DispAircraft.Analyze_Cases[i];
                this.listView1.Items.Add(new ListViewItem(new string[] { "Case " + rc.Case_Index.ToString(), "Status", "Ready" }, 2, this.listView1.Groups[0])).Tag = rc.Case_Index;
                //add the group to represent all the case parameters
                group = this.listView1.Groups.Add("Case " + rc.Case_Index.ToString(), "Case " + rc.Case_Index.ToString());
                group.Tag = rc.Case_Index;
                //add each parameter to the ith-case group
                foreach (var kvp in rc.Parameters)
                {
                    string[] subitems = new string[3];

                    if (!string.IsNullOrEmpty(kvp.Key.GetLabel()))
                        subitems[0] = kvp.Key.GetLabel();
                    else//special situation for the control surfaces and section span/chord
                        //if we are a control surface display the actual surface's name
                        if ((int)kvp.Key >= (int)Run_Case.Independant_Vars.D1 && (int)kvp.Key <= (int)Run_Case.Independant_Vars.D10)
                            subitems[0] = DispAircraft.Initial_AVL_File.Controls[(int)kvp.Key - (int)Run_Case.Independant_Vars.D1];

                    if (!string.IsNullOrEmpty(kvp.Value.Item1.GetLabel()))
                        subitems[1] = kvp.Value.Item1.GetLabel();
                    else//special situation for the control surfaces and section span/chord
                        //if we are a control surface display the actual surface's name
                        if ((int)kvp.Value.Item1 >= (int)Run_Case.Constraint.D1 && (int)kvp.Value.Item1 <= (int)Run_Case.Constraint.D10)
                            subitems[1] = DispAircraft.Initial_AVL_File.Controls[(int)kvp.Value.Item1 - (int)Run_Case.Constraint.D1];

                    subitems[2] = kvp.Value.Item2.ToString();

                    this.listView1.Items.Add(new ListViewItem(subitems, -1, group));
                }
            }
            this.listView1.ResumeLayout();
            this.statusLabel.Text = DispAircraft.Configuration_Name.Substring(0, Math.Min(DispAircraft.Configuration_Name.Length, 35)) +" Ready: " + DispAircraft.Analyze_Cases.Count.ToString() + " Cases Total";
        }

        private void listView1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (listView1.SelectedItems.Count > 0)
                {
                    foreach (ListViewItem lvi in listView1.SelectedItems)
                    {
                        //need to add code here to remove the constraint from the case
                        //we are deleting a full case from the 'status' group
                        if (lvi.Group == listView1.Groups[0])
                        {
                            int caseNum = (int)lvi.Tag;
                            DispAircraft.Remove_Run_Case(caseNum);
                        }
                        else//otherwise we are removing a parameter from a single run
                        {
                            string[] words = lvi.Group.Name.Split();
                            int caseNum = -1;
                            if (int.TryParse(words[1], out caseNum))
                            {
                                //m_ac.Analyze_Cases[caseNum].Parameters;
                            }
                        }
                        lvi.Remove();

                    }
                    PopulateData();
                }
            }
        }

        private void runCaseButton_Click(object sender, EventArgs e)
        {
            if (DispAircraft.Analyze_Cases.Count > 0)
            {
                DispAircraft.Run_All_Cases(Properties.Settings.Default.AVL_Inst_Count ? 1 : Environment.ProcessorCount);
                this.statusLabel.Text = "Executing cases, please wait...";
                runCaseButton.Enabled = false;
            }
        }

        private void clearCasesButton_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            listView1.Groups.Clear();
            DispAircraft.Clear_Run_Cases();
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                if (listView1.SelectedItems[0].Group.Name == "Status")
                {
                    foreach (ListViewItem lvi in listView1.Groups[listView1.SelectedItems[0].SubItems[0].Text].Items)
                    {
                        lvi.Selected = true;
                        lvi.EnsureVisible();
                    }
                }
            }
        }
    }
}
