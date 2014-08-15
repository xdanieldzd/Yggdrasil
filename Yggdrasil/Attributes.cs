using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Yggdrasil
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

    public class PrioritizedCategory : CategoryAttribute
    {
        public byte Priority;

        public PrioritizedCategory() : base() { }
        public PrioritizedCategory(string category) : base(category) { }

        public PrioritizedCategory(string category, byte priority)
            : base(string.Format("{0}{1}", new string('\t', priority), category))
        {
            this.Priority = priority;
        }
    }
}
