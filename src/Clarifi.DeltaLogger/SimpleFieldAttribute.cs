using System;

namespace Clarifi.DeltaLogger
{

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class SimpleFieldAttribute : Attribute
    {
        public SimpleFieldAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }

}
