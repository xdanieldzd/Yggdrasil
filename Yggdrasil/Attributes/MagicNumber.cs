using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yggdrasil.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class MagicNumber : Attribute
    {
        public object Magic;

        public MagicNumber(object magic)
        {
            this.Magic = magic;
        }
    }
}
