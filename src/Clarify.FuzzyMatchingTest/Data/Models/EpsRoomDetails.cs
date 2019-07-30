using System;
using System.Collections.Generic;
using System.Text;

namespace Clarify.FuzzyMatchingTest.Data.Models
{
    public class EpsRoomDetails
    {
        public string EpsRoomId { get; set; }

        public string EpsMatchingString { get; set; }

        public EpsRoomDetails(string id, string matchingString)
        {
            EpsRoomId = id;
            EpsMatchingString = matchingString;
        }

        public override string ToString()
        {
            return EpsRoomId + "|" + EpsMatchingString;
        }

    }
}
