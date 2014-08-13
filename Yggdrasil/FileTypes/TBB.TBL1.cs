using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Yggdrasil.Helpers;

namespace Yggdrasil.FileTypes
{
    public partial class TBB
    {
        public class TBL1 : ITable
        {
            TBB parent;
            public TBB GetParent() { return parent; }

            public uint Offset { get; private set; }

            string tag;
            public string GetTag() { return tag; }

            public uint Unknown { get; private set; }
            public uint DataSize { get; private set; }
            public uint EntrySize { get; private set; }

            int numEntries;
            public byte[][] Data { get; private set; }

            public TBL1(TBB parent, uint offset)
            {
                this.parent = parent;
                Offset = offset;

                tag = Encoding.ASCII.GetString(parent.Data, (int)Offset, 4);

                Unknown = BitConverter.ToUInt32(parent.Data, (int)Offset + 4);
                DataSize = BitConverter.ToUInt32(parent.Data, (int)Offset + 8);
                EntrySize = BitConverter.ToUInt32(parent.Data, (int)Offset + 12);

                numEntries = (int)(DataSize / EntrySize);
                Data = new byte[numEntries][];

                for (int i = 0; i < numEntries; i++)
                {
                    Data[i] = new byte[EntrySize];
                    Buffer.BlockCopy(parent.Data, (int)(Offset + 16 + (i * EntrySize)), Data[i], 0, (int)EntrySize);
                }
            }

            public void Save()
            {
                for (int i = 0; i < numEntries; i++)
                {
                    Buffer.BlockCopy(Data[i], 0, parent.Data, (int)(Offset + 16 + (i * EntrySize)), (int)EntrySize);
                }
            }
        }
    }
}
