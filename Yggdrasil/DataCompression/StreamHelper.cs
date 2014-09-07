using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Yggdrasil.Exceptions;
using Yggdrasil.Helpers;

namespace Yggdrasil.DataCompression
{
    public static class StreamHelper
    {
        public static MemoryStream Decompress(MemoryStream inputStream)
        {
            return Decompress(inputStream, (int)inputStream.Position, (int)inputStream.Length);
        }

        public static MemoryStream Decompress(MemoryStream inputStream, int count)
        {
            return Decompress(inputStream, (int)inputStream.Position, count);
        }

        public static MemoryStream Decompress(MemoryStream inputStream, int offset, int count)
        {
            MemoryStream outputStream = null;
            CompressionType checkByte = (CompressionType)inputStream.ReadByte();
            inputStream.Seek(-1, SeekOrigin.Current);

            try
            {
                switch (checkByte)
                {
                    case CompressionType.LZ77:
                        outputStream = new LZ77Stream(CompressionMode.Decompress);
                        outputStream.Write(inputStream.ToArray(), offset, count);
                        break;

                    case CompressionType.Huffman4Bit:
                    case CompressionType.Huffman8Bit:
                        outputStream = new HuffmanStream(CompressionMode.Decompress);
                        outputStream.Write(inputStream.ToArray(), offset, count);
                        break;

                    case CompressionType.RLE:
                        outputStream = new RLEStream(CompressionMode.Decompress);
                        outputStream.Write(inputStream.ToArray(), offset, count);
                        break;
                }
            }
            catch (CompressedStreamException csex)
            {
                Program.Logger.LogMessage(Logger.Level.Warning, csex.Message);
            }
            finally
            {
                if (outputStream == null || outputStream.Length == 0)
                    outputStream = new MemoryStream(inputStream.ToArray(), offset, count);
            }

            return outputStream;
        }
    }
}
