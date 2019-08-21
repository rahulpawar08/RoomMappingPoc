using System;

namespace Clarifi.DeltaLogger.Internal
{
    internal class SimpleFieldInfo : FieldInfo, ISimpleField
    {
        public SimpleFieldInfo(string name, Type type, string propertyName) : base(name, type, propertyName)
        {
        }

        public object GetFieldValue(object target)
        {
            var property = target.GetType().GetProperty(PropertyName);
            return property?.GetValue(target);
        }
    }

}
