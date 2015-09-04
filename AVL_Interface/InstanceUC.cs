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
    public partial class InstanceUC : UserControl
    {
#region events
        public class SelectedChangedEventArgs : EventArgs
        {
            public bool Selected;

            public SelectedChangedEventArgs(bool selected)
            {
                Selected = selected;
            }
        }

        public delegate void SelectedEventHandler(InstanceUC sender, SelectedChangedEventArgs e);
        public event SelectedEventHandler OnSelected = null;

        private void OnSelectedChanged(InstanceUC inst, SelectedChangedEventArgs e)
        {
            if (OnSelected != null)
                OnSelected(inst, e);
        }
#endregion
        public Aircraft aircraft { get; private set; }

        private bool m_selected = false;

        public bool Selected
        {
            get { return m_selected; }
            set
            {
                if (Selectable)
                {
                    m_selected = value;
                    if (m_selected && this.BackColor != SystemColors.ActiveCaption)
                        this.BackColor = SystemColors.ActiveCaption;
                    else
                        this.BackColor = SystemColors.Window;
                    OnSelectedChanged(this, new SelectedChangedEventArgs(m_selected));
                }
            }
        }

        [Browsable(true)]
        public bool Selectable { get; set; }

        [Browsable(true)]
        public bool MultiSelect { get; set; }

        public InstanceUC(Aircraft instance)
        {
            aircraft = instance;
            InitializeComponent();
            this.configLabel.Text = aircraft.Configuration_Name;
            if (aircraft.PrimaryInstance != null)
                this.pictureBox1.Image = aircraft.PrimaryInstance.Geometry_Image;
            AVL_Instance.OnUpdateMessages += new MessageEventHandler(AVL_Instance_OnUpdateMessages);
        }

        private void AVL_Instance_OnUpdateMessages(AVL_Instance sender, AVLEventArgs e)
        {
            if (this.InvokeRequired)
                this.Invoke((MethodInvoker)delegate { AVL_Instance_OnUpdateMessages(sender, e);});
            else
            {
                //if the AVL_Instance's airplane is our airplane
                if (sender.AVL_aircraft == aircraft)
                {
                    //if this is a geom pict update
                    if (e.DataSource == AVLEventArgs.AVLData_Source.Geometry_Pict)
                        this.pictureBox1.Image = ((AVL_PictureEventArgs)e).Img;
                }
            }
        }

        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            PictureForm pf = new PictureForm();
            pf.LoadPicture(this.pictureBox1.Image);
            pf.Show();
        }

        private void InstanceUC_Click(object sender, EventArgs e)
        {
            if (Selectable)
                Selected = !Selected;

            if (!MultiSelect && Selected)
            {
                if (this.Parent != null)
                {
                    foreach (Control c in this.Parent.Controls)
                        if (c.GetType() == typeof(InstanceUC) && c != this)
                            (c as InstanceUC).Selected = false;
                }
            }

            this.OnClick(e);
        }

        private void pictureBox1_Resize(object sender, EventArgs e)
        {
            this.SuspendLayout();
            if (pictureBox1.Image != null)
                this.pictureBox1.Height = (int)((double)pictureBox1.Image.Height * ((double)pictureBox1.Width / (double)pictureBox1.Image.Width));
            this.ResumeLayout();
        }

        private void InstanceUC_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                this.DoDragDrop(this.aircraft, DragDropEffects.Copy | DragDropEffects.Move);
            }
        }
    }
}
