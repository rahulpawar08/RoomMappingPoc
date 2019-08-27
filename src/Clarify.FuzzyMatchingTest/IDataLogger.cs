using Clarify.FuzzyMatchingTest.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clarify.FuzzyMatchingTest
{
    public interface IDataLogger
    {
        void LogHotelBedsRoomMatching(string fileName, List<RoomMappingResult> result);

        void LogEPSRoomMatching(string fileName, List<EpsMappedRooms> epsMappedRooms);

        void LogRoomMatchingMetaData(List<RoomMappingResult> roomMappingResultWithThreshold, ClarifiModel epsSupplierData, ClarifiModel hotelBedsSupplierData);

        void LogSupplierRoomData(ClarifiModel supplierRoomData);
    }
}
