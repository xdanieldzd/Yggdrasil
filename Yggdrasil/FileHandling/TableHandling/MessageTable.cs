using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Yggdrasil.Helpers;
using Yggdrasil.Attributes;

namespace Yggdrasil.FileHandling.TableHandling
{
    [MagicNumber("MTBL")]
    public class MessageTable : BaseTable
    {
        public MessageTable(GameDataManager gameDataManager, TableFile tableFile, int number) : base(gameDataManager, tableFile, number) { Parse(); }

        public uint Unknown { get; private set; }
        public uint Size { get; private set; }
        public uint NumMessages { get; private set; }
        public uint Unknown2 { get; private set; }
        public uint[] MessageOffsets { get; private set; }

        public EtrianString[] Messages { get; private set; }

        protected override void Parse()
        {
            Unknown = BitConverter.ToUInt32(TableFile.Data, (int)Offset + 4);
            Size = BitConverter.ToUInt32(TableFile.Data, (int)Offset + 8);
            NumMessages = BitConverter.ToUInt32(TableFile.Data, (int)Offset + 12);
            Unknown2 = BitConverter.ToUInt32(TableFile.Data, (int)Offset + 16);

            MessageOffsets = new uint[NumMessages];
            for (int i = 0; i < NumMessages; i++) MessageOffsets[i] = BitConverter.ToUInt32(TableFile.Data, (int)Offset + 20 + (i * sizeof(uint)));

            Messages = new EtrianString[NumMessages];
            for (int i = 0; i < NumMessages; i++)
            {
                if (MessageOffsets[i] == 0)
                    Messages[i] = string.Empty;
                else
                    Messages[i] = new EtrianString(TableFile.Data, (int)(Offset + 0x10 + MessageOffsets[i]));
            }
        }
    }
}
