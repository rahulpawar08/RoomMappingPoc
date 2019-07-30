using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BoomTown.FuzzySharp;
using Clarify.FuzzyMatchingTest.Data.Models;
using Newtonsoft.Json;

namespace Clarify.FuzzyMatchingTest
{
    public class RoomMappingCore
    {
        public RoomMappingCore()
        {
            RoomMatchingAlgo = new FuzzyRoomMatchingAlgo();
            DataWriter = new FileWriter();

        }
        public List<InputFile> InputFiles { get; set; }
        public ClarifiModel EpsSupplierData { get; set; }

        public ClarifiModel HotelBedSupplierData { get; set; }

        public IRoomMatchingAlgo RoomMatchingAlgo { get; set; }

        public IDataWriter DataWriter { get; set; }

        public void Initialize()
        {
            InputFiles = new List<InputFile>();
            PopulateInputData();
        }

        public List<RoomMappingResult> ExecuteHotelBedEanRoomMapping(List<string> matchingFields, int threshold)
        {
            List<RoomMappingResult> roomMappingResults = null;
            
            foreach (var inputFile in InputFiles)
            {
                roomMappingResults = new List<RoomMappingResult>();
                EpsSupplierData = PopulateData(inputFile.EpsDataFileName);
                HotelBedSupplierData = PopulateData(inputFile.HbDataFileName);

                foreach (var hotelBedRoom in HotelBedSupplierData.RoomsData)
                {
                    RoomMappingResult roomMappingResult = new RoomMappingResult("HotelBeds", HotelBedSupplierData.HotelClarifiId, HotelBedSupplierData.SupplierId, hotelBedRoom.SupplierRoomId);
                    
                    foreach (var targetRoom in EpsSupplierData.RoomsData)
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
                    roomMappingResults.Add(roomMappingResult);
                }
                var roomMappingResultWithThreshold = GetResultMatchingThreshold(roomMappingResults, threshold);

                DataWriter.WriteHotelBedsRoomMatching($"{inputFile.ClarifiHotelId}_{threshold}_{DateTime.Now.ToString("yyyyMMddTHHmmss")}.json", roomMappingResultWithThreshold);
                DataWriter.WriteRoomMatchingMetaData(roomMappingResultWithThreshold, EpsSupplierData, HotelBedSupplierData);
                DataWriter.WriteEPSRoomMatching($"{inputFile.ClarifiHotelId}_'EPSMappedView'_{DateTime.Now.ToString("yyyyMMddTHHmmss")}.json", EpsSupplierData, roomMappingResults);
               
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

        private void PopulateInputData()
        {
            string[] filePaths = Directory.GetFiles(Directory.GetCurrentDirectory() + "\\Input\\");
            foreach (var epsFileName in filePaths.Where(n => n.Contains("EPS")))
            {
                string[] words = epsFileName.Split('_');

                if (words != null && words.Length > 0)
                {
                    string hotelBedFileName = filePaths.FirstOrDefault(n => n.Contains(words[0]) && n.Contains("HB"));
                    InputFiles.Add(new InputFile(Path.GetFileName(words[0]), epsFileName, hotelBedFileName));
                }
            }
        }

        
    }

    public class EpsMappedRooms
    {
        public string EpsRoomId { get; set; }
        public string EpsRoomName { get; set; }


        public List<HotelBedMappedRoomDetail> MappedRooms { get; set; }
    }

    public class HotelBedMappedRoomDetail
    {
        public string HBMatchingString { get; set; }
        public string HBRoomName { get; set; }
        public int MatchScore { get; set; }
        public string EpsMatchingString { get; set; }
        public string MatchingFields { get; set; }
    }
}
