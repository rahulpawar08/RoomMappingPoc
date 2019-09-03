namespace Clarify.FuzzyMatchingTest
{
    public class HotelLevelStats
    {
        public string VersionId { get; set; }
        public string ClarifiHotelId { get; set; }
        public int EPSRoomsCount { get; set; }
        public int EPSMappedRoomsCount { get; set; }
        public int HBRoomsCount { get; set; }
        public double MappingPercentage { get; set; }

        public HotelLevelStats()
        {
        }

        public HotelLevelStats(string versionId, string clarifiHotelId, int epsRoomsCount,
            int epsMappedRoomsCount, double mappingPercentage,int hbRoomsCount)
        {
            VersionId = versionId;
            ClarifiHotelId = clarifiHotelId;
            EPSRoomsCount = epsRoomsCount;
            EPSMappedRoomsCount = epsMappedRoomsCount;
            MappingPercentage = mappingPercentage;
            HBRoomsCount = hbRoomsCount;
        }
    }

    public class RoomMappingSummary
    {
        public string VersionId { get; set; }
        public int TotalHotels { get; set; }
        public int HotelsWithoutRooms { get; set; }
        public int HotelsWithRooms { get; set; }
        public int TotalRooms { get; set; }
        public int TotalMappedRooms { get; set; }
        public int TotalRoomsWithoutMatchingRooms { get; set; }
        public int TotalRoomsWithNoMatch { get; set; }
        public double OverallMappingPercentage { get; set; }
        public string AppliedStrategyName { get; set; }
        public string MatchingAlgorithm { get; set; }

        public RoomMappingSummary()
        {
        }

        public RoomMappingSummary(string versionId, int hotelsWithoutRooms, int hotelsWithRooms,
            int totalHotels, int totalMappedRooms, int totalRooms, double overallMappingPercentage,
            string appliedStrategyName, string matchingAlgorithm, int totalRoomsWithoutMatchingRooms,
            int totalRoomsWithNoMatch)
        {
            VersionId = versionId;
            HotelsWithoutRooms = hotelsWithoutRooms;
            HotelsWithRooms = hotelsWithRooms;
            TotalHotels = totalHotels;
            TotalMappedRooms = totalMappedRooms;
            TotalRooms = totalRooms;
            OverallMappingPercentage = overallMappingPercentage;
            AppliedStrategyName = appliedStrategyName;
            MatchingAlgorithm = matchingAlgorithm;
            TotalRoomsWithoutMatchingRooms = totalRoomsWithoutMatchingRooms;
            TotalRoomsWithNoMatch = totalRoomsWithNoMatch;
        }
    }

    public class RoomLevelStats
    {
        public string VersionId { get; set; }
        public string ClarifiHotelId { get; set; }
        public string EpsRoomId { get; set; }
        public string EpsRoomName { get; set; }
        public int HBRoomsMappedCount { get; set; }
        public int HBRoomsCount { get; set; }
        public string AppliedStrategyName { get; set; }
        public string MatchingAlgorithm { get; set; }
        public string MatchingStatus { get; set; }

        public RoomLevelStats()
        {
        }

        public RoomLevelStats(string versionId, string clarifiHotelId, string epsRoomId,
            string epsRoomName, int hbRoomsMappedCount, string appliedStrategyName,
            string matchingAlgorithm, string matchingStatus, int hbRoomsCount)
        {
            VersionId = versionId;
            ClarifiHotelId = clarifiHotelId;
            EpsRoomId = epsRoomId;
            EpsRoomName = epsRoomName;
            HBRoomsMappedCount = hbRoomsMappedCount;
            AppliedStrategyName = appliedStrategyName;
            MatchingAlgorithm = matchingAlgorithm;
            MatchingStatus = matchingStatus;
            HBRoomsCount = hbRoomsCount;
        }
    }
}
