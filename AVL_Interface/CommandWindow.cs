using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace AVL_Interface
{
    public partial class CommandWindow : Form
    {
        AVL_Instance m_inst;

        public CommandWindow(AVL_Instance instance)
        {
            if (instance == null)
                return;
            m_inst = instance;
            InitializeComponent();

            m_inst.AVL_Process.OutputDataReceived += new System.Diagnostics.DataReceivedEventHandler(AVL_process_OutputDataReceived);

            configLabel.Text = instance.AVL_aircraft.Configuration_Name;
            avl_instanceLabel.Text = m_inst.Instance_Notes;

        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }
        protected override void OnShown(EventArgs e)
        {
            if (m_inst == null)
                this.Close();
            else
            {
                base.OnShown(e);
                //print the current menu options
                m_inst.Write("?", false);
            }
        }

        private void AVL_process_OutputDataReceived(object sender, System.Diagnostics.DataReceivedEventArgs e)
        {
            if (this.richTextBox1.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate { this.richTextBox1.Text += e.Data + "\r\n"; });
            }
        }

        private void sendButton_Click(object sender, EventArgs e)
        {
            if(m_inst == null )
                return;

            if (!String.IsNullOrEmpty(this.inputTextBox.Text))
                m_inst.Write(this.inputTextBox.Text, false);
            else
                m_inst.Write(" ", false);

            this.inputTextBox.Text = "";
        }

        private void clearMenuItem_Click(object sender, EventArgs e)
        {
            this.richTextBox1.Text = "";
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            richTextBox1.SelectionStart = richTextBox1.Text.Length; //Set the current caret position at the end
            richTextBox1.ScrollToCaret(); //Now scroll it automatically
        }
    }
}
