using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Yggdrasil.Forms
{
	public class ModernForm : Form
	{
		protected override void OnCreateControl()
		{
			base.OnCreateControl();

			foreach (Control control in this.Controls)
			{
				if (control.Font == Form.DefaultFont) control.Font = SystemFonts.MessageBoxFont;
			}
		}
	}
}
