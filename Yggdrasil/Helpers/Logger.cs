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
        public enum Level { Info = 0, Warning = 1, Error = 2 };

        List<Tuple<DateTime, Level, string>> logEntries;

        public Logger()
        {
            logEntries = new List<Tuple<DateTime, Level, string>>();
        }

        public void LogMessage(string message, params object[] parameters)
        {
            LogMessage(false, Level.Info, message, parameters);
        }

        public void LogMessage(Level level, string message, params object[] parameters)
        {
            LogMessage(false, level, message, parameters);
        }

        public void LogMessage(bool sendToMain, string message, params object[] parameters)
        {
            LogMessage(sendToMain, Level.Info, message, parameters);
        }

        public void LogMessage(bool sendToMain, Level level, string message, params object[] parameters)
        {
            string formatted = string.Format(System.Globalization.CultureInfo.InvariantCulture, message, parameters);
            logEntries.Add(new Tuple<DateTime, Level, string>(DateTime.Now, level, formatted));
            if (sendToMain) Program.MainForm.StatusText = formatted;
        }

        public DialogResult ShowDialog()
        {
            return new LogForm(logEntries).ShowDialog();
        }
    }
}
