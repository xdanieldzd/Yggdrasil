using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Yggdrasil.Helpers;

namespace Yggdrasil.Forms
{
    public partial class LogForm : ModernForm
    {
        bool isInitialized;
        List<Tuple<DateTime, Logger.Level, string>> logEntries;

        public LogForm()
        {
            isInitialized = false;

            InitializeComponent();

            cmbLogLevel.DataSource = Enum.GetValues(typeof(Logger.Level));
            cmbLogLevel.SelectedItem = Configuration.LogLevel;

            isInitialized = true;
        }

        public LogForm(List<Tuple<DateTime, Logger.Level, string>> logEntries)
            : this()
        {
            this.logEntries = logEntries;
            txtLog.Text = BuildLog(Configuration.LogLevel);
        }

        private string BuildLog(Logger.Level level)
        {
            if (logEntries == null) return string.Empty;

            StringBuilder builder = new StringBuilder();
            foreach (Tuple<DateTime, Logger.Level, string> entry in logEntries.Where(x => x.Item2 <= level))
            {
                if (entry.Item2 != Logger.Level.Info)
                    builder.AppendLine(string.Format(System.Globalization.CultureInfo.InvariantCulture, "[{0}] {1}: {2}", entry.Item1, entry.Item2.ToString().ToUpperInvariant(), entry.Item3));
                else
                    builder.AppendLine(string.Format(System.Globalization.CultureInfo.InvariantCulture, "[{0}] {1}", entry.Item1, entry.Item3));
            }

            return builder.ToString();
        }

        private void RefreshTextBox()
        {
            txtLog.SelectionStart = txtLog.Text.Length;
            txtLog.ScrollToCaret();
            txtLog.Refresh();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LogForm_Shown(object sender, EventArgs e)
        {
            RefreshTextBox();
        }

        private void cmbLogLevel_SelectedValueChanged(object sender, EventArgs e)
        {
            if (isInitialized) Configuration.LogLevel = (Logger.Level)((sender as ComboBox).SelectedItem);

            txtLog.Text = BuildLog(Configuration.LogLevel);
            RefreshTextBox();
        }
    }
}
