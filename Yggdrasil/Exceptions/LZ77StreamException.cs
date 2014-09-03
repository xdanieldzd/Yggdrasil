using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yggdrasil.Exceptions
{
    public class LZ77StreamException : Exception
    {
        public LZ77StreamException(string message) : base(message) { }
        public LZ77StreamException(string message, Exception innerException) : base(message, innerException) { }
        public LZ77StreamException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
