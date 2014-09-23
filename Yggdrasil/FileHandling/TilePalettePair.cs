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
        [Flags]
        public enum Formats
        {
            Auto = 0x00,

            Texture = 0x10,
            Tiled = 0x20,

            _2bpp = 0x01,
            _4bpp = 0x02,
            _8bpp = 0x04,

            TypeMask = 0xF0,
            BPPMask = 0x0F,
        };

        string[] tileExtensions = new string[] { ".ntft", ".nbfc", ".cmp" };
        string[] paletteExtensions = new string[] { ".ntfp", ".nbfp" };

        Dictionary<Formats, int> modifiers = new Dictionary<Formats, int>()
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
        public TilePalettePair(GameDataManager gameDataManager, string path, Formats format, int width, int height) : this(gameDataManager, path, format, -1, -1, false) { }
        public TilePalettePair(GameDataManager gameDataManager, string path, Formats format, int width, int height, bool forceIndex0Transparent)
        {
            string tilePath = string.Empty;
            foreach (string extension in tileExtensions) if (File.Exists(tilePath = (path + extension))) break;

            string palettePath = string.Empty;
            foreach (string extension in paletteExtensions) if (File.Exists(palettePath = (path + extension))) break;

            Load(gameDataManager, tilePath, palettePath, format, width, height, forceIndex0Transparent);
        }
        public TilePalettePair(GameDataManager gameDataManager, string tilePath, string palettePath) { Load(gameDataManager, tilePath, palettePath, Formats.Auto, -1, -1, false); }
        public TilePalettePair(GameDataManager gameDataManager, string tilePath, string palettePath, Formats format) { Load(gameDataManager, tilePath, palettePath, format, -1, -1, false); }
        public TilePalettePair(GameDataManager gameDataManager, string tilePath, string palettePath, Formats format, int width, int height) { Load(gameDataManager, tilePath, palettePath, format, width, height, false); }
        public TilePalettePair(GameDataManager gameDataManager, string tilePath, string palettePath, Formats format, int width, int height, bool forceIndex0Transparent) { Load(gameDataManager, tilePath, palettePath, format, width, height, forceIndex0Transparent); }

        private void Load(GameDataManager gameDataManager, string tilePath, string palettePath, Formats format, int width, int height, bool forceIndex0Transparent)
        {
            if (!File.Exists(tilePath)) throw new Exception("Tile data file not found");
            if (!File.Exists(palettePath)) throw new Exception("Palette data file not found");

            this.GameDataManager = gameDataManager;
            this.palette = new PaletteFile(gameDataManager, palettePath);
            this.tileData = new TileDataFile(gameDataManager, tilePath);

            if (forceIndex0Transparent)
                this.palette.Colors[0] = Color.FromArgb(0, this.palette.Colors[0]);

            if (format == Formats.Auto)
            {
                if (this.Palette.Colors.Count <= 2) format = Formats._2bpp;
                else if (this.Palette.Colors.Count <= 16) format = Formats._4bpp;
                else if (this.Palette.Colors.Count <= 256) format = Formats._8bpp;
            }

            format |= (Path.GetExtension(palettePath) == ".nbfp" ? Formats.Tiled : Formats.Texture);

            this.Format = format;

            if (width == -1 || height == -1)
            {
                int baseWidth = (int)Math.Round(Math.Sqrt(this.TileData.Data.Length));
                width = 1;
                while (width < baseWidth) width <<= 1;

                height = (this.TileData.Data.Length / width);
                if (height == 0) height = 1;
                height *= modifiers[this.Format & Formats.BPPMask];
            }

            this.Image = new Bitmap(width, height);
            Convert();
        }

        public void Convert()
        {
            Formats type = (this.Format & Formats.TypeMask);
            Formats bpp = (this.Format & Formats.BPPMask);

            int offset = 0;
            if (type == Formats.Texture)
            {
                for (int y = 0; y < this.Image.Height; y++) ConvertRow(bpp, ref offset, 0, 0, y, this.Image.Width);
            }
            else if (type == Formats.Tiled)
            {
                for (int gy = 0; gy < this.Image.Height; gy += 8)
                {
                    for (int gx = 0; gx < this.Image.Width; gx += 8)
                    {
                        for (int y = 0; y < 8; y++) ConvertRow(bpp, ref offset, gx, gy, y, 8);
                    }
                }
            }

            //this.Image.Save(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(this.TileData.Filename), System.IO.Path.GetFileNameWithoutExtension(this.TileData.Filename) + ".png"));
        }

        private void ConvertRow(Formats bpp, ref int offset, int gx, int gy, int y, int width)
        {
            byte idx;
            switch (bpp)
            {
                case Formats._2bpp:
                    for (int x = 0; x < width; x += modifiers[bpp])
                    {
                        for (int i = 3; i >= 0; i--)
                        {
                            idx = (byte)((this.tileData.Data[offset] >> (i << 1)) & 0x03);
                            if (idx >= this.Palette.Colors.Count) this.Image.SetPixel(gx + x, gy + y, Color.Gray);
                            else this.Image.SetPixel(gx + x + i, gy + y, this.Palette.Colors[idx]);
                        }
                        offset++;
                    }
                    break;

                case Formats._4bpp:
                    for (int x = 0; x < width; x += modifiers[bpp])
                    {
                        for (int i = 1; i >= 0; i--)
                        {
                            idx = (byte)((this.tileData.Data[offset] >> (i << 2)) & 0x0F);
                            if (idx >= this.Palette.Colors.Count) this.Image.SetPixel(gx + x, gy + y, Color.Gray);
                            else this.Image.SetPixel(gx + x + i, gy + y, this.Palette.Colors[idx]);
                        }
                        offset++;
                    }
                    break;

                case Formats._8bpp:
                    for (int x = 0; x < width; x++)
                    {
                        idx = this.tileData.Data[offset];
                        if (idx >= this.Palette.Colors.Count) this.Image.SetPixel(gx + x, gy + y, Color.Gray);
                        else this.Image.SetPixel(gx + x, gy + y, this.Palette.Colors[idx]);
                        offset++;
                    }
                    break;
            }
        }
    }
}
