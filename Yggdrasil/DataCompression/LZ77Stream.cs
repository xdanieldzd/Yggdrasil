using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Yggdrasil.Exceptions;

namespace Yggdrasil.DataCompression
{
	public class LZ77Stream : CompressedStream
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

		public LZ77Stream(CompressionMode compressionMode) : base(compressionMode) { }

		public override byte[] Decompress(byte[] buffer, int offset)
		{
			int count = (BitConverter.ToInt32(buffer, offset + 1) & 0xFFFFFF);
			byte[] output = new byte[count];

			uint inOffset = (uint)(offset + 4), outOffset = 0;
			ushort windowOffset;
			byte length, compFlags;

			while (outOffset < count)
			{
				compFlags = buffer[inOffset++];
				for (int i = 0; i < 8; i++)
				{
					if ((compFlags & 0x80) != 0)
					{
						ushort data = (ushort)((buffer[inOffset] << 8) | buffer[inOffset + 1]);
						inOffset += 2;

						length = (byte)((data >> 12) + 3);
						windowOffset = (ushort)((data & 0xFFF) + 1);
						compFlags <<= 1;

						uint startOffset = outOffset - windowOffset;
						if (outOffset + length > output.Length || startOffset + length > output.Length) throw new IndexOutOfRangeException();
						for (int j = 0; j < length; j++) output[outOffset++] = output[startOffset + j];
					}
					else
					{
						if (outOffset >= output.Length || inOffset >= buffer.Length) return output;
						output[outOffset++] = buffer[inOffset++];
						compFlags <<= 1;
					}
				}
			}

			return output;
		}

		public override unsafe byte[] Compress(byte[] buffer, int offset, int count)
		{
			if (count > 0xFFFFFF) throw new CompressedStreamException("Input data too large");

			MemoryStream outStream = new MemoryStream();

			outStream.WriteByte((byte)CompressionType.LZ77);
			outStream.WriteByte((byte)(count & 0xFF));
			outStream.WriteByte((byte)((count >> 8) & 0xFF));
			outStream.WriteByte((byte)((count >> 16) & 0xFF));

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
						outStream.Write(outbuffer, 0, bufferlength);
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
					outStream.Write(outbuffer, 0, bufferlength);
					compressedLength += bufferlength;
				}
			}

			return outStream.ToArray();
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

		public static void Test()
		{
			Yggdrasil.DataCompression.LZ77Stream lz77 = new DataCompression.LZ77Stream(DataCompression.CompressionMode.Decompress);
			byte[] data = System.IO.File.ReadAllBytes(@"E:\Translations\NDS Etrian Odyssey Hacking\Compression Testing\EventMessageDungeonGimmic_EN_File1.lz");
			lz77.Write(data, 0, data.Length);

			Yggdrasil.DataCompression.LZ77Stream lz77comp = new DataCompression.LZ77Stream(DataCompression.CompressionMode.Compress);
			lz77comp.Write(lz77.ToArray(), 0, (int)lz77.Length);
			System.IO.FileStream outstream = System.IO.File.Open(@"E:\Translations\NDS Etrian Odyssey Hacking\Compression Testing\EventMessageDungeonGimmic_EN_File1.lz.lz", System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.ReadWrite, System.IO.FileShare.ReadWrite);
			outstream.Write(lz77comp.ToArray(), 0, (int)lz77comp.Length);
			outstream.Close();

			Yggdrasil.DataCompression.LZ77Stream lz77check = new DataCompression.LZ77Stream(DataCompression.CompressionMode.Decompress);
			byte[] datacheck = System.IO.File.ReadAllBytes(@"E:\Translations\NDS Etrian Odyssey Hacking\Compression Testing\EventMessageDungeonGimmic_EN_File1.lz.lz");
			lz77check.Write(datacheck, 0, datacheck.Length);

			outstream = System.IO.File.Open(@"E:\Translations\NDS Etrian Odyssey Hacking\Compression Testing\EventMessageDungeonGimmic_EN_File1.lz.lz.dec", System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.ReadWrite, System.IO.FileShare.ReadWrite);
			outstream.Write(lz77check.ToArray(), 0, (int)lz77check.Length);
			outstream.Close();
		}
	}
}
