using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;

namespace Yggdrasil.FileHandling
{
    public class TilePalettePair
    {
        public GameDataManager GameDataManager { get; private set; }
        public TileDataFile TileData { get; private set; }
        public PaletteFile Palette { get; private set; }

        public Bitmap Image { get; private set; }

        public TilePalettePair(GameDataManager gameDataManager, string path) : this(gameDataManager, path, -1, -1) { }

        public TilePalettePair(GameDataManager gameDataManager, string path, int width, int height)
        {
            string tilePath = (path + ".ntft");
            if (!File.Exists(tilePath) && !File.Exists(tilePath = (path + ".cmp"))) throw new Exception("Tile data file not found");

            string palettePath = (path + ".ntfp");
            if (!File.Exists(palettePath) && !File.Exists(palettePath = (path + ".cmp"))) throw new Exception("Palette data file not found");

            Load(gameDataManager, tilePath, palettePath, width, height);
        }

        public TilePalettePair(GameDataManager gameDataManager, string tilePath, string palettePath) { Load(gameDataManager, tilePath, palettePath, -1, -1); }
        public TilePalettePair(GameDataManager gameDataManager, string tilePath, string palettePath, int width, int height) { Load(gameDataManager, tilePath, palettePath, width, height); }

        private void Load(GameDataManager gameDataManager, string tilePath, string palettePath, int width, int height)
        {
            this.GameDataManager = gameDataManager;
            this.Palette = new PaletteFile(gameDataManager, palettePath);
            this.TileData = new TileDataFile(gameDataManager, tilePath);

            if (width == -1 || height == -1)
            {
                int baseWidth = (int)Math.Round(Math.Sqrt(this.TileData.Data.Length));
                width = 1;
                while (width < baseWidth) width <<= 1;

                height = (this.TileData.Data.Length / width);
                if (height == 0) height = 1;
                if (this.Palette.Colors.Count == 16) height *= 2;
            }

            this.Image = new Bitmap(width, height);
            for (int y = 0; y < this.Image.Height; y++)
            {
                if (this.Palette.Colors.Count == 16)
                {
                    for (int x = 0, xa = 0; x < this.Image.Width; x += 2, xa++)
                    {
                        this.Image.SetPixel(x, y, this.Palette.Colors[this.TileData.Data[(y * width / 2) + xa] & 0xF]);
                        this.Image.SetPixel(x + 1, y, this.Palette.Colors[this.TileData.Data[(y * width / 2) + xa] >> 4]);
                    }
                }
                else if (this.Palette.Colors.Count == 256)
                {
                    for (int x = 0; x < this.Image.Width; x++)
                    {
                        this.Image.SetPixel(x, y, this.Palette.Colors[this.TileData.Data[(y * width) + x]]);
                    }
                }
            }

            //this.Image.Save(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(tilePath), System.IO.Path.GetFileNameWithoutExtension(tilePath) + ".png"));
        }
    }
}
