using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;

using Microsoft.WindowsAPICodePack.Dialogs;

namespace Yggdrasil.Helpers
{
    public static class GUIHelpers
    {
        public static void ShowErrorMessage(string caption, string instructionText, string text)
        {
            ShowErrorMessage(caption, instructionText, text, string.Empty, string.Empty, IntPtr.Zero);
        }

        public static void ShowErrorMessage(string caption, string instructionText, string text, string footerText)
        {
            ShowErrorMessage(caption, instructionText, text, footerText, string.Empty, IntPtr.Zero);
        }

        public static void ShowErrorMessage(string caption, string instructionText, string text, string footerText, string details)
        {
            ShowErrorMessage(caption, instructionText, text, footerText, details, IntPtr.Zero);
        }

        public static void ShowErrorMessage(string caption, string instructionText, string text, IntPtr ownerHandle)
        {
            ShowErrorMessage(caption, instructionText, text, string.Empty, string.Empty, ownerHandle);
        }

        public static void ShowErrorMessage(string caption, string instructionText, string text, string footerText, IntPtr ownerHandle)
        {
            ShowErrorMessage(caption, instructionText, text, footerText, string.Empty, ownerHandle);
        }

        public static void ShowErrorMessage(string caption, string instructionText, string text, string footerText, string details, IntPtr ownerHandle)
        {
            if (TaskDialog.IsPlatformSupported)
            {
                TaskDialog dialog = new TaskDialog();
                dialog.OwnerWindowHandle = ownerHandle;

                dialog.DetailsExpanded = false;
                dialog.Cancelable = false;
                dialog.Icon = TaskDialogStandardIcon.Error;
                dialog.FooterIcon = TaskDialogStandardIcon.Error;
                dialog.ExpansionMode = TaskDialogExpandedDetailsLocation.ExpandFooter;

                dialog.Caption = caption;
                dialog.InstructionText = instructionText;
                dialog.Text = text;
                dialog.DetailsExpandedText = details;
                dialog.FooterText = footerText;

                dialog.Opened += ((s, e) =>
                {
                    TaskDialog taskDialog = (s as TaskDialog);
                    taskDialog.Icon = taskDialog.Icon;
                    if (dialog.FooterText != string.Empty) taskDialog.FooterIcon = taskDialog.FooterIcon;
                    taskDialog.InstructionText = taskDialog.InstructionText;
                });

                dialog.Show();
            }
            else
            {
                string message = string.Format("{1} {2}{0}{0}{3}", Environment.NewLine, instructionText, footerText, text);
                if (details != string.Empty)
                {
                    message += string.Format("{0}{0}Do you want to see the error details?", Environment.NewLine);
                    if (MessageBox.Show(message, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Error, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        MessageBox.Show(details, "Error Details", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                    MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static DialogResult ShowMessageBox(string message, string title, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            if (TaskDialog.IsPlatformSupported)
            {
                DialogResult result = DialogResult.None;

                TaskDialog dialog = new TaskDialog();
                switch (icon)
                {
                    case MessageBoxIcon.None: dialog.Icon = TaskDialogStandardIcon.None; break;
                    case MessageBoxIcon.Information: dialog.Icon = TaskDialogStandardIcon.Information; break;
                    case MessageBoxIcon.Error: dialog.Icon = TaskDialogStandardIcon.Error; break;
                    case MessageBoxIcon.Warning: dialog.Icon = TaskDialogStandardIcon.Warning; break;
                    default: throw new Exception(string.Format("Unsupported icon {0}", icon));
                }
                switch (buttons)
                {
                    case MessageBoxButtons.OK: dialog.StandardButtons = TaskDialogStandardButtons.Ok; break;
                    case MessageBoxButtons.OKCancel: dialog.StandardButtons = TaskDialogStandardButtons.Ok | TaskDialogStandardButtons.Cancel; break;
                    case MessageBoxButtons.YesNo: dialog.StandardButtons = TaskDialogStandardButtons.Yes | TaskDialogStandardButtons.No; break;
                    case MessageBoxButtons.YesNoCancel: dialog.StandardButtons = TaskDialogStandardButtons.Yes | TaskDialogStandardButtons.No | TaskDialogStandardButtons.Cancel; break;
                    case MessageBoxButtons.RetryCancel: dialog.StandardButtons = TaskDialogStandardButtons.Retry | TaskDialogStandardButtons.Cancel; break;
                    default: throw new Exception(string.Format("Unsupported buttons {0}", buttons));
                }

                dialog.FooterIcon = TaskDialogStandardIcon.None;
                dialog.InstructionText = "";
                dialog.Text = message;
                dialog.Caption = title;

                dialog.Opened += ((s, e) =>
                {
                    TaskDialog taskDialog = (s as TaskDialog);
                    taskDialog.Icon = taskDialog.Icon;
                    taskDialog.InstructionText = taskDialog.InstructionText;
                });

                TaskDialogResult tdResult = dialog.Show();
                switch (tdResult)
                {
                    case TaskDialogResult.Ok: result = DialogResult.OK; break;
                    case TaskDialogResult.Cancel: result = DialogResult.Cancel; break;
                    case TaskDialogResult.Yes: result = DialogResult.Yes; break;
                    case TaskDialogResult.No: result = DialogResult.No; break;
                    case TaskDialogResult.Retry: result = DialogResult.Retry; break;
                    default: throw new Exception(string.Format("Unsupported result {0}", tdResult));
                }

                return result;
            }
            else
                return MessageBox.Show(message, title, buttons, icon);
        }
    }
}
