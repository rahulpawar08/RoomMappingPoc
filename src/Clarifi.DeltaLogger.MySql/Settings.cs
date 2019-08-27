using System;

namespace Clarifi.RoomMappingLogger.MySql
{
    internal class Settings
    {
        internal static string GetConnectionString()
        {
            var connStr = Environment.GetEnvironmentVariable("conn_str");
            if (string.IsNullOrEmpty(connStr) == true)
                return "Server=127.0.0.1;Port=3306;Database=room_delta_logger;Uid=root;Pwd=zaq1ZAQ!";
            return connStr;
        }
    }
}
