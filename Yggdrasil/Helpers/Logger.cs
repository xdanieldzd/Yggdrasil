using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Yggdrasil.Forms;

namespace Yggdrasil.Helpers
{
    public class Logger
    {
        List<Tuple<DateTime, string>> logEntries;

        public Logger()
        {
            logEntries = new List<Tuple<DateTime, string>>();
        }

        public void LogMessage(string message, params object[] parameters)
        {
            LogMessage(false, message, parameters);
        }

        public void LogMessage(bool sendToMain, string message, params object[] parameters)
        {
            string formatted = string.Format(System.Globalization.CultureInfo.InvariantCulture, message, parameters);
            logEntries.Add(new Tuple<DateTime, string>(DateTime.Now, formatted));
            if (sendToMain) Program.MainForm.StatusText = formatted;
        }

        public DialogResult ShowDialog()
        {
            StringBuilder builder = new StringBuilder();
            foreach (Tuple<DateTime, string> entry in logEntries)
                builder.AppendLine(string.Format(System.Globalization.CultureInfo.InvariantCulture, "[{0}] {1}", entry.Item1, entry.Item2));

            return new LogForm(builder.ToString()).ShowDialog();
        }
    }
}
