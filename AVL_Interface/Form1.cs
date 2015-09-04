using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace AVL_Interface
{
    public partial class Form1 : Form
    {
        public List<Aircraft> designs = new List<Aircraft>();

        public Form1()
        {
            InitializeComponent();
            Aircraft.OnUpdateAircraft += Aircraft_OnUpdateAircraft;

            if (String.IsNullOrEmpty(Properties.Settings.Default.AVL_Location))
            {
                newFileButton.Enabled = false;
                openFileButton.Enabled = false;
                saveAllButton.Enabled = false;
                constraintWindowButton.Enabled = false;
            }
        }

        private void Aircraft_OnUpdateAircraft(Aircraft sender, Aircraft.AircraftUpdateEventArgs e)
        {
            switch(e.UpdateType)
            {
                case Aircraft.AircraftUpdateEventArgs.Update_Type.Closed:
                    {
                        //remove the instance usercontrol
                        InstanceUC iuc = null;
                        foreach (Control c in flowLayoutPanel1.Controls)
                            if (c.GetType() == typeof(InstanceUC))
                                if ((c as InstanceUC).aircraft == sender)
                                    iuc = (c as InstanceUC);

                        if (iuc != null)
                            flowLayoutPanel1.Controls.Remove(iuc);

                        //remove the constraint usercontrol
                        ConstraintsUC cuc = null;
                        foreach (Control c in flowLayoutPanel2.Controls)
                            if (c.GetType() == typeof(ConstraintsUC))
                                if ((c as ConstraintsUC).DispAircraft == sender)
                                    cuc = (c as ConstraintsUC);

                        if (cuc != null)
                            flowLayoutPanel2.Controls.Remove(cuc);

                        //finally remove this aircraft from our list of designs
                        designs.Remove(sender);
                    }; break;
            }
        }

        private void aVLLocationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Title = "Select instance of AVL.exe";
            fd.Multiselect = false;
            fd.Filter = "Executable Files (*.exe)|*.exe";
            fd.InitialDirectory = Properties.Settings.Default.AVL_Location;

            if (fd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (!fd.CheckFileExists)
                    return;

                string AVL = fd.FileName;
                Properties.Settings.Default.AVL_Location = AVL;
                Properties.Settings.Default.Save();

                newFileButton.Enabled = true;
                openFileButton.Enabled = true;
                saveAllButton.Enabled = true;
                constraintWindowButton.Enabled = true;
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog(); 
            fd.Title = "AVL Vehicle File";
            fd.Multiselect = true;
            fd.Filter = "AVL Files (*.avl)|*.avl";
            fd.InitialDirectory = Properties.Settings.Default.AVL_Location; 
            
            if (fd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                foreach (string file in fd.FileNames)
                {
                    if (!fd.CheckFileExists)
                        return;

                    Aircraft des = new Aircraft(file);
                    designs.Add(des);

                    InstanceUC iuc = new InstanceUC(des);
                    this.flowLayoutPanel1.Controls.Add(iuc);
                    iuc.Click += new EventHandler(iuc_Click);

                    System.Threading.Thread.Sleep(75);
                    ConstraintsUC cuc = new ConstraintsUC(des);
                    cuc.Height = flowLayoutPanel2.Height-25;
                    this.flowLayoutPanel2.Controls.Add(cuc);
                    //RibbonButton rb = new RibbonButton(des.Configuration_Name.Substring(0, 10));
                    //if (des.PrimaryInstance.Geometry_Image != null)
                    //    rb.Image = des.PrimaryInstance.Geometry_Image.GetThumbnailImage(40, 40, new Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero);
                    //aircraftButtonList.Buttons.Add(rb);
                    //rb.Visible = true;
                }
            }
        }

        void iuc_Click(object sender, EventArgs e)
        {
            Aircraft selected = ((InstanceUC)sender).aircraft;
            if (this.aircraft_Info1.Displayed_Aircraft != selected)
                aircraft_Info1.SetAircraft(selected);
        }

        private bool ThumbnailCallback()
        {
            return false;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            aircraft_Info1.Dispose();//dispose of the uc so it doesnt try to update while we delete aircraft
            Aircraft.OnUpdateAircraft -= Aircraft_OnUpdateAircraft;//unregister from the aircraft's events
            foreach (Aircraft inst in designs)//close them all
                inst.Close();
        }

        private void flowLayoutPanel1_ControlAdded(object sender, ControlEventArgs e)
        {
            e.Control.Width = flowLayoutPanel1.Width;
            if (flowLayoutPanel1.Controls.Count == 1 && e.Control.GetType() == typeof(InstanceUC))
                iuc_Click(e.Control, null);
        }

        private void flowLayoutPanel1_Resize(object sender, EventArgs e)
        {
            foreach (Control c in flowLayoutPanel1.Controls)
                c.Width = flowLayoutPanel1.Width;
        }

        private void Form1_ResizeBegin(object sender, EventArgs e)
        {
            this.SuspendLayout();
        }

        private void Form1_ResizeEnd(object sender, EventArgs e)
        {
            this.ResumeLayout();
        }

        private void flowLayoutPanel2_Resize(object sender, EventArgs e)
        {
            foreach (Control c in flowLayoutPanel2.Controls)
            {
                if (c.GetType() == typeof(ConstraintsUC))
                    (c as ConstraintsUC).Height = flowLayoutPanel2.Height - 25;
            }
        }

        private void constraintWindowButton_Click(object sender, EventArgs e)
        {
            DesignCasesWindow drc = new DesignCasesWindow(designs);
            if (drc.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                this.tabControl2.SelectedIndex = 1;
        }

        private void ribbonCheckBox1_CheckBoxCheckChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.AVL_Inst_Count = ribbonCheckBox1.Checked;
            Properties.Settings.Default.Save();
        }

        private void saveAllButton_Click(object sender, EventArgs e)
        {
            foreach (Aircraft ac in designs)
                ac.SaveFile();
        }

        private void newFileButton_Click(object sender, EventArgs e)
        {
            //here we create a new default airplane
            InputBox ib = new InputBox("Create a new aircraft design", "Name of new design:");
            if(ib.ShowDialog()== System.Windows.Forms.DialogResult.OK)
            {
                if (ib.InputText == string.Empty)
                    return;

                Aircraft ac = new Aircraft();
                ac.Initial_AVL_File.Title = ib.InputText;
                
                //make wing surface
                AVL_File.Surface wingSurf = new AVL_File.Surface("Wing", ac.Initial_AVL_File);
                AVL_File.Surface.Section startSec = new AVL_File.Surface.Section(wingSurf);
                startSec.Chord = 12;
                wingSurf.Sections.Add(startSec);
                AVL_File.Surface.Section endsec = new AVL_File.Surface.Section(wingSurf);
                endsec.Chord = 12;
                endsec.Y_LeadingEdge = 20;
                wingSurf.Sections.Add(endsec);

                ac.Initial_AVL_File.Surfaces.Add(wingSurf);

                //make hstab surface
                AVL_File.Surface hSurf = new AVL_File.Surface("HTail", ac.Initial_AVL_File);
                AVL_File.Surface.Section startHSec = new AVL_File.Surface.Section(hSurf);
                startHSec.Chord = 7;
                startHSec.X_LeadingEdge = 30;
                hSurf.Sections.Add(startHSec);
                AVL_File.Surface.Section endHsec = new AVL_File.Surface.Section(hSurf);
                endHsec.Chord = 7;
                endHsec.X_LeadingEdge = 30;
                endHsec.Y_LeadingEdge = 9;
                hSurf.Sections.Add(endHsec);

                ac.Initial_AVL_File.Surfaces.Add(hSurf);

                ac.Initial_AVL_File.Sref = 480;
                ac.Initial_AVL_File.Bref = 40;
                ac.Initial_AVL_File.Cref = 12;
                ac.Initial_AVL_File.Xref = 3;

                this.designs.Add(ac);

                InstanceUC iuc = new InstanceUC(ac);
                this.flowLayoutPanel1.Controls.Add(iuc);
                iuc.Click += new EventHandler(iuc_Click);

                System.Threading.Thread.Sleep(75);
                ConstraintsUC cuc = new ConstraintsUC(ac);
                cuc.Height = flowLayoutPanel2.Height - 25;
                this.flowLayoutPanel2.Controls.Add(cuc);
            }
        }
    }
}
