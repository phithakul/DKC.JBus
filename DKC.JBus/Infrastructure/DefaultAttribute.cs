using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DKC.JBus
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    internal sealed class DefaultAttribute : Attribute
    {
        public DefaultAttribute(object defaultValue)
        {
            DefaultValue = defaultValue;
        }

        public object DefaultValue { get; private set; }
    }
}