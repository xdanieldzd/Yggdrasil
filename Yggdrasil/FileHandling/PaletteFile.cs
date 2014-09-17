using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;

namespace Yggdrasil.FileHandling
{
    public class PaletteFile : BaseFile
    {
        public PaletteFile(GameDataManager gameDataManager, string path) : base(gameDataManager, path) { }

        public List<Color> Colors { get; private set; }

        public override void Parse()
        {
            Colors = new List<Color>();

            this.Stream.Seek(0, SeekOrigin.Begin);
            while (this.Stream.Position < this.Stream.Length)
            {
                ushort color = (ushort)(this.Stream.ReadByte() | (this.Stream.ReadByte() << 8));
                Colors.Add(ConvertBGR565(color));
            }

            if (Colors.Count < 256)
            {
                if (Colors.Count < 16)
                {
                    while (Colors.Count < 16) Colors.Add(Color.Gray);
                }
                else if (Colors.Count > 16)
                {
                    while (Colors.Count < 256) Colors.Add(Color.Gray);
                }
            }
        }

        public static Color ConvertBGR565(ushort val)
        {
            return Color.FromArgb(
                255,
                (byte)((val & 0x001F) << 3),
                (byte)(((val & 0x03E0) >> 5) << 3),
                (byte)(((val & 0x7C00) >> 10) << 3)
                );
        }
    }
}
