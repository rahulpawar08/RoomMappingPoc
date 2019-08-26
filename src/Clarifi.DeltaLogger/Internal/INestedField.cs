using System.Collections.Generic;

namespace Clarifi.RoomMappingLogger.Internal
{
    public interface INestedField
    {
        IEnumerable<LogEntryBase> GetChildren(object target);
    }

}
