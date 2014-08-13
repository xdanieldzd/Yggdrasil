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

        bool isCompressed;
        public bool IsCompressed { get { return isCompressed; } }

        public BaseFile(GameDataManager game, string path)
        {
            Game = game;
            Filename = path;

            Data = Helpers.Decompressor.Decompress(Filename, out isCompressed);

            Parse();
        }

        public virtual void Parse() { }
    }
}
