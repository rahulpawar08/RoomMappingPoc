using System;
using System.Collections.Generic;
using System.Text;
using Clarify.FuzzyMatchingTest.Data.Models;

namespace Clarify.FuzzyMatchingTest
{
    public class PerFieldRoomMatchingStrategy : BaseRoomMappingStrategy
    {
      
        public PerFieldRoomMatchingStrategy(IMatchingAlgorithm matchingAlgorithm) : base(matchingAlgorithm)
        {
            
        }

        public override List<RoomMappingResult> ExecuteHotelBedEanRoomMapping(List<string> matchingFields)
        {
            return null;
        }
    }
}
