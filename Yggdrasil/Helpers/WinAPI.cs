using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Yggdrasil.Helpers
{
    public class WinAPI
    {
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, Int32 wMsg, bool wParam, Int32 lParam);

        public const int WM_SETREDRAW = 0x000B;
        public const int WM_ERASEBKGND = 0x0014;

        public static void SuspendDrawing(Control parent, ref bool drawingSuspended)
        {
            drawingSuspended = true;
            SendMessage(parent.Handle, WM_SETREDRAW, false, 0);
        }

        public static void ResumeDrawing(Control parent, ref bool drawingSuspended)
        {
            drawingSuspended = false;
            SendMessage(parent.Handle, WM_SETREDRAW, true, 0);
            parent.Refresh();
        }
    }
}
