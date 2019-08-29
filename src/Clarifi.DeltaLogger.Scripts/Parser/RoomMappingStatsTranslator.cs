using System;
using System.Collections.Generic;
using System.Text;
using LoggerScripts = Clarifi.RoomMappingLogger.Scripts;
using DataModels = Clarify.FuzzyMatchingTest;

namespace Clarifi.DeltaLogger.Scripts.Parser
{
    public class RoomMappingStatsTranslator
    {
        #region Public Methods
        public static RoomMappingSummary GetRoomMappingSummary(DataModels.RoomMappingSummary roomMappingSummary)
        {
            return new RoomMappingSummary()
            {
                HotelsWithoutRooms = roomMappingSummary.HotelsWithoutRooms,
                HotelsWithRooms = roomMappingSummary.HotelsWithRooms,
                OverallMappingPercentage = roomMappingSummary.OverallMappingPercentage,
                TotalHotels = roomMappingSummary.TotalHotels,
                TotalMappedRooms = roomMappingSummary.TotalMappedRooms,
                TotalRooms = roomMappingSummary.TotalRooms,
                VersionId = roomMappingSummary.VersionId
            };
        }

        public static List<HotelLevelStats> GetHotelLevelStats(List<DataModels.HotelLevelStats> hotelLevelStatsData)
        {
            var hotelLevelStats = new List<HotelLevelStats>();

            if (hotelLevelStatsData != null && hotelLevelStatsData.Count > 0)
                hotelLevelStatsData.ForEach(x => hotelLevelStats.Add(GetHotelLevelStats(x)));

            return hotelLevelStats;
        }

        public static List<RoomLevelStats> GetRoomLevelStats(List<DataModels.RoomLevelStats> roomLevelStatsData)
        {
            var roomLevelStats = new List<RoomLevelStats>();

            if (roomLevelStatsData != null && roomLevelStatsData.Count > 0)
                roomLevelStatsData.ForEach(x => roomLevelStats.Add(GetRoomLevelStats(x)));

            return roomLevelStats;
        }
        #endregion

        #region Priavte Methods

        private static HotelLevelStats GetHotelLevelStats(DataModels.HotelLevelStats hotelLevelStatsData)
        {
            return new HotelLevelStats()
            {
                ClarifiHotelId = hotelLevelStatsData.ClarifiHotelId,
                EPSMappedRoomsCount = hotelLevelStatsData.EPSMappedRoomsCount,
                EPSRoomsCount = hotelLevelStatsData.EPSRoomsCount,
                MappingPercentage = hotelLevelStatsData.MappingPercentage,
                VersionId = hotelLevelStatsData.VersionId
            };
        }

        private static RoomLevelStats GetRoomLevelStats(DataModels.RoomLevelStats roomLevelStatsData)
        {
            return new RoomLevelStats()
            {
                ClarifiHotelId = roomLevelStatsData.ClarifiHotelId,
                EpsRoomId = roomLevelStatsData.EpsRoomId,
                EpsRoomName = roomLevelStatsData.EpsRoomName,
                HBRoomsMappedCount = roomLevelStatsData.HBRoomsMappedCount,
                VersionId = roomLevelStatsData.VersionId
            };
        }
        #endregion
    }
}
