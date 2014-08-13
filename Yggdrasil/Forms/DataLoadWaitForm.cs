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
    public partial class DataLoadWaitForm : Form
    {
        public DataLoadWaitForm()
        {
            InitializeComponent();
        }

        public void PrintStatus(string status)
        {
            lblStatus.Text = status;
        }
    }
}
