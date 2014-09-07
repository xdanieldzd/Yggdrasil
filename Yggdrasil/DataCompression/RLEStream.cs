using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Yggdrasil.Exceptions;

namespace Yggdrasil.DataCompression
{
    public class RLEStream : CompressedStream
    {
        public RLEStream(CompressionMode compressionMode) : base(compressionMode) { }

        public const byte MaxRunLength = 0x7F;
        public const byte CompressionFlag = 0x80;

        public override bool Decompress(byte[] buffer, int offset, out byte[] output)
        {
            int dataLen = (BitConverter.ToInt32(buffer, offset + 1) & 0xFFFFFF);
            output = new byte[dataLen];

            int inOffset = offset + 4, outOffset = 0;

            while (outOffset < dataLen)
            {
                if (inOffset >= buffer.Length)
                {
                    for (int i = outOffset; i < dataLen; i++) output[outOffset++] = buffer[inOffset - 1];
                    return true;
                }

                byte controlByte = buffer[inOffset++];
                bool compFlag = (controlByte & CompressionFlag) != 0;
                byte byteCount = (byte)(controlByte & MaxRunLength);

                if (compFlag)
                {
                    if (inOffset >= buffer.Length)
                    {
                        for (int i = outOffset; i < dataLen; i++) output[outOffset++] = buffer[inOffset - 1];
                        return true;
                    }

                    for (int i = 0; i < byteCount + 3; i++)
                    {
                        if (inOffset >= buffer.Length)
                        {
                            for (int j = outOffset; j < dataLen; j++) output[outOffset++] = buffer[inOffset - 1];
                            return true;
                        }

                        output[outOffset++] = buffer[inOffset];
                    }
                    inOffset++;
                }
                else
                {
                    for (int i = 0; i < byteCount + 1; i++)
                    {
                        if (inOffset >= buffer.Length || outOffset >= output.Length)
                        {
                            for (int j = outOffset; j < dataLen; j++) output[outOffset++] = buffer[inOffset - 1];
                            return true;
                        }

                        output[outOffset++] = buffer[inOffset++];
                    }
                }
            }

            return true;
        }

        public override bool Compress(byte[] buffer, int offset, int count, out byte[] output)
        {
            if (count > 0xFFFFFF) throw new CompressedStreamException("Input data too large");

            MemoryStream outStream = new MemoryStream();

            outStream.WriteByte((byte)CompressionType.RLE);
            outStream.WriteByte((byte)(count & 0xFF));
            outStream.WriteByte((byte)((count >> 8) & 0xFF));
            outStream.WriteByte((byte)((count >> 16) & 0xFF));

            int inOffset = 0;
            byte runLength = 0, rawDataLength = 0;
            bool compFlag = false;

            while (inOffset < count)
            {
                for (int i = 0; i <= MaxRunLength; i++)
                {
                    if (inOffset + rawDataLength + 2 >= count)
                    {
                        rawDataLength = (byte)(count - inOffset);
                        break;
                    }

                    if (buffer[inOffset + i] == buffer[inOffset + i + 1] && buffer[inOffset + i] == buffer[inOffset + i + 2])
                    {
                        compFlag = true;
                        break;
                    }
                    rawDataLength++;
                }

                if (rawDataLength > 0)
                {
                    outStream.WriteByte((byte)(rawDataLength - 1));
                    for (int i = 0; i < rawDataLength; i++) outStream.WriteByte(buffer[inOffset++]);
                    rawDataLength = 0;
                }

                if (compFlag)
                {
                    runLength = 3;
                    for (int i = 3; i < MaxRunLength + 3; i++)
                    {
                        if (inOffset + runLength >= count)
                        {
                            runLength = (byte)(count - inOffset);
                            break;
                        }

                        if (buffer[inOffset] != buffer[inOffset + runLength]) break;
                        runLength++;
                    }

                    outStream.WriteByte((byte)(CompressionFlag | runLength - 3));
                    outStream.WriteByte(buffer[inOffset]);
                    inOffset += runLength;
                    compFlag = false;
                }
            }

            output = outStream.ToArray();
            return true;
        }

        public static void Test()
        {
            Yggdrasil.DataCompression.RLEStream rle = new DataCompression.RLEStream(DataCompression.CompressionMode.Decompress);
            byte[] data = System.IO.File.ReadAllBytes(@"E:\Translations\NDS Etrian Odyssey Hacking\Font10x9_00.cmp");
            rle.Write(data, 0, data.Length);

            Yggdrasil.DataCompression.RLEStream rlecomp = new DataCompression.RLEStream(DataCompression.CompressionMode.Compress);
            rlecomp.Write(rle.ToArray(), 0, (int)rle.Length);
            System.IO.FileStream outstream = System.IO.File.Open(@"E:\Translations\NDS Etrian Odyssey Hacking\Font10x9_00_recomp.cmp", System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.ReadWrite, System.IO.FileShare.ReadWrite);
            outstream.Write(rlecomp.ToArray(), 0, (int)rlecomp.Length);
            outstream.Close();

            Yggdrasil.DataCompression.RLEStream rlecheck = new DataCompression.RLEStream(DataCompression.CompressionMode.Decompress);
            byte[] datacheck = System.IO.File.ReadAllBytes(@"E:\Translations\NDS Etrian Odyssey Hacking\Font10x9_00_recomp.cmp");
            rlecheck.Write(datacheck, 0, datacheck.Length);

            outstream = System.IO.File.Open(@"E:\Translations\NDS Etrian Odyssey Hacking\Font10x9_00_recomp-decomp.cmp", System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.ReadWrite, System.IO.FileShare.ReadWrite);
            outstream.Write(rlecheck.ToArray(), 0, (int)rlecheck.Length);
            outstream.Close();
            /*
            System.IO.MemoryStream inputStream = new System.IO.MemoryStream();
            byte[] data = System.IO.File.ReadAllBytes(@"D:\ROMs\N64\N64Ren\Super Mario 64 (U) [!].z64");
            inputStream.Write(data, 0, data.Length);

            Yggdrasil.DataCompression.RLEStream rleCompressed = new DataCompression.RLEStream(DataCompression.CompressionMode.Compress);
            rleCompressed.Write(inputStream.ToArray(), 0, (int)inputStream.Length);

            Yggdrasil.DataCompression.RLEStream rleDecompressed = new DataCompression.RLEStream(DataCompression.CompressionMode.Decompress);
            rleDecompressed.Write(rleCompressed.ToArray(), 0, (int)rleCompressed.Length);

            System.IO.FileStream outputStream = System.IO.File.Open(@"D:\ROMs\N64\Super Mario 64 (U) [!] RLETEST.z64", System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.ReadWrite, System.IO.FileShare.ReadWrite);
            outputStream.Write(rleDecompressed.ToArray(), 0, (int)rleDecompressed.Length);
            outputStream.Close();*/
        }
    }
}
