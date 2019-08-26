using System;
using System.Collections.Generic;
using System.Text;
using Clarify.FuzzyMatchingTest.Data.Models;

namespace Clarify.FuzzyMatchingTest
{
    public class MySqlWriter : IDataWriter
    {
        public MySqlWriter(Logger )
        {
        }

        public void WriteEPSRoomMatching(string fileName, List<EpsMappedRooms> epsMappedRooms)
        {
            throw new NotImplementedException();
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
