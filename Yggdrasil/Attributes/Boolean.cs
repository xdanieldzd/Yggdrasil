using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yggdrasil.Attributes
{
	[AttributeUsage(AttributeTargets.Property)]
	public abstract class Boolean : Attribute
	{
		public bool Value;

		public Boolean(bool value)
		{
			Value = value;
		}
	}
}
