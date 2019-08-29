namespace Clarify.FuzzyMatchingTest
{
    public class HotelLevelStats
    {
        public string VersionId { get; set; }
        public string ClarifiHotelId { get; set; }
        public int EPSRoomsCount { get; set; }
        public int EPSMappedRoomsCount { get; set; }
        public double MappingPercentage { get; set; }

        public HotelLevelStats()
        {
        }

        public HotelLevelStats(string versionId, string clarifiHotelId, int epsRoomsCount,
            int epsMappedRoomsCount, double mappingPercentage)
        {
            VersionId = versionId;
            ClarifiHotelId = clarifiHotelId;
            EPSRoomsCount = epsRoomsCount;
            EPSMappedRoomsCount = epsMappedRoomsCount;
            MappingPercentage = mappingPercentage;
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
        public double OverallMappingPercentage { get; set; }

        public RoomMappingSummary()
        {
        }

        public RoomMappingSummary(string versionId, int hotelsWithoutRooms, int hotelsWithRooms,
            int totalHotels, int totalMappedRooms, int totalRooms, double overallMappingPercentage)
        {
            VersionId = versionId;
            HotelsWithoutRooms = hotelsWithoutRooms;
            HotelsWithRooms = hotelsWithRooms;
            TotalHotels = totalHotels;
            TotalMappedRooms = totalMappedRooms;
            TotalRooms = totalRooms;
            OverallMappingPercentage = overallMappingPercentage;
        }
    }

    public class RoomLevelStats
    {
        public string VersionId { get; set; }
        public string ClarifiHotelId { get; set; }
        public string EpsRoomId { get; set; }
        public string EpsRoomName { get; set; }
        public int HBRoomsMappedCount { get; set; }

        public RoomLevelStats()
        {
        }

        public RoomLevelStats(string versionId, string clarifiHotelId, string epsRoomId,
            string epsRoomName, int hbRoomsMappedCount)
        {
            VersionId = versionId;
            ClarifiHotelId = clarifiHotelId;
            EpsRoomId = epsRoomId;
            EpsRoomName = epsRoomName;
            HBRoomsMappedCount = hbRoomsMappedCount;
        }
    }
}
