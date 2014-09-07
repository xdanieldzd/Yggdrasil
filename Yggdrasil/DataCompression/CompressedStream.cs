using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Yggdrasil.Exceptions;

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

                byte[] decompressed;
                if (!Decompress(buffer, offset, out decompressed))
                    throw new CompressedStreamException(string.Format("{0} decompression failed.", this.GetType().Name));

                base.Write(decompressed, 0, decompressed.Length);
            }
            else if (CompressionMode == CompressionMode.Compress)
            {
                OriginalStream = new MemoryStream(buffer, offset, count);

                byte[] compressed;
                if (!Compress(buffer, offset, count, out compressed))
                    throw new CompressedStreamException(string.Format("{0} compression failed.", this.GetType().Name));

                base.Write(compressed, 0, compressed.Length);
            }
        }

        public virtual bool Decompress(byte[] buffer, int sOffset, out byte[] output)
        {
            throw new NotImplementedException(string.Format("Decompression not implemented for compression type {0}", this.GetType().FullName));
        }

        public virtual bool Compress(byte[] buffer, int offset, int count, out byte[] output)
        {
            throw new NotImplementedException(string.Format("Compression not implemented for compression type {0}", this.GetType().FullName));
        }
    }
}
