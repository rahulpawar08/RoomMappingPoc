using System;

namespace Clarifi.RoomMappingLogger.Internal
{
    // Marker interfaces for POCOs
    internal interface ILogEntry
    {
        string Id { get; }
    }
}
