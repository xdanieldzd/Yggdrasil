using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using Yggdrasil.Attributes;

namespace Yggdrasil.FileHandling
{
    [MagicNumber("TBB1")]
    public class TableFile : BaseFile
    {
        public TableFile(GameDataManager gameDataManager, string path) : base(gameDataManager, path) { }

        static readonly List<Type> TableTypes;

        public string FileSignature { get; private set; }
        public uint Unknown { get; private set; }
        public uint NumTables { get; private set; }
        public uint FileSize { get; private set; }
        public uint[] TableOffsets { get; private set; }

        public BaseTable[] Tables { get; private set; }

        static TableFile()
        {
            TableTypes = Assembly.GetExecutingAssembly().GetTypes().Where(x => x.GetAttribute<MagicNumber>() != null).ToList();
        }

        public override void Parse()
        {
            FileSignature = Encoding.ASCII.GetString(Data, 0, 4);

            if (FileSignature != this.GetAttribute<MagicNumber>().Magic)
                throw new Exception(string.Format("Invalid file signature, got '{0}' expected '{1}'", FileSignature, this.GetAttribute<MagicNumber>()).Magic);

            Unknown = BitConverter.ToUInt32(Data, 4);
            NumTables = BitConverter.ToUInt32(Data, 8);
            FileSize = BitConverter.ToUInt32(Data, 12);

            TableOffsets = new uint[NumTables];
            for (int i = 0; i < NumTables; i++) TableOffsets[i] = BitConverter.ToUInt32(Data, 16 + (i * sizeof(uint)));

            Tables = new BaseTable[NumTables];
            for (int i = 0; i < NumTables; i++)
            {
                string tableSignature = Encoding.ASCII.GetString(Data, (int)TableOffsets[i], 4);
                Type tableType = TableTypes.FirstOrDefault(x => x.GetAttribute<MagicNumber>().Magic == tableSignature);
                Tables[i] = (BaseTable)Activator.CreateInstance(tableType, new object[] { GameDataManager, this, i });
            }
        }

        public override void Save()
        {
            List<byte> rebuilt = new List<byte>();

            rebuilt.AddRange(Encoding.ASCII.GetBytes(FileSignature));
            rebuilt.AddRange(BitConverter.GetBytes(Unknown));
            rebuilt.AddRange(BitConverter.GetBytes(NumTables));
            rebuilt.AddRange(BitConverter.GetBytes(FileSize));

            int tableDataLocation = ((int)(rebuilt.Count + (NumTables * sizeof(uint)))).Round(16);

            List<int> tableOffsets = new List<int>();
            List<byte> tableData = new List<byte>();

            int offset = tableDataLocation;
            for (int i = 0; i < NumTables; i++)
            {
                tableData.AddRange(Tables[i].Rebuild());
                tableData.AddRange(new byte[(tableData.Count.Round(16) - tableData.Count)]);

                tableOffsets.Add(offset);
                offset = tableDataLocation + tableData.Count;
            }

            foreach (int tableOffset in tableOffsets) rebuilt.AddRange(BitConverter.GetBytes(tableOffset));
            rebuilt.AddRange(new byte[(rebuilt.Count.Round(16) - rebuilt.Count)]);

            rebuilt.AddRange(tableData);

            Data = rebuilt.ToArray();
        }
    }
}
