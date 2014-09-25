using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Reflection;

using Yggdrasil.FileHandling;
using Yggdrasil.FileHandling.MapDataHandling;
using Yggdrasil.TableParsing;

namespace Yggdrasil.Controls
{
    public partial class FloorMapEditor : UserControl, IEditorControl
    {
        public bool IsInitialized() { return (gameDataManager != null); }
        public bool IsBusy() { return false; }

        GameDataManager gameDataManager;
        MapDataFile mapDataFile;
        List<GatherItemParser> gatherItemParsers;
        List<BaseTile> mapTiles;

        Bitmap mapImage;
        SolidBrush brushEmpty, brushFloor, brushWall;
        TilePalettePair mainMapTiles, mapTileGeomagneticField, mapTileRefreshingWater;

        public FloorMapEditor()
        {
            InitializeComponent();

            gridEditControl.Columns = 35;
            gridEditControl.Rows = 30;
        }

        public override void Refresh()
        {
            pgMapTile.Refresh();
        }

        public void Initialize(GameDataManager gameDataManager)
        {
            Program.Logger.LogMessage("Initializing {0}...", this.GetType().Name);

            brushEmpty = new SolidBrush(Color.FromArgb(0x01, 0x28, 0x47));
            brushFloor = new SolidBrush(Color.FromArgb(0x2F, 0x83, 0xB2));
            brushWall = new SolidBrush(Color.FromArgb(0x94, 0xFF, 0xEF));

            this.gameDataManager = gameDataManager;

            mainMapTiles = new TilePalettePair(gameDataManager, Path.Combine(gameDataManager.DataPath, @"data\Data\Tex\Map\Bg\i_dmp_bg_01"), TilePalettePair.Formats.Auto, 256, 128, true);

            mapTileGeomagneticField = new TilePalettePair(gameDataManager,
                Path.Combine(gameDataManager.DataPath, @"data\Data\Tex\Map\Obj\p_10\i_dmp_icon12_00.nbfc"),
                Path.Combine(gameDataManager.DataPath, @"data\Data\Tex\Map\Obj\p_10\map_obj_palette_10.nbfp"),
                TilePalettePair.Formats.Auto, 16, 16, true);

            mapTileRefreshingWater = new TilePalettePair(gameDataManager,
                Path.Combine(gameDataManager.DataPath, @"data\Data\Tex\Map\Obj\p_09\i_dmp_icon11_00.nbfc"),
                Path.Combine(gameDataManager.DataPath, @"data\Data\Tex\Map\Obj\p_09\map_obj_palette_09.nbfp"),
                TilePalettePair.Formats.Auto, 16, 16, true);

            mapImage = new Bitmap(
                (gridEditControl.ZoomedTileSize.Width + gridEditControl.ZoomedTileGap) * MapDataFile.MapWidth,
                (gridEditControl.ZoomedTileSize.Height + gridEditControl.ZoomedTileGap) * MapDataFile.MapHeight);

            Rebuild();
        }

        public void Rebuild()
        {
            pgMapTile.SelectedObject = null;
            cbMaps.DataSource = gameDataManager.MapDataFiles;
            cbMaps.DisplayMember = "FloorName";
        }

        public void Terminate()
        {
            this.gameDataManager = null;

            cbMaps.DataSource = null;
            pgMapTile.SelectedObject = null;

            brushEmpty.Dispose();
            brushFloor.Dispose();
            brushWall.Dispose();

            mapImage.Dispose();
        }

        private void cbMaps_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = (sender as ComboBox);
            if (comboBox.SelectedItem != null)
            {
                mapDataFile = (comboBox.SelectedItem as MapDataFile);
                mapTiles = gameDataManager.MapTileData.Where(xx => xx.MapDataFile == mapDataFile).ToList();

                gatherItemParsers = gameDataManager.ParsedData.Where(x => x is GatherItemParser && (x as GatherItemParser).FloorNumber == (mapDataFile.FloorNumber - 1)).Cast<GatherItemParser>().ToList();
                pgMapTile.SelectedObject = mapTiles.FirstOrDefault(xx => xx.Coordinates.X == 0 && xx.Coordinates.Y == 0);

                GenerateMapImage();
            }

            gridEditControl.Invalidate();
            pgMapTile.Refresh();
        }

        private void chkGatherOverlay_CheckedChanged(object sender, EventArgs e)
        {
            GenerateMapImage();
            gridEditControl.Invalidate();
        }

        private void chkEventOverlay_CheckedChanged(object sender, EventArgs e)
        {
            GenerateMapImage();
            gridEditControl.Invalidate();
        }

        private void gridEditControl_TileClick(object sender, TileClickEventArgs e)
        {
            // TODO: Allowing the user to actually redraw the map!

            if ((e.Button & System.Windows.Forms.MouseButtons.Left) != 0)
            {
                pgMapTile.SelectedObject = mapTiles.FirstOrDefault(xx => xx.Coordinates.X == e.Coordinates.X && xx.Coordinates.Y == e.Coordinates.Y);
            }
            else if ((e.Button & System.Windows.Forms.MouseButtons.Right) != 0)
            {
                GatherItemParser gatherItemParser = gatherItemParsers.FirstOrDefault(x => x.XCoord == e.Coordinates.X && x.YCoord == e.Coordinates.Y);
                if (gatherItemParser != null)
                {
                    /* Kinda messy, eh... Might wanna improve this? */
                    TabControl tabControl = (this.Parent.Parent as TabControl);
                    TabPage tableDataTab = tabControl.TabPages["tpTableData"];
                    TableEntryEditor entryEditor = (tableDataTab.Controls["tableEntryEditor"] as TableEntryEditor);

                    Action selectTab = new Action(() =>
                    {
                        entryEditor.SelectNodeByTag(gatherItemParser);
                        tabControl.SelectTab(tableDataTab);
                    });

                    if (!entryEditor.IsInitialized())
                    {
                        entryEditor.Initialize(gameDataManager);
                        Timer waitTimer = new Timer();
                        waitTimer.Interval = 50;
                        waitTimer.Tick += ((s, ea) => { if (!entryEditor.IsBusy()) { (s as Timer).Stop(); selectTab(); } });
                        waitTimer.Start();
                    }
                    else
                        selectTab();
                }
            }
        }

        private void GenerateMapImage()
        {
            if (!IsInitialized() || mapDataFile == null) return;

            using (Graphics g = Graphics.FromImage(mapImage))
            {
                g.Clear(Color.FromArgb(0xFF, 0x08, 0x38, 0x58));

                for (int y = 0; y < MapDataFile.MapHeight; y++)
                {
                    for (int x = 0; x < MapDataFile.MapWidth; x++)
                    {
                        Rectangle tileRect = new Rectangle((gridEditControl.ZoomedTileSize.Width + gridEditControl.ZoomedTileGap) * x, (gridEditControl.ZoomedTileSize.Height + gridEditControl.ZoomedTileGap) * y,
                            gridEditControl.ZoomedTileSize.Width, gridEditControl.ZoomedTileSize.Height);

                        BaseTile tile = mapTiles.FirstOrDefault(xx => xx.Coordinates.X == x && xx.Coordinates.Y == y);

                        switch (tile.TileType)
                        {
                            case MapDataFile.TileTypes.Nothing:
                                g.FillRectangle(Brushes.Black, tileRect);
                                break;
                            case MapDataFile.TileTypes.Wall:
                                DrawWallRectangle(g, brushWall, brushEmpty, x, y);
                                break;
                            case MapDataFile.TileTypes.Floor:
                            case MapDataFile.TileTypes.FOEFloor:
                            case MapDataFile.TileTypes.StairsUp:
                            case MapDataFile.TileTypes.StairsDown:
                            case MapDataFile.TileTypes.OneWayShortcutN:
                            case MapDataFile.TileTypes.OneWayShortcutS:
                            case MapDataFile.TileTypes.OneWayShortcutW:
                            case MapDataFile.TileTypes.OneWayShortcutE:
                            case MapDataFile.TileTypes.DoorNS:
                            case MapDataFile.TileTypes.DoorWE:
                            case MapDataFile.TileTypes.TreasureChest:
                            case MapDataFile.TileTypes.GeomagneticField:
                            case MapDataFile.TileTypes.CollapsingFloor:
                            case MapDataFile.TileTypes.RefreshingWater:
                                g.FillRectangle(brushFloor, tileRect);
                                break;
                            case MapDataFile.TileTypes.SandConveyorN:
                            case MapDataFile.TileTypes.SandConveyorS:
                            case MapDataFile.TileTypes.SandConveyorW:
                            case MapDataFile.TileTypes.SandConveyorE:
                                g.FillRectangle(Brushes.SandyBrown, tileRect);
                                break;
                            case MapDataFile.TileTypes.Water:
                                DrawWallRectangle(g, brushWall, Brushes.DarkCyan, x, y);
                                break;
                            case MapDataFile.TileTypes.WarpEntrance:
                                g.FillRectangle(Brushes.Purple, tileRect);
                                break;
                            case MapDataFile.TileTypes.Transporter:
                                g.FillRectangle(Brushes.Orange, tileRect);
                                break;
                            case MapDataFile.TileTypes.DamagingFloor:
                                g.FillRectangle(Brushes.Red, tileRect);
                                break;
                            default:
                                g.FillRectangle(brushEmpty, tileRect);
                                break;
                        }

                        if (MapDataFile.TileImageCoords.ContainsKey(tile.TileType))
                        {
                            Point coord = MapDataFile.TileImageCoords[tile.TileType];
                            if (tile.TileType == MapDataFile.TileTypes.GeomagneticField)
                                g.DrawImage(mapTileGeomagneticField.Image, tileRect, new Rectangle(coord.X, coord.Y, 16, 16), GraphicsUnit.Pixel);
                            else if (tile.TileType == MapDataFile.TileTypes.RefreshingWater)
                                g.DrawImage(mapTileRefreshingWater.Image, tileRect, new Rectangle(coord.X, coord.Y, 16, 16), GraphicsUnit.Pixel);
                            else
                                g.DrawImage(mainMapTiles.Image, tileRect, new Rectangle(coord.X, coord.Y, 16, 16), GraphicsUnit.Pixel);
                        }

                        if (chkGatherOverlay.Checked)
                        {
                            GatherItemParser gatherItemParser = gatherItemParsers.FirstOrDefault(xx => xx.XCoord == x && xx.YCoord == y);
                            if (gatherItemParser != null)
                                g.DrawImage(mainMapTiles.Image, tileRect, 32, 90, 16, 16, GraphicsUnit.Pixel);
                        }
                    }
                }
            }
        }

        private void gridEditControl_Paint(object sender, PaintEventArgs e)
        {
            if (!IsInitialized() || mapDataFile == null) return;
            e.Graphics.DrawImage(mapImage, Point.Empty);
        }

        private void DrawWallRectangle(Graphics g, Brush wallBrush, Brush emptyBrush, int x, int y)
        {
            int paddedWidth = gridEditControl.ZoomedTileSize.Width + gridEditControl.ZoomedTileGap;
            int paddedHeight = gridEditControl.ZoomedTileSize.Height + gridEditControl.ZoomedTileGap;

            int drawXCoord = paddedWidth * x;
            int drawYCoord = paddedHeight * y;

            bool left, right, top, bottom;
            bool topLeft, topRight, bottomLeft, bottomRight;

            g.FillRectangle(emptyBrush, new Rectangle(drawXCoord, drawYCoord, gridEditControl.ZoomedTileSize.Width, gridEditControl.ZoomedTileSize.Height));

            left = (x - 1 >= 0 && MapDataFile.IsTileWalkable[mapTiles.FirstOrDefault(xx => xx.Coordinates.X == x - 1 && xx.Coordinates.Y == y).TileType]);
            right = (x + 1 < MapDataFile.MapWidth && MapDataFile.IsTileWalkable[mapTiles.FirstOrDefault(xx => xx.Coordinates.X == x + 1 && xx.Coordinates.Y == y).TileType]);
            top = (y - 1 >= 0 && MapDataFile.IsTileWalkable[mapTiles.FirstOrDefault(xx => xx.Coordinates.X == x && xx.Coordinates.Y == y - 1).TileType]);
            bottom = (y + 1 < MapDataFile.MapHeight && MapDataFile.IsTileWalkable[mapTiles.FirstOrDefault(xx => xx.Coordinates.X == x && xx.Coordinates.Y == y + 1).TileType]);

            topLeft = (x - 1 >= 0 && y - 1 >= 0 && MapDataFile.IsTileWalkable[mapTiles.FirstOrDefault(xx => xx.Coordinates.X == x - 1 && xx.Coordinates.Y == y - 1).TileType]);
            topRight = (x + 1 < MapDataFile.MapWidth && y - 1 >= 0 && MapDataFile.IsTileWalkable[mapTiles.FirstOrDefault(xx => xx.Coordinates.X == x + 1 && xx.Coordinates.Y == y - 1).TileType]);
            bottomLeft = (x - 1 >= 0 && y + 1 < MapDataFile.MapHeight && MapDataFile.IsTileWalkable[mapTiles.FirstOrDefault(xx => xx.Coordinates.X == x - 1 && xx.Coordinates.Y == y + 1).TileType]);
            bottomRight = (x + 1 < MapDataFile.MapWidth && y + 1 < MapDataFile.MapHeight && MapDataFile.IsTileWalkable[mapTiles.FirstOrDefault(xx => xx.Coordinates.X == x + 1 && xx.Coordinates.Y == y + 1).TileType]);

            if (left) g.FillRectangle(wallBrush, new Rectangle(drawXCoord - gridEditControl.ZoomedTileGap, drawYCoord, 2, paddedHeight));
            if (right) g.FillRectangle(wallBrush, new Rectangle(drawXCoord + (paddedWidth - 2), drawYCoord, 2, paddedHeight));
            if (top) g.FillRectangle(wallBrush, new Rectangle(drawXCoord, drawYCoord - gridEditControl.ZoomedTileGap, paddedWidth, 2));
            if (bottom) g.FillRectangle(wallBrush, new Rectangle(drawXCoord, drawYCoord + (paddedWidth - 2), paddedWidth, 2));

            if (topLeft) g.FillRectangle(wallBrush, new Rectangle(drawXCoord - gridEditControl.ZoomedTileGap, drawYCoord - gridEditControl.ZoomedTileGap, 2, 2));
            if (topRight) g.FillRectangle(wallBrush, new Rectangle(drawXCoord + (paddedWidth - 2), drawYCoord - gridEditControl.ZoomedTileGap, 2, 2));
            if (bottomLeft) g.FillRectangle(wallBrush, new Rectangle(drawXCoord - gridEditControl.ZoomedTileGap, drawYCoord + (paddedHeight - 2), 2, 2));
            if (bottomRight) g.FillRectangle(wallBrush, new Rectangle(drawXCoord + (paddedWidth - 2), drawYCoord + (paddedHeight - 2), 2, 2));
        }
    }
}
