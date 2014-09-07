using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Yggdrasil.Attributes;
using Yggdrasil.DataCompression;
using Yggdrasil.Helpers;
using Yggdrasil.Exceptions;

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

        public BaseFile[] Blocks { get; private set; }

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

            Blocks = new BaseFile[NumBlocks];
            for (int i = 0; i < NumBlocks; i++)
            {
                MemoryStream blockStream = null;
                CompressionType checkByte = (CompressionType)Stream.ReadByte();
                Stream.Seek(-1, SeekOrigin.Current);

                try
                {
                    switch (checkByte)
                    {
                        case CompressionType.LZ77:
                            blockStream = new LZ77Stream(CompressionMode.Decompress);
                            blockStream.Write(Stream.ToArray(), (int)Stream.Position, (int)BlockLengths[i]);
                            break;

                        case CompressionType.RLE:
                            blockStream = new RLEStream(CompressionMode.Decompress);
                            blockStream.Write(Stream.ToArray(), (int)Stream.Position, (int)BlockLengths[i]);
                            break;
                    }
                }
                catch (CompressedStreamException csex)
                {
                    Program.Logger.LogMessage(Logger.Level.Warning, csex.Message);
                }
                finally
                {
                    if (blockStream == null || blockStream.Length == 0)
                        blockStream = new MemoryStream(Stream.ToArray(), (int)Stream.Position, (int)BlockLengths[i]);
                }

                Stream.Seek(BlockLengths[i], SeekOrigin.Current);

                string fileSignature = Encoding.ASCII.GetString(blockStream.ToArray(), 0, 4);
                Type fileType = System.Reflection.Assembly.GetExecutingAssembly().GetTypes().FirstOrDefault(x => x.GetAttribute<MagicNumber>() != null && x.GetAttribute<MagicNumber>().Magic == fileSignature);
                if (fileType != null) Blocks[i] = (BaseFile)Activator.CreateInstance(fileType, new object[] { GameDataManager, blockStream, this, i });
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
                Blocks[i].Save();

                CompressedStream stream = (CompressedStream)
                    (Blocks[i].Stream is CompressedStream ?
                    Activator.CreateInstance(Blocks[i].Stream.GetType(), new object[] { CompressionMode.Compress }) :
                    new MemoryStream());

                Blocks[i].Stream.CopyTo(stream);

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
