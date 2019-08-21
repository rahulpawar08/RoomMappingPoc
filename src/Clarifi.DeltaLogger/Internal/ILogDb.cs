﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace Clarifi.DeltaLogger.Internal
{
    public interface ILogDb
    {
        Task ProvisionAsync(TypeInfo[] types);

        Task LogAsync(TypeInfo type, string id, string parentId, IDictionary<string, object> values);
    }
}
