using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Yggdrasil.Helpers
{
    public static partial class Decompressor
    {
        public static byte[] Decompress(string path, out bool isCompressed)
        {
            BinaryReader reader = new BinaryReader(File.Open(path, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite));
            byte[] data = new byte[reader.BaseStream.Length];
            if (reader.Read(data, 0, data.Length) != data.Length)
            {
                throw new Exception("BinaryReader Read length mismatch");
            }
            reader.Close();

            return Decompress(data, out isCompressed);
        }

        public static byte[] Decompress(byte[] data, out bool isCompressed)
        {
            return Decompress(data, 0, out isCompressed);
        }

        public static byte[] Decompress(byte[] data, int offset, out bool isCompressed)
        {
            isCompressed = true;

            switch (data[offset])
            {
                case 0x10: return ProcessLZ77(data, offset);

                case 0x24:
                case 0x28: return ProcessHuffman(data, offset);

                case 0x30: return ProcessRLE(data, offset);

                default:
                    isCompressed = false;
                    byte[] outData = new byte[data.Length - offset];
                    Buffer.BlockCopy(data, offset, outData, 0, outData.Length);
                    return data;
            }
        }
    }
}
