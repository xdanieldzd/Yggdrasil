using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Yggdrasil
{
    static class Program
    {
        public static MainForm MainForm = null;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(MainForm = new MainForm());
        }
    }
}
