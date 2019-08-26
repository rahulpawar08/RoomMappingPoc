using Clarifi.RoomMappingLogger;
using Clarifi.RoomMappingLogger.Scripts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clarifi.DeltaLogger.Scripts
{
    public class Setup
    {
        public void Run()
        {
            // Add script registrations here.
            //ScriptRegistry
            //    .Instance
            //    .Register<SampleScript>(name: "sample")
            //    .Register<HotelSearchScript>(name: "hotelsearch")
            //    .Register<CarSearchScript>(name: "carsearch")
            //    .Register<RoomSearchScript>(name: "roomsearch")
            //    .Register<KenobiHotelSearchScript>(name: "kenobiHotelSearchDelta")
            //    .Register<KenobiRoomSearchScript>(name: "kenobiHotelRoomSearchDelta")
            //    .Register<ActivitySearchScript>(name: "activitySearch")
            //    .Register<ActivityDetailsScript>(name: "activityDetails")
            //    .Register<ActivityOptionsScript>(name: "activityOptions");

            // Add type registrations here.
            KnownTypes
                .Instance
                .Register<Description>()
                .Register<Image>()
                .Register<Amenity>()
                .Register<BedDetail>()
                .Register<RoomView>()
                .Register<RoomsData>()
                .Register<Hotel>()
                .Register<ClarifiModel>()
                .Register<EpsMappedRooms>()
                .Register<HotelBedsMappedRoomDetail>();
        }
    }
}
