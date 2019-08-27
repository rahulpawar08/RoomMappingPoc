using Clarify.FuzzyMatchingTest;
using System;
using System.Collections.Generic;
using System.Text;
using Clarify.FuzzyMatchingTest.Data.Models;
using Clarifi.RoomMappingLogger;
using Clarifi.DeltaLogger.Scripts.Parser;
using System.Threading.Tasks;


namespace ConsoleApp.FuzzyAlgo
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
            foreach (var epsMappedRoom in epsMappedRooms)
            {
                var epsMappedRoomData = EpsMappedRoomsTranslator.GetEpsMappedRoom(epsMappedRoom);

                Task.Run(() => _logger.RecordEntryAsync(epsMappedRoomData)).Wait();
            }
        }

        public void LogHotelBedsRoomMatching(string fileName, List<RoomMappingResult> result)
        {
            throw new NotImplementedException();
        }

        public void LogRoomMatchingMetaData(List<RoomMappingResult> roomMappingResultWithThreshold, ClarifiModel epsSupplierData, ClarifiModel hotelBedsSupplierData)
        {
            throw new NotImplementedException();
        }

        public void LogSupplierRoomData(ClarifiModel supplierRoomData)
        {
            var supplier_roomdata = ClarifiRoomDataTranslator.GetClarifiModel(supplierRoomData);

            Task.Run(() => _logger.RecordEntryAsync(supplier_roomdata)).Wait();
        }
    }
}
