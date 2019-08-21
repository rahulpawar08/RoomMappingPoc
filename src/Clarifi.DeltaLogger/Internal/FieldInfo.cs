using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Clarifi.DeltaLogger.Internal
{
    public abstract class FieldInfo
    {
        public static FieldInfo Create(string name, Type type, string propertyName)
        {
            if (IsASimpleType(type) == true)
                return new SimpleFieldInfo(name, type, propertyName);
            if (IsLogEntryType(type) == true)
                return new LogEntryFieldInfo(name, type, propertyName);
            if (IsLogEntryCollectionType(type) == true)
                return new LogEntryCollectionFieldInfo(name, type, propertyName);

            throw new ArgumentException($"{type.Name} type fields are not supported.");
        }

        private static bool IsASimpleType(Type type) => type.IsValueType || (type == typeof(string));

        private static bool IsLogEntryType( Type type) => typeof(ILogEntry).IsAssignableFrom(type);

        private static bool IsLogEntryCollectionType(Type type)
        {
            // Type must be an Ienumerable of T where T : logentry
            if (typeof(IEnumerable).IsAssignableFrom(type) == false)
                return false;
            var interfaces = type.GetInterfaces().Where(t => t.IsGenericType == true);
            var enumerables = interfaces.Where(t => typeof(IEnumerable<>).Equals(t.GetGenericTypeDefinition()));
            foreach (var item in enumerables)
            {
                var args = item.GetGenericArguments();
                if (args.Length != 1) continue;
                if (typeof(ILogEntry).IsAssignableFrom(args[0]) == true)
                    return true;
            }
            return false;
        }

        

        protected FieldInfo(string name, Type type, string propertyName)
        {
            Name = name;
            Type = type;
            PropertyName = propertyName;
        }






        public string Name { get; }

        public string PropertyName { get; set; }

        public Type Type { get; }




    }

}
