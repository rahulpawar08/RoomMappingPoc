using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BoomTown.FuzzySharp;
using Clarify.FuzzyMatchingTest.Data.Models;
using Newtonsoft.Json;

namespace Clarify.FuzzyMatchingTest
{
    public class SingleStringMatchingStrategy : BaseRoomMappingStrategy
    {
        public SingleStringMatchingStrategy(IMatchingAlgorithm matchingAlgorithm, string versionId) : base(matchingAlgorithm, "Single String Matching", versionId)
        {

        }

        public override List<RoomMappingResult> ExecuteHotelBedEanRoomMapping(List<string> matchingFields)
        {
            List<RoomMappingResult> roomMappingResults = new List<RoomMappingResult>();

            foreach (var hotelBedSupplierdata in HotelBedSupplierData)
            {

                var epsSupplierData = EpsSupplierData.FirstOrDefault(x => x.HotelClarifiId == hotelBedSupplierdata.HotelClarifiId);
                foreach (var hotelBedRoom in hotelBedSupplierdata.RoomsData)
                {
                    RoomMappingResult roomMappingResult = new RoomMappingResult("HotelBeds", hotelBedSupplierdata.HotelClarifiId, hotelBedSupplierdata.SupplierId, hotelBedRoom.SupplierRoomId, hotelBedRoom.Name);

                    foreach (var targetRoom in epsSupplierData.RoomsData)
                    {
                        foreach (var matchingField in matchingFields)
                        {
                            string hotelBedMappingString = hotelBedRoom.GetMappingString(matchingField);
                            string epsMappingString = targetRoom.GetMappingString(matchingField);

                            int score = (!string.IsNullOrEmpty(hotelBedMappingString) && !string.IsNullOrEmpty(epsMappingString)) ?
                                           RoomMatchingAlgo.GetMatchingScore(hotelBedMappingString, epsMappingString) : 0;

                            roomMappingResult.RoomMatchingScore.Add(new MatchResult()
                            {
                                MatchingMethod = "TokenSetRatio",
                                EPSRoomId = targetRoom.SupplierRoomId,
                                MatchingScore = score,
                                EPSMatchingString = epsMappingString,
                                HotelBedMatchingString = hotelBedMappingString,
                                MatchingField = matchingField
                            });
                        }
                        roomMappingResult.RoomMatchingScore.OrderByDescending(s => s.MatchingScore);
                    }


                    roomMappingResult.SetMatchedRoom();
                    roomMappingResult.AppliedStrategyName = StrategyName;
                    roomMappingResult.VersionId = VersionId;
                    roomMappingResults.Add(roomMappingResult);
                }
                //var roomMappingResultWithThreshold = GetResultMatchingThreshold(roomMappingResults, threshold);

                //DataWriter.WriteHotelBedsRoomMatching($"{inputFile.ClarifiHotelId}_{threshold}_{DateTime.Now.ToString("yyyyMMddTHHmmss")}.json", roomMappingResultWithThreshold);
                //DataWriter.WriteRoomMatchingMetaData(roomMappingResultWithThreshold, EpsSupplierData, HotelBedSupplierData);
                //DataWriter.WriteEPSRoomMatching($"{inputFile.ClarifiHotelId}_'EPSMappedView'_{DateTime.Now.ToString("yyyyMMddTHHmmss")}.json", EpsSupplierData, roomMappingResults);

            }
            return roomMappingResults;
        }

        private List<RoomMappingResult> GetResultMatchingThreshold(List<RoomMappingResult> roomMappingResult, int expectedMatchingScore)
        {
            List<RoomMappingResult> roomMappingResultWithExpectedScore = new List<RoomMappingResult>();
            foreach (var result in roomMappingResult)
            {
                foreach (var score in result.RoomMatchingScore)
                {
                    if (score.MatchingScore >= expectedMatchingScore)
                    {
                        if (!roomMappingResultWithExpectedScore.Any(r => r.RoomId == result.RoomId))
                        {
                            roomMappingResultWithExpectedScore.Add(result);
                        }
                    }
                }
            }

            return roomMappingResultWithExpectedScore.Where(x => x.HighestMatchedScore >= expectedMatchingScore).ToList();
        }

        private ClarifiModel PopulateData(string fileName)
        {
            ClarifiModel model = null;
            using (StreamReader r = new StreamReader(fileName))
            {
                string json = r.ReadToEnd();
                model = JsonConvert.DeserializeObject<ClarifiModel>(json);
                model.RoomsData.ForEach(room => room.UpdateNameIfAccessible());
            }
            return model;
        }




    }

    public class EpsMappedRooms
    {
        public string VersionId { get; set; }
        public string EpsHotelId { get; set; }
        public string EpsRoomId { get; set; }
        public string EpsRoomName { get; set; }
        public string AppliedStrategyName { get; set; }
        public DateTime AddedDate { get; set; }
        public List<HotelBedMappedRoomDetail> MappedRooms { get; set; }
    }

    public class HotelBedMappedRoomDetail
    {
        public string HBHotelId { get; set; }
        public string HBRoomId { get; set; }
        public string HBMatchingString { get; set; }
        public string HBRoomName { get; set; }
        public int MatchScore { get; set; }
        public string EpsMatchingString { get; set; }
        public string MatchingFields { get; set; }
        public string AppliedStrategyName { get; set; }
    }
}
