using System;

namespace Clarifi.DeltaLogger
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class NestedFieldAttribute : Attribute
    {
        public NestedFieldAttribute()
        {
        }
    }

}
