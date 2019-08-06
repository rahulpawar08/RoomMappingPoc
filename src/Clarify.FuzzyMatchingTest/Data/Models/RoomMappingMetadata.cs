using System;
using System.Collections.Generic;
using System.Text;

namespace Clarify.FuzzyMatchingTest.Data.Models
{
    public class RoomMappingMetadata
    {
        public string ClarifiHotelId { get; set; }
        public int EpsRoomCount { get; set; }
        public int HotelBedsRoomCount { get; set; }
        public int MappedRoomCount { get; set; }
        //percentage of total rooms mapped
        public decimal MappingPercentage { get; set; }
        public List<RoomMappingOverview> RoomMappingOverview { get;  set; }

    }
}
