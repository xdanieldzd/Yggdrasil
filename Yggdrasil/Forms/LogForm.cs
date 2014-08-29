using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Yggdrasil.Forms
{
    public partial class LogForm : ModernForm
    {
        public LogForm()
        {
            InitializeComponent();
        }

        public LogForm(string log)
        {
            InitializeComponent();
            txtLog.Text = log;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LogForm_Shown(object sender, EventArgs e)
        {
            txtLog.SelectionStart = txtLog.Text.Length;
            txtLog.ScrollToCaret();
            txtLog.Refresh();
        }
    }
}
