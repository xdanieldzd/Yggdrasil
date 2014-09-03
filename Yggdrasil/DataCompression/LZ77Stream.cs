using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Yggdrasil.Exceptions;

namespace Yggdrasil.DataCompression
{
    public class LZ77Stream : MemoryStream
    {
        // Compression code based on https://code.google.com/p/dsdecmp/

        //Copyright (c) 2010 Nick Kraayenbrink
        //
        //Permission is hereby granted, free of charge, to any person obtaining a copy
        //of this software and associated documentation files (the "Software"), to deal
        //in the Software without restriction, including without limitation the rights
        //to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
        //copies of the Software, and to permit persons to whom the Software is
        //furnished to do so, subject to the following conditions:
        //
        //The above copyright notice and this permission notice shall be included in
        //all copies or substantial portions of the Software.
        //
        //THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
        //IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
        //FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
        //AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
        //LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
        //OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
        //THE SOFTWARE.

        public enum CompressionMode { Decompress, Compress };

        CompressionMode compressionMode;
        MemoryStream originalStream;

        public LZ77Stream(CompressionMode compressionMode)
        {
            this.compressionMode = compressionMode;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (compressionMode == CompressionMode.Decompress)
            {
                originalStream = new MemoryStream(buffer, offset, count);

                byte[] decompressed = Decompress(buffer, offset);
                base.Write(decompressed, 0, decompressed.Length);
            }
            else if (compressionMode == CompressionMode.Compress)
            {
                originalStream = new MemoryStream(buffer, offset, count);

                byte[] compressed = Compress(buffer, offset, count);
                base.Write(compressed, 0, compressed.Length);
            }
        }

        private byte[] Decompress(byte[] buffer, int sOffset)
        {
            int dataLen = (BitConverter.ToInt32(buffer, sOffset + 1) & 0xFFFFFF);
            byte[] decData = new byte[dataLen];

            int i, j, xIn, xOut;
            xIn = sOffset + 4;
            xOut = 0;
            int length, offset, windowOffset, data;
            byte d;

            while (dataLen > 0)
            {
                d = buffer[xIn++];
                if (d != 0)
                {
                    for (i = 0; i < 8; i++)
                    {
                        if ((d & 0x80) != 0)
                        {
                            data = ((buffer[xIn] << 8) | buffer[xIn + 1]);
                            xIn += 2;
                            length = (data >> 12) + 3;
                            offset = data & 0xFFF;
                            windowOffset = xOut - offset - 1;
                            for (j = 0; j < length; j++)
                            {
                                decData[xOut++] = decData[windowOffset++];
                                dataLen--;
                                if (dataLen == 0) return decData;
                            }
                        }
                        else
                        {
                            decData[xOut++] = buffer[xIn++];
                            dataLen--;
                            if (dataLen == 0) return decData;
                        }
                        d <<= 1;
                    }
                }
                else
                {
                    for (i = 0; i < 8; i++)
                    {
                        decData[xOut++] = buffer[xIn++];
                        dataLen--;
                        if (dataLen == 0) return decData;
                    }
                }
            }

            return decData;
        }

        private unsafe byte[] Compress(byte[] buffer, int offset, int count)
        {
            if (count > 0xFFFFFF) throw new LZ77StreamException("Input data too large");

            MemoryStream outstream = new MemoryStream();

            outstream.WriteByte(0x10);
            outstream.WriteByte((byte)(count & 0xFF));
            outstream.WriteByte((byte)((count >> 8) & 0xFF));
            outstream.WriteByte((byte)((count >> 16) & 0xFF));

            int compressedLength = 4;

            fixed (byte* instart = &buffer[0])
            {
                byte[] outbuffer = new byte[8 * 2 + 1];
                outbuffer[0] = 0;
                int bufferlength = 1, bufferedBlocks = 0;
                int readBytes = 0;
                while (readBytes < count)
                {
                    if (bufferedBlocks == 8)
                    {
                        outstream.Write(outbuffer, 0, bufferlength);
                        compressedLength += bufferlength;

                        outbuffer[0] = 0;
                        bufferlength = 1;
                        bufferedBlocks = 0;
                    }

                    int disp;
                    int oldLength = Math.Min(readBytes, 0x1000);
                    int length = GetOccurrenceLength(instart + readBytes, (int)Math.Min(count - readBytes, 0x12), instart + readBytes - oldLength, oldLength, out disp);

                    if (length < 3)
                    {
                        outbuffer[bufferlength++] = *(instart + (readBytes++));
                    }
                    else
                    {
                        readBytes += length;

                        outbuffer[0] |= (byte)(1 << (7 - bufferedBlocks));

                        outbuffer[bufferlength] = (byte)(((length - 3) << 4) & 0xF0);
                        outbuffer[bufferlength] |= (byte)(((disp - 1) >> 8) & 0x0F);
                        bufferlength++;
                        outbuffer[bufferlength] = (byte)((disp - 1) & 0xFF);
                        bufferlength++;
                    }
                    bufferedBlocks++;
                }

                if (bufferedBlocks > 0)
                {
                    outstream.Write(outbuffer, 0, bufferlength);
                    compressedLength += bufferlength;
                }
            }

            return outstream.ToArray();
        }

        public unsafe int GetOccurrenceLength(byte* newPtr, int newLength, byte* oldPtr, int oldLength, out int disp, int minDisp = 1)
        {
            disp = 0;
            if (newLength == 0) return 0;
            int maxLength = 0;

            for (int i = 0; i < oldLength - minDisp; i++)
            {
                byte* currentOldStart = oldPtr + i;
                int currentLength = 0;
                for (int j = 0; j < newLength; j++)
                {
                    if (*(currentOldStart + j) != *(newPtr + j)) break;
                    currentLength++;
                }

                if (currentLength > maxLength)
                {
                    maxLength = currentLength;
                    disp = oldLength - i;

                    if (maxLength == newLength) break;
                }
            }
            return maxLength;
        }
    }
}
