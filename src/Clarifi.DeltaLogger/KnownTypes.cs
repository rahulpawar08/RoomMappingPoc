using Clarifi.DeltaLogger.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Clarifi.DeltaLogger
{
    public class KnownTypes
    {
        private KnownTypes() { }

        public static readonly KnownTypes Instance = new KnownTypes();

        private static readonly List<Type> _types = new List<Type>();

        public IEnumerable<Type> Types => _types;

        public KnownTypes Register<T>()
        {
            var type = typeof(T);
            type.Ensure().IsOfType<LogEntryBase>();
            type.Ensure().HasAttribute<TypeInfoAttribute>();
            if ( _types.Contains(type) == false )
                _types.Add(typeof(T));
            return this;
        }

        private static readonly string All = "all";
        public static async Task ProvisionAsync(string typesCsv, ILogDb db = null)
        {
            // Provision the known types
            if (string.IsNullOrWhiteSpace(typesCsv) == true)
                return;
            var matchingTypes = KnownTypes.Instance.Types;
            if (typesCsv != All)
            {
                var hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                Array.ForEach(typesCsv.Split(','), t => hashSet.Add(t));
                matchingTypes = matchingTypes
                                    .Select(t => new { Type = t, t.GetCustomAttribute<TypeInfoAttribute>().Name })
                                    .Where(t => hashSet.Contains(t.Name) == true)
                                    .Select(t => t.Type);
            }
            
            var typeInfos = matchingTypes.Select(Internal.TypeInfo.Extract).ToArray();
            Console.WriteLine($"Provisioning types {string.Join(", ", typeInfos.Select(x => x.Name))}");
            await db.ProvisionAsync(typeInfos);
            Console.WriteLine("Provisioning done.");
        }
    }
}
