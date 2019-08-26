using Clarifi.RoomMappingLogger.MySql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Clarifi.RoomMappingLogger.MySql
{
    public abstract class BaseScript : IScript
    {
        public Task ExecuteAsync(IDictionary<string, string> args)
        {
            var db = new LogDb(Settings.GetConnectionString());
            return ExecuteInternalAsync(new Logger(db), args);
        }

        protected abstract Task ExecuteInternalAsync(Logger logger, IDictionary<string, string> args);
    }
}
