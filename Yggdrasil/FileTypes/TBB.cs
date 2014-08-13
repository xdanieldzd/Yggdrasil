using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Yggdrasil.FileTypes
{
    public partial class TBB : BaseFile
    {
        public TBB(GameDataManager game, string path) : base(game, path) { }

        public string Tag { get; private set; }
        public uint Unknown { get; private set; }
        public uint NumTables { get; private set; }
        public uint FileSize { get; private set; }
        public uint[] TableOffsets { get; private set; }

        public ITable[] Tables { get; private set; }

        bool valid = false;
        public bool IsValid() { return valid; }

        public override void Parse()
        {
            Tag = Encoding.ASCII.GetString(Data, 0, 4);

            if (Tag != "TBB1") return;

            Unknown = BitConverter.ToUInt32(Data, 4);
            NumTables = BitConverter.ToUInt32(Data, 8);
            FileSize = BitConverter.ToUInt32(Data, 12);

            TableOffsets = new uint[NumTables];
            for (int i = 0; i < NumTables; i++) TableOffsets[i] = BitConverter.ToUInt32(Data, 16 + (i * sizeof(uint)));

            Tables = new ITable[NumTables];
            for (int i = 0; i < NumTables; i++)
            {
                string tableTag = Encoding.ASCII.GetString(Data, (int)TableOffsets[i], 4);
                string typeName = string.Format("{0}+{1}", this.GetType().FullName, tableTag);
                Type type = Type.GetType(typeName);
                if (type != null) Tables[i] = (ITable)Activator.CreateInstance(type, new object[] { this, TableOffsets[i] });
            }

            valid = true;
        }

        public interface ITable
        {
            TBB GetParent();
            string GetTag();
        }
    }
}
