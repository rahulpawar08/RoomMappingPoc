using Clarifi.DeltaLogger.Scripts;
using Clarifi.RoomMappingLogger;
using Clarifi.RoomMappingLogger.MySql;
using Clarify.FuzzyMatchingTest;
using Clarify.FuzzyMatchingTest.Strategy;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.FuzzyAlgo
{
    class Program
    {
        static void Main(string[] args)
        {
            //new Setup().Run();

            //Comment this line if MySql schema is already created
            //Task.Run(() => KnownTypes.ProvisionAsync("all", new LogDb())).Wait();

            //using (var logDB = new LogDb(Settings.GetConnectionString()))
            //{
            //    IDataLogger dataWriter = new MySqlLogger(new Logger(logDB));

                int expectedMatchingScore = 80;
                //Available Fields => SQF: SquareFoot, TY: Type, BD: Bed Details, RV: Room View, DESC: Room Description
                List<string> matchingFields = new List<string>() { "SQF_TY_BD_RV" };
                var roomMappingviewExtractor = new RoomMappingViewExtractor();

                //BaseRoomMappingStrategy roomMappingStrategy = new HotelBedsDataAvailabilityStrategy(new FuzzyStringMatchingAlgo(),
                //    new ElasticSearchProvider("s_clarifihotels", new List<Uri> { new Uri(@"http://tavsrvtest042:9200/") }));


                BaseRoomMappingStrategy roomMappingStrategy = new HotelBedsDataAvailabilityStrategy(new FuzzyStringMatchingAlgo());

                roomMappingStrategy.Initialize();

                Console.WriteLine($"Starting with Room Mapping with fields - {GetCommaSeperatedFields(matchingFields)}.");
                var roomMappingResult = roomMappingStrategy.ExecuteHotelBedEanRoomMapping(matchingFields);

                Console.WriteLine("Results calculated...");

                var hotelBedsMappedView = roomMappingviewExtractor.GetRoomMappingWithTresholdPerHotel(roomMappingResult, expectedMatchingScore);

                Console.WriteLine("hotel beds mapped view created...");

                var epsMappedView = roomMappingviewExtractor.GetEpsMappedRooms(roomMappingStrategy.EpsSupplierData, hotelBedsMappedView);

                Console.WriteLine("eps mapped view created...");

            //This is HotelBeds mapped view
            //foreach (var kvPair in hotelBedsMappedView)
            //{
            //    dataWriter.WriteHotelBedsRoomMatching($"{kvPair.Key}_{expectedMatchingScore}_{DateTime.Now.ToString("yyyyMMddTHHmmss")}.json", kvPair.Value);
            //}

            //roomMappingStrategy.EpsSupplierData.ForEach(x => dataWriter.LogSupplierRoomData(x));
            //roomMappingStrategy.HotelBedSupplierData.ForEach(x => dataWriter.LogSupplierRoomData(x));

            //foreach (var epsMappingKvPair in epsMappedView)
            //    dataWriter.LogEPSRoomMatching($"{epsMappingKvPair.Key}_'EPSMappedView'_{DateTime.Now.ToString("yyyyMMddTHHmmss")}.json", epsMappingKvPair.Value);

            Console.WriteLine($"The result of algorithm is stored in the output folder.");

                #region RemoveMe
                //foreach (var result in roomMappingResult)
                //{
                //    foreach(var score in result.RoomMatchingScore)
                //    {
                //        if(score.MatchingScore> expectedMatchingScore)
                //        {
                //            if (!roomMappingResultWithExpectedScore.Any(r => r.RoomId == result.RoomId))
                //            {
                //                roomMappingResultWithExpectedScore.Add(result);

                //                Console.WriteLine($" HB RoomId: {result.RoomId}, EPS RoomId: {score.EPSRoomId}, Score:{score.MatchingScore}, " +
                //                             $" HB Key: {score.HotelBedMatchingString}, EPS Key: {score.EPSMatchingString}");
                //            }
                //        }
                //    }
                //}
                //roomMappingCore.SaveMappingResult($"{matchingFields}_{DateTime.Now.ToString("yyyyMMddTHHmmss")}.json", roomMappingResult);
                //roomMappingCore.SaveMappingResult($"{matchingFields}_ExpectedMatchingScore_{expectedMatchingScore}_{DateTime.Now.ToString("yyyyMMddTHHmmss")}.json", roomMappingResultWithExpectedScore);
                #endregion
            //}

            CSVStatsWriter(epsMappedView);

            Console.WriteLine("Room Mapping Complete.");
            Console.ReadLine();
        }

        private static object GetCommaSeperatedFields(List<string> matchingFields)
        {
            if (matchingFields == null)
                return string.Empty;

            StringBuilder commaSeperatedFields = new StringBuilder();
            matchingFields.ForEach(f => commaSeperatedFields.Append(f + ", "));
            return commaSeperatedFields.ToString().Trim(',');
        }

        private static void CSVStatsWriter(Dictionary<string, List<EpsMappedRooms>> epsMappedView)
        {
            List<string> logs1 = new List<string>();
            logs1.Add("ClarifiId,EpsRoomId,EpsRoomName,HBRoomsMappedCount");

            List<string> logs2 = new List<string>();
            logs2.Add("ClarifiId,EPSTotalRoomCount,EPSMappedRoomCount,MappingPercentage");

            int zeroRoomCount = 0;
            int hotelCount = 0;
            int total = 0;
            int totalRooms = 0;
            int totalMappedRooms = 0;
            double avgPercentage = 0;

            foreach(var kvp in epsMappedView)
            {
                foreach (var map in kvp.Value)
                {
                    logs1.Add($"{kvp.Key},{map.EpsRoomId},\"{map.EpsRoomName}\",{map.MappedRooms.Count}");
                }
                int totalCount = kvp.Value.Count;
                int mappedRoomCount = kvp.Value.Where(m => m.MappedRooms.Count > 0).Count();
                double mappedPercentage = totalCount != 0 ? (double)(mappedRoomCount  * 100) / totalCount : 0;
                logs2.Add($"{kvp.Key},{totalCount},{mappedRoomCount},{mappedPercentage}");

                if (totalCount == 0)
                    zeroRoomCount++;
                else
                    hotelCount++;
                total++;
                totalRooms += totalCount;
                totalMappedRooms += mappedRoomCount;
            }
            avgPercentage = (double)(totalMappedRooms * 100) / totalRooms;

            logs2.Add(",,,");
            logs2.Add(",,,");

            logs2.Add($"ZeroRoomHotelCount,{zeroRoomCount},,");
            logs2.Add($"HotelsWithRooms,{hotelCount},,");
            logs2.Add($"TotalHotels,{total},,");
            logs2.Add($"TotalMappedRooms,{totalMappedRooms},,");
            logs2.Add($"totalRooms,{totalRooms},,");
            logs2.Add($"AverageMappingPercentage,{avgPercentage},,");

            File.WriteAllLines(@"C:\Clarifi\RoomMapping\Summary\RoomMappingSummary.csv", logs1);
            File.WriteAllLines(@"C:\Clarifi\RoomMapping\Summary\HotelRoomMappingSummary.csv", logs2);
        }
    }
}
