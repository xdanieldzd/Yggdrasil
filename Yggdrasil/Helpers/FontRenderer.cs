using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Drawing.Imaging;

using Yggdrasil.TextHandling;

namespace Yggdrasil.Helpers
{
	public class FontRenderer
	{
		static byte[] paletteRaw = new byte[]
		{
			0xE0, 0x7F, 0xFF, 0x7F, 0x21, 0x04, 0x3F, 0x04, 0x21, 0x6C, 0x1F, 0x7C, 0xFF, 0x03, 0xFB, 0x33,
			0x7B, 0x6F, 0x58, 0x6F, 0x7B, 0x6F, 0x7B, 0x6F, 0x7B, 0x6F, 0x7B, 0x6F, 0x7B, 0x6F, 0x7B, 0x6F
		};

		public GameDataManager GameDataManager { get; private set; }
		public string Filename { get; private set; }

		byte[] lrOffsetRaw;
		FileHandling.TilePalettePair font;

		public Size CharacterSize { get; private set; }
		public List<Tuple<byte, byte>> CharacterLROffsets { get; private set; }
		public List<Character> Characters { get; private set; }

		public FontRenderer(GameDataManager gameDataManager, string fontPath, int arm9LROffset, ushort characterCount)
		{
			GameDataManager = gameDataManager;
			Filename = fontPath;

			var matches = Regex.Matches(Path.GetFileName(Filename), "([0-9]{2})|([0-9])");
			if (matches.Count < 2) throw new Exception("Cannot extract font size from filename");

			CharacterSize = new Size(int.Parse(matches[1].Value), int.Parse(matches[0].Value));

			int fontOrgWidth = 0, fontOrgHeight = 0;

			switch (gameDataManager.Version)
			{
				case GameDataManager.Versions.European:
					font = new FileHandling.TilePalettePair(gameDataManager, Path.Combine(Path.GetDirectoryName(Filename), Path.GetFileNameWithoutExtension(Filename)),
						FileHandling.TilePalettePair.Formats.Auto, fontOrgWidth = 256, fontOrgHeight = 128, true)
					{
						Palette = new FileHandling.PaletteFile(gameDataManager, new MemoryStream(paletteRaw), null, -1)
					};
					break;

				case GameDataManager.Versions.American:
					CharacterSize = new Size(CharacterSize.Width + 2, CharacterSize.Height);
					font = new FileHandling.TilePalettePair(gameDataManager, Path.Combine(Path.GetDirectoryName(Filename), Path.GetFileNameWithoutExtension(Filename)),
						FileHandling.TilePalettePair.Formats.Auto, fontOrgWidth = 128, fontOrgHeight = 64, true);
					fontOrgWidth = CharacterSize.Width * 16;
					fontOrgHeight = CharacterSize.Height * 6;
					break;

				case GameDataManager.Versions.Japanese:
					font = new FileHandling.TilePalettePair(gameDataManager, Path.Combine(Path.GetDirectoryName(Filename), Path.GetFileNameWithoutExtension(Filename)),
						FileHandling.TilePalettePair.Formats.Auto, fontOrgWidth = 512, fontOrgHeight = 512, true);
					break;
			}

			Characters = new List<Character>();
			ushort id = 0;
			int height = (fontOrgHeight / CharacterSize.Height) * CharacterSize.Height;
			int width = (fontOrgWidth / CharacterSize.Width) * CharacterSize.Width;

			if (arm9LROffset != -1)
			{
				lrOffsetRaw = new byte[characterCount * 2];
				byte[] arm9data = File.ReadAllBytes(Path.Combine(gameDataManager.DataPath, "arm9.bin"));
				Buffer.BlockCopy(arm9data, arm9LROffset, lrOffsetRaw, 0, lrOffsetRaw.Length);

				CharacterLROffsets = new List<Tuple<byte, byte>>();
				for (int i = 0; i < lrOffsetRaw.Length; i += 2)
				{
					CharacterLROffsets.Add(new Tuple<byte, byte>(lrOffsetRaw[i], lrOffsetRaw[i + 1]));
				}
			}

			for (int y = 0; y < height; y += CharacterSize.Height)
			{
				for (int x = 0; x < width; x += CharacterSize.Width)
				{
					byte chrLeftOffset = (byte)(CharacterLROffsets != null ? CharacterLROffsets[id].Item1 : 0);
					byte chrRightOffset = (byte)(CharacterLROffsets != null ? CharacterLROffsets[id].Item2 : 0);
					Bitmap chrBmp = new Bitmap(CharacterSize.Width - (chrLeftOffset + chrRightOffset) + 1, CharacterSize.Height);

					using (Graphics g = Graphics.FromImage(chrBmp))
					{
						g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
						g.DrawImage(font.Image, new Rectangle(0, 0, chrBmp.Width - 1, chrBmp.Height), new Rectangle(x + chrLeftOffset, y, chrBmp.Width - 1, chrBmp.Height), GraphicsUnit.Pixel);
					}

					Characters.Add(new Character(id++, chrBmp, chrLeftOffset, chrRightOffset));

					if (characterCount != 0 && id >= characterCount) return;
				}
			}
		}

		public Bitmap RenderString(EtrianString str, int width = 256, int spacingModifier = 0, int zoom = 1)
		{
			if (str == null) return null;

			int newLines = str.ConvertedString.Count(xx => xx == '\n') + 1;
			int yIncrease = CharacterSize.Height;
			if (GameDataManager.Version != GameDataManager.Versions.European) yIncrease += 4;

			Bitmap rendered = new Bitmap(width, Math.Max(newLines, 1) * yIncrease);

			ColorMap[] colorMap = new ColorMap[1] { new ColorMap() { OldColor = font.Palette.Colors[1], NewColor = font.Palette.Colors[1] } };
			ImageAttributes imageAttrib = new ImageAttributes();

			int x = 0, y = 0;
			using (Graphics g = Graphics.FromImage(rendered))
			{
				for (int i = 0; i < str.RawData.Length; i++)
				{
					if ((str.RawData[i] & 0x8000) == 0x8000 && !EtrianString.CharacterMap.ContainsKey(str.RawData[i]))
					{
						/* TODO: Figure out at least colors, not sure what else might be needed for preview here... */
						switch (str.RawData[i] & 0xFF)
						{
							case 0x04:
								// Color
								ushort color = str.RawData[i + 1];
								if (color == 0) colorMap[0].NewColor = colorMap[0].OldColor;
								else if (color == 4) colorMap[0].NewColor = font.Palette.Colors[1];
								else if (color == 6) colorMap[0].NewColor = Color.FromArgb(0xF8, 0x70, 0x48);
								else if (color == 7) colorMap[0].NewColor = Color.FromArgb(0x4A, 0xBD, 0xA5);
								else if (color == 9) colorMap[0].NewColor = Color.FromArgb(0xEF, 0xF7, 0xAD);
								else if (color == 0xA) colorMap[0].NewColor = Color.FromArgb(0x5A, 0xEF, 0xCE);
								imageAttrib.SetRemapTable(colorMap);
								i++;
								continue;

							default:
								break;
						}
					}

					if (i + 1 < str.RawData.Length && (str.RawData[i] == 0x8001 && str.RawData[i + 1] == 0x8002))
					{
						y += yIncrease;
						using (Pen pen = new Pen(Color.FromArgb(128, Color.White)))
						{
							g.DrawLine(pen, new Point(0, y + 1), new Point(rendered.Width, y + 1));
						}
						x = 0;
					}
					else if (str.RawData[i] == 0x8001)
					{
						y += yIncrease;
						x = 0;
					}
					else
					{
						Character chrData = Characters.FirstOrDefault(xx => xx.ID == (ushort)((str.RawData[i] & 0xFF00) | (str.RawData[i] & 0xFF) - 1));
						if (chrData != null)
						{
							g.DrawImage(chrData.Image, new Rectangle(x, y, chrData.Image.Width, chrData.Image.Height), 0, 0, chrData.Image.Width, chrData.Image.Height, GraphicsUnit.Pixel, imageAttrib);
							x += (chrData.Image.Width + spacingModifier);
						}
					}
				}
			}

			if (zoom == 1)
				return rendered;
			else
			{
				Bitmap scaledRendered = new Bitmap(rendered.Width * zoom, rendered.Height * zoom);
				using (Graphics g = Graphics.FromImage(scaledRendered))
				{
					g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
					g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
					g.DrawImage(rendered, 0.0f, 0.0f, scaledRendered.Width, scaledRendered.Height);
				}

				return scaledRendered;
			}
		}

		public class Character
		{
			public ushort ID { get; private set; }
			public Bitmap Image { get; private set; }
			public byte LeftOffset { get; private set; }
			public byte RightOffset { get; private set; }

			public Character(ushort id, Bitmap image, byte leftOffset, byte rightOffset)
			{
				ID = id;
				Image = image;
				LeftOffset = leftOffset;
				RightOffset = rightOffset;
			}
		}

		public void DumpMkwinfont(string path, string name)
		{
			using (StreamWriter sw = new StreamWriter(new FileStream(path, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite)))
			{
				sw.WriteLine("facename {0}", name);
				sw.WriteLine("copyright (C) ATLUS; font dumped via {0}", Program.TitleString);
				sw.WriteLine();
				sw.WriteLine("height {0}", CharacterSize.Height);
				sw.WriteLine("ascent {0}", CharacterSize.Height - 1);
				sw.WriteLine("pointsize {0}", CharacterSize.Height - 1);
				sw.WriteLine();

				for (int i = 0; i < 256; i++)
				{
					KeyValuePair<ushort, char> chEntry = EtrianString.CharacterMap.FirstOrDefault(x => x.Value == i);

					if (chEntry.Value == '\0')
					{
						sw.WriteLine("char {0}", i);
						sw.WriteLine("width 0");
						sw.WriteLine();
					}
					else
					{
						Character character = Characters.FirstOrDefault(x => x.ID == chEntry.Key - 1);
						if (character == null)
						{
							sw.WriteLine("char {0}", i);
							sw.WriteLine("width 0");
							sw.WriteLine();
							continue;
						}

						sw.WriteLine("char {0}", (int)chEntry.Value);
						sw.WriteLine("width {0}", character.Image.Width);
						for (int y = 0; y < character.Image.Height; y++)
						{
							for (int x = 0; x < character.Image.Width; x++)
							{
								Color pixel = character.Image.GetPixel(x, y);
								if (pixel.A == 0) sw.Write("0");
								else sw.Write("1");
							}
							sw.WriteLine();
						}
						sw.WriteLine();
					}
				}
			}
		}
	}
}
