using System;
using System.Collections.Generic;
using System.Text;

namespace Clarifi.DeltaLogger.Scripts
{
    public class HotelBedMappedRoomDetail
    {
        public string HBMatchingString { get; set; }
        public string HBRoomName { get; set; }
        public int MatchScore { get; set; }
        public string EpsMatchingString { get; set; }
        public string MatchingFields { get; set; }
    }
}
