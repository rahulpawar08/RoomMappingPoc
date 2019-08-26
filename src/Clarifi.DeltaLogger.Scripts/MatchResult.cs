using System;
using System.Collections.Generic;
using System.Text;

namespace Clarifi.DeltaLogger.Scripts
{
    public class MatchResult
    {
        public string MatchingMethod { get; set; }
        public string MatchingField { get; set; }

        public string HotelBedMatchingString { get; set; }

        public string EPSMatchingString { get; set; }

        public string EPSRoomId { get; set; }

        public int MatchingScore { get; set; }
    }
}
