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

        public static DialogResult ShowFolderBrowser(string title, string description, string initialPath, out string selectedPath)
        {
            if (CommonOpenFileDialog.IsPlatformSupported)
            {
                CommonOpenFileDialog ofd = new CommonOpenFileDialog();
                ofd.IsFolderPicker = true;
                ofd.InitialDirectory = initialPath;
                ofd.Title = title;
                if (ofd.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    selectedPath = ofd.FileName;
                    return DialogResult.OK;
                }
                else
                {
                    selectedPath = string.Empty;
                    return DialogResult.Cancel;
                }
            }
            else
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                fbd.Description = description;
                fbd.SelectedPath = initialPath;
                selectedPath = fbd.SelectedPath;
                return fbd.ShowDialog();
            }
        }
    }
}
