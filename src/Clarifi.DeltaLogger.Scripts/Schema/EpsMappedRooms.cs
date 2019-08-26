using System;
using System.Collections.Generic;
using System.Text;

namespace Clarifi.RoomMappingLogger.Scripts
{
    [TypeInfo("eps_mapped_rooms")]
    public class EpsMappedRooms : LogEntryBase
    {

        [SimpleField("eps_roomid")]
        public string EpsRoomId { get; set; }

        [SimpleField("eps_room_name")]
        public string EpsRoomName { get; set; }

        [NestedField()]
        public List<HotelBedsMappedRoomDetail> MappedRooms { get; set; }
    }

    [TypeInfo("hotelBeds_mapped_room_details")]
    public class HotelBedsMappedRoomDetail : LogEntryBase
    {
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
    }
}
