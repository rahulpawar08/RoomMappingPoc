using Clarify.FuzzyMatchingTest;
using Clarify.FuzzyMatchingTest.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApp.FuzzyAlgo
{
    class Program
    {
       
        static void Main(string[] args)
        {
            int expectedMatchingScore = 70;
            //Available Fields => SQF: SquareFoot, NM: Name, BD: Bed Details, RV: Room View, DESC: Room Description
            List<string> matchingFields = new List<string>() { "SQF_NM_BD_RV", "SQF_NM_BD_RV_DESC" }; 

            RoomMappingCore roomMappingCore = new RoomMappingCore();
            roomMappingCore.Initialize();

            Console.WriteLine($"Starting with Room Mapping with fields - {GetCommaSeperatedFields(matchingFields)}.");

            List<RoomMappingResult> roomMappingResult = roomMappingCore.ExecuteHotelBedEanRoomMapping(matchingFields, expectedMatchingScore); 
            
            Console.WriteLine($"The result of algorithm is stored in the output folder.");

            #region RemoveMe
            //foreach (var result in roomMappingResult)
            //{
            //    foreach(var score in result.RoomMatchingScore)
            //    {
            //        if(score.MatchingScore> expectedMatchingScore)
            //        {
            //            if (!roomMappingResultWithExpectedScore.Any(r => r.RoomId == result.RoomId))
            //            {
            //                roomMappingResultWithExpectedScore.Add(result);

            //                Console.WriteLine($" HB RoomId: {result.RoomId}, EPS RoomId: {score.EPSRoomId}, Score:{score.MatchingScore}, " +
            //                             $" HB Key: {score.HotelBedMatchingString}, EPS Key: {score.EPSMatchingString}");
            //            }
            //        }
            //    }
            //}
            //roomMappingCore.SaveMappingResult($"{matchingFields}_{DateTime.Now.ToString("yyyyMMddTHHmmss")}.json", roomMappingResult);
            //roomMappingCore.SaveMappingResult($"{matchingFields}_ExpectedMatchingScore_{expectedMatchingScore}_{DateTime.Now.ToString("yyyyMMddTHHmmss")}.json", roomMappingResultWithExpectedScore);
            #endregion

            Console.WriteLine("Room Mapping Complete.");
            Console.ReadLine();
        }

        private static object GetCommaSeperatedFields(List<string> matchingFields)
        {
            if (matchingFields == null)
                return string.Empty;

            StringBuilder commaSeperatedFields = new StringBuilder();
            matchingFields.ForEach(f => commaSeperatedFields.Append(f + ", "));
            return commaSeperatedFields.ToString().Trim(',');
        }
    }
}
