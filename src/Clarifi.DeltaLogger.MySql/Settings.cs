using System;

namespace Clarifi.DeltaLogger.MySql
{
    internal class Settings
    {
        internal static string GetConnectionString()
        {
            var connStr = Environment.GetEnvironmentVariable("conn_str");
            if (string.IsNullOrEmpty(connStr) == true)
                throw new ApplicationException("Connection string is not configured.");
            return connStr;
        }
    }
}
