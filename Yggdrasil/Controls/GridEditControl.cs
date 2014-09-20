using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;

namespace Yggdrasil.Controls
{
    class GridEditControl : Panel
    {
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;   // WS_EX_COMPOSITED
                return cp;
            }
        }

        [DefaultValue(typeof(Color), "Black")]
        public override Color BackColor
        {
            get { return base.BackColor; }
            set { base.BackColor = value; }
        }

        int columns;
        [DefaultValue(8)]
        public int Columns { get { return columns; } set { columns = value; this.UpdateDisplay(); } }

        int rows;
        [DefaultValue(8)]
        public int Rows { get { return rows; } set { rows = value; this.UpdateDisplay(); } }

        Size tileSize;
        [DefaultValue(typeof(Size), "8, 8")]
        public Size TileSize { get { return tileSize; } set { tileSize = value; this.UpdateDisplay(); } }

        int zoom;
        [DefaultValue(1), NotifyParentProperty(true), RefreshProperties(System.ComponentModel.RefreshProperties.Repaint)]
        public int Zoom { get { return zoom; } set { zoom = value; this.UpdateDisplay(); } }

        bool showGrid;
        [DefaultValue(false)]
        public bool ShowGrid { get { return showGrid; } set { showGrid = value; this.UpdateDisplay(); } }

        protected override Size DefaultSize
        {
            get { return new Size(this.zoom * this.tileSize.Width, this.zoom * this.tileSize.Height); }
        }

        public override Size MinimumSize
        {
            get { return DefaultSize; }
        }

        [DefaultValue(true), Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new bool AutoSize
        {
            get { return true; }
        }

        [DefaultValue(typeof(System.Windows.Forms.AutoSizeMode), "GrowAndShrink"), Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new AutoSizeMode AutoSizeMode
        {
            get { return System.Windows.Forms.AutoSizeMode.GrowAndShrink; }
        }

        Size zoomedTileSize;
        Point hoverTileCoords, selectedTileCoords;
        Rectangle selectionRect;

        public GridEditControl()
        {
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw, true);

            this.BackColor = Color.Black;

            this.columns = 8;
            this.rows = 8;
            this.tileSize = new Size(8, 8);
            this.zoom = 1;
            this.showGrid = false;

            this.zoomedTileSize = Size.Empty;
            this.hoverTileCoords = Point.Empty;
            this.selectedTileCoords = Point.Empty;
            this.selectionRect = Rectangle.Empty;
        }

        private void UpdateDisplay()
        {
            this.zoomedTileSize = new Size(this.tileSize.Width * this.zoom, this.tileSize.Height * this.zoom);
            this.Invalidate();
            this.OnResize(EventArgs.Empty);
        }

        protected override void OnResize(EventArgs eventargs)
        {
            Size newSize = new Size(this.Columns * this.zoomedTileSize.Width, this.Rows * this.zoomedTileSize.Height);
            if (this.Size != newSize)
            {
                this.Size = newSize;
                base.OnResize(eventargs);
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            hoverTileCoords = new Point((e.X / this.zoomedTileSize.Width), (e.Y / this.zoomedTileSize.Height));
            selectionRect = new Rectangle(new Point(hoverTileCoords.X * this.zoomedTileSize.Width, hoverTileCoords.Y * this.zoomedTileSize.Height), zoomedTileSize);
            this.Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            selectedTileCoords = new Point((e.X / this.zoomedTileSize.Width), (e.Y / this.zoomedTileSize.Height));
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (showGrid)
            {
                using (Pen pen = new Pen(Color.FromArgb(128, Color.Gray)))
                {
                    e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                    e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                    e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
                    for (int x = 0; x < this.columns; x++)
                    {
                        for (int y = 0; y < this.rows; y++)
                        {
                            e.Graphics.DrawRectangle(pen, (x * this.zoomedTileSize.Width), (y * this.zoomedTileSize.Height), this.zoomedTileSize.Width, this.zoomedTileSize.Height);
                        }
                    }
                }
            }

            e.Graphics.DrawRectangle(Pens.Red, selectionRect);
            e.Graphics.DrawLine(Pens.Red, Point.Empty, selectionRect.Location);
            e.Graphics.DrawString(selectedTileCoords.ToString(), SystemFonts.MessageBoxFont, Brushes.Yellow, Point.Empty);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            e.Graphics.Clear(this.BackColor);
        }
    }
}
