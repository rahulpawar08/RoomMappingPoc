using System;

namespace Clarifi.RoomMappingLogger
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class NestedFieldAttribute : Attribute
    {
        public NestedFieldAttribute()
        {
        }
    }

}
