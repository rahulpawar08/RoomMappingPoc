using System;
using System.Threading.Tasks;
using Clarifi.RoomMappingLogger.Internal;

namespace Clarifi.RoomMappingLogger
{
    public abstract partial class LogEntryBase : ILogEntry
    {
        public string Id { get; } = Guid.NewGuid().ToString();

        private TypeInfo GetTypeInfo() => TypeInfo.Extract(this.GetType());
    }
}
