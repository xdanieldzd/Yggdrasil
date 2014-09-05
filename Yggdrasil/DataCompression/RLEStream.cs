using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Yggdrasil.DataCompression
{
    public class RLEStream : CompressedStream
    {
        public RLEStream(CompressionMode compressionMode) : base(compressionMode) { }

        public override byte[] Decompress(byte[] buffer, int sOffset)
        {
            int dataLen = (BitConverter.ToInt32(buffer, sOffset + 1) & 0xFFFFFF);
            byte[] decData = new byte[dataLen];

            int inOffset = sOffset + 4, outOffset = 0;

            while (outOffset < dataLen)
            {
                byte data = buffer[inOffset++];
                bool compFlag = (data & 0x80) != 0;
                byte count = (byte)(data & 0x7F);

                if (compFlag)
                {
                    for (int i = 0; i < count + 3; i++) decData[outOffset++] = buffer[inOffset];
                    inOffset++;
                }
                else
                    for (int i = 0; i < count + 1; i++) decData[outOffset++] = buffer[inOffset++];
            }

            return decData;
        }
    }
}
