using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AVL_Interface
{
    public partial class InputBox : Form
    {
        public string InputText { get { return textBox1.Text; } }

        public InputBox()
        {
            InitializeComponent();
            this.ActiveControl = textBox1;
        }

        public InputBox(string title, string label):this()
        {
            this.Text = title;
            this.label1.Text = label;
        }

        private void acceptButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }
    }
}
