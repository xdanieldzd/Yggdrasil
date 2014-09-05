using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Yggdrasil.DataCompression
{
    public enum CompressionMode { Decompress, Compress };
    public enum CompressionType : byte { LZ77 = 0x10, RLE = 0x30 };

    public abstract class CompressedStream : MemoryStream
    {
        public CompressionMode CompressionMode { get; private set; }
        public MemoryStream OriginalStream { get; private set; }

        public CompressedStream(CompressionMode compressionMode)
        {
            CompressionMode = compressionMode;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (CompressionMode == CompressionMode.Decompress)
            {
                OriginalStream = new MemoryStream(buffer, offset, count);

                byte[] decompressed = Decompress(buffer, offset);
                base.Write(decompressed, 0, decompressed.Length);
            }
            else if (CompressionMode == CompressionMode.Compress)
            {
                OriginalStream = new MemoryStream(buffer, offset, count);

                byte[] compressed = Compress(buffer, offset, count);
                base.Write(compressed, 0, compressed.Length);
            }
        }

        public virtual byte[] Decompress(byte[] buffer, int sOffset)
        {
            throw new NotImplementedException(string.Format("Decompression not implemented for compression type {0}", this.GetType().FullName));
        }

        public virtual byte[] Compress(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException(string.Format("Compression not implemented for compression type {0}", this.GetType().FullName));
        }
    }
}
