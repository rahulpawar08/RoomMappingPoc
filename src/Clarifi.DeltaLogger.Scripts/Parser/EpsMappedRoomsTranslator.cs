﻿using System;
using System.Collections.Generic;
using System.Text;
using LoggerScripts = Clarifi.RoomMappingLogger.Scripts;
using DataModels = Clarify.FuzzyMatchingTest;

namespace Clarifi.DeltaLogger.Scripts.Parser
{
    public class EpsMappedRoomsTranslator
    {
        #region Private Methods
        private static List<LoggerScripts.HotelBedsMappedRoomDetail> GetHotelBedsMappedRoomDetails(List<DataModels.HotelBedMappedRoomDetail> hotelBedMappedRoomDetailsData)
        {
            var hotelBedsMappedRoomDetails = new List<LoggerScripts.HotelBedsMappedRoomDetail>();

            if (hotelBedMappedRoomDetailsData != null && hotelBedMappedRoomDetailsData.Count > 0)
                hotelBedMappedRoomDetailsData.ForEach(x => hotelBedsMappedRoomDetails.Add(GetHotelBedsMappedRoomDetail(x)));

            return hotelBedsMappedRoomDetails;
        }

        private static LoggerScripts.HotelBedsMappedRoomDetail GetHotelBedsMappedRoomDetail(DataModels.HotelBedMappedRoomDetail hotelBedsMappedRoomDetail)
        {
            return new LoggerScripts.HotelBedsMappedRoomDetail()
            {
                HBHotelId = hotelBedsMappedRoomDetail.HBHotelId,
                HBRoomId = hotelBedsMappedRoomDetail.HBRoomId,
                EpsMatchingString = hotelBedsMappedRoomDetail.EpsMatchingString,
                HBMatchingString = hotelBedsMappedRoomDetail.HBMatchingString,
                HBRoomName = hotelBedsMappedRoomDetail.HBRoomName,
                MatchingFields = hotelBedsMappedRoomDetail.MatchingFields,
                MatchScore = hotelBedsMappedRoomDetail.MatchScore,
                AppliedStrategyName = hotelBedsMappedRoomDetail.AppliedStrategyName,
                MatchingAlgorithm = hotelBedsMappedRoomDetail.MatchingAlgorithm,
                HBRoomsCount = hotelBedsMappedRoomDetail.HBRoomsCount
            };
        }
        #endregion

        #region Public Methods
        public static LoggerScripts.EpsMappedRooms GetEpsMappedRoom(DataModels.EpsMappedRooms epsMappedRoomsData)
        {
            if (epsMappedRoomsData == null)
                return null;

            return new LoggerScripts.EpsMappedRooms()
            {
                VersionId = epsMappedRoomsData.VersionId,
                ClarifiHotelId = epsMappedRoomsData.ClarifiHotelId,
                EpsHotelId = epsMappedRoomsData.EpsHotelId,
                EpsRoomId = epsMappedRoomsData.EpsRoomId,
                EpsRoomName = epsMappedRoomsData.EpsRoomName,
                EpsHotelName = epsMappedRoomsData.EpsHotelName,
                AppliedStrategyName = epsMappedRoomsData.AppliedStrategyName,
                MatchingAlgorithm = epsMappedRoomsData.MatchingAlgorithm,
                MatchingStatus = epsMappedRoomsData.MatchingStatus.ToString(),
                AddedDate = epsMappedRoomsData.AddedDate,
                MappedRooms = GetHotelBedsMappedRoomDetails(epsMappedRoomsData.MappedRooms),
                HBRoomsCount = epsMappedRoomsData.HBRoomsCount
            };
        }
        #endregion
    }
}
