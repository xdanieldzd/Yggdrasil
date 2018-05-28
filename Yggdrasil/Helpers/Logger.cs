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
		public enum Level { Info = 0, Warning = 1, Error = 2, Debug = 3 };

		public sealed class LogEntry
		{
			public DateTime Timestamp { get; private set; }
			public Level Level { get; private set; }
			public string Message { get; private set; }

			public LogEntry(DateTime timestamp, Level level, string message)
			{
				Timestamp = timestamp;
				Level = level;
				Message = message;
			}
		}

		List<LogEntry> logEntries;

		public Logger()
		{
			logEntries = new List<LogEntry>();
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
			logEntries.Add(new LogEntry(DateTime.Now, level, formatted));
			if (sendToMain) Program.MainForm.StatusText = formatted;
		}

		public DialogResult ShowDialog()
		{
			return new LogForm(logEntries).ShowDialog();
		}
	}
}
