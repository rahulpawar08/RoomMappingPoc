using System;
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
                EpsMatchingString = hotelBedsMappedRoomDetail.EpsMatchingString,
                HBMatchingString = hotelBedsMappedRoomDetail.HBMatchingString,
                HBRoomName = hotelBedsMappedRoomDetail.HBRoomName,
                MatchingFields = hotelBedsMappedRoomDetail.MatchingFields,
                MatchScore = hotelBedsMappedRoomDetail.MatchScore,
                AppliedStrategyName = hotelBedsMappedRoomDetail.AppliedStrategyName
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
                EpsHotelId = epsMappedRoomsData.EpsHotelId,
                EpsRoomId = epsMappedRoomsData.EpsRoomId,
                EpsRoomName = epsMappedRoomsData.EpsRoomName,
                MappedRooms = GetHotelBedsMappedRoomDetails(epsMappedRoomsData.MappedRooms)
            };
        }
        #endregion
    }
}
