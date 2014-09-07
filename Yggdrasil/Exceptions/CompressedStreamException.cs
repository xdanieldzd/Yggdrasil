using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yggdrasil.Exceptions
{
    public class CompressedStreamException : Exception
    {
        public CompressedStreamException(string message) : base(message) { }
        public CompressedStreamException(string message, Exception innerException) : base(message, innerException) { }
        public CompressedStreamException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
