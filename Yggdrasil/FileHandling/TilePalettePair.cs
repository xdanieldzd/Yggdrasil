using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

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

        public bool ForcedIndex0Transparent { get; private set; }
        public Formats Format { get; private set; }
        public Bitmap Image { get; private set; }

        BitmapData bmpData;
        byte[] pixelData;

        public TilePalettePair(GameDataManager gameDataManager, string path) : this(gameDataManager, path, Formats.Auto, -1, -1) { }
        public TilePalettePair(GameDataManager gameDataManager, string path, Formats format) : this(gameDataManager, path, format, -1, -1) { }
        public TilePalettePair(GameDataManager gameDataManager, string path, Formats format, int width, int height) : this(gameDataManager, path, format, width, height, false) { }
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
            if (!File.Exists(tilePath)) throw new FileNotFoundException("Tile data file not found");
            if (!File.Exists(palettePath)) throw new FileNotFoundException("Palette data file not found");

            this.GameDataManager = gameDataManager;
            this.palette = new PaletteFile(gameDataManager, palettePath);
            this.tileData = new TileDataFile(gameDataManager, tilePath);

            this.ForcedIndex0Transparent = forceIndex0Transparent;

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

            switch (this.Format & Formats.BPPMask)
            {
                case Formats._2bpp:
                    /* TODO: no native 2bpp format in .NET, keep using 8bpp or make 4bpp? */
                    this.Image = new Bitmap(width, height, PixelFormat.Format8bppIndexed);
                    break;

                case Formats._4bpp:
                    this.Image = new Bitmap(width, height, PixelFormat.Format4bppIndexed);
                    break;

                case Formats._8bpp:
                    this.Image = new Bitmap(width, height, PixelFormat.Format8bppIndexed);
                    break;
            }

            Convert();
        }

        public void Convert()
        {
            if (ForcedIndex0Transparent) this.palette.Colors[0] = Color.FromArgb(0, this.palette.Colors[0]);

            ColorPalette imagePalette = this.Image.Palette;
            for (int i = 0; i < Math.Min(this.palette.Colors.Count, imagePalette.Entries.Length); i++) imagePalette.Entries[i] = this.palette.Colors[i];
            this.Image.Palette = imagePalette;

            bmpData = this.Image.LockBits(new Rectangle(0, 0, this.Image.Width, this.Image.Height), ImageLockMode.ReadWrite, this.Image.PixelFormat);
            pixelData = new byte[bmpData.Height * bmpData.Stride];
            Marshal.Copy(bmpData.Scan0, pixelData, 0, pixelData.Length);

            Formats type = (this.Format & Formats.TypeMask);
            Formats bpp = (this.Format & Formats.BPPMask);

            if (type == Formats.Texture)
            {
                switch (bpp)
                {
                    case Formats._2bpp:
                        for (int i = 0; i < this.tileData.Data.Length; i++)
                        {
                            for (int j = 3; j >= 0; j--)
                            {
                                byte idx = (byte)((this.tileData.Data[i] >> (j << 1)) & 0x03);
                                pixelData[(i * 4) + j] = idx;
                            }
                        }
                        break;

                    case Formats._4bpp:
                        Buffer.BlockCopy(this.tileData.Data, 0, pixelData, 0, pixelData.Length);
                        for (int i = 0; i < pixelData.Length; i++) pixelData[i] = (byte)((pixelData[i] >> 4) | (pixelData[i] << 4));
                        break;

                    case Formats._8bpp:
                        Buffer.BlockCopy(this.tileData.Data, 0, pixelData, 0, pixelData.Length);
                        break;
                }
            }
            else if (type == Formats.Tiled)
            {
                int offset = 0;
                switch (bpp)
                {
                    case Formats._4bpp:
                        for (int yGlobal = 0; yGlobal < this.Image.Height; yGlobal += 8)
                            for (int xGlobal = 0; xGlobal < bmpData.Stride; xGlobal += 4)
                                for (int yTile = 0; yTile < 8; yTile++)
                                    for (int xTile = 0; xTile < 4; xTile++)
                                        pixelData[(yGlobal * bmpData.Stride) + (yTile * bmpData.Stride) + (xGlobal + xTile)] = this.tileData.Data[offset++];

                        for (int i = 0; i < pixelData.Length; i++) pixelData[i] = (byte)((pixelData[i] >> 4) | (pixelData[i] << 4));
                        break;

                    default:
                        throw new NotImplementedException("Conversion of non-8bpp tiled images not implemented");
                }
            }

            Marshal.Copy(pixelData, 0, bmpData.Scan0, pixelData.Length);
            this.Image.UnlockBits(bmpData);
        }
    }
}
