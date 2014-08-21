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

            // temporary stupid "file where I ident kanji -> file with C#-dictionary-like text" converter
            /*using (System.IO.StreamReader sr = System.IO.File.OpenText(@"C:\Users\Daniel\Desktop\eo-jpn.txt"))
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                int id = 0x12D;
                char ch;
                while ((ch = (char)sr.Read()) != '哀') { }

                while (!sr.EndOfStream)
                {
                    if (ch != '＿') sb.AppendFormat("            {{ 0x{0:X4}, '{1}' }},{2}", id, ch, Environment.NewLine);
                    id++;
                    ch = (char)sr.Read();
                    if (ch == '\r')
                    {
                        for (int i = 0; i < 9; i++) ch = (char)sr.Read();
                    }
                }

                System.IO.File.WriteAllText(@"C:\temp\EOJPN-TABLE.txt", sb.ToString());
            }
            */
            Application.Run(MainForm = new MainForm());
        }
    }
}
