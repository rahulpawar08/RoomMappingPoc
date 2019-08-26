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
    internal class MySqlWriter : IDataWriter
    {
        Logger _logger = null;

        public MySqlWriter(Logger logger)
        {
            _logger = logger;
        }

        public void WriteEPSRoomMatching(string fileName, List<EpsMappedRooms> epsMappedRooms)
        {
            foreach (var epsMappedRoom in epsMappedRooms)
            {
                var epsMappedRoomData = EpsMappedRoomsTranslator.GetEpsMappedRoom(epsMappedRoom);

                Task.Run(() => _logger.RecordEntryAsync(epsMappedRoomData)).Wait();
            }
        }

        public void WriteHotelBedsRoomMatching(string fileName, List<RoomMappingResult> result)
        {
            throw new NotImplementedException();
        }

        public void WriteRoomMatchingMetaData(List<RoomMappingResult> roomMappingResultWithThreshold, ClarifiModel epsSupplierData, ClarifiModel hotelBedsSupplierData)
        {
            throw new NotImplementedException();
        }
    }
}
