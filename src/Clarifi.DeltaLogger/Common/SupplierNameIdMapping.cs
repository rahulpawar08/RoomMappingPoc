using System;
using System.Collections.Generic;

namespace Clarifi.DeltaLogger
{
    public static class SupplierNameIdMapping
    {
        private static Dictionary<string, int> GetStageCarSupplierNamesIds()
        {
            return new Dictionary<string, int>
            {
                {"Avis",114 },
                {"Alamo",153 },
                {"Budget",113 },
                {"National",154 },
                {"Dollar",123 },
                {"Thrifty",124 },
                {"Enterprise",152},
                {"Hertz",126 }
            };
        }

        private static Dictionary<string, int> GetProdCarSupplierNamesIds()
        {
            return new Dictionary<string, int>
            {
                {"Avis",214 },
                {"Alamo",253 },
                {"Budget",213 },
                {"National",254 },
                {"Dollar",223 },
                {"Thrifty",224 },
                {"Enterprise",252},
                {"Hertz",226 }
            };
        }

        private static readonly Dictionary<string, string> KenobiStageHotelSupplierNames = new Dictionary<string, string>
        {

                { "1qma1dvgb9c","EAN Retail" },
                { "1h8r62gj08w","GTA" },
                { "1h8rc3x6ry8","getaroom"},
                { "1h8r80r9ibk","Tourico" },
                { "1h8r3qo8v7k","PricelinePN NetRates" },
                { "1h8qlenkq2o","hotelbeds" },
                { "1h8qopmjvgg","PricelinePN SemiOpaque"},
                { "1h8ra7gxtkw","EAN" }

        };

        private static readonly Dictionary<string, int> KenobiStageHotelSupplierId = new Dictionary<string, int>
        {

                { "1h8r80r9ibk",194 },
                { "1h8rc3x6ry8",163 },
                { "1h8r62gj08w",150},
                { "1qma1dvgb9c",188 },
                { "1h8qlenkq2o",111 },
                { "1h8qopmjvgg",190 },
                { "1h8r3qo8v7k",189},
                { "1h8ra7gxtkw",184 }

        };

        // TODO change supplierId as per production use (There are multiple entries in prod dataapi)
        private static readonly Dictionary<string, string> KenobiProdHotelSupplierNames = new Dictionary<string, string>
        {

                { "203g5b7o7pc","EAN Retail" },
                { "tkmddo7e9s","GTA" },
                { "tkmdgjrdhc","getaroom"},
                { "hc8n9z13pc","Tourico" },
                { "tkmd6tx5og","PricelinePN NetRates" },
                { "w9msda1la8","hotelbeds" },
                { "totvwatj40","PricelinePN SemiOpaque"},
                { "hc8nea0feo","EAN" },
                {"hc92x4mfi8", "Tourico"},
                {"hc92vbrvnk", "EAN"},
                {"hc8acfo8hs", "Tourico"},
                {"hc8agqnk74", "EAN"},
                {"isawyvzfgg","Tourico" },
                {"isax2idtds","EAN" },
                { "iz7oq82wao","Tourico" },
                { "iz7otoe5mo","EAN" },
                { "izr7cds0sg","Tourico" },
                { "izr7fu639c","EAN" },
                { "t5jvjcritc","PricelinePN SemiOpaque" },
                { "t5jvraaups","GTA" },
                { "t5jvuvdhc0","getaroom" },
                { "t9965xiuio","PricelinePN NetRates" },
                { "t9ki9oxqf4","PricelinePN NetRates" },
                { "t9kifh7tvk","PricelinePN SemiOpaque" },
                { "t9kigrq6tc","GTA" },
                { "tkmd9qq328","PricelinePN SemiOpaque" },
                { "tkwomy9v5s","PricelinePN NetRates" },
                { "tkwoyzrojk","PricelinePN SemiOpaque" },
                { "tkwou789a8","GTA" },
                { "tkwp6b55vk","getaroom" },
                { "tladkwxczk","PricelinePN NetRates" },
                { "tladg2zda8","PricelinePN SemiOpaque" },
                { "tlads27pq8","GTA" },
                { "tladn5umtc","getaroom" },
                { "totvklnl6o","PricelinePN NetRates" },
                { "totvsk7fuo","GTA" },
                { "totw45pce8","getaroom" },
                { "zab7hgqhog","Tourico" },
                { "zab7kqymf4","EAN" },
                { "zab7nlf9q8","PricelinePN NetRates" },
                { "zab7r1tc74","PricelinePN SemiOpaque" },
                { "zab7ugvncw","GTA" },
                {"zab7wwqy9s","getaroom" },
                {"10yckruanls","Tourico" },
                {"10yckvzi39c","EAN" },
                {"10yckxj5534","PricelinePN NetRates" },
                {"10ycl1t9kao","PricelinePN SemiOpaque" },
                {"10ycl37zmkg","GTA" },
                {"10ycl7ja6tc","getaroom" },
                {"1466qbzyuww","Tourico" },
                {"1466kv6lxq8","EAN" },
                {"1466h4jw7b4","PricelinePN NetRates" },
                {"1466bvqc2yo","PricelinePN SemiOpaque" },
                {"14662yr3o5c","GTA" },
                {"1465jt68q2o","getaroom" },
                {"13f4gtst8g0","hotelbeds" }

        };

        // TODO change supplierId as per production use (There are multiple entries in prod dataapi)
        private static readonly Dictionary<string, int> KenobiProdHotelSupplierId = new Dictionary<string, int>
        {

                { "hc8n9z13pc",207 },
                { "tkmdgjrdhc",263 },
                { "tkmddo7e9s",250},
                { "203g5b7o7pc",288 },
                { "w9msda1la8",211 },
                { "totvwatj40",290 },
                { "tkmd6tx5og",289},
                { "hc8nea0feo",284 },
                {"hc92x4mfi8", 207},
                {"hc92vbrvnk", 284},
                {"hc8acfo8hs", 207},
                {"hc8agqnk74", 284},
                {"isawyvzfgg",207 },
                {"isax2idtds",284 },
                { "iz7oq82wao",207 },
                { "iz7otoe5mo",284 },
                { "izr7cds0sg",207 },
                { "izr7fu639c",284 },
                { "t5jvjcritc",290 },
                { "t5jvraaups",250 },
                { "t5jvuvdhc0",263 },
                { "t9965xiuio",289 },
                { "t9ki9oxqf4",289 },
                { "t9kifh7tvk",290 },
                { "t9kigrq6tc",250 },
                { "tkmd9qq328",290 },
                { "tkwomy9v5s",289 },
                { "tkwoyzrojk",290 },
                { "tkwou789a8",250 },
                { "tkwp6b55vk",263 },
                { "tladkwxczk",289 },
                { "tladg2zda8",290 },
                { "tlads27pq8",250 },
                { "tladn5umtc",263 },
                { "totvklnl6o",289 },
                { "totvsk7fuo",250 },
                { "totw45pce8",263 },
                { "zab7hgqhog",207 },
                { "zab7kqymf4",284 },
                { "zab7nlf9q8",289 },
                { "zab7r1tc74",290 },
                { "zab7ugvncw",250 },
                {"zab7wwqy9s",263 },
                {"10yckruanls",207 },
                {"10yckvzi39c",284 },
                {"10yckxj5534",289 },
                {"10ycl1t9kao",290 },
                {"10ycl37zmkg",250 },
                {"10ycl7ja6tc",263 },
                {"1466qbzyuww",207 },
                {"1466kv6lxq8",284 },
                {"1466h4jw7b4",289 },
                {"1466bvqc2yo",290 },
                {"14662yr3o5c",250 },
                {"1465jt68q2o",263 },
                {"13f4gtst8g0",211 }

        };

        private static string GetStageKenobiHotelSupplierName(string supplierId)
        {
            var supplierName = KenobiStageHotelSupplierNames.ContainsKey(supplierId) ? KenobiStageHotelSupplierNames[supplierId] : "Unknown-"+ supplierId;
            return supplierName;
        }

        private static int GetStageKenobiHotelSupplierId(string supplierId)
        {
            var id = KenobiStageHotelSupplierId.ContainsKey(supplierId) ? KenobiStageHotelSupplierId[supplierId] : 0;
            return id;
        }

        private static string GetProdKenobiHotelSupplierName(string supplierId)
        {
            var supplierName = KenobiProdHotelSupplierNames.ContainsKey(supplierId) ? KenobiProdHotelSupplierNames[supplierId] : "Unknown-" + supplierId;
            return supplierName;
        }

        private static int GetProdKenobiHotelSupplierId(string supplierId)
        {
            var id = KenobiProdHotelSupplierId.ContainsKey(supplierId) ? KenobiProdHotelSupplierId[supplierId] : 0;
            return id;
        }

        public static string GetKenobiHotelSupplierName(string environment, string supplierId)
        {
            var supplierName = environment.Equals("prod", StringComparison.InvariantCultureIgnoreCase) ?
                        GetProdKenobiHotelSupplierName(supplierId) :
                        GetStageKenobiHotelSupplierName(supplierId);

            return supplierName;
        }

        public static int GetKenobiHotelSupplierId(string environment, string supplierId)
        {
            var id = environment.Equals("prod", StringComparison.InvariantCultureIgnoreCase) ?
                        GetProdKenobiHotelSupplierId(supplierId) :
                        GetStageKenobiHotelSupplierId(supplierId);

            return id;
        }

        public static Dictionary<string, int> GetCarSupplierNamesIds(string environment)
        {
            var namesIds = environment.Equals("prod", StringComparison.InvariantCultureIgnoreCase) ?
                 GetProdCarSupplierNamesIds()
                 : GetStageCarSupplierNamesIds();

            return namesIds;
        }
    }
}
