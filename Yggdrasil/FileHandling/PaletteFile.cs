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
        public PaletteFile(GameDataManager gameDataManager, MemoryStream memoryStream, ArchiveFile archiveFile, int fileNumber) : base(gameDataManager, memoryStream, archiveFile, fileNumber) { }

        public List<Color> Colors { get; private set; }

        public override void Parse()
        {
            Colors = new List<Color>();

            Stream.Seek(0, SeekOrigin.Begin);
            while (Stream.Position < Stream.Length)
            {
                ushort color = (ushort)(Stream.ReadByte() | (Stream.ReadByte() << 8));
                Colors.Add(ConvertBGR565(color));
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
