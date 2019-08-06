using Clarify.FuzzyMatchingTest.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clarify.FuzzyMatchingTest
{
    public interface IDataWriter
    {
        void WriteHotelBedsRoomMatching(string fileName, List<RoomMappingResult> result);

        void WriteEPSRoomMatching(string fileName, List<EpsMappedRooms> epsMappedRooms);

        void WriteRoomMatchingMetaData(List<RoomMappingResult> roomMappingResultWithThreshold, ClarifiModel epsSupplierData, ClarifiModel hotelBedsSupplierData);
    }
}
