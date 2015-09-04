using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace AVL_Interface
{
    public partial class DesignCasesWindow : Form
    {
        private Aircraft selected_Aircraft;

        public DesignCasesWindow()
        {
            InitializeComponent();
            DefineVarOptions(primaryVarCombobox);
            DefineVarOptions(additionalVarCombobox);
            additionalVarCombobox.SelectedIndex = 8;
            DefineConOptions(additionalConCombobox);
            additionalConCombobox.SelectedIndex = 9;
        }

        public DesignCasesWindow(List<Aircraft> craft):this()
        {
            foreach (Aircraft ac in craft)
            {
                if (ac.PrimaryInstance == null)
                    continue;
                InstanceUC iuc = new InstanceUC(ac);
                iuc.Selectable = true;
                iuc.MultiSelect = false;
                iuc.OnSelected += new InstanceUC.SelectedEventHandler(iuc_OnSelected);
                this.flowLayoutPanel1.Controls.Add(iuc);
                //if this is the first control added, auto-select it
                if (this.flowLayoutPanel1.Controls.Count == 1)
                    iuc.Selected = true;
            }
        }

        private void iuc_OnSelected(InstanceUC sender, InstanceUC.SelectedChangedEventArgs e)
        {
            if (e.Selected)
            {
                this.selected_Aircraft = sender.aircraft;
                primaryVarCombobox_SelectedIndexChanged(sender, null);
                additionalVarCombobox_SelectedIndexChanged(sender, null);
                DefineControlSurfaceOptions(primaryExtraCombobox); 
                DefineControlSurfaceOptions(additionalExtraCombobox);
                listView1.Items.Clear();
            }
        }

        private void DefineVarOptions(ComboBox cxbx)
        {
            List<KeyValuePair<string, int>> list = new List<KeyValuePair<string, int>>();
            Run_Case.Independant_Vars[] e = (Run_Case.Independant_Vars[])(Enum.GetValues(typeof(Run_Case.Independant_Vars)));
            for (int i = 0; i < e.Length; i++)
            {
                switch (e[i])
                {
                    case Run_Case.Independant_Vars.Section_Span: break;
                    case Run_Case.Independant_Vars.Section_Chord: break;
                    default:
                        {
                            if (!string.IsNullOrEmpty(e[i].GetLabel()))
                                list.Add(new KeyValuePair<string, int>(e[i].GetLabel(), i));
                        } break;
                }
            } 
            cxbx.DisplayMember = "Key";
            cxbx.ValueMember = "Value";
            cxbx.DataSource = list;
        }
        private void DefineConOptions(ComboBox cxbx)
        {
            List<KeyValuePair<string, int>> list = new List<KeyValuePair<string, int>>();
            Run_Case.Constraint[] e = (Run_Case.Constraint[])(Enum.GetValues(typeof(Run_Case.Constraint)));
            for (int i = 0; i < e.Length; i++)
            {
                if (!string.IsNullOrEmpty(e[i].GetLabel()))
                    list.Add(new KeyValuePair<string, int>(e[i].GetLabel(), i));
            }

            cxbx.DisplayMember = "Key";
            cxbx.ValueMember = "Value";
            cxbx.DataSource = list;
        }

        private void DefineControlSurfaceOptions(ComboBox cxbx)
        {
            if (selected_Aircraft == null)
                return;

            List<KeyValuePair<string, int>> list = new List<KeyValuePair<string, int>>();
            int d1_enumIndex = (int)Run_Case.Independant_Vars.D1;

            int startIndex = 0;
            for (int i = 0; i < selected_Aircraft.Initial_AVL_File.Controls.Length; i++)
            {
                list.Add(new KeyValuePair<string, int>(selected_Aircraft.Initial_AVL_File.Controls[i], d1_enumIndex + i));
                if (startIndex == 0 && selected_Aircraft.Initial_AVL_File.Controls[i].ToLowerInvariant().Contains("elev"))
                    startIndex = i;
            }

            if (list.Count > 0)
            {
                cxbx.DisplayMember = "Key";
                cxbx.ValueMember = "Value";
                cxbx.DataSource = list;
                cxbx.SelectedIndex = startIndex;
            }
            else
            {
                cxbx.DataSource = null;
                cxbx.Items.Clear();
            }
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            if (additionalConCombobox.SelectedItem == null || additionalVarCombobox.SelectedItem == null)
                return;

            if ((int)additionalVarCombobox.SelectedValue == (int)Run_Case.Independant_Vars.Control_Surface && additionalExtraCombobox.SelectedValue == null)
                return;

            string varText = "";
            int varValue = -1;
            string conText = "";
            int conValue = -1;

            switch ((int)additionalVarCombobox.SelectedValue)
            {
                case (int)Run_Case.Independant_Vars.Control_Surface:
                    {
                        varText = additionalExtraCombobox.Text;
                        varValue = (int)additionalExtraCombobox.SelectedValue;
                    }break;
                default:
                    {
                        varText = additionalVarCombobox.Text;
                        varValue = (int)additionalVarCombobox.SelectedValue;
                    }break;
            }

            if ((int)additionalConCombobox.SelectedValue == (int)Run_Case.Constraint.Value)
            {
                switch ((int)additionalVarCombobox.SelectedValue)
                {
                    case (int)Run_Case.Independant_Vars.Section_Span: break;
                    case (int)Run_Case.Independant_Vars.Section_Chord: break;
                    case (int)Run_Case.Independant_Vars.X_cg:
                    case (int)Run_Case.Independant_Vars.Y_cg:
                    case (int)Run_Case.Independant_Vars.Z_cg: conValue = -1; conText = "Value"; break;
                    case (int)Run_Case.Independant_Vars.Control_Surface: conValue = (int)((Run_Case.Independant_Vars)varValue).GetMatchingCon(); conText = varText; break;
                    default: additionalConCombobox.SelectedValue = (int)((Run_Case.Independant_Vars)varValue).GetMatchingCon(); break;
                }
            }

            if (string.IsNullOrEmpty(conText))
                conText = additionalConCombobox.Text;
            if (conValue == -1)
                conValue = (int)additionalConCombobox.SelectedValue;
            
            ListViewItem lvi = new ListViewItem(new string[] { varText, conText, addionalValueUpDown.Value.ToString() });
            lvi.Tag = new Tuple<Run_Case.Independant_Vars, Run_Case.Constraint, double>((Run_Case.Independant_Vars)varValue,
                (Run_Case.Constraint)conValue, (double)addionalValueUpDown.Value);
            listView1.Items.Add(lvi);
        }

        private void listView1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
                foreach (ListViewItem listViewItem in listView1.SelectedItems)
                    listViewItem.Remove();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void acceptButton_Click(object sender, EventArgs e)
        {
            if (selected_Aircraft == null)
                return;

            //you must actually select a proper variable not type junk in
            if (primaryVarCombobox.SelectedValue == null)
                return;

            if ((int)primaryVarCombobox.SelectedValue == (int)Run_Case.Independant_Vars.Control_Surface && primaryExtraCombobox.SelectedValue == null)
                return;

            //for loop generating one new RunCase per step in the independant variable sweep
            for (double sweep = (double)indMinUpDown.Value; sweep <= (double)indMaxUpDown.Value; sweep += (double)indStepUpDown.Value)
            {
                Run_Case tempCase = new Run_Case();
                tempCase.CaseNotes = primaryVarCombobox.Text + " Sweep = " + sweep.ToString();
                //tempCase.CaseNotes = "Sweep of " + primaryVarCombobox.Text + " from " + indMinUpDown.Value.ToString() + " to " + indMaxUpDown.Value.ToString() + " by " + indStepUpDown.Value.ToString() + "\r\n";
                //tempCase.CaseNotes += "Case Value = " + sweep.ToString();
                switch ((int)primaryVarCombobox.SelectedValue)
                {
                    case (int)Run_Case.Independant_Vars.Section_Chord:
                    case (int)Run_Case.Independant_Vars.Section_Span: break;
                    case (int)Run_Case.Independant_Vars.Control_Surface:
                        {//if we are playing with a control surface, then we need to use the selected value of the extra combobox instead of the primary one
                            tempCase.SetParameters((Run_Case.Independant_Vars)primaryExtraCombobox.SelectedValue, ((Run_Case.Independant_Vars)primaryExtraCombobox.SelectedValue).GetMatchingCon(), sweep);
                        }break;
                    default: tempCase.SetParameters((Run_Case.Independant_Vars)primaryVarCombobox.SelectedValue, ((Run_Case.Independant_Vars)primaryVarCombobox.SelectedValue).GetMatchingCon(), sweep); break;
                }
                //finally add in all the addional constraints
                for (int i = 0; i < listView1.Items.Count; i++)
                {
                    var data = (Tuple<Run_Case.Independant_Vars, Run_Case.Constraint, double>)listView1.Items[i].Tag;
                    tempCase.SetParameters(data.Item1, data.Item2, data.Item3);
                }
                selected_Aircraft.Add_Run_Case(tempCase);
            }
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void indMinUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (indMinUpDown.Value > indMaxUpDown.Value)
                indMinUpDown.Value = indMaxUpDown.Value;
        }

        private void primaryVarCombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (primaryVarCombobox.SelectedIndex == (int)Run_Case.Independant_Vars.Control_Surface)
            {
                DefineControlSurfaceOptions(primaryExtraCombobox);
                primaryExtraCombobox.Enabled = true;
            }
            else
            {
                primaryExtraCombobox.DataSource = null;
                primaryExtraCombobox.Items.Clear();
                primaryExtraCombobox.Enabled = false;
            }
        }

        private void additionalVarCombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (additionalVarCombobox.SelectedIndex == (int)Run_Case.Independant_Vars.Control_Surface)
            {
                DefineControlSurfaceOptions(additionalExtraCombobox);
                additionalExtraCombobox.Enabled = true;
            }
            else
            {
                additionalExtraCombobox.DataSource = null;
                additionalExtraCombobox.Items.Clear();
                additionalExtraCombobox.Enabled = false;
            }
        }
    }
}
