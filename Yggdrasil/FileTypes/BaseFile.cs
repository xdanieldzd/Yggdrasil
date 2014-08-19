using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Yggdrasil.FileTypes
{
    public class BaseFile
    {
        public GameDataManager GameDataManager { get; private set; }
        public string Filename { get; private set; }
        public byte[] Data { get; private set; }

        bool isCompressed;
        public bool IsCompressed { get { return isCompressed; } }

        public BaseFile(GameDataManager gameDataManager, string path)
        {
            GameDataManager = gameDataManager;
            Filename = path;

            Data = Helpers.Decompressor.Decompress(Filename, out isCompressed);

            Parse();
        }

        public virtual void Parse() { }
    }
}
