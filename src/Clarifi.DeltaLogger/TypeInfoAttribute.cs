using System;

namespace Clarifi.RoomMappingLogger
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class TypeInfoAttribute : Attribute
    {
        public TypeInfoAttribute(string name) => Name = name;

        public string Name { get; }
    }

}
