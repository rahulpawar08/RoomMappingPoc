using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp.FuzzyAlgo
{
    internal class Settings
    {
        internal static string GetConnectionString()
        {
            var connStr = Environment.GetEnvironmentVariable("conn_str");
            if (string.IsNullOrEmpty(connStr) == true)
            {
                return "Server=127.0.0.1;Port=3306;Database=room_delta_logger;Uid=root;Pwd=zaq1ZAQ!";
                //throw new ApplicationException("Connection string is not configured.");
            }
            return connStr;
        }
    }
}
