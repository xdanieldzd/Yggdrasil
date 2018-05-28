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
        public TileDataFile(GameDataManager gameDataManager, MemoryStream memoryStream, ArchiveFile archiveFile, int fileNumber) : base(gameDataManager, memoryStream, archiveFile, fileNumber) { }

        public byte[] Data { get; private set; }

        public override void Parse()
        {
            Stream.Seek(0, SeekOrigin.Begin);
            Data = new byte[Stream.Length];
            Stream.Read(Data, 0, Data.Length);
        }
    }
}
