using System;
using System.Collections.Generic;

namespace Clarifi.RoomMappingLogger.MySql
{
    internal class Settings
    {
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
