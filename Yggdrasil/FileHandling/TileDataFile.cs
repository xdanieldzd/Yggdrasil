using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Yggdrasil.FileHandling
{
    public class TileDataFile : BaseFile
    {
        public TileDataFile(GameDataManager gameDataManager, string path) : base(gameDataManager, path) { }

        public byte[] Data { get; private set; }

        public override void Parse()
        {
            this.Stream.Seek(0, SeekOrigin.Begin);
            Data = new byte[this.Stream.Length];
            this.Stream.Read(Data, 0, Data.Length);
        }
    }
}
