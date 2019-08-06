using System;
using System.Collections.Generic;
using System.Text;

namespace Clarify.FuzzyMatchingTest.Data.Models
{
    public class RoomMappingOverview
    {
        public string HotelBedsRoomId { get; set; }
        public string HotelBedsMappingString { get; set; }
        public List<EpsRoomDetails> EpsRoomDetails { get; set; }
    }
}
