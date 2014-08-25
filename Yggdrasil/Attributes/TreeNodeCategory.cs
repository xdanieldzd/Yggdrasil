using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Yggdrasil.Attributes
{
    public class TreeNodeCategory : Attribute
    {
        public string CategoryName;

        public TreeNodeCategory(string categoryName)
        {
            this.CategoryName = categoryName;
        }
    }
}
