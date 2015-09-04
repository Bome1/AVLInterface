using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AVL_Interface
{
    public partial class Aircraft_Info : UserControl
    {
        private Aircraft m_ap;

        public Aircraft Displayed_Aircraft { get { return m_ap; } }

        private GeometryModeler.Views m_currentView = GeometryModeler.Views.Isomeric;
        private int selected_section = -1;
        private string selected_surf = string.Empty;

        public Aircraft_Info()
        {
            InitializeComponent();
            //AVL_Instance.OnUpdateMessages += AVL_Instance_OnUpdateMessages;
            SurfaceUC.OnUpdateSelection += SurfaceUC_OnUpdateSelection;
            Aircraft.OnUpdateAircraft += Aircraft_OnUpdateAircraft;
        }

        private void Aircraft_OnUpdateAircraft(Aircraft sender, Aircraft.AircraftUpdateEventArgs e)
        {
            //if we are showing an aircraft and that aircraft raises a message, act
            if (m_ap != null && sender == m_ap)
            {
                switch (e.UpdateType)
                {
                    case Aircraft.AircraftUpdateEventArgs.Update_Type.File_Load:
                        {
                            m_ap = null;
                            SetAircraft(sender);
                        }; break;
                    case Aircraft.AircraftUpdateEventArgs.Update_Type.Closed: this.Clear(); break;
                }
            }
        }

        private void SurfaceUC_OnUpdateSelection(SurfaceUC sender, SurfaceUC.SectionSelectEventArgs e)
        {
            if (sender == null)
                return;

            if (m_ap.Initial_AVL_File.Surfaces.Contains(sender.DispSurface))
            {
                selected_section = e.Selected_Section;
                selected_surf = sender.DispSurface.Name;
                this.pictureBox1.Image = GeometryModeler.DrawAirplane(m_ap, m_currentView, sender.DispSurface.Name, e.Selected_Section);
            }
        }

        public void SetAircraft(Aircraft ap)
        {
            if (ap == null)
                return;

            //if the displayed aircraft is the one we want to change to, dont bother changing
            if (m_ap != null && m_ap == ap)
                return;

            this.SuspendLayout();
            this.flowLayoutPanel1.Controls.Clear();

            m_ap = ap;
            this.designLabel.Text = m_ap.Configuration_Name;
            //this.pictureBox1.Image = m_ap.PrimaryInstance.Geometry_Image;
            this.pictureBox1.Image = GeometryModeler.DrawAirplane(m_ap, GeometryModeler.Views.Isomeric);

            RefreshValues();

            foreach (AVL_File.Surface surf in ap.Initial_AVL_File.Surfaces)
            {
                SurfaceUC suc = new SurfaceUC(surf);
                this.flowLayoutPanel1.Controls.Add(suc);
            }

            this.cmdPrmpButton.Visible = true;

            foreach (Control c in flowLayoutPanel2.Controls)
                c.Visible = true;

            this.revertButton.Enabled = m_ap.HasBackup;

            this.ResumeLayout();
        }

        public void RefreshValues()
        {
            this.srefTextBox.TextBoxText = m_ap.Initial_AVL_File.Sref.ToString();
            this.crefTextBox.TextBoxText = m_ap.Initial_AVL_File.Cref.ToString();
            this.brefTextBox.TextBoxText = m_ap.Initial_AVL_File.Bref.ToString();

            this.xrefTextBox.TextBoxText = m_ap.Initial_AVL_File.Xref.ToString();
            this.yrefTextBox.TextBoxText = m_ap.Initial_AVL_File.Yref.ToString();
            this.zrefTextBox.TextBoxText = m_ap.Initial_AVL_File.Zref.ToString();

            this.cdpTextBox.TextBoxText = m_ap.Initial_AVL_File.CDp.ToString();
            this.machTextBox.TextBoxText = m_ap.Initial_AVL_File.Mach.ToString();
        }

        private void Clear()
        {
            if (this.IsDisposed)
                return;

            this.SuspendLayout();
            m_ap = null;
            this.flowLayoutPanel1.Controls.Clear();

            this.pictureBox1.Image = null;
            this.designLabel.Text = String.Empty;
            this.srefTextBox.TextBoxText = String.Empty;
            this.crefTextBox.TextBoxText = String.Empty;
            this.brefTextBox.TextBoxText = String.Empty;

            this.xrefTextBox.TextBoxText = String.Empty;
            this.yrefTextBox.TextBoxText = String.Empty;
            this.zrefTextBox.TextBoxText = String.Empty;

            this.cdpTextBox.TextBoxText = String.Empty;
            this.machTextBox.TextBoxText = String.Empty;


            foreach (Control c in flowLayoutPanel2.Controls)
                c.Visible = false;

            this.cmdPrmpButton.Visible = false;

            this.ResumeLayout();
        }

        private void AVL_Instance_OnUpdateMessages(AVL_Instance sender, AVLEventArgs e)
        {
            if (this.InvokeRequired)
                this.Invoke((MethodInvoker)delegate { AVL_Instance_OnUpdateMessages(sender, e); });
            else
            {
                //if the AVL_Instance's airplane is our airplane
                if (sender.AVL_aircraft == m_ap)
                {
                    //if this is a geom pict update
                    // if (e.DataSource == AVLEventArgs.AVLData_Source.Geometry_Pict)
                    //this.pictureBox1.Image = ((AVL_PictureEventArgs)e).Img;
                }
            }
        }

        //when we add a surfaceUC to the flow panel, set it's width to be that of the flowlayoutpanel
        private void flowLayoutPanel1_ControlAdded(object sender, ControlEventArgs e)
        {
            e.Control.Width = flowLayoutPanel1.Width;
            this.pictureBox1.Image = GeometryModeler.DrawAirplane(m_ap, GeometryModeler.Views.Isomeric);
        }

        //when we remove a usercontrol from the flowlayout, it usually means we have removed a surface. Regenerate the aircraft's picture
        private void flowLayoutPanel1_ControlRemoved(object sender, ControlEventArgs e)
        {
            if (m_ap != null)
                this.pictureBox1.Image = GeometryModeler.DrawAirplane(m_ap, m_currentView);
        }

        //When the main flowlayoutpanel is resized, resize all children controls so they fill the width
        private void flowLayoutPanel1_Resize(object sender, EventArgs e)
        {
            foreach (Control c in flowLayoutPanel1.Controls)
                c.Width = flowLayoutPanel1.Width;
        }

        private void cmdPrmptButton_Click(object sender, EventArgs e)
        {
            if (m_ap == null || m_ap.PrimaryInstance == null)
                return;

            CommandWindow cw = new CommandWindow(m_ap.PrimaryInstance);
            cw.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Title = "AVL Postscript File";
            fd.Multiselect = false;
            fd.Filter = "Postscript (*.ps)|*.ps";
            fd.InitialDirectory = Properties.Settings.Default.AVL_Location;

            if (fd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (!fd.CheckFileExists)
                    return;

                PostScript_Interp psi = new PostScript_Interp();
                this.pictureBox1.Image = psi.Load(fd.FileName);
                psi = null;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            PictureForm pf = new PictureForm();
            pf.LoadPicture(this.pictureBox1.Image);
            pf.Show();
        }

        private void isoButton_Click(object sender, EventArgs e)
        {
            if (m_ap != null)
                this.pictureBox1.Image = GeometryModeler.DrawAirplane(m_ap, GeometryModeler.Views.Isomeric, selected_surf, selected_section);
            m_currentView = GeometryModeler.Views.Isomeric;
        }

        private void topButton_Click(object sender, EventArgs e)
        {
            if (m_ap != null)
                this.pictureBox1.Image = GeometryModeler.DrawAirplane(m_ap, GeometryModeler.Views.Planform, selected_surf, selected_section);
            m_currentView = GeometryModeler.Views.Planform;
        }

        private void sideButton_Click(object sender, EventArgs e)
        {
            if (m_ap != null)
                this.pictureBox1.Image = GeometryModeler.DrawAirplane(m_ap, GeometryModeler.Views.Side, selected_surf, selected_section);
            m_currentView = GeometryModeler.Views.Side;
        }

        private void frontButton_Click(object sender, EventArgs e)
        {
            if (m_ap != null)
                this.pictureBox1.Image = GeometryModeler.DrawAirplane(m_ap, GeometryModeler.Views.Front, selected_surf, selected_section);
            m_currentView = GeometryModeler.Views.Front;
        }

        private void rotateRightButton_Click(object sender, EventArgs e)
        {
            this.pictureBox1.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
            this.pictureBox1.Refresh();
        }

        private void rotateLeftButton_Click(object sender, EventArgs e)
        {
            this.pictureBox1.Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
            this.pictureBox1.Refresh();
        }

        private void saveDesignButton_Click(object sender, EventArgs e)
        {
            if (m_ap != null)
                m_ap.SaveFile();
        }

        private void revertButton_Click(object sender, EventArgs e)
        {
            if (m_ap != null)
                m_ap.RevertToBackup();
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            if (m_ap != null)
                m_ap.Close();
        }

        private void addSurfButton_Click(object sender, EventArgs e)
        {
            InputBox ib = new InputBox("Create a new surface for this aircraft", "Enter name for the new lifting surface:");
            if (ib.ShowDialog() == DialogResult.OK)
            {
                if (ib.InputText != string.Empty)
                {
                    AVL_File.Surface surf = new AVL_File.Surface(ib.InputText, m_ap.Initial_AVL_File);
                    AVL_File.Surface.Section sec1 = new AVL_File.Surface.Section(surf);
                    sec1.Chord = 10;
                    AVL_File.Surface.Section sec2 = new AVL_File.Surface.Section(surf);
                    sec2.Chord = 10;
                    sec2.Y_LeadingEdge = 10;

                    surf.Sections.Add(sec1);
                    surf.Sections.Add(sec2);

                    m_ap.Initial_AVL_File.Surfaces.Add(surf);

                    SurfaceUC suc = new SurfaceUC(surf);
                    this.flowLayoutPanel1.Controls.Add(suc);
                }
            }
        }
        #region textbox handlers
        private void srefTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.') && (e.KeyChar != '-'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as RibbonTextBox).TextBoxText.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void srefTextBox_TextChanged(object sender, EventArgs e)
        {
            if (m_ap != null)
                double.TryParse(srefTextBox.TextBoxText, out m_ap.Initial_AVL_File.Sref);
        }

        private void crefTextBox_TextAlignChanged(object sender, EventArgs e)
        {
            if (m_ap != null)
                double.TryParse(crefTextBox.TextBoxText, out m_ap.Initial_AVL_File.Cref);
        }

        private void brefTextBox_TextChanged(object sender, EventArgs e)
        {
            if (m_ap != null)
                double.TryParse(brefTextBox.TextBoxText, out m_ap.Initial_AVL_File.Bref);
        }

        private void machTextBox_TextChanged(object sender, EventArgs e)
        {
            if (m_ap != null)
                double.TryParse(machTextBox.TextBoxText, out m_ap.Initial_AVL_File.Mach);
        }

        private void xrefTextBox_TextChanged(object sender, EventArgs e)
        {
            if (m_ap != null)
                double.TryParse(xrefTextBox.TextBoxText, out m_ap.Initial_AVL_File.Xref);
        }

        private void yrefTextBox_TextChanged(object sender, EventArgs e)
        {
            if (m_ap != null)
                double.TryParse(yrefTextBox.TextBoxText, out m_ap.Initial_AVL_File.Yref);
        }

        private void zrefTextBox_TextChanged(object sender, EventArgs e)
        {
            if (m_ap != null)
                double.TryParse(zrefTextBox.TextBoxText, out m_ap.Initial_AVL_File.Zref);
        }

        private void cdpTextBox_TextChanged(object sender, EventArgs e)
        {
            if (m_ap != null)
                double.TryParse(cdpTextBox.TextBoxText, out m_ap.Initial_AVL_File.CDp);
        }
        #endregion
    }
}
