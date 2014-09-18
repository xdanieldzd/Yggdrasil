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
        public enum Formats { Auto, _2bpp, _4bpp, _8bpp };
        public Dictionary<Formats, int> modifiers = new Dictionary<Formats, int>()
        {
            { Formats._2bpp, 4 },
            { Formats._4bpp, 2 },
            { Formats._8bpp, 1 }
        };

        public GameDataManager GameDataManager { get; private set; }

        TileDataFile tileData;
        PaletteFile palette;
        public TileDataFile TileData
        {
            get { return tileData; }
            set { tileData = value; Convert(); }
        }
        public PaletteFile Palette
        {
            get { return palette; }
            set { palette = value; Convert(); }
        }

        public Formats Format { get; private set; }
        public Bitmap Image { get; private set; }

        public TilePalettePair(GameDataManager gameDataManager, string path) : this(gameDataManager, path, Formats.Auto, -1, -1) { }
        public TilePalettePair(GameDataManager gameDataManager, string path, Formats format) : this(gameDataManager, path, format, -1, -1) { }
        public TilePalettePair(GameDataManager gameDataManager, string path, Formats format, int width, int height)
        {
            string tilePath = (path + ".ntft");
            if (!File.Exists(tilePath) && !File.Exists(tilePath = (path + ".cmp"))) throw new Exception("Tile data file not found");

            string palettePath = (path + ".ntfp");
            if (!File.Exists(palettePath) && !File.Exists(palettePath = (path + ".cmp"))) throw new Exception("Palette data file not found");

            Load(gameDataManager, tilePath, palettePath, format, width, height);
        }
        public TilePalettePair(GameDataManager gameDataManager, string tilePath, string palettePath) { Load(gameDataManager, tilePath, palettePath, Formats.Auto, -1, -1); }
        public TilePalettePair(GameDataManager gameDataManager, string tilePath, string palettePath, Formats format) { Load(gameDataManager, tilePath, palettePath, format, -1, -1); }
        public TilePalettePair(GameDataManager gameDataManager, string tilePath, string palettePath, Formats format, int width, int height) { Load(gameDataManager, tilePath, palettePath, format, width, height); }

        private void Load(GameDataManager gameDataManager, string tilePath, string palettePath, Formats format, int width, int height)
        {
            this.GameDataManager = gameDataManager;
            this.palette = new PaletteFile(gameDataManager, palettePath);
            this.tileData = new TileDataFile(gameDataManager, tilePath);

            if (format == Formats.Auto)
            {
                if (this.Palette.Colors.Count <= 2) format = Formats._2bpp;
                else if (this.Palette.Colors.Count <= 16) format = Formats._4bpp;
                else if (this.Palette.Colors.Count <= 256) format = Formats._8bpp;
            }

            this.Format = format;

            if (width == -1 || height == -1)
            {
                int baseWidth = (int)Math.Round(Math.Sqrt(this.TileData.Data.Length));
                width = 1;
                while (width < baseWidth) width <<= 1;

                height = (this.TileData.Data.Length / width);
                if (height == 0) height = 1;
                height *= modifiers[this.Format];
            }

            this.Image = new Bitmap(width, height);
            Convert();
        }

        public void Convert()
        {
            for (int y = 0; y < this.Image.Height; y++)
            {
                byte idx;
                switch (this.Format)
                {
                    case Formats._2bpp:
                        for (int x = 0, xa = 0; x < this.Image.Width; x += modifiers[this.Format], xa++)
                        {
                            for (int i = 3; i >= 0; i--)
                            {
                                idx = (byte)((this.TileData.Data[(y * (this.Image.Width / modifiers[this.Format])) + xa] >> (i << 1)) & 0x03);
                                if (idx >= this.Palette.Colors.Count) this.Image.SetPixel(x + i, y, Color.Gray);
                                else this.Image.SetPixel(x + i, y, this.Palette.Colors[idx]);
                            }
                        }
                        break;

                    case Formats._4bpp:
                        for (int x = 0, xa = 0; x < this.Image.Width; x += modifiers[this.Format], xa++)
                        {
                            for (int i = 1; i >= 0; i--)
                            {
                                idx = (byte)((this.TileData.Data[(y * (this.Image.Width / modifiers[this.Format])) + xa] >> (i << 2)) & 0x0F);
                                if (idx >= this.Palette.Colors.Count) this.Image.SetPixel(x + i, y, Color.Gray);
                                else this.Image.SetPixel(x + i, y, this.Palette.Colors[idx]);
                            }
                        }
                        break;

                    case Formats._8bpp:
                        for (int x = 0; x < this.Image.Width; x++)
                        {
                            idx = this.TileData.Data[(y * this.Image.Width) + x];
                            if (idx >= this.Palette.Colors.Count) this.Image.SetPixel(x, y, Color.Gray);
                            else this.Image.SetPixel(x, y, this.Palette.Colors[idx]);
                        }
                        break;
                }
            }

            //this.Image.Save(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(this.TileData.Filename), System.IO.Path.GetFileNameWithoutExtension(this.TileData.Filename) + ".png"));
        }
    }
}
