using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace AVL_Interface
{
    public partial class PictureForm : Form
    {
        public PictureForm()
        {
            InitializeComponent();
        }

        public void LoadPicture(Image img)
        {
            this.pictureBox1.Image = img;
        }

        private void saveImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var SD = new SaveFileDialog();
            SD.Title = "Save Image to file";
            SD.OverwritePrompt = true;
            SD.Filter = "PNG (*.png)|*.png|JPEG (*.jpg)|*.jpg";
            if (SD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (SD.FilterIndex == 1)
                    this.pictureBox1.Image.Save(SD.FileName, System.Drawing.Imaging.ImageFormat.Png);
                else if (SD.FilterIndex == 2)
                    this.pictureBox1.Image.Save(SD.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
        }
    }
}
