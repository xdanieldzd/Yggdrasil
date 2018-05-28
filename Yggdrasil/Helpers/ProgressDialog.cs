using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Microsoft.WindowsAPICodePack.Dialogs;

using Yggdrasil.Forms;

namespace Yggdrasil.Helpers
{
	public class ProgressDialog
	{
		ProgressForm defaultDialog;
		TaskDialog modernDialog;

		public Form ParentForm { get; set; }

		string title, instructionText, text, details;

		public string Title
		{
			get { return title; }
			set
			{
				title = value;
				if (modernDialog != null) modernDialog.Caption = title;
				else if (defaultDialog != null) defaultDialog.Text = title;
			}
		}

		public string InstructionText
		{
			get { return instructionText; }
			set
			{
				instructionText = value;
				if (modernDialog != null) modernDialog.InstructionText = instructionText;
				else if (defaultDialog != null) defaultDialog.lblInstructionText.Text = string.Format("{1}{0}{0}{2}", Environment.NewLine, instructionText, text);
			}
		}

		public string Text
		{
			get { return text; }
			set
			{
				text = value;
				if (modernDialog != null) modernDialog.Text = text;
				else if (defaultDialog != null) defaultDialog.lblInstructionText.Text = string.Format("{1}{0}{0}{2}", Environment.NewLine, instructionText, text);
			}
		}

		public string Details
		{
			get { return details; }
			set
			{
				details = value;
				if (defaultDialog != null) defaultDialog.lblDetailText.Text = details;
			}
		}

		public bool Cancel { get; set; }

		public ProgressDialog()
		{
			title = instructionText = text = details = "-";
		}

		public void Show(Form parentForm, bool forceClassic = false)
		{
			if (TaskDialog.IsPlatformSupported && !forceClassic)
			{
				modernDialog = new TaskDialog
				{
					OwnerWindowHandle = parentForm.Handle,
					Cancelable = true,
					StartupLocation = TaskDialogStartupLocation.CenterOwner,
					StandardButtons = TaskDialogStandardButtons.Cancel,
					ExpansionMode = TaskDialogExpandedDetailsLocation.ExpandFooter,

					Icon = TaskDialogStandardIcon.Information,
					Caption = title,
					InstructionText = instructionText,
					Text = text,
					DetailsExpandedText = details,

					ProgressBar = new TaskDialogProgressBar() { State = TaskDialogProgressBarState.Marquee }
				};

				modernDialog.Opened += ((s, e) =>
				{
					TaskDialog taskDialog = (s as TaskDialog);
					taskDialog.Icon = taskDialog.Icon;
					taskDialog.InstructionText = taskDialog.InstructionText;
				});
				modernDialog.Tick += ((s, e) =>
				{
					TaskDialog taskDialog = (s as TaskDialog);
					taskDialog.DetailsExpandedText = details;
				});

				modernDialog.Closing += ((s, e) =>
				{
					Cancel = (e.TaskDialogResult == TaskDialogResult.Cancel);
				});

				modernDialog.Show();
			}
			else
			{
				defaultDialog = new ProgressForm(this);
				defaultDialog.ShowDialog(parentForm);
			}
		}

		public void Close()
		{
			try
			{
				if (TaskDialog.IsPlatformSupported && modernDialog != null)
					modernDialog.Close();
				else if (defaultDialog != null)
					defaultDialog.Close();
			}
			catch (InvalidOperationException)
			{
				modernDialog.Dispose();
			}
		}
	}
}
