using System;
using System.Collections.Generic;

namespace Clarifi.DeltaLogger.Internal
{
    internal class LogEntryFieldInfo : FieldInfo, INestedField
    {
        public LogEntryFieldInfo(string name, Type type, string propertyName) : base(name, type, propertyName)
        {
        }

        public IEnumerable<LogEntryBase> GetChildren(object target)
        {
            var property = target.GetType().GetProperty(PropertyName);
            if (property?.GetValue(target) is LogEntryBase entry)
                yield return entry;
        }
    }

}
