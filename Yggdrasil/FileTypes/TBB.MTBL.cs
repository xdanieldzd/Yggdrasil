using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Yggdrasil.Helpers;

namespace Yggdrasil.FileTypes
{
    public partial class TBB
    {
        public class MTBL : ITable
        {
            TBB parent;
            public TBB GetParent() { return parent; }

            public uint Offset { get; private set; }

            string tag;
            public string GetTag() { return tag; }

            public uint Unknown { get; private set; }
            public uint Size { get; private set; }
            public uint NumMessages { get; private set; }
            public uint Unknown2 { get; private set; }
            public uint[] MessageOffsets { get; private set; }

            public EtrianString[] Messages { get; private set; }

            public MTBL(TBB parent, uint offset)
            {
                this.parent = parent;
                Offset = offset;

                tag = Encoding.ASCII.GetString(parent.Data, (int)Offset, 4);

                Unknown = BitConverter.ToUInt32(parent.Data, (int)Offset + 4);
                Size = BitConverter.ToUInt32(parent.Data, (int)Offset + 8);
                NumMessages = BitConverter.ToUInt32(parent.Data, (int)Offset + 12);
                Unknown2 = BitConverter.ToUInt32(parent.Data, (int)Offset + 16);

                MessageOffsets = new uint[NumMessages];
                for (int i = 0; i < NumMessages; i++) MessageOffsets[i] = BitConverter.ToUInt32(parent.Data, (int)Offset + 20 + (i * sizeof(uint)));

                Messages = new EtrianString[NumMessages];
                for (int i = 0; i < NumMessages; i++)
                {
                    if (MessageOffsets[i] == 0)
                        Messages[i] = string.Empty;
                    else
                        Messages[i] = new EtrianString(parent.Data, (int)(Offset + 0x10 + MessageOffsets[i]));
                }
            }
        }
    }
}
