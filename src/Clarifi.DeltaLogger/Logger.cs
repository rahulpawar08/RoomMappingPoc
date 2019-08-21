using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Clarifi.DeltaLogger.Internal;

namespace Clarifi.DeltaLogger
{
    public class Logger
    {
        public Logger(ILogDb db = null)
        {
            Db = db ?? GetDefaultDb();
        }

        private ILogDb GetDefaultDb()
        {
            throw new NotImplementedException();
        }

        public ILogDb Db { get; }

        public async Task RecordEntryAsync<T>(T entry) where T : LogEntryBase
        {
            if( entry != null )
                await RecordEntryAsync(null, entry, new HashSet<string>(StringComparer.OrdinalIgnoreCase));
        }

        public async Task RecordEntryAsync<T>(List<T> entry) where T : LogEntryBase
        {
            if (entry != null && entry.Count != 0)
            {
                foreach (var data in entry)
                {
                    await RecordEntryAsync(null, data, new HashSet<string>(StringComparer.OrdinalIgnoreCase));
                }
            }
        }

        private async Task RecordEntryAsync(string parentId, LogEntryBase entry, HashSet<string> existing)
        {
            // Check for any cyclic references.
            if (existing.Contains(entry.Id) == true) return;
            var type = Internal.TypeInfo.Extract(entry.GetType());
            // Log the root log entry along with all its simple fields.
            await RecordSingleEntryAsync(entry, parentId, type);
            existing.Add(entry.Id);
            // Recursively log all inner log entries with the current entry's id as the parent id.
            var children = type.GetChildValues(entry);
            foreach (var child in children)
                await RecordEntryAsync(entry.Id, child, existing);
        }

        private async Task RecordSingleEntryAsync(LogEntryBase entry, string parentId, TypeInfo type)
        {
            var values = type.GetSimpleValues(entry);
            await Db.LogAsync(type, entry.Id, parentId, values);
        }
    }

}
