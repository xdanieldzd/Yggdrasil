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
	public partial class ProgressForm : ModernForm
	{
		ProgressDialog dialogData;

		public ProgressForm(ProgressDialog dialogData)
		{
			InitializeComponent();

			this.dialogData = dialogData;
			this.Text = this.dialogData.Title;
			lblInstructionText.Text = string.Format("{1}{0}{2}", Environment.NewLine, this.dialogData.InstructionText, this.dialogData.Text);
			lblDetailText.Text = this.dialogData.Details;
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			dialogData.Cancel = true;
			this.Close();
		}
	}
}
