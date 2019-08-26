using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Clarifi.RoomMappingLogger
{
    public class ScriptRunner
    {
        private static readonly IDictionary<string, string> Empty = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        public async Task RunAsync(string scriptName, IDictionary<string, string> args)
        {
            var script = ScriptRegistry.Instance.BuildScript(scriptName);
            await script?.ExecuteAsync(args ?? Empty);
        }
    }
}
