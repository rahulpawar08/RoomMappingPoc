﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Clarify.FuzzyMatchingTest.Data.Models;
using Newtonsoft.Json;

namespace Clarify.FuzzyMatchingTest
{
    public class FileWriter : IDataWriter
    {
        public FileWriter()
        {
            File.AppendAllText("RoomResultStats.csv", "ClarifiHotelId,EPSRooms,HotelBedsRooms,MatchedRooms,MatchingPercentage,MatchedRoomsDetails");
        }
        public void WriteEPSRoomMatching(string fileName, ClarifiModel epsSupplierData, List<RoomMappingResult> roomMappingResults)
        {
            List<EpsMappedRooms> epsMappedRoomView = new List<EpsMappedRooms>();
            foreach (var epsRoom in epsSupplierData.RoomsData)
            {
                var roomsMappedToEpsRoom = roomMappingResults.FindAll(r => r.MostMatchedRoomId == epsRoom.SupplierRoomId);
                if (roomsMappedToEpsRoom != null)
                {
                    epsMappedRoomView.Add(new EpsMappedRooms()
                    {
                        EpsRoomId = epsRoom.SupplierRoomId,
                        EpsRoomName = epsRoom.Name,
                        MappedRooms = GetMappedRooms(epsRoom, roomsMappedToEpsRoom)
                    });
                }
            }
            if (epsMappedRoomView.Count > 0)
            {
                string json = JsonConvert.SerializeObject(epsMappedRoomView);

                System.IO.File.WriteAllText(Directory.GetCurrentDirectory() + "\\Output\\" + fileName, json);
            }
        }

        public void WriteHotelBedsRoomMatching(string fileName, List<RoomMappingResult> result)
        {
            if (result.Count > 0)
            {
                result.ForEach(r => r.RoomMatchingScore.OrderByDescending(s => s.MatchingScore));
                string json = JsonConvert.SerializeObject(result);

                System.IO.File.WriteAllText(Directory.GetCurrentDirectory() + "\\Output\\" + fileName, json);


            }
        }

        public void WriteRoomMatchingMetaData(List<RoomMappingResult> roomMappingResultWithThreshold, ClarifiModel epsSupplierData, ClarifiModel hotelBedsSupplierData)
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

    }
}