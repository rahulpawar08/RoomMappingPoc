using System;
using System.Collections;
using System.Collections.Generic;

namespace Clarifi.RoomMappingLogger.Internal
{
    internal class LogEntryCollectionFieldInfo : FieldInfo, INestedField
    {
        public LogEntryCollectionFieldInfo(string name, Type type, string propertyName) : base(name, type, propertyName)
        {
        }

        public IEnumerable<LogEntryBase> GetChildren(object target)
        {
            var property = target.GetType().GetProperty(PropertyName);
            var enumerable = property?.GetValue(target) as IEnumerable;
            List<LogEntryBase> result = new List<LogEntryBase>();
            foreach (var item in enumerable)
            {
                if (item is LogEntryBase logEntry)
                    yield return logEntry;
            }
        }
    }

}
