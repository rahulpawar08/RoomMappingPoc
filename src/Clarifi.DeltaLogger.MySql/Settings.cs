using System;

namespace Clarifi.RoomMappingLogger.MySql
{
    internal class Settings
    {
        internal static string GetConnectionString()
        {
            var connStr = Environment.GetEnvironmentVariable("conn_str");
            if (string.IsNullOrEmpty(connStr) == true)
                return "Server=devmysql.cufptq5ac0c7.us-east-1.rds.amazonaws.com;Port=3306;Database=Room_Delta_Logger;Uid=clarifi;Pwd=tavisca123";
            return connStr;
        }
    }
}
