using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Yggdrasil.FileTypes
{
    public class BaseFile
    {
        public GameDataManager Game { get; private set; }
        public string Filename { get; private set; }
        public byte[] Data { get; private set; }

        public BIN ParentArchive { get; private set; }
        public uint ParentArchiveOffset { get; private set; }

        bool isCompressed;
        public bool IsCompressed { get { return isCompressed; } }

        public BaseFile(GameDataManager game, string path)
        {
            Game = game;
            Filename = path;

            Data = Helpers.Decompressor.Decompress(Filename, out isCompressed);

            Parse();
        }

        public BaseFile(GameDataManager game, BIN parentArchive, uint offset)
        {
            Game = game;

            ParentArchive = parentArchive;
            ParentArchiveOffset = offset;

            Filename = string.Format("{0}_0x{1:X}", ParentArchive.Filename, ParentArchiveOffset);

            Data = Helpers.Decompressor.Decompress(ParentArchive.Data, (int)ParentArchiveOffset, out isCompressed);

            Parse();
        }

        public virtual void Parse() { }
    }
}
