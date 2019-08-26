using Clarifi.DeltaLogger.Scripts;
using Clarifi.RoomMappingLogger;
using Clarifi.RoomMappingLogger.MySql;
using Clarify.FuzzyMatchingTest;
using Clarify.FuzzyMatchingTest.Strategy;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.FuzzyAlgo
{
    class Program
    {
        static void Main(string[] args)
        {
            new Setup().Run();

            //Comment this line if MySql schema is already created
            Task.Run(() => KnownTypes.ProvisionAsync("all", new LogDb())).Wait();

            var logger = new Logger(new LogDb(Settings.GetConnectionString()));
            IDataWriter dataWriter = new MySqlWriter(logger);

            int expectedMatchingScore = 80;
            //Available Fields => SQF: SquareFoot, TY: Type, BD: Bed Details, RV: Room View, DESC: Room Description
            List<string> matchingFields = new List<string>() { "SQF_TY_BD_RV" };
            var roomMappingviewExtractor = new RoomMappingViewExtractor();

            BaseRoomMappingStrategy roomMappingStrategy = new HotelBedsDataAvailabilityStrategy(new FuzzyStringMatchingAlgo());
            roomMappingStrategy.Initialize();

            Console.WriteLine($"Starting with Room Mapping with fields - {GetCommaSeperatedFields(matchingFields)}.");
            var roomMappingResult = roomMappingStrategy.ExecuteHotelBedEanRoomMapping(matchingFields);

            var hotelBedsMappedView = roomMappingviewExtractor.GetRoomMappingWithTresholdPerHotel(roomMappingResult, expectedMatchingScore);

            var epsMappedView = roomMappingviewExtractor.GetEpsMappedRooms(roomMappingStrategy.EpsSupplierData, hotelBedsMappedView);

            //This is HotelBeds mapped view
            //foreach (var kvPair in hotelBedsMappedView)
            //{
            //    dataWriter.WriteHotelBedsRoomMatching($"{kvPair.Key}_{expectedMatchingScore}_{DateTime.Now.ToString("yyyyMMddTHHmmss")}.json", kvPair.Value);
            //}

            foreach (var epsMappingKvPair in epsMappedView)
            {
                dataWriter.WriteEPSRoomMatching($"{epsMappingKvPair.Key}_'EPSMappedView'_{DateTime.Now.ToString("yyyyMMddTHHmmss")}.json", epsMappingKvPair.Value);
            }

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
