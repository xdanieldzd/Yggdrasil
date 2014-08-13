using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Drawing.Imaging;

namespace Yggdrasil.Helpers
{
    public class FontRenderer
    {
        public GameDataManager Game { get; private set; }
        public string Filename { get; private set; }

        bool fontCompressed;
        byte[] fontRaw, paletteRaw;
        List<Color> palette;
        Bitmap fontImage;

        public Bitmap FontImage { get { return fontImage; } }
        public Size CharacterSize { get; private set; }
        public List<Character> Characters { get; private set; }

        public FontRenderer(GameDataManager game, string path)
        {
            this.Game = game;
            Filename = path;
            fontRaw = Decompressor.Decompress(Filename, out fontCompressed);

            paletteRaw = File.ReadAllBytes(Path.Combine(Path.GetDirectoryName(Filename), Path.GetFileNameWithoutExtension(Filename) + ".ntfp"));
            palette = new List<Color>()
            {
                Color.FromArgb(0, Color.Pink),
                Color.White,
                Color.LightGray,
                Color.DarkGray
            };
            /*palette = new List<Color>();
            for (int i = 0; i < paletteRaw.Length; i += 2)
            {
                palette.Add(ConvertRGBA5551(BitConverter.ToUInt16(paletteRaw, i)));
            }
            palette[0] = Color.FromArgb(0, palette[0]);
            */
            var matches = Regex.Matches(Path.GetFileName(Filename), "([0-9]{2})|([0-9])");
            if (matches.Count < 2) throw new Exception("Cannot extract font size from filename");

            int fontOrgWidth = 0, fontOrgHeight = 0;

            switch (game.Version)
            {
                case GameDataManager.Versions.European:
                    CharacterSize = new Size(int.Parse(matches[1].Value), int.Parse(matches[0].Value));
                    fontImage = new Bitmap(256, 128);
                    fontOrgWidth = fontImage.Width;
                    fontOrgHeight = fontImage.Height;
                    Convert2bpp(ref fontImage, fontRaw, palette);
                    break;

                case GameDataManager.Versions.American:
                    CharacterSize = new Size(int.Parse(matches[1].Value).Round(8) - 1, int.Parse(matches[0].Value));
                    fontImage = new Bitmap(128, 64);
                    fontOrgWidth = fontImage.Width - 16;
                    fontOrgHeight = fontImage.Height;
                    Convert4bpp(ref fontImage, fontRaw, palette);
                    break;
            }

            Characters = new List<Character>();
            ushort id = 1;
            int height = (fontOrgHeight / CharacterSize.Height);
            int width = (fontOrgWidth / CharacterSize.Width);
            for (int y = 0; y < height * CharacterSize.Height; y += CharacterSize.Height)
            {
                for (int x = 0; x < width * CharacterSize.Width; x += CharacterSize.Width)
                {
                    int charWidth = -1, offset = 0;
                    Bitmap chrBmp = new Bitmap(CharacterSize.Width, CharacterSize.Height);

                    using (Graphics g = Graphics.FromImage(chrBmp))
                    {
                        for (int xx = 0; xx < CharacterSize.Width; xx++)
                        {
                            int filled = 0;
                            for (int yy = 0; yy < CharacterSize.Height; yy++)
                            {
                                if (fontImage.GetPixel(x + xx, y + yy).A != 0) filled++;
                            }

                            if (filled > 0)
                            {
                                offset = xx;
                                break;
                            }
                        }

                        for (int xx = offset; xx < CharacterSize.Width; xx++)
                        {
                            int filled = 0;
                            for (int yy = 0; yy < CharacterSize.Height; yy++)
                            {
                                if (fontImage.GetPixel(x + xx, y + yy).A != 0) filled++;
                            }

                            if (filled > 0 && charWidth < (xx - offset))
                            {
                                charWidth = (xx - offset);
                            }
                        }
                        if (charWidth == -1) charWidth = 1;
                        charWidth += 2;

                        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                        g.DrawImage(fontImage, new Rectangle(0, 0, charWidth, chrBmp.Height), new Rectangle(x + offset, y, charWidth, CharacterSize.Height), GraphicsUnit.Pixel);
                    }

                    Characters.Add(new Character(id++, chrBmp, charWidth));
                }
            }
        }

        private void Convert2bpp(ref Bitmap image, byte[] data, List<Color> palette)
        {
            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0, x2 = 0; x < image.Width; x += 4, x2++)
                {
                    int ofs = (y * (image.Width / 4)) + x2;
                    byte read = data[ofs];

                    for (int i = 3; i >= 0; i--)
                    {
                        byte idx = (byte)((read >> (i << 1)) & 0x03);
                        image.SetPixel(x + i, y, palette[idx]);
                    }
                }
            }
        }

        private void Convert4bpp(ref Bitmap image, byte[] data, List<Color> palette)
        {
            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0, x2 = 0; x < image.Width; x += 2, x2++)
                {
                    int ofs = (y * (image.Width / 2)) + x2;
                    byte read = data[ofs];

                    for (int i = 1; i >= 0; i--)
                    {
                        byte idx = (byte)((read >> (i << 2)) & 0x0F);
                        image.SetPixel(x + i, y, palette[idx]);
                    }
                }
            }
        }

        public Bitmap RenderString(EtrianString str, int width = 256, int spacingModifier = 0)
        {
            int newLines = str.RawData.Count(xx => xx == EtrianString.CharacterMap.GetByValue('\n')) + 1;

            Bitmap rendered = new Bitmap(width, Math.Max(newLines, 1) * CharacterSize.Height);

            ColorMap[] colorMap = new ColorMap[1] { new ColorMap() { OldColor = palette[1], NewColor = palette[1] } };
            ImageAttributes imageAttrib = new ImageAttributes();

            int x = 0, y = 0;
            using (Graphics g = Graphics.FromImage(rendered))
            {
                for (int i = 0; i < str.RawData.Length; i++)
                {
                    if ((str.RawData[i] & 0x8000) == 0x8000 && !EtrianString.CharacterMap.ContainsKey(str.RawData[i]))
                    {
                        /* TODO  control codes go HERE */
                        switch (str.RawData[i] & 0xFF)
                        {
                            case 0x04:
                                // Color
                                //6==red??
                                ushort color = str.RawData[i + 1];
                                if (color == 4) colorMap[0].NewColor = palette[1];
                                else if (color == 6) colorMap[0].NewColor = Color.FromArgb(0xF8, 0x70, 0x48);
                                imageAttrib.SetRemapTable(colorMap);
                                i++;
                                continue;

                            default:
                                break;
                        }
                    }

                    if (str.RawData[i] == EtrianString.CharacterMap.GetByValue('\n'))
                    {
                        colorMap[0].NewColor = colorMap[0].OldColor;
                        y += (CharacterSize.Height - 1);
                        x = 0;
                    }
                    else
                    {
                        Character chrData = Characters.FirstOrDefault(xx => xx.ID == str.RawData[i]);
                        if (chrData != null)
                        {
                            g.DrawImage(chrData.Image, new Rectangle(x, y, chrData.Image.Width, chrData.Image.Height), 0, 0, chrData.Image.Width, chrData.Image.Height, GraphicsUnit.Pixel, imageAttrib);
                            x += chrData.Width + spacingModifier;
                        }
                    }
                }
            }

            return rendered;
        }

        public Color ConvertRGBA5551(ushort val)
        {
            return Color.FromArgb(
                (byte)((val & 0x0100) != 0 ? 0xFF : 0),
                (byte)((val & 0x00F8) >> 8),
                (byte)(((val & 0xC007) << 5) >> 8),
                (byte)(((val & 0x3E00) << 18) >> 16));
        }

        public class Character
        {
            public ushort ID { get; private set; }
            public Bitmap Image { get; private set; }
            public int Width { get; private set; }

            public Character(ushort id, Bitmap image, int width)
            {
                ID = id;
                Image = image;
                Width = width;
            }
        }
    }
}
