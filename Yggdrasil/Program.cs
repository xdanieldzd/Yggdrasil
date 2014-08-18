using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using Yggdrasil.Helpers;

namespace Yggdrasil
{
    static class Program
    {
        public static MainForm MainForm = null;
        public static Logger Logger = null;

        [STAThread]
        static void Main()
        {
            Logger = new Logger();
            Logger.LogMessage("{0} {1} starting up...", Application.ProductName, VersionManagement.CreateVersionString(Application.ProductVersion));

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(MainForm = new MainForm());
        }
    }
}
