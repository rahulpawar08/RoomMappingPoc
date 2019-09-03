using Clarifi.RoomMappingLogger;
using Clarifi.RoomMappingLogger.MySql;
using Clarify.FuzzyMatchingTest;
using Clarify.FuzzyMatchingTest.Strategy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.FuzzyAlgo.Interfaces;
using WebApp.FuzzyAlgo.Models;
using DataModels = Clarify.FuzzyMatchingTest;

namespace WebApp.FuzzyAlgo.Services
{
    public class FuzzySharpRunService : IAlgoRunService
    {
        private readonly int expectedMatchingScore = 80;

        public async Task<AlgoRunResponse> RunAlgo(AlgoRunRequest request)
        {
            BaseRoomMappingStrategy roomMappingStrategy = null;
            string versionId = Guid.NewGuid().ToString();
            List<string> fields = request.Fields;

            switch (request.Strategy.ToLower())
            {
                case "singlestring":
                    roomMappingStrategy = new SingleStringMatchingStrategy(new FuzzyStringMatchingAlgo(), versionId);
                    break;
                case "perfield":
                    roomMappingStrategy = new PerFieldRoomMatchingStrategy(new FuzzyStringMatchingAlgo(), versionId);
                    break;
                case "hotelbedsavailability":
                    roomMappingStrategy = new HotelBedsDataAvailabilityStrategy(new FuzzyStringMatchingAlgo(), versionId);
                    break;
                default:
                    roomMappingStrategy = new HotelBedsDataAvailabilityStrategy(new FuzzyStringMatchingAlgo(), versionId);
                    break;
            }

            var result = await ExecuteStrategy(roomMappingStrategy, fields, versionId);
            return result;
        }

        private async Task<AlgoRunResponse> ExecuteStrategy(BaseRoomMappingStrategy strategy, List<string> fields, string versionId)
        {
            var roomMappingviewExtractor = new RoomMappingViewExtractor();
            DataModels.RoomMappingSummary summary = null;

            try
            {
                using (var logDB = new LogDb(Settings.GetConnectionString()))
                {
                    IDataLogger dataLogger = new MySqlLogger(new Logger(logDB));

                    strategy.Initialize(versionId, @"C:\Clarifi\RoomMapping\ExportedRooms");

                    var roomMappingResult = strategy.ExecuteHotelBedEanRoomMapping(fields);

                    var hotelBedsMappedView = roomMappingviewExtractor.GetRoomMappingWithTresholdPerHotel(roomMappingResult, expectedMatchingScore);

                    var epsMappedView = roomMappingviewExtractor.GetEpsMappedRooms(strategy.EpsSupplierData, hotelBedsMappedView, versionId, strategy.GetStrategyName(), strategy.GetMatchingAlgorithmName());

                    dataLogger.LogSupplierRoomData(strategy.EpsSupplierData.ToList());
                    dataLogger.LogSupplierRoomData(strategy.HotelBedSupplierData.ToList());

                    foreach (var epsMappingKvPair in epsMappedView)
                        dataLogger.LogEPSRoomMatching($"{epsMappingKvPair.Key}_'EPSMappedView'_{DateTime.Now.ToString("yyyyMMddTHHmmss")}.json", epsMappingKvPair.Value);

                    summary = StatsWriter(epsMappedView, versionId, dataLogger, strategy.GetStrategyName(), strategy.GetMatchingAlgorithmName());
                }

                return new AlgoRunResponse
                {
                    Message = "Algo execution completed.",
                    Summary = summary
                };
            }
            catch(Exception e)
            {
                return new AlgoRunResponse
                {
                    Message = $"Exception occurred with msg : {e.Message}",
                };
            }
            return new AlgoRunResponse
            {
                Message = "Internal server error.",
            };
        }

        private static RoomMappingSummary StatsWriter(Dictionary<string, List<EpsMappedRooms>> epsMappedView, string versionId,
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
                    string appliedStrategyName = string.Empty;
                    string matchingAlgorithm = string.Empty;

                    int hbRoomsCount = 0;
                    foreach (var map in kvp.Value)
                    {
                        roomLevelStats.Add(new DataModels.RoomLevelStats(versionId, kvp.Key,
                            map.EpsRoomId, map.EpsRoomName, map.MappedRooms.Count, map.AppliedStrategyName,
                            map.MatchingAlgorithm, map.MatchingStatus.ToString(), map.HBRoomsCount));

                        if (map.MatchingStatus == MatchingStatus.HBRoomsNotAvailable)
                            totalRoomsWithoutMatchingRooms++;

                        hbRoomsCount = map.HBRoomsCount;
                        appliedStrategyName = map.AppliedStrategyName;
                        matchingAlgorithm = map.MatchingAlgorithm;
                    }

                    int totalCount = kvp.Value.Count;
                    int mappedRoomCount = kvp.Value.Where(m => m.MappedRooms.Count > 0).Count();
                    double mappedPercentage = totalCount != 0 ? (double)(mappedRoomCount * 100) / totalCount : 0;

                    hotelLevelStats.Add(new DataModels.HotelLevelStats(versionId, kvp.Key, totalCount,
                        mappedRoomCount, mappedPercentage, hbRoomsCount, appliedStrategyName, matchingAlgorithm));

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

            return roomMappingSummary;
        }
    }
}
