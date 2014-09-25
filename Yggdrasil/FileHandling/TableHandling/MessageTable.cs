using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Yggdrasil.Attributes;
using Yggdrasil.TextHandling;

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
        EtrianString[] originalMessages;

        public bool HasChanges { get { return !originalMessages.CompareElements(Messages, (IEqualityComparer<EtrianString>)new EtrianString.EtrianStringComparer()); } }

        protected override void Parse()
        {
            BinaryReader reader = new BinaryReader(TableFile.Stream);

            reader.BaseStream.Seek(Offset + 4, SeekOrigin.Begin);
            Unknown = reader.ReadUInt32();
            Size = reader.ReadUInt32();
            NumMessages = reader.ReadUInt32();
            Unknown2 = reader.ReadUInt32();

            MessageOffsets = new uint[NumMessages];
            for (int i = 0; i < NumMessages; i++)
            {
                reader.BaseStream.Seek(Offset + 20 + (i * sizeof(uint)), SeekOrigin.Begin);
                MessageOffsets[i] = reader.ReadUInt32();
            }

            byte[] fileData = TableFile.Stream.ToArray();

            Messages = new EtrianString[NumMessages];
            originalMessages = new EtrianString[NumMessages];

            for (int i = 0; i < NumMessages; i++)
            {
                if (MessageOffsets[i] == 0)
                    Messages[i] = new EtrianString(string.Empty);
                else
                    Messages[i] = new EtrianString(fileData, (int)(Offset + 0x10 + MessageOffsets[i]), GameDataManager.GameDataPropertyChanged);

                originalMessages[i] = new EtrianString(Messages[i].RawData);
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
                if (Messages[i].ConvertedString != string.Empty)
                {
                    originalMessages[i] = new EtrianString(Messages[i].RawData);

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
