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
        public List<InputFile> InputFiles { get; set; }
        public ClarifiModel EpsSupplierData { get; set; }

        public ClarifiModel HotelBedSupplierData { get; set; }

        public void Initialize()
        {
            InputFiles = new List<InputFile>();
            PopulateInputData();
        }

        public List<RoomMappingResult> ExecuteHotelBedEanRoomMapping(List<string> matchingFields, int threshold)
        {
            List<RoomMappingResult> roomMappingResults = new List<RoomMappingResult>();

            foreach (var inputFile in InputFiles)
            {
                EpsSupplierData = PopulateData(inputFile.EpsDataFileName);
                HotelBedSupplierData = PopulateData(inputFile.HbDataFileName);

                foreach (var hotelBedRoom in HotelBedSupplierData.RoomsData)
                {
                    RoomMappingResult roomMappingResult = new RoomMappingResult("HotelBeds", HotelBedSupplierData.HotelClarifiId, HotelBedSupplierData.SupplierId, hotelBedRoom.SupplierRoomId);

                    //var associatedHotel = EpsSupplier.Find(h => h.clarifyHotelId == hotel.clarifyHotelId);
                    //if (associatedHotel != null)
                    //{
                    foreach (var targetRoom in EpsSupplierData.RoomsData)
                    {
                        foreach (var matchingField in matchingFields)
                        {
                            string hotelBedMappingString = hotelBedRoom.GetMappingString(matchingField);
                            string epsMappingString = targetRoom.GetMappingString(matchingField);

                            int score = (!string.IsNullOrEmpty(hotelBedMappingString) && !string.IsNullOrEmpty(epsMappingString)) ? 
                                            Fuzzy.TokenSetRatio(hotelBedMappingString, epsMappingString) : 0;


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
                    //}

                    roomMappingResult.SetMatchedRoom();
                    roomMappingResults.Add(roomMappingResult);
                }

                SaveMappingResult($"{inputFile.ClarifiHotelId}_{threshold}_{DateTime.Now.ToString("yyyyMMddTHHmmss")}.json",
                    GetResultMatchingThreshold(roomMappingResults, threshold));
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

            return roomMappingResultWithExpectedScore;
        }

        private ClarifiModel PopulateData(string fileName)
        {
            ClarifiModel model = null;
            using (StreamReader r = new StreamReader(fileName))
            {
                string json = r.ReadToEnd();
                model = JsonConvert.DeserializeObject<ClarifiModel>(json);
            }
            return model;
        }

        private void PopulateInputData()
        {
            string[] filePaths = Directory.GetFiles(Directory.GetCurrentDirectory()+"\\Input\\");
            foreach(var epsFileName in filePaths.Where(n=>n.Contains("EPS")))
            {
                string[] words = epsFileName.Split('_');

                if (words!=null && words.Length > 0)
                {
                    string hotelBedFileName = filePaths.FirstOrDefault(n => n.Contains(words[0]) && n.Contains("HB"));
                    InputFiles.Add(new InputFile(Path.GetFileName(words[0]), epsFileName, hotelBedFileName));
                }
            }
        }

        public void SaveMappingResult(string fileName, List<RoomMappingResult> result)
        {
            result.ForEach(r => r.RoomMatchingScore.OrderByDescending(s=>s.MatchingScore));
            string json = JsonConvert.SerializeObject(result);

            System.IO.File.WriteAllText(Directory.GetCurrentDirectory() + "\\Output\\" + fileName, json);
        }
    }
}
