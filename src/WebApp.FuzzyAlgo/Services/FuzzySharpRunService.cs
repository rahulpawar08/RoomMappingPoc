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

                    strategy.Initialize();

                    var roomMappingResult = strategy.ExecuteHotelBedEanRoomMapping(fields);

                    var hotelBedsMappedView = roomMappingviewExtractor.GetRoomMappingWithTresholdPerHotel(roomMappingResult, expectedMatchingScore);

                    var epsMappedView = roomMappingviewExtractor.GetEpsMappedRooms(strategy.EpsSupplierData, hotelBedsMappedView, versionId, strategy.GetStrategyName(), strategy.GetMatchingAlgorithmName());

                    strategy.EpsSupplierData.ToList().ForEach(x => dataLogger.LogSupplierRoomData(x));
                    strategy.HotelBedSupplierData.ToList().ForEach(x => dataLogger.LogSupplierRoomData(x));

                    foreach (var epsMappingKvPair in epsMappedView)
                        dataLogger.LogEPSRoomMatching($"{epsMappingKvPair.Key}_'EPSMappedView'_{DateTime.Now.ToString("yyyyMMddTHHmmss")}.json", epsMappingKvPair.Value);

                    summary = StatsWriter(epsMappedView, versionId, dataLogger);
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

        private static DataModels.RoomMappingSummary StatsWriter(Dictionary<string, List<EpsMappedRooms>> epsMappedView, string versionId, IDataLogger dataLogger)
        {
            List<DataModels.HotelLevelStats> hotelLevelStats = new List<DataModels.HotelLevelStats>();
            List<DataModels.RoomLevelStats> roomLevelStats = new List<DataModels.RoomLevelStats>();

            int zeroRoomCount = 0;
            int hotelCount = 0;
            int total = 0;
            int totalRooms = 0;
            int totalMappedRooms = 0;
            double avgPercentage = 0;

            foreach (var kvp in epsMappedView)
            {
                foreach (var map in kvp.Value)
                {
                    roomLevelStats.Add(new DataModels.RoomLevelStats(versionId, kvp.Key,
                        map.EpsRoomId, map.EpsRoomName, map.MappedRooms.Count));
                }

                int totalCount = kvp.Value.Count;
                int mappedRoomCount = kvp.Value.Where(m => m.MappedRooms.Count > 0).Count();
                double mappedPercentage = totalCount != 0 ? (double)(mappedRoomCount * 100) / totalCount : 0;

                hotelLevelStats.Add(new DataModels.HotelLevelStats(versionId, kvp.Key, totalCount, mappedRoomCount, mappedPercentage));

                if (totalCount == 0)
                    zeroRoomCount++;
                else
                    hotelCount++;
                total++;
                totalRooms += totalCount;
                totalMappedRooms += mappedRoomCount;
            }
            avgPercentage = (double)(totalMappedRooms * 100) / totalRooms;

            DataModels.RoomMappingSummary roomMappingSummary = new DataModels.RoomMappingSummary(versionId, zeroRoomCount, hotelCount, total, totalMappedRooms,
                totalRooms, avgPercentage);

            dataLogger.LogHotelLevelStats(hotelLevelStats);
            dataLogger.LogRoomLevelStats(roomLevelStats);
            dataLogger.LogRoomMappingSummary(roomMappingSummary);

            return roomMappingSummary;
        }
    }
}
