using System;
using System.Threading.Tasks;
using Clarifi.DeltaLogger.Internal;

namespace Clarifi.DeltaLogger
{
    public abstract partial class LogEntryBase : ILogEntry
    {
        public string Id { get; } = Guid.NewGuid().ToString();

        private TypeInfo GetTypeInfo() => TypeInfo.Extract(this.GetType());
    }
}
