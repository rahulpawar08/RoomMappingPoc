using System.Collections.Generic;

namespace Clarifi.DeltaLogger.Internal
{
    public interface INestedField
    {
        IEnumerable<LogEntryBase> GetChildren(object target);
    }

}
