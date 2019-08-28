using Clarify.FuzzyMatchingTest.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clarify.FuzzyMatchingTest
{
    public interface IRoomMappingStrategy
    {
        List<RoomMappingResult> ExecuteHotelBedEanRoomMapping(List<string> matchingFields);
        void Initialize();
        string GetStrategyName();
    }
}
