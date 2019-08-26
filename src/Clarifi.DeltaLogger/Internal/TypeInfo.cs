using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Clarifi.RoomMappingLogger.Internal
{
    public class TypeInfo
    {
        public static TypeInfo Extract<T>() => Extract(typeof(T));

        public static TypeInfo Extract(Type type)
        {
            type.Ensure()
                .IsOfType<ILogEntry>()
                .HasAttribute<TypeInfoAttribute>();
            var apiAttr = type.GetCustomAttributes(typeof(TypeInfoAttribute), false).Single() as TypeInfoAttribute;
            var simpleFields = type
                            .GetProperties()
                            .Where(p => p.IsDefined(typeof(SimpleFieldAttribute), true) == true)
                            .Select(p => new { Attr = p.GetCustomAttribute<SimpleFieldAttribute>(true), Property = p })
                            .Select(x => FieldInfo.Create(x.Attr.Name, x.Property.PropertyType, x.Property.Name))
                            .ToArray();
            var nestedFields = type
                            .GetProperties()
                            .Where(p => p.IsDefined(typeof(NestedFieldAttribute), true) == true)
                            .Select(p => new { Attr = p.GetCustomAttribute<NestedFieldAttribute>(true), Property = p })
                            .Select(x => FieldInfo.Create(null, x.Property.PropertyType, x.Property.Name))
                            .ToArray();
            return new TypeInfo(apiAttr.Name, simpleFields.Concat(nestedFields));
        }

        public IEnumerable<FieldInfo> GetSimpleFields()
        {
            return Fields.Where(f => f is ISimpleField);
        }

        public TypeInfo(string name, IEnumerable<FieldInfo> facets = null)
        {
            Name = name;
            if (facets != null)
                Fields.AddRange(facets);
        }

        public string Name { get; set; }


        public List<FieldInfo> Fields { get; } = new List<FieldInfo>();

        internal Dictionary<string, object> GetSimpleValues(object obj)
        {
            return this
                .GetSimpleFields()
                .Select(f => new KeyValuePair<string, object>(f.Name, ((ISimpleField)f).GetFieldValue(obj)))
                .ToDictionary(x => x.Key, x => x.Value, StringComparer.OrdinalIgnoreCase);
        }

        internal IEnumerable<LogEntryBase> GetChildValues(LogEntryBase logEntry)
        {
            return Fields
                    .Where(f => f is INestedField)
                    .SelectMany(f => ((INestedField)f).GetChildren(logEntry))
                    .ToArray();
        }

    }

}
