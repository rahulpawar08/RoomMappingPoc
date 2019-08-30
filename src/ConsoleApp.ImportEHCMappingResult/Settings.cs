using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp.ImportEHCMappingResult
{
    internal class Settings
    {
        internal static string GetConnectionString()
        {
            var connStr = Environment.GetEnvironmentVariable("conn_str");
            if (string.IsNullOrEmpty(connStr) == true)
            {
                return "Server=127.0.0.1;Port=3306;Database=room_delta_logger1;Uid=root;Pwd=zaq1ZAQ!";
                //throw new ApplicationException("Connection string is not configured.");
            }
            return connStr;
        }

        public static List<Uri> ElasticCoreHotelsClusterNodeUrls
        {
            get
            {
                var config = Environment.GetEnvironmentVariable("ElasticSearchUrl");
                var urlList = config.Split('|');
                List<Uri> nodeUrls = new List<Uri>();
                foreach (var url in urlList)
                {
                    nodeUrls.Add(new Uri(url));
                }

                if (nodeUrls != null && nodeUrls.Count > 0)
                    nodeUrls.Add(new Uri(""));

                return nodeUrls;
            }
        }
    }
}
