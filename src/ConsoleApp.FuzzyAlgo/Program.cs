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
using DataModels = Clarify.FuzzyMatchingTest;

namespace ConsoleApp.FuzzyAlgo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"Program Started. Time:{DateTime.Now}");

            new Setup().Run();

            //Comment this line if MySql schema is already created
            Task.Run(() => KnownTypes.ProvisionAsync("all", new LogDb())).Wait();

            string versionId = Guid.NewGuid().ToString();
            using (var logDB = new LogDb(Settings.GetConnectionString()))
            {
                IDataLogger dataLogger = new MySqlLogger(new Logger(logDB));

                int expectedMatchingScore = 80;
                //Available Fields => SQF: SquareFoot, TY: Type, BD: Bed Details, RV: Room View, DESC: Room Description
                List<string> matchingFields = new List<string>() { "SQF_TY_BD_RV" };
                var roomMappingviewExtractor = new RoomMappingViewExtractor();

                BaseRoomMappingStrategy roomMappingStrategy = new HotelBedsDataAvailabilityStrategy(new FuzzyStringMatchingAlgo(), versionId);

                roomMappingStrategy.Initialize(versionId);

                Console.WriteLine($"Starting with Room Mapping with fields - {GetCommaSeperatedFields(matchingFields)}.");
                var roomMappingResult = roomMappingStrategy.ExecuteHotelBedEanRoomMapping(matchingFields);

                var hotelBedsMappedView = roomMappingviewExtractor.GetRoomMappingWithTresholdPerHotel(roomMappingResult, expectedMatchingScore);

                var epsMappedView = roomMappingviewExtractor.GetEpsMappedRooms(roomMappingStrategy.EpsSupplierData, hotelBedsMappedView, versionId, roomMappingStrategy.GetStrategyName(), roomMappingStrategy.GetMatchingAlgorithmName());

                //This is HotelBeds mapped view
                //foreach (var kvPair in hotelBedsMappedView)
                //{
                //    dataLogger.LogHotelBedsRoomMatching($"{kvPair.Key}_{expectedMatchingScore}_{DateTime.Now.ToString("yyyyMMddTHHmmss")}.json", kvPair.Value);
                //}

                foreach (var epsMappingKvPair in epsMappedView)
                    dataLogger.LogEPSRoomMatching($"{epsMappingKvPair.Key}_'EPSMappedView'_{DateTime.Now.ToString("yyyyMMddTHHmmss")}.json", epsMappingKvPair.Value);

                dataLogger.LogSupplierRoomData(roomMappingStrategy.EpsSupplierData.ToList());
                dataLogger.LogSupplierRoomData(roomMappingStrategy.HotelBedSupplierData.ToList());

                StatsWriter(epsMappedView, versionId, dataLogger, roomMappingStrategy.GetStrategyName(), roomMappingStrategy.GetMatchingAlgorithmName());

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
            }

            Console.WriteLine($"Room Mapping Complete. Time:{DateTime.Now}");
            Console.ReadLine();
        }

        private static void StatsWriter(Dictionary<string, List<EpsMappedRooms>> epsMappedView, string versionId,
            IDataLogger dataLogger, string strategyName, string algorithmName)
        {
            List<DataModels.HotelLevelStats> hotelLevelStats = new List<DataModels.HotelLevelStats>();
            List<DataModels.RoomLevelStats> roomLevelStats = new List<DataModels.RoomLevelStats>();

            int zeroRoomCount = 0;
            int hotelCount = 0;
            int total = 0;
            int totalRooms = 0;
            int totalMappedRooms = 0;
            int totalRoomsWithoutMatchingRooms = 0;
            double avgPercentage = 0;

            foreach (var kvp in epsMappedView)
            {
                if (kvp.Key != null)
                {
                    int hbRoomsCount = 0;
                    foreach (var map in kvp.Value)
                    {
                        roomLevelStats.Add(new DataModels.RoomLevelStats(versionId, kvp.Key,
                            map.EpsRoomId, map.EpsRoomName, map.MappedRooms.Count, map.AppliedStrategyName,
                            map.MatchingAlgorithm, map.MatchingStatus.ToString(), map.HBRoomsCount));

                        if (map.MatchingStatus == MatchingStatus.MatchingRoomsNotAvailable)
                            totalRoomsWithoutMatchingRooms++;

                        hbRoomsCount = map.HBRoomsCount;
                    }

                    int totalCount = kvp.Value.Count;
                    int mappedRoomCount = kvp.Value.Where(m => m.MappedRooms.Count > 0).Count();
                    double mappedPercentage = totalCount != 0 ? (double)(mappedRoomCount * 100) / totalCount : 0;

                    hotelLevelStats.Add(new DataModels.HotelLevelStats(versionId, kvp.Key, totalCount, mappedRoomCount, mappedPercentage, hbRoomsCount));

                    if (totalCount == 0)
                        zeroRoomCount++;
                    else
                        hotelCount++;
                    total++;
                    totalRooms += totalCount;
                    totalMappedRooms += mappedRoomCount;
                }
                else
                {
                    Console.WriteLine("Null Value Found");
                }
            }
            avgPercentage = (double)(totalMappedRooms * 100) / totalRooms;

            DataModels.RoomMappingSummary roomMappingSummary = new DataModels.RoomMappingSummary(versionId, zeroRoomCount, hotelCount,
                total, totalMappedRooms, totalRooms, avgPercentage, strategyName, algorithmName, totalRoomsWithoutMatchingRooms,
                (totalRooms - totalMappedRooms - totalRoomsWithoutMatchingRooms));

            dataLogger.LogHotelLevelStats(hotelLevelStats);
            dataLogger.LogRoomLevelStats(roomLevelStats);
            dataLogger.LogRoomMappingSummary(roomMappingSummary);
        }

        private static object GetCommaSeperatedFields(List<string> matchingFields)
        {
            if (matchingFields == null)
                return string.Empty;

            StringBuilder commaSeperatedFields = new StringBuilder();
            matchingFields.ForEach(f => commaSeperatedFields.Append(f + ", "));
            return commaSeperatedFields.ToString().Trim(',');
        }
    }
}
