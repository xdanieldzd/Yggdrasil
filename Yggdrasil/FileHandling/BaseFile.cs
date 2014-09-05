using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

using Yggdrasil.DataCompression;

namespace Yggdrasil.FileHandling
{
    [DebuggerDisplay("{Filename}")]
    public abstract class BaseFile
    {
        public GameDataManager GameDataManager { get; private set; }
        public string Filename { get; private set; }
        public bool InArchive { get; private set; }
        public ArchiveFile ArchiveFile { get; private set; }
        public int FileNumber { get; private set; }
        public MemoryStream Stream { get; set; }

        public BaseFile(GameDataManager gameDataManager, string path)
        {
            GameDataManager = gameDataManager;
            Filename = path;
            InArchive = false;
            FileNumber = -1;

            using (FileStream fileStream = File.OpenRead(Filename))
            {
                if (Path.GetExtension(Filename) == ".cmp" && fileStream.ReadByte() == 0x10)
                    Stream = new LZ77Stream(LZ77Stream.CompressionMode.Decompress);
                else
                    Stream = new MemoryStream();

                fileStream.CopyTo(Stream);

                Parse();
            }
        }

        public BaseFile(GameDataManager gameDataManager, MemoryStream memoryStream, ArchiveFile archiveFile, int fileNumber)
        {
            if (memoryStream == null) throw new ArgumentException("MemoryStream cannot be null.");

            GameDataManager = gameDataManager;
            Filename = archiveFile.Filename;
            InArchive = true;
            ArchiveFile = archiveFile;
            FileNumber = fileNumber;
            Stream = memoryStream;

            Parse();
        }

        public virtual void Parse()
        {
            throw new NotImplementedException(string.Format("Parsing not implemented for file type {0}", this.GetType().FullName));
        }

        public virtual void Save()
        {
            throw new NotImplementedException(string.Format("Saving not implemented for file type {0}", this.GetType().FullName));
        }
    }
}
