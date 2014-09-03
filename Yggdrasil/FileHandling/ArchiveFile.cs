using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Yggdrasil.Attributes;
using Yggdrasil.DataCompression;

namespace Yggdrasil.FileHandling
{
    [MagicNumber("FBIN")]
    public class ArchiveFile : BaseFile
    {
        public ArchiveFile(GameDataManager gameDataManager, string path) : base(gameDataManager, path) { }

        public string FileSignature { get; private set; }
        public uint NumBlocks { get; private set; }
        public uint DataOffset { get; private set; }
        public uint[] BlockLengths { get; private set; }

        public TableFile[] TableFiles { get; private set; }

        public override void Parse()
        {
            FileSignature = Encoding.ASCII.GetString(Stream.ToArray(), 0, 4);

            string magic = this.GetAttribute<MagicNumber>().Magic;
            if (FileSignature != magic) throw new Exception(string.Format("Invalid file signature, got '{0}' expected '{1}'", FileSignature, magic));

            BinaryReader reader = new BinaryReader(Stream);

            reader.BaseStream.Seek(4, SeekOrigin.Begin);
            NumBlocks = reader.ReadUInt32();
            DataOffset = reader.ReadUInt32();

            BlockLengths = new uint[NumBlocks];
            for (int i = 0; i < NumBlocks; i++) BlockLengths[i] = reader.ReadUInt32();

            Stream.Seek(DataOffset, SeekOrigin.Begin);

            TableFiles = new TableFile[NumBlocks];
            for (int i = 0; i < NumBlocks; i++)
            {
                byte checkByte = (byte)Stream.ReadByte();
                if (checkByte != 0x10) throw new Exceptions.GameDataManagerException(string.Format("Compression type 0x{0:X2} in FBIN unhandled.", checkByte));
                Stream.Seek(-1, SeekOrigin.Current);

                LZ77Stream blockStream = new LZ77Stream(LZ77Stream.CompressionMode.Decompress);
                blockStream.Write(Stream.ToArray(), (int)Stream.Position, (int)BlockLengths[i]);
                Stream.Seek(BlockLengths[i], SeekOrigin.Current);

                TableFiles[i] = new TableFile(GameDataManager, blockStream, this, i);
            }
        }

        public override void Save()
        {
            List<byte> rebuilt = new List<byte>();

            rebuilt.AddRange(Encoding.ASCII.GetBytes(FileSignature));
            rebuilt.AddRange(BitConverter.GetBytes(NumBlocks));
            rebuilt.AddRange(BitConverter.GetBytes(DataOffset));

            List<int> blockLengths = new List<int>();
            List<byte> blocks = new List<byte>();

            for (int i = 0; i < NumBlocks; i++)
            {
                TableFiles[i].Save();

                LZ77Stream stream = new LZ77Stream(LZ77Stream.CompressionMode.Compress);
                TableFiles[i].Stream.CopyTo(stream);

                blocks.AddRange(stream.ToArray());

                blockLengths.Add((int)stream.Length);
            }

            foreach (int tableOffset in blockLengths) rebuilt.AddRange(BitConverter.GetBytes(tableOffset));
            rebuilt.AddRange(new byte[(rebuilt.Count.Round(16) - rebuilt.Count)]);

            rebuilt.AddRange(blocks);

            Stream = new MemoryStream(rebuilt.ToArray());
            using (FileStream fileStream = new FileStream(Filename, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
            {
                Stream.CopyTo(fileStream);
            }
        }
    }
}
