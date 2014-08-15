using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yggdrasil.FileTypes
{
    public class BIN : BaseFile
    {
        public BIN(GameDataManager game, string path) : base(game, path) { }

        public string Tag { get; private set; }
        public uint NumFiles { get; private set; }
        public uint[] FileOffsets { get; private set; }

        public override void Parse()
        {
            Tag = Encoding.ASCII.GetString(Data, 0, 4);

            if (Tag != "FBIN") return;

            NumFiles = BitConverter.ToUInt32(Data, 4);

            FileOffsets = new uint[NumFiles];
            for (int i = 0; i < NumFiles; i++)
            {
                uint offset = (i > 0 ? FileOffsets[i - 1] : 0);
                offset += BitConverter.ToUInt32(Data, 8 + (i * sizeof(uint)));
                FileOffsets[i] = offset;
            }
        }
    }
}
