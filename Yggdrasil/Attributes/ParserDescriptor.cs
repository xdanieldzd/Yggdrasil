using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Yggdrasil.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ParserDescriptor : Attribute
    {
        public string FileName;
        public int TableNo;
        public string Description;
        public byte Priority;

        public ParserDescriptor(string fileName, int tableNo, string description, byte priority)
        {
            this.FileName = fileName;
            this.TableNo = tableNo;
            this.Description = description;
            this.Priority = priority;
        }
    }
}
