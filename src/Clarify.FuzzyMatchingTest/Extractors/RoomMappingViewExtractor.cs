using Clarify.FuzzyMatchingTest.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clarify.FuzzyMatchingTest
{
    public class RoomMappingViewExtractor
    {
        public Dictionary<string, List<EpsMappedRooms>> GetEpsMappedRooms(List<ClarifiModel> epsSupplierData, Dictionary<string, List<RoomMappingResult>> roomMappingResults, string versionId, string strategyName)
        {
            Dictionary<string, List<EpsMappedRooms>> epsMappedRoomView = new Dictionary<string, List<EpsMappedRooms>>();
            foreach (var kvPair in roomMappingResults)
            {
                var epsData = epsSupplierData.FirstOrDefault(x => x.HotelClarifiId == kvPair.Key);
                var epsMappedRooms = new List<EpsMappedRooms>();
                foreach (var epsRoom in epsData.RoomsData)
                {

                    var roomsMappedToEpsRoom = kvPair.Value.FindAll(r => r.MostMatchedRoomId == epsRoom.SupplierRoomId);
                    if (roomsMappedToEpsRoom != null)
                    {
                        epsMappedRooms.Add(new EpsMappedRooms()
                        {
                            VersionId = versionId,
                            EpsHotelId = epsRoom.SupplierId,
                            EpsRoomId = epsRoom.SupplierRoomId,
                            EpsRoomName = epsRoom.Name,
                            AppliedStrategyName= strategyName,
                            AddedDate= DateTime.UtcNow,
                            MappedRooms = GetMappedRooms(epsRoom, roomsMappedToEpsRoom)
                        });
                    }
                }
                epsMappedRoomView.Add(kvPair.Key, epsMappedRooms);
            }
            return epsMappedRoomView;
        }

        public Dictionary<string, List<RoomMappingResult>> GetRoomMappingWithTresholdPerHotel(List<RoomMappingResult> roomMappingResult, int expectedMatchingScore)
        {

            List<RoomMappingResult> roomMappingResultWithExpectedScore = new List<RoomMappingResult>();
            foreach (var result in roomMappingResult)
            {
                foreach (var score in result.RoomMatchingScore)
                {
                    if (score.MatchingScore >= expectedMatchingScore)
                    {

                        if (!roomMappingResultWithExpectedScore.Any(r => r.RoomId == result.RoomId && r.ClarifiHotelId == result.ClarifiHotelId))
                        {
                            roomMappingResultWithExpectedScore.Add(result);
                        }
                    }
                }
            }

            return roomMappingResultWithExpectedScore.GroupBy(x => x.ClarifiHotelId).ToDictionary(z => z.Key, y => y.ToList());
        }

        public List<RoomMappingMetadata> GetRoomMappingMetadata(List<RoomMappingResult> roomMappingResultWithThreshold, List<ClarifiModel> epsSupplierData, List<ClarifiModel> hotelBedsSupplierData)
        {
            var roomMappingPerHotel = roomMappingResultWithThreshold.GroupBy(x => x.ClarifiHotelId).ToDictionary(z => z.Key, y => y.ToList());
            var result = new List<RoomMappingMetadata>();
            foreach (var roomMappingResult in roomMappingPerHotel)
            {
                var roomMappingMetaData = new RoomMappingMetadata();
                roomMappingMetaData.ClarifiHotelId = roomMappingResult.Key;
                roomMappingMetaData.EpsRoomCount = epsSupplierData.FirstOrDefault(x => x.HotelClarifiId == roomMappingResult.Key).RoomsData.Count;
                roomMappingMetaData.HotelBedsRoomCount = hotelBedsSupplierData.FirstOrDefault(x => x.HotelClarifiId == roomMappingResult.Key).RoomsData.Count;
                roomMappingMetaData.MappedRoomCount = roomMappingResult.Value.Count;
                roomMappingMetaData.MappingPercentage = Convert.ToDecimal(roomMappingMetaData.MappedRoomCount * 100 / roomMappingMetaData.HotelBedsRoomCount);
                roomMappingMetaData.RoomMappingOverview = GetRoomMappingOverview(roomMappingResult.Value);

                result.Add(roomMappingMetaData);
            }

            return result;
        }

        private List<RoomMappingOverview> GetRoomMappingOverview(List<RoomMappingResult> value)
        {
            var roomMappingOverview = new List<RoomMappingOverview>();
            foreach (var mapping in value)
            {
                var overview = new RoomMappingOverview();
                overview.HotelBedsRoomId = mapping.RoomId;
                overview.HotelBedsMappingString = mapping.HBMatchingStringForHighestMatch;
                overview.EpsRoomDetails = new List<EpsRoomDetails>();
                mapping.RoomMatchingScore.ForEach(x =>
                {
                    overview.EpsRoomDetails.Add(new EpsRoomDetails(x.EPSRoomId, x.EPSMatchingString));
                });
                roomMappingOverview.Add(overview);
            }
            return roomMappingOverview;
        }

        private List<HotelBedMappedRoomDetail> GetMappedRooms(RoomsData epsRoom, List<RoomMappingResult> roomsMappedToEpsRoom)
        {
            List<HotelBedMappedRoomDetail> hotelBedMappedRoomDetails = new List<HotelBedMappedRoomDetail>();
            foreach (var room in roomsMappedToEpsRoom)
            {
                HotelBedMappedRoomDetail hotelBedMappedRoomDetail = new HotelBedMappedRoomDetail();
                hotelBedMappedRoomDetail.HBHotelId = room.SupplierHotelId;
                hotelBedMappedRoomDetail.HBRoomId = room.RoomId;
                hotelBedMappedRoomDetail.HBRoomName = room.RoomName;
                hotelBedMappedRoomDetail.HBMatchingString = room.HBMatchingStringForHighestMatch;
                hotelBedMappedRoomDetail.MatchingFields = room.FieldsUsedForHighestMatch;
                hotelBedMappedRoomDetail.MatchScore = room.HighestMatchedScore;
                hotelBedMappedRoomDetail.EpsMatchingString = room.EpsMatchingStringForHighestMatch;
                hotelBedMappedRoomDetail.AppliedStrategyName = room.AppliedStrategyName;
                hotelBedMappedRoomDetails.Add(hotelBedMappedRoomDetail);
            }
            return hotelBedMappedRoomDetails;
        }
    }
}
