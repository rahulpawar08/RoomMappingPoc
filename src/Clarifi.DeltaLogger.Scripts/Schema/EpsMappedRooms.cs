using System;
using System.Collections.Generic;
using System.Text;

namespace Clarifi.RoomMappingLogger.Scripts
{
    [TypeInfo("eps_mapped_rooms")]
    public class EpsMappedRooms : LogEntryBase
    {
        [SimpleField("version_id")]
        public string VersionId { get; set; }

        [SimpleField("clarifi_hotelid")]
        public string ClarifiHotelId { get; set; }

        [SimpleField("eps_hotelid")]
        public string EpsHotelId { get; set; }

        [SimpleField("eps_roomid")]
        public string EpsRoomId { get; set; }

        [SimpleField("eps_room_name")]
        public string EpsRoomName { get; set; }

        [SimpleField("eps_hotel_name")]
        public string EpsHotelName { get; set; }

        [SimpleField("applied_strategy_name")]
        public string AppliedStrategyName { get; set; }

        [SimpleField("matching_algorithm")]
        public string MatchingAlgorithm { get; set; }

        [SimpleField("matching_status")]
        public string MatchingStatus { get; set; }

        [SimpleField("hb_rooms_count")]
        public int HBRoomsCount { get; set; }

        [SimpleField("added_date")]
        public DateTime AddedDate { get; set; }

        [NestedField()]
        public List<HotelBedsMappedRoomDetail> MappedRooms { get; set; }
    }

    [TypeInfo("hotelBeds_mapped_room_details")]
    public class HotelBedsMappedRoomDetail : LogEntryBase
    {
        [SimpleField("hb_hotelid")]
        public string HBHotelId { get; set; }

        [SimpleField("hb_roomid")]
        public string HBRoomId { get; set; }

        [SimpleField("hb_matching_string")]
        public string HBMatchingString { get; set; }

        [SimpleField("hb_room_name")]
        public string HBRoomName { get; set; }

        [SimpleField("match_score")]
        public int MatchScore { get; set; }

        [SimpleField("eps_matching_string")]
        public string EpsMatchingString { get; set; }

        [SimpleField("matching_field")]
        public string MatchingFields { get; set; }

        [SimpleField("applied_strategy_name")]
        public string AppliedStrategyName { get; set; }

        [SimpleField("matching_algorithm")]
        public string MatchingAlgorithm { get; set; }

        [SimpleField("hb_rooms_count")]
        public int HBRoomsCount { get; set; }
    }
}
