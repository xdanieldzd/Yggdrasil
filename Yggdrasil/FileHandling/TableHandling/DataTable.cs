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
            DataOffsets = new int[numEntries];
            Data = new byte[numEntries][];

            for (int i = 0; i < numEntries; i++)
            {
                DataOffsets[i] = (int)(Offset + 16 + (i * EntrySize));

                Data[i] = new byte[EntrySize];
                Buffer.BlockCopy(TableFile.Data, DataOffsets[i], Data[i], 0, (int)EntrySize);
            }
        }

        public override byte[] Rebuild()
        {
            List<byte> rebuilt = new List<byte>();

            rebuilt.AddRange(Encoding.ASCII.GetBytes(TableSignature));
            rebuilt.AddRange(BitConverter.GetBytes(Unknown));
            rebuilt.AddRange(BitConverter.GetBytes(DataSize));
            rebuilt.AddRange(BitConverter.GetBytes(EntrySize));

            foreach (byte[] entryData in Data) rebuilt.AddRange(entryData);
            rebuilt.AddRange(new byte[(rebuilt.Count.Round(16) - rebuilt.Count)]);

            return rebuilt.ToArray();
        }
    }
}
