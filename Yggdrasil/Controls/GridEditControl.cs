using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;

namespace Yggdrasil.Controls
{
    class TileClickEventArgs : EventArgs
    {
        public MouseButtons Button { get; private set; }
        public Point Coordinates { get; private set; }

        public TileClickEventArgs(MouseButtons button, Point coords)
        {
            this.Button = button;
            this.Coordinates = coords;
        }
    }

    class GridEditControl : Panel
    {
        public event EventHandler<TileClickEventArgs> TileClick;

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

        int tileGap;
        [DefaultValue(2)]
        public int TileGap { get { return tileGap; } set { tileGap = value; this.UpdateDisplay(); } }

        int zoom;
        [DefaultValue(1), NotifyParentProperty(true), RefreshProperties(System.ComponentModel.RefreshProperties.Repaint)]
        public int Zoom { get { return zoom; } set { zoom = value; this.UpdateDisplay(); } }

        bool showGrid;
        [DefaultValue(false)]
        public bool ShowGrid { get { return showGrid; } set { showGrid = value; this.UpdateDisplay(); } }

        Color gridColor;
        [DefaultValue(typeof(Color), "128, 128, 128, 128")]
        public Color GridColor { get { return gridColor; } set { gridColor = value; this.UpdateDisplay(); } }

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
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public Size ZoomedTileSize { get { return zoomedTileSize; } }

        int zoomedTileGap;
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public int ZoomedTileGap { get { return zoomedTileGap; } }

        Point hoverTileCoords, selectedTileCoords;
        Rectangle selectionRect;

        public GridEditControl()
        {
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw, true);

            this.BackColor = Color.Black;

            this.columns = 8;
            this.rows = 8;
            this.tileSize = new Size(8, 8);
            this.tileGap = 2;
            this.zoom = 1;
            this.showGrid = false;
            this.gridColor = Color.FromArgb(128, Color.Gray);

            this.zoomedTileSize = Size.Empty;
            this.zoomedTileGap = -1;
            this.hoverTileCoords = this.selectedTileCoords = Point.Empty;
            this.selectionRect = Rectangle.Empty;
        }

        private void UpdateDisplay()
        {
            this.zoomedTileSize = new Size(this.tileSize.Width * this.zoom, this.tileSize.Height * this.zoom);
            this.zoomedTileGap = this.tileGap * this.zoom;
            this.Invalidate();
            this.OnResize(EventArgs.Empty);
        }

        protected override void OnResize(EventArgs eventargs)
        {
            Size newSize = new Size(this.Columns * (this.zoomedTileSize.Width + this.zoomedTileGap), this.Rows * (this.zoomedTileSize.Height + this.zoomedTileGap));
            if (this.Size != newSize)
            {
                this.Size = newSize;
                base.OnResize(eventargs);
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            hoverTileCoords = new Point((e.X / (this.zoomedTileSize.Width + this.zoomedTileGap)), (e.Y / (this.zoomedTileSize.Height + this.zoomedTileGap)));
            selectionRect = new Rectangle(
                hoverTileCoords.X * (this.zoomedTileSize.Width + this.zoomedTileGap), hoverTileCoords.Y * (this.zoomedTileSize.Height + this.zoomedTileGap),
                zoomedTileSize.Width - 1, zoomedTileSize.Height - 1);
            this.Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            selectedTileCoords = new Point((e.X / (this.zoomedTileSize.Width + this.zoomedTileGap)), (e.Y / (this.zoomedTileSize.Height + this.zoomedTileGap)));

            var handler = TileClick;
            if (handler != null) handler(this, new TileClickEventArgs(e.Button, selectedTileCoords));
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.None;
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;

            if (showGrid)
            {
                using (SolidBrush brush = new SolidBrush(this.gridColor))
                {
                    for (int x = 0; x < this.columns; x++)
                    {
                        for (int y = 0; y < this.rows; y++)
                        {
                            /* Right */
                            e.Graphics.FillRectangle(brush, new Rectangle(
                                x * (this.zoomedTileSize.Width + this.zoomedTileGap) + this.zoomedTileSize.Width, y * (this.zoomedTileSize.Height + this.zoomedTileGap),
                                this.zoomedTileGap, this.zoomedTileSize.Height));

                            /* Bottom */
                            e.Graphics.FillRectangle(brush, new Rectangle(
                                x * (this.zoomedTileSize.Width + this.zoomedTileGap), y * (this.zoomedTileSize.Height + this.zoomedTileGap) + this.zoomedTileSize.Height,
                                this.zoomedTileSize.Width, this.zoomedTileGap));

                            /* Corner */
                            e.Graphics.FillRectangle(brush, new Rectangle(
                                x * (this.zoomedTileSize.Width + this.zoomedTileGap) + this.zoomedTileSize.Width, y * (this.zoomedTileSize.Height + this.zoomedTileGap) + this.zoomedTileSize.Height,
                                this.zoomedTileGap, this.zoomedTileGap));
                        }
                    }
                }
            }

            e.Graphics.DrawRectangle(Pens.Red, selectionRect);
            e.Graphics.DrawLine(Pens.Red, Point.Empty, selectionRect.Location);
            e.Graphics.DrawString(string.Format("Hover:{0}, Selected:{1}", hoverTileCoords, selectedTileCoords), SystemFonts.MessageBoxFont, Brushes.Yellow, Point.Empty);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            e.Graphics.Clear(this.BackColor);
        }
    }
}
