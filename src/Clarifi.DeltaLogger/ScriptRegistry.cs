using System;
using System.Collections.Generic;

namespace Clarifi.RoomMappingLogger
{
    public class ScriptRegistry
    {
        private ScriptRegistry() { }

        public static ScriptRegistry Instance = new ScriptRegistry();

        internal static Dictionary<string, Type> Scripts { get; } = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);

        public ScriptRegistry Register<T>(string name) where T : IScript, new()
        {
            Scripts[name] = typeof(T);
            return this;
        }

        public IScript BuildScript(string name)
        {
            if (Scripts.TryGetValue(name, out Type type) == false)
                return null;
            return Activator.CreateInstance(type) as IScript;
        }
    }
}
