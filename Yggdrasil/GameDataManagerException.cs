using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yggdrasil
{
    class GameDataManagerException : Exception
    {
        public GameDataManagerException(string message) : base(message) { }
        public GameDataManagerException(string message, Exception innerException) : base(message, innerException) { }
        public GameDataManagerException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
