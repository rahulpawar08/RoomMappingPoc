using Clarify.FuzzyMatchingTest.Data.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Clarify.FuzzyMatchingTest
{
    public interface IRoomMappingStrategy
    {
        ConcurrentBag<RoomMappingResult> ExecuteHotelBedEanRoomMapping(List<string> matchingFields);
        void Initialize(string versionId);
        string GetStrategyName();
    }
}
