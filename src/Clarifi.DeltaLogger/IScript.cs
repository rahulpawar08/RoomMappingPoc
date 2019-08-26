using Clarifi.RoomMappingLogger.Internal;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Clarifi.RoomMappingLogger
{
    public interface IScript
    {
        Task ExecuteAsync(IDictionary<string, string> args);
    }
}
