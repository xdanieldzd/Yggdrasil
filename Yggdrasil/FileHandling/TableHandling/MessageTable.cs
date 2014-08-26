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

        public override byte[] Rebuild()
        {
            List<byte> rebuilt = new List<byte>();

            rebuilt.AddRange(Encoding.ASCII.GetBytes(TableSignature));
            rebuilt.AddRange(BitConverter.GetBytes(Unknown));
            rebuilt.AddRange(BitConverter.GetBytes(Size));
            rebuilt.AddRange(BitConverter.GetBytes(NumMessages));
            rebuilt.AddRange(BitConverter.GetBytes(Unknown2));

            int messageDataLocation = ((int)(rebuilt.Count + (NumMessages * sizeof(uint)))).Round(16) - 16;

            List<int> messageOffsets = new List<int>();
            List<byte> messageData = new List<byte>();

            int offset = messageDataLocation;
            for (int i = 0; i < NumMessages; i++)
            {
                if (Messages[i] != string.Empty)
                {
                    foreach (ushort val in Messages[i].RawData) messageData.AddRange(BitConverter.GetBytes(val));
                    messageData.AddRange(new byte[2]);

                    int padding = (messageData.Count.Round(16) - messageData.Count);
                    messageData.AddRange(new byte[padding]);

                    messageOffsets.Add(offset);
                    offset = messageDataLocation + messageData.Count;
                }
                else
                {
                    messageOffsets.Add(0);
                }
            }

            foreach (int messageOffset in messageOffsets) rebuilt.AddRange(BitConverter.GetBytes(messageOffset));
            rebuilt.AddRange(new byte[(rebuilt.Count.Round(16) - rebuilt.Count)]);

            rebuilt.AddRange(messageData);

            return rebuilt.ToArray();
        }
    }
}
