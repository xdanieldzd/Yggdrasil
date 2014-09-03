using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Yggdrasil.FileHandling
{
    public abstract class BaseTable
    {
        public GameDataManager GameDataManager { get; private set; }
        public TableFile TableFile { get; private set; }

        public int Number { get; private set; }
        public uint Offset { get; private set; }

        public string TableSignature { get; private set; }

        public BaseTable(GameDataManager gameDataManager, TableFile tableFile, int number)
        {
            GameDataManager = gameDataManager;
            TableFile = tableFile;

            Number = number;
            Offset = TableFile.TableOffsets[number];

            TableSignature = Encoding.ASCII.GetString(TableFile.Stream.ToArray(), (int)Offset, 4);
        }

        protected virtual void Parse()
        {
            throw new NotImplementedException(string.Format("Parsing not implemented for table type {0}", this.GetType().FullName));
        }

        public virtual byte[] Rebuild()
        {
            throw new NotImplementedException(string.Format("Rebuilding not implemented for table type {0}", this.GetType().FullName));
        }
    }
}
