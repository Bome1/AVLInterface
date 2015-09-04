using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace AVL_Interface
{
    public partial class RawDataForm : Form
    {
        public RawDataForm()
        {
            InitializeComponent();
        }

        public RawDataForm(string config_name, string data_source, string data):this()
        {
            configLabel.Text = config_name;
            datasourceLabel.Text = data_source;
            richTextBox1.Text = data;
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            var sfd = new SaveFileDialog();
            sfd.Title = "Save " + datasourceLabel.Text + " to a file";
            sfd.Filter = "Text Files (*.txt)|*.txt";
            sfd.InitialDirectory = Properties.Settings.Default.AVL_Location;
            sfd.FileName = configLabel.Text + "_" + datasourceLabel.Text + ".txt";
            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                using (StreamWriter sw = new StreamWriter(sfd.FileName, false))
                {
                    sw.Write(richTextBox1.Text);
                    sw.Flush();
                    sw.Close();
                }
            }
        }
    }
}
