using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Clarify.FuzzyMatchingTest.Data.Models;
using Newtonsoft.Json;

namespace Clarify.FuzzyMatchingTest
{
    public class FileWriter : IDataLogger
    { 
       
        public void LogEPSRoomMatching(string fileName, List<EpsMappedRooms> epsMappedRoomView)
        {
            
            if (epsMappedRoomView.Count > 0)
            {
                string json = JsonConvert.SerializeObject(epsMappedRoomView);

                System.IO.File.WriteAllText(Directory.GetCurrentDirectory() + "\\Output\\RoomType\\" + fileName, json);
            }
        }

        public void LogHotelBedsRoomMatching(string fileName, List<RoomMappingResult> result)
        {
            if (result.Count > 0)
            {
                result.ForEach(r => r.RoomMatchingScore.OrderByDescending(s => s.MatchingScore));
                string json = JsonConvert.SerializeObject(result);

                System.IO.File.WriteAllText(Directory.GetCurrentDirectory() + "\\Output\\RoomType\\" + fileName, json);
                
            }
        }

        public void LogRoomMatchingMetaData(List<RoomMappingResult> roomMappingResultWithThreshold, ClarifiModel epsSupplierData, ClarifiModel hotelBedsSupplierData)
        {
            if (roomMappingResultWithThreshold.Count > 0)
            {
                var clarifiHotelId = hotelBedsSupplierData.HotelClarifiId;
                var totalRoomMatchCount = roomMappingResultWithThreshold.Count;
                var totalhotelBedsRoom = hotelBedsSupplierData.RoomsData.Count;
                var totalEpsRoom = epsSupplierData.RoomsData.Count;
                decimal matchingPercentage = (Convert.ToDecimal(totalRoomMatchCount * 100 / totalhotelBedsRoom));
               
                File.AppendAllText("RoomResultStats.csv", Environment.NewLine + $"{clarifiHotelId},{totalEpsRoom},{totalhotelBedsRoom},{totalRoomMatchCount},{matchingPercentage},{GetMatchingRoomsWithoutComma(roomMappingResultWithThreshold)}");
            }
        }
        private string GetMatchingRooms(List<RoomMappingResult> roomMappingResultWithThreshold)
        {
            var matchingRoomPairs = string.Empty;
            roomMappingResultWithThreshold.ForEach(x =>
            {
                matchingRoomPairs += $"||{x.RoomId} |{x.HBMatchingStringForHighestMatch} | {x.RoomMatchingScore.Where(v => v.MatchingScore >= 80).Select(y => new EpsRoomDetails(y.EPSRoomId, y.EPSMatchingString).ToString()).Aggregate((m, n) => m + " | " + n)}";

            });

            return matchingRoomPairs;
        }
        private string GetMatchingRoomsWithoutComma(List<RoomMappingResult> roomMappingResultWithThreshold)
        {
            string str = GetMatchingRooms(roomMappingResultWithThreshold);
            return str.Replace(',', ' ');
        }

        private List<HotelBedMappedRoomDetail> GetMappedRooms(RoomsData epsRoom, List<RoomMappingResult> roomsMappedToEpsRoom)
        {
            List<HotelBedMappedRoomDetail> hotelBedMappedRoomDetails = new List<HotelBedMappedRoomDetail>();
            foreach (var room in roomsMappedToEpsRoom)
            {
                HotelBedMappedRoomDetail hotelBedMappedRoomDetail = new HotelBedMappedRoomDetail();
                //hotelBedMappedRoomDetail.HBRoomName=room.
                hotelBedMappedRoomDetail.HBMatchingString = room.HBMatchingStringForHighestMatch;
                hotelBedMappedRoomDetail.MatchingFields = room.FieldsUsedForHighestMatch;
                hotelBedMappedRoomDetail.MatchScore = room.HighestMatchedScore;
                hotelBedMappedRoomDetail.EpsMatchingString = room.EpsMatchingStringForHighestMatch;
                hotelBedMappedRoomDetails.Add(hotelBedMappedRoomDetail);
            }
            return hotelBedMappedRoomDetails;
        }

        public void LogSupplierRoomData(List<ClarifiModel> supplierRoomsData)
        {
            throw new NotImplementedException();
        }

        public void LogRoomLevelStats(List<RoomLevelStats> roomLevelStats)
        {
            throw new NotImplementedException();
        }

        public void LogHotelLevelStats(List<HotelLevelStats> hotelLevelStats)
        {
            throw new NotImplementedException();
        }

        public void LogRoomMappingSummary(RoomMappingSummary roomMappingSummary)
        {
            throw new NotImplementedException();
        }
    }
}
