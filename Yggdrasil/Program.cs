using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using Yggdrasil.Helpers;

namespace Yggdrasil
{
    static class Program
    {
        public static string TitleString = string.Format("{0} {1}", Application.ProductName, VersionManagement.CreateVersionString(Application.ProductVersion));

        public static MainForm MainForm = null;
        public static Logger Logger = null;

        [STAThread]
        static void Main()
        {
            Logger = new Logger();
            Logger.LogMessage("{0} starting up...", TitleString);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //Yggdrasil.DataCompression.RLEStream.Test();
            //Yggdrasil.DataCompression.LZ77Stream.Test();

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

            // temporary stupid "sort-of auto-generate tableparser property code" code
            /*using (System.IO.StreamWriter w = System.IO.File.CreateText(@"C:\temp\__eo-codetemp.txt"))
            {
                System.Reflection.FieldInfo[] fis = typeof(Yggdrasil.FileHandling.MapDataHandling.TransporterTile).GetFields(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                foreach (System.Reflection.FieldInfo fi in fis.Where(x => x.FieldType != typeof(Dictionary<string, object>)))
                {
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();

                    string format =
                        "{0} {1};" + Environment.NewLine +
                        "public {0} {2}" + Environment.NewLine +
                        "{{" + Environment.NewLine +
                        "   get {{ return {1}; }}" + Environment.NewLine +
                        "   set {{ base.SetProperty(ref {1}, value, () => this.{2}); }}" + Environment.NewLine +
                        "}}" + Environment.NewLine +
                        "public bool ShouldSerialize{2}() {{ return !(this.{2} == (dynamic)base.originalValues[\"{2}\"]); }}" + Environment.NewLine +
                        "public void Reset{2}() {{ this.{2} = (dynamic)base.originalValues[\"{2}\"]; }}" + Environment.NewLine;

                    sb.AppendFormat(format, fi.FieldType.Name, fi.Name, fi.Name.First().ToString().ToUpper() + String.Join("", fi.Name.Skip(1)));

                    w.WriteLine(sb.ToString());
                }
            }*/

            Application.Run(MainForm = new MainForm());
        }
    }
}
