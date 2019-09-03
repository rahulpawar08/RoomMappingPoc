using Clarify.FuzzyMatchingTest;
using System;
using System.Collections.Generic;
using System.Text;
using Clarify.FuzzyMatchingTest.Data.Models;
using Clarifi.RoomMappingLogger;
using Clarifi.DeltaLogger.Scripts.Parser;
using System.Threading.Tasks;
using System.Linq;

namespace WebApp.FuzzyAlgo
{
    internal class MySqlLogger : IDataLogger
    {
        Logger _logger = null;

        public MySqlLogger(Logger logger)
        {
            _logger = logger;
        }

        public void LogEPSRoomMatching(string fileName, List<EpsMappedRooms> epsMappedRooms)
        {
            if (epsMappedRooms != null && epsMappedRooms.Count > 0)
            {
                var epsMappedRoomDatas = epsMappedRooms.Select(x => EpsMappedRoomsTranslator.GetEpsMappedRoom(x)).ToList();
                Task.Run(() => _logger.RecordEntryAsync(epsMappedRoomDatas)).Wait();
            }
        }

        public void LogHotelBedsRoomMatching(string fileName, List<RoomMappingResult> result)
        {
            throw new NotImplementedException();
        }

        public void LogHotelLevelStats(List<HotelLevelStats> hotelLevelStatsData)
        {
            var hotelLevelStats = RoomMappingStatsTranslator.GetHotelLevelStats(hotelLevelStatsData);
            foreach (var hotelLevelStat in hotelLevelStats)
            {
                Task.Run(() => _logger.RecordEntryAsync(hotelLevelStat)).Wait();
            }
        }

        public void LogRoomLevelStats(List<RoomLevelStats> roomLevelStatsData)
        {
            var roomLevelStats = RoomMappingStatsTranslator.GetRoomLevelStats(roomLevelStatsData);
            foreach (var roomLevelStat in roomLevelStats)
            {
                Task.Run(() => _logger.RecordEntryAsync(roomLevelStat)).Wait();
            }
        }

        public void LogRoomMappingSummary(RoomMappingSummary roomMappingSummaryData)
        {
            var roomMappingSummary = RoomMappingStatsTranslator.GetRoomMappingSummary(roomMappingSummaryData);

            Task.Run(() => _logger.RecordEntryAsync(roomMappingSummary)).Wait();
        }

        public void LogRoomMatchingMetaData(List<RoomMappingResult> roomMappingResultWithThreshold, ClarifiModel epsSupplierData, ClarifiModel hotelBedsSupplierData)
        {
            throw new NotImplementedException();
        }

        public void LogSupplierRoomData(List<ClarifiModel> supplierRoomsData)
        {
            if (supplierRoomsData != null && supplierRoomsData.Count > 0)
            {
                var supplierRoomsInfo = supplierRoomsData.Select(x => ClarifiRoomDataTranslator.GetClarifiModel(x)).ToList();
                Task.Run(() => _logger.RecordEntryAsync(supplierRoomsInfo)).Wait();
            }
        }
    }
}
