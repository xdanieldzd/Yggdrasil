using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            this.GameDataManager = gameDataManager;
            this.TableFile = tableFile;
            this.Number = number;

            Offset = TableFile.TableOffsets[number];

            TableSignature = Encoding.ASCII.GetString(TableFile.Data, (int)Offset, 4);
        }

        protected virtual void Parse()
        {
            throw new NotImplementedException(string.Format("Parsing not implemented for file type {0}", this.GetType().FullName));
        }

        public virtual void Save()
        {
            throw new NotImplementedException(string.Format("Saving not implemented for file type {0}", this.GetType().FullName));
        }
    }
}
