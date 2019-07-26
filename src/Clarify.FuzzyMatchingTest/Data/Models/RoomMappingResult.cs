using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clarify.FuzzyMatchingTest.Data.Models
{
    public class RoomMappingResult
    {
        public RoomMappingResult()
        {
            RoomMatchingScore = new List<MatchResult>();
        }

        public RoomMappingResult(string supplierName, string clarifiHotelId, string supplierHotelId, string roomId)
        {
            SupplierHotelId = supplierHotelId;
            SupplierName = supplierName;
            RoomId = roomId;
            ClarifiHotelId = clarifiHotelId;
            RoomMatchingScore = new List<MatchResult>();
            MostMatchedRooms = new List<MostMatchedRoom>();
        }
        public string SupplierName { get; set; }

        public string SupplierHotelId { get; set; }

        public string ClarifiHotelId { get; set; }

        public string RoomId { get; set; }

        public string MostMatchedRoomId { get; set; }

        public int HighestMatchedScore { get; set; }

        public string FieldsUsedForHighestMatch { get; set; }

        public string EpsMatchingStringForHighestMatch { get; set; }

        public string HBMatchingStringForHighestMatch { get; set; }

        public List<MatchResult> RoomMatchingScore { get; set; }

        public List<MostMatchedRoom> MostMatchedRooms { get; set; }

        public void SetMatchedRoom()
        {
            RoomMatchingScore.Sort();
            MostMatchedRoomId = RoomMatchingScore.Last().EPSRoomId;
            HighestMatchedScore = RoomMatchingScore.Last().MatchingScore;
            FieldsUsedForHighestMatch = RoomMatchingScore.Last().MatchingField;
            EpsMatchingStringForHighestMatch = RoomMatchingScore.Last().EPSMatchingString;
            HBMatchingStringForHighestMatch = RoomMatchingScore.Last().HotelBedMatchingString;

            var mostMatchedRooms = RoomMatchingScore.FindAll(r => r.MatchingScore >= RoomMatchingScore.Last().MatchingScore);
            if (mostMatchedRooms != null)
            {
                foreach (var room in mostMatchedRooms)
                {
                    MostMatchedRooms.Add(new MostMatchedRoom
                    {
                        MostMatchedRoomId = room.EPSRoomId,
                        FieldsUsedForHighestMatch = room.EPSMatchingString,
                        HighestMatchedScore = room.MatchingScore
                    });
                }
            }
        }
    }

    public class MatchResult:IComparable
    {
        public string MatchingMethod { get; set; }
        public string MatchingField { get; set; }

        public string HotelBedMatchingString { get; set; }

        public string EPSMatchingString { get; set; }

        public string EPSRoomId { get; set; }

        public int MatchingScore { get; set; }

        public int CompareTo(object obj)
        {
            return MatchingScore.CompareTo(((MatchResult)obj).MatchingScore);
        }
    }

    public class MostMatchedRoom
    {
        public string MostMatchedRoomId { get; set; }

        public int HighestMatchedScore { get; set; }

        public string FieldsUsedForHighestMatch { get; set; }
    }
}
