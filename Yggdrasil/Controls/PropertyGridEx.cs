using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Drawing;
using System.ComponentModel;
using System.Runtime.InteropServices;

using Yggdrasil.Helpers;

namespace Yggdrasil.Controls
{
    public class PropertyGridEx : PropertyGrid
    {
        int autoSizeColumnMargin;
        [DefaultValue(32)]
        public int AutoSizeColumnMargin
        {
            get { return autoSizeColumnMargin; }
            set
            {
                autoSizeColumnMargin = value;
                if (autoSizeColumns) PerformColumnAutoSize();
            }
        }

        bool autoSizeColumns;
        [DefaultValue(false)]
        public bool AutoSizeColumns
        {
            get { return autoSizeColumns; }
            set { if (autoSizeColumns = value) PerformColumnAutoSize(); }
        }

        object gridView;
        FieldInfo gridViewEntries;

        bool isRuntime;
        bool drawingSuspended;

        public PropertyGridEx()
            : base()
        {
            isRuntime = (LicenseManager.UsageMode != LicenseUsageMode.Designtime);

            autoSizeColumnMargin = 32;
            autoSizeColumns = false;

            gridView = this.GetType().BaseType.InvokeMember("gridView", BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance, null, this, null);
            if (gridView != null) gridViewEntries = gridView.GetType().GetField("allGridEntries", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        }

        protected override void WndProc(ref Message m)
        {
            if (drawingSuspended && m.Msg == WinAPI.WM_ERASEBKGND) return;
            base.WndProc(ref m);
        }

        public override void Refresh()
        {
            WinAPI.SuspendDrawing(gridView as Control, ref drawingSuspended);
            base.Refresh();
            if (AutoSizeColumns) PerformColumnAutoSize();
            WinAPI.ResumeDrawing(gridView as Control, ref drawingSuspended);
        }

        protected override void OnResize(EventArgs e)
        {
            WinAPI.SuspendDrawing(gridView as Control, ref drawingSuspended);
            base.OnResize(e);
            if (AutoSizeColumns) PerformColumnAutoSize();
            WinAPI.ResumeDrawing(gridView as Control, ref drawingSuspended);
        }

        protected void PerformColumnAutoSize()
        {
            if (!isRuntime) return;

            GridItemCollection gridItems = (System.Windows.Forms.GridItemCollection)gridViewEntries.GetValue(gridView);
            if (gridItems == null) return;

            int splitterDist = 0;
            using (Graphics g = Graphics.FromHwnd(this.Handle))
            {
                int curWidth = 0;
                foreach (GridItem gridItem in gridItems)
                {
                    if (gridItem.GridItemType == GridItemType.Property)
                    {
                        curWidth = (int)g.MeasureString(gridItem.Label, this.Font).Width + autoSizeColumnMargin;
                        if (curWidth > splitterDist) splitterDist = curWidth;
                    }
                }
            }

            gridView.GetType().InvokeMember("MoveSplitterTo", BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Instance, null, gridView, new object[] { splitterDist });
        }
    }
}
