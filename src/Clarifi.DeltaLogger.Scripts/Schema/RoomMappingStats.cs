using Clarifi.RoomMappingLogger;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clarifi.DeltaLogger.Scripts
{
    [TypeInfo("HotelLevelStats")]
    public class HotelLevelStats : LogEntryBase
    {
        [SimpleField("version_id")]
        public string VersionId { get; set; }

        [SimpleField("clarifi_hotelid")]
        public string ClarifiHotelId { get; set; }

        [SimpleField("eps_rooms_count")]
        public int EPSRoomsCount { get; set; }

        [SimpleField("eps_mapped_rooms_count")]
        public int EPSMappedRoomsCount { get; set; }

        [SimpleField("mapping_percentage")]
        public double MappingPercentage { get; set; }
    }


    [TypeInfo("RoomMappingSummary")]
    public class RoomMappingSummary : LogEntryBase
    {
        [SimpleField("version_id")]
        public string VersionId { get; set; }

        [SimpleField("total_hotels")]
        public int TotalHotels { get; set; }

        [SimpleField("hotels_without_rooms")]
        public int HotelsWithoutRooms { get; set; }

        [SimpleField("hotels_with_rooms")]
        public int HotelsWithRooms { get; set; }

        [SimpleField("total_rooms")]
        public int TotalRooms { get; set; }

        [SimpleField("total_mapped_rooms")]
        public int TotalMappedRooms { get; set; }

        [SimpleField("overall_mapping_percentage")]
        public double OverallMappingPercentage { get; set; }
    }

    [TypeInfo("RoomLevelStats")]
    public class RoomLevelStats : LogEntryBase
    {
        [SimpleField("version_id")]
        public string VersionId { get; set; }

        [SimpleField("clarifi_hotelid")]
        public string ClarifiHotelId { get; set; }

        [SimpleField("eps_roomid")]
        public string EpsRoomId { get; set; }

        [SimpleField("eps_room_name")]
        public string EpsRoomName { get; set; }

        [SimpleField("hb_rooms_mapped_count")]
        public int HBRoomsMappedCount { get; set; }
    }
}
