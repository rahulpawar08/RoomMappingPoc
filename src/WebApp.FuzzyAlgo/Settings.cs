using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.FuzzyAlgo
{
    public class Settings
    {
        internal static string GetConnectionString()
        {
            var connStr = Environment.GetEnvironmentVariable("conn_str");
            if (string.IsNullOrEmpty(connStr) == true)
                return "Server=devmysql.cufptq5ac0c7.us-east-1.rds.amazonaws.com;Port=3306;Database=room_delta_logger_new;Uid=clarifi;Pwd=tavisca123";
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
