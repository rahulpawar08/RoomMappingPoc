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
                return "Server=devmysql.cufptq5ac0c7.us-east-1.rds.amazonaws.com;Port=3306;Database=Room_Delta_Logger;Uid=clarifi;Pwd=tavisca123";
                //throw new ApplicationException("Connection string is not configured.");
            }
            return connStr;
        }
    }
}
