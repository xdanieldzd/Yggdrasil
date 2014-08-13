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

    [AttributeUsage(AttributeTargets.Class)]
    public class ItemNameDescriptionFiles : Attribute
    {
        public string NameFile, DescriptionFile;
        public int NameTableNo, DescriptionTableNo;

        public ItemNameDescriptionFiles(string nameFile, int nameTableNo, string descriptionFile, int descriptionTableNo)
        {
            this.NameFile = nameFile;
            this.NameTableNo = nameTableNo;
            this.DescriptionFile = descriptionFile;
            this.DescriptionTableNo = descriptionTableNo;
        }
    }

    public class PrioritizedCategory : CategoryAttribute
    {
        public int Priority;

        public PrioritizedCategory() : base() { }
        public PrioritizedCategory(string category) : base(category) { }

        public PrioritizedCategory(string category, int priority)
            : base(string.Format("{0}{1}", new string('\t', priority), category))
        {
            this.Priority = priority;
        }
    }
}
