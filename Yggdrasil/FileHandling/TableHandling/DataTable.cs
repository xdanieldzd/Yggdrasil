using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Yggdrasil.Attributes;

namespace Yggdrasil.FileHandling.TableHandling
{
    [MagicNumber("TBL1")]
    public class DataTable : BaseTable
    {
        public DataTable(GameDataManager gameDataManager, TableFile tableFile, int number) : base(gameDataManager, tableFile, number) { Parse(); }

        public uint Unknown { get; private set; }
        public uint DataSize { get; private set; }
        public uint EntrySize { get; private set; }

        int numEntries;
        public int[] DataOffsets { get; private set; }
        public byte[][] Data { get; private set; }

        protected override void Parse()
        {
            Unknown = BitConverter.ToUInt32(TableFile.Data, (int)Offset + 4);
            DataSize = BitConverter.ToUInt32(TableFile.Data, (int)Offset + 8);
            EntrySize = BitConverter.ToUInt32(TableFile.Data, (int)Offset + 12);

            numEntries = (int)(DataSize / EntrySize);
            Data = new byte[numEntries][];
            DataOffsets = new int[numEntries];

            for (int i = 0; i < numEntries; i++)
            {
                Data[i] = new byte[EntrySize];
                DataOffsets[i] = (int)(Offset + 16 + (i * EntrySize));
                Buffer.BlockCopy(TableFile.Data, DataOffsets[i], Data[i], 0, (int)EntrySize);
            }
        }

        public override void Save()
        {
            for (int i = 0; i < numEntries; i++)
            {
                Buffer.BlockCopy(Data[i], 0, TableFile.Data, DataOffsets[i], (int)EntrySize);
            }
        }
    }
}
