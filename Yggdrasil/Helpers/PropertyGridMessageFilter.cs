using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Yggdrasil.Helpers
{
    public class PropertyGridMessageFilter : IMessageFilter
    {
        public Control Control;
        public MouseEventHandler MouseEvent;

        public PropertyGridMessageFilter(Control control, MouseEventHandler mouseEventHandler)
        {
            this.Control = control;
            this.MouseEvent = mouseEventHandler;
        }

        public bool PreFilterMessage(ref Message m)
        {
            if (!this.Control.IsDisposed && m.HWnd == this.Control.Handle && MouseEvent != null)
            {
                MouseButtons mouseButton = MouseButtons.None;

                switch (m.Msg)
                {
                    case 0x0202: mouseButton = MouseButtons.Left; break;
                    case 0x0205: mouseButton = MouseButtons.Right; break;
                    case 0x0208: mouseButton = MouseButtons.Middle; break;
                }

                if (mouseButton != MouseButtons.None)
                    MouseEvent(Control, new MouseEventArgs(mouseButton, 1, m.LParam.ToInt32() & 0xFFff, m.LParam.ToInt32() >> 16, 0));
            }
            return false;
        }
    }
}
