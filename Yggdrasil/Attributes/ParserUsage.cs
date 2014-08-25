using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Yggdrasil.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ParserUsage : Attribute
    {
        public string FileName;
        public int TableNo;

        public ParserUsage(string fileName, int tableNo)
        {
            this.FileName = fileName;
            this.TableNo = tableNo;
        }
    }
}
