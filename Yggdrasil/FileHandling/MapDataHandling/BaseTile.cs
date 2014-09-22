using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Yggdrasil.FileHandling.MapDataHandling
{
    public class BaseTile
    {
        public GameDataManager GameDataManager { get; private set; }
        public MapDataFile MapDataFile { get; private set; }

        public int Offset { get; private set; }

        public byte[] Data { get; private set; }

        public MapDataFile.TileTypes TileType { get; private set; }

        public BaseTile(GameDataManager gameDataManager, MapDataFile mapDataFile, int offset)
        {
            this.GameDataManager = gameDataManager;
            this.MapDataFile = mapDataFile;
            this.Offset = offset;

            Load();
        }

        protected virtual void Load()
        {
            this.MapDataFile.Stream.Seek(this.Offset, SeekOrigin.Begin);
            this.Data = new byte[16];
            this.MapDataFile.Stream.Read(this.Data, 0, this.Data.Length);

            this.TileType = (MapDataFile.TileTypes)this.Data[0];
        }
    }
}
