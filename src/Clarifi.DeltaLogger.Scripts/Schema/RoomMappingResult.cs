//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace Clarifi.RoomMappingLogger.Scripts
//{
//    [TypeInfo("room_mapping_results")]
//    public class RoomMappingResult
//    {
//        [SimpleField("supplier_name")]
//        public string SupplierName { get; set; }

//        [SimpleField("supplier_hotelid")]
//        public string SupplierHotelId { get; set; }

//        [SimpleField("clarifi_hotelid")]
//        public string ClarifiHotelId { get; set; }

//        [SimpleField("roomid")]
//        public string RoomId { get; set; }

//        [SimpleField("most_matched_roomid")]
//        public string MostMatchedRoomId { get; set; }

//        [SimpleField("highest_matched_score")]
//        public int HighestMatchedScore { get; set; }

//        [SimpleField("fields_used_for_highest_match")]
//        public string FieldsUsedForHighestMatch { get; set; }

//        [SimpleField("eps_matching_string_for_highest_match")]
//        public string EpsMatchingStringForHighestMatch { get; set; }

//        [SimpleField("hb_matching_string_for_highest_match")]
//        public string HBMatchingStringForHighestMatch { get; set; }

//        [NestedField()]
//        public List<MatchResult> RoomMatchingScore { get; set; }

//        [NestedField()]
//        public List<MostMatchedRoom> MostMatchedRooms { get; set; }
//    }

//    [TypeInfo("match_results")]
//    public class MatchResult
//    {
//        [SimpleField("matching_method")]
//        public string MatchingMethod { get; set; }

//        [SimpleField("matching_field")]
//        public string MatchingField { get; set; }

//        [SimpleField("hb_matching_string")]
//        public string HotelBedMatchingString { get; set; }

//        [SimpleField("eps_matching_string")]
//        public string EPSMatchingString { get; set; }

//        [SimpleField("eps_roomid")]
//        public string EPSRoomId { get; set; }

//        [SimpleField("match_score")]
//        public int MatchScore { get; set; }
//    }

//    [TypeInfo("most_matched_rooms")]
//    public class MostMatchedRoom
//    {
//        [SimpleField("most_matched_roomid")]
//        public string MostMatchedRoomId { get; set; }

//        [SimpleField("highest_matched_score")]
//        public int HighestMatchedScore { get; set; }

//        [SimpleField("fields_used_for_highest_match")]
//        public string FieldsUsedForHighestMatch { get; set; }
//    }
//}
