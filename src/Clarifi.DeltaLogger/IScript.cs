using Clarifi.DeltaLogger.Internal;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Clarifi.DeltaLogger
{
    public interface IScript
    {
        Task ExecuteAsync(IDictionary<string, string> args);
    }
}
