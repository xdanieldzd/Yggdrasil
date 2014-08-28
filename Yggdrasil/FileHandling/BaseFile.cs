using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace Yggdrasil.FileHandling
{
    [DebuggerDisplay("{Filename}")]
    public abstract class BaseFile
    {
        public GameDataManager GameDataManager { get; private set; }
        public string Filename { get; private set; }
        public byte[] Data { get; set; }

        bool isCompressed;
        public bool IsCompressed { get { return isCompressed; } }

        public BaseFile(GameDataManager gameDataManager, string path)
        {
            GameDataManager = gameDataManager;
            Filename = path;

            Data = DataCompression.Decompressor.Decompress(Filename, out isCompressed);

            Parse();
        }

        public virtual void Parse()
        {
            throw new NotImplementedException(string.Format("Parsing not implemented for file type {0}", this.GetType().FullName));
        }

        public virtual void Save()
        {
            throw new NotImplementedException(string.Format("Saving not implemented for file type {0}", this.GetType().FullName));
        }
    }
}
