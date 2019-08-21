using System;

namespace Clarifi.DeltaLogger.Internal
{
    // Marker interfaces for POCOs
    internal interface ILogEntry
    {
        string Id { get; }
    }
}
