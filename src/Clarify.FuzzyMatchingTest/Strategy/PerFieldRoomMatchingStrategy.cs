using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Clarify.FuzzyMatchingTest.Data.Models;

namespace Clarify.FuzzyMatchingTest
{
    public class PerFieldRoomMatchingStrategy : BaseRoomMappingStrategy
    {
        public HotelBedsKeywordExtractor HotelBedsKeywordExtractor { get; set; }

        public EPSRoomTypeExtractor EpsRoomTypeExtractor { get; set; }

        public PerFieldRoomMatchingStrategy(IMatchingAlgorithm matchingAlgorithm, string versionId) : base(matchingAlgorithm, "PerField Room Matching", versionId, "FuzzyAlgorithm")
        {
            HotelBedsKeywordExtractor = new HotelBedsKeywordExtractor();
            EpsRoomTypeExtractor = new EPSRoomTypeExtractor();
        }

        public override List<RoomMappingResult> ExecuteHotelBedEanRoomMapping(List<string> matchingFields)
        {
            return ExecuteRoomMappingByRoomType();
            //var filteredRoomTypeMappingResults = FilterResults(roomTypeMappingResults);
            //List<RoomMappingResult> roomBeddingMappingResults = ExecuteRoomMappingByBedding(filteredRoomTypeMappingResults);

        }

        private List<RoomMappingResult> FilterResults(List<RoomMappingResult> roomMappingResult)
        {
            List<RoomMappingResult> roomMappingResultWithExpectedScore = new List<RoomMappingResult>();
            foreach (var result in roomMappingResult)
            {
                foreach (var score in result.RoomMatchingScore)
                {
                    if (score.MatchingScore >= 95)
                    {
                        if (!roomMappingResultWithExpectedScore.Any(r => r.RoomId == result.RoomId))
                        {
                            roomMappingResultWithExpectedScore.Add(result);
                        }
                    }
                }
            }

            return roomMappingResultWithExpectedScore;

        }

        //private List<RoomMappingResult> ExecuteRoomMappingByBedding(List<RoomMappingResult> roomTypeMappingResults)
        //{

        //}

        private List<RoomMappingResult> ExecuteRoomMappingByRoomType()
        {
            List<RoomMappingResult> roomMappingResults = new List<RoomMappingResult>();
            foreach (var hotelBedSupplierdata in HotelBedSupplierData)
            {
                var epsSupplierData = EpsSupplierData.FirstOrDefault(x => x.HotelClarifiId == hotelBedSupplierdata.HotelClarifiId);
                foreach (var hotelBedRoom in hotelBedSupplierdata.RoomsData)
                {

                    RoomMappingResult roomMappingResult = new RoomMappingResult("HotelBeds", hotelBedSupplierdata.HotelClarifiId, hotelBedSupplierdata.SupplierId, hotelBedRoom.SupplierRoomId, hotelBedRoom.Name);
                    var hotelBedRoomType = HotelBedsKeywordExtractor.GetExtractedString(hotelBedRoom.SupplierRoomId, "roomtype");
                    foreach (var targetRoom in epsSupplierData.RoomsData)
                    {
                        var epsRoomType = EpsRoomTypeExtractor.GetRoomType(targetRoom.SupplierRoomId);

                        int score = (!string.IsNullOrEmpty(hotelBedRoomType) && !string.IsNullOrEmpty(epsRoomType)) ?
                                          RoomMatchingAlgo.GetMatchingScore(hotelBedRoomType, epsRoomType) : 100;


                        roomMappingResult.RoomMatchingScore.Add(new MatchResult()
                        {
                            MatchingMethod = "TokenSetRatio",
                            EPSRoomId = targetRoom.SupplierRoomId,
                            MatchingScore = score,
                            EPSMatchingString = " $ " + epsRoomType + " $ ",
                            HotelBedMatchingString = " $ " + hotelBedRoomType + " $ ",
                            MatchingField = "roomtype"
                        });
                        roomMappingResult.RoomMatchingScore.OrderByDescending(s => s.MatchingScore);
                        roomMappingResult.AppliedStrategyName = StrategyName;
                        roomMappingResult.MatchingAlgorithm = MatchingAlgo;
                        roomMappingResult.VersionId = VersionId;
                    }
                    roomMappingResult.SetMatchedRoom();

                    roomMappingResults.Add(roomMappingResult);
                }
            }

            return roomMappingResults;
        }
    }
}

