using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Clarify.FuzzyMatchingTest.Data.Models;

namespace Clarify.FuzzyMatchingTest.Strategy
{
    //This Strategy is meant to check only the data which is present in hotelbeds and try to find a possible match with EPS
    public class HotelBedsDataAvailabilityStrategy : BaseRoomMappingStrategy
    {
        public HotelBedsKeywordExtractor HotelBedsKeywordExtractor { get; set; }
        public EPSRoomTypeExtractor EpsRoomTypeExtractor { get; set; }


        public HotelBedsDataAvailabilityStrategy(IMatchingAlgorithm matchingAlgorithm, string versionId) : base(matchingAlgorithm, "DynamicFieldSelection", versionId)
        {
            HotelBedsKeywordExtractor = new HotelBedsKeywordExtractor();
            EpsRoomTypeExtractor = new EPSRoomTypeExtractor();
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

                    var hotelBedsKeywordMapping = HotelBedsKeywordExtractor.GetKeywordMapping(hotelBedRoom.SupplierRoomId);
                    string hotelBedsMatchingString = string.Empty;
                    if (hotelBedsKeywordMapping.Count > 0)
                    {
                        //Create a matching string considering only the room type meanings
                        hotelBedsMatchingString = hotelBedsKeywordMapping.Values.Aggregate((x, y) => x + " " + y);
                    }

                    foreach (var targetRoom in epsSupplierData.RoomsData)
                    {
                        //Create the eps matching string dynamically, considering the information available from hotelbeds.
                        // this is done to ensure that we get relevant string for matching, the score will be higher.
                        var epsMatchingString = EpsRoomTypeExtractor.GetFields(targetRoom, hotelBedsKeywordMapping.Keys.ToList());


                        int score = (!string.IsNullOrEmpty(hotelBedsMatchingString) && !string.IsNullOrEmpty(epsMatchingString)) ?
                                          RoomMatchingAlgo.GetMatchingScore(hotelBedsMatchingString, epsMatchingString) : 0;


                        roomMappingResult.RoomMatchingScore.Add(new MatchResult()
                        {
                            MatchingMethod = "TokenSetRatio",
                            EPSRoomId = targetRoom.SupplierRoomId,
                            MatchingScore = score,
                            EPSMatchingString = epsMatchingString,
                            HotelBedMatchingString = hotelBedsMatchingString,
                            MatchingField = "roomtype"
                        });
                        roomMappingResult.RoomMatchingScore.OrderByDescending(s => s.MatchingScore);
                        roomMappingResult.AppliedStrategyName = StrategyName;
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
