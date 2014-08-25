using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Yggdrasil.Attributes
{
    public class PrioritizedDescription : DescriptionAttribute
    {
        public byte Priority;

        public PrioritizedDescription() : base() { }
        public PrioritizedDescription(string description) : base(description) { }

        public PrioritizedDescription(string description, byte priority)
            : base(description)
        {
            this.Priority = priority;
        }
    }
}
