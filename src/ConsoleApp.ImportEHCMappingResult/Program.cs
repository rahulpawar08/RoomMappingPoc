using Clarifi.DeltaLogger.Scripts;
using Clarifi.RoomMappingLogger;
using Clarifi.RoomMappingLogger.MySql;
using Clarify.FuzzyMatchingTest;
using Clarify.FuzzyMatchingTest.Strategy;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DataModels = Clarify.FuzzyMatchingTest;

namespace ConsoleApp.ImportEHCMappingResult
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"EHC mapping result tool started. Time:{DateTime.Now}");

            new Setup().Run();

            //Comment this line if MySql schema is already created
            //Task.Run(() => KnownTypes.ProvisionAsync("all", new LogDb())).Wait();

            Dictionary<string, List<EpsMappedRooms>> epsMappedView = null;
            List<DataModels.HotelLevelStats> hotelLevelStats = null;
            List<DataModels.RoomLevelStats> roomLevelStats = null;
            DataModels.RoomMappingSummary roomMappingSummary = null;

            ReadDataFromFile(@"C:\logs\EHCExportedResult\epsMappedRoomViewJSON.txt", ref epsMappedView);
            ReadDataFromFile(@"C:\logs\EHCExportedResult\HotelLevelStats.txt", ref hotelLevelStats);
            ReadDataFromFile(@"C:\logs\EHCExportedResult\RoomLevelStats.txt", ref roomLevelStats);
            ReadDataFromFile(@"C:\logs\EHCExportedResult\RoomMappingSummary.txt", ref roomMappingSummary);

            string versionId = Guid.NewGuid().ToString();
            using (var logDB = new LogDb(Settings.GetConnectionString()))
            {
                IDataLogger dataLogger = new MySqlLogger(new Logger(logDB));

                //Log Clarifi Model
                //roomMappingStrategy.EpsSupplierData.ToList().ForEach(x => dataLogger.LogSupplierRoomData(x));
                //roomMappingStrategy.HotelBedSupplierData.ToList().ForEach(x => dataLogger.LogSupplierRoomData(x));

                //Log EPSMappedView View
                foreach (var epsMappingKvPair in epsMappedView)
                {
                    epsMappingKvPair.Value.ForEach(x =>
                    {
                        x.EpsHotelName = Regex.Replace((x.EpsHotelName ?? string.Empty), "\"", string.Empty, RegexOptions.IgnoreCase);
                        x.EpsRoomName = Regex.Replace((x.EpsRoomName ?? string.Empty), "\"", string.Empty, RegexOptions.IgnoreCase);
                        x.MappedRooms.ForEach(y =>
                        {
                            y.HBRoomName = Regex.Replace((y.HBRoomName ?? string.Empty), "\"", string.Empty, RegexOptions.IgnoreCase);
                        });
                    });

                    dataLogger.LogEPSRoomMatching($"{epsMappingKvPair.Key}_'EPSMappedView'_{DateTime.Now.ToString("yyyyMMddTHHmmss")}.json", epsMappingKvPair.Value);
                }

                //Log HotelLevelStats View
                dataLogger.LogHotelLevelStats(hotelLevelStats);

                //Log RoomLevelStats View
                roomLevelStats.ForEach(x => { x.EpsRoomName = Regex.Replace((x.EpsRoomName ?? string.Empty), "\"", string.Empty, RegexOptions.IgnoreCase); });
                dataLogger.LogRoomLevelStats(roomLevelStats);

                //Log RoomMappingSummary View
                dataLogger.LogRoomMappingSummary(roomMappingSummary);

                Console.WriteLine($"The result of algorithm is stored in the output folder.");
            }

            Console.WriteLine($"EHC mapping result tool completed. Time:{DateTime.Now}");
            Console.ReadLine();
        }

        private static void ReadDataFromFile<T>(string fileName, ref T model)
        {
            using (StreamReader r = new StreamReader(fileName))
            {
                string json = r.ReadToEnd();
                model = JsonConvert.DeserializeObject<T>(json);
            }
        }
    }
}
