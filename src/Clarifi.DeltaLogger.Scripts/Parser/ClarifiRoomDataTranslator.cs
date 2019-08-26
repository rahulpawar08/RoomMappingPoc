using System;
using System.Collections.Generic;
using System.Text;
using LoggerScripts = Clarifi.RoomMappingLogger.Scripts;
using DataModels = Clarify.FuzzyMatchingTest.Data.Models;

namespace Clarifi.DeltaLogger.Scripts.Parser
{
    public class ClarifiRoomDataTranslator
    {
        #region Private Methods
        private static List<LoggerScripts.RoomsData> ParseRooms(List<DataModels.RoomsData> roomsData)
        {
            var rooms = new List<LoggerScripts.RoomsData>();

            if (roomsData != null && roomsData.Count > 0)
                roomsData.ForEach(x => rooms.Add(ParseRoom(x)));

            return rooms;
        }

        private static LoggerScripts.RoomsData ParseRoom(DataModels.RoomsData roomsData)
        {
            return new LoggerScripts.RoomsData()
            {
                AddedDate = roomsData.AddedDate,
                ModifiedDate = roomsData.ModifiedDate,
                Amenities = GetAmenities(roomsData.Amenities),
                RoomViews = GetRoomViews(roomsData.RoomViews),
                BedDetails = GetBedDetails(roomsData.BedDetails),
                Descriptions = GetDescriptions(roomsData.Descriptions),
                Images = GetImages(roomsData.Images),
                ClarifiRoomId = roomsData.ClarifiRoomId,
                IsDisabled = roomsData.IsDisabled,
                Name = roomsData.Name,
                SmokingIndicator = roomsData.SmokingIndicator,
                SquareFootage = roomsData.SquareFootage,
                SupplierHotelId = roomsData.SupplierId,
                SupplierName = roomsData.SupplierFamily,
                SupplierRoomId = roomsData.SupplierRoomId,
                //CategoryId = activityItinerary.Category?.Id,
                //IsEnableForDisplay
            };
        }

        private static List<LoggerScripts.Amenity> GetAmenities(List<DataModels.Amenity> amenitiesData)
        {
            var amenities = new List<LoggerScripts.Amenity>();

            if (amenitiesData != null)
            {
                foreach (var amenityData in amenitiesData)
                {
                    var amenity = new LoggerScripts.Amenity()
                    {
                        AmenityGroupId = amenityData.AmenityGroupId,
                        Description = (string)amenityData.Description ,
                        ImageName = (string)amenityData.ImageName,
                        Name = amenityData.Name,
                        SupplierImageUrl = (string)amenityData.SupplierImageUrl,
                        Type = amenityData.Type
                    };
                    amenities.Add(amenity);
                }
            }

            return amenities;
        }

        private static List<LoggerScripts.RoomView> GetRoomViews(List<DataModels.RoomView> roomViewsData)
        {
            var roomViews = new List<LoggerScripts.RoomView>();

            if (roomViewsData != null)
            {
                foreach (var roomViewData in roomViewsData)
                {
                    var roomView = new LoggerScripts.RoomView()
                    {
                        Type = roomViewData.Type,
                        Value = roomViewData.Value
                    };
                    roomViews.Add(roomView);
                }
            }

            return roomViews;
        }

        private static List<LoggerScripts.BedDetail> GetBedDetails(List<DataModels.BedDetail> bedDetailsData)
        {
            var bedDetails = new List<LoggerScripts.BedDetail>();

            if (bedDetailsData != null)
            {
                foreach (var bedDetailData in bedDetailsData)
                {
                    var bedDetail = new LoggerScripts.BedDetail()
                    {
                        Count = bedDetailData.Count,
                        Type = bedDetailData.Type,
                        Description = bedDetailData.Desc
                    };
                    bedDetails.Add(bedDetail);
                }
            }

            return bedDetails;
        }

        private static List<LoggerScripts.Description> GetDescriptions(List<DataModels.Description> descriptionsData)
        {
            var descriptions = new List<LoggerScripts.Description>();

            if (descriptionsData != null)
            {
                foreach (var descriptionData in descriptionsData)
                {
                    var description = new LoggerScripts.Description()
                    {
                        Type = descriptionData.Type,
                        Value = descriptionData.Value
                    };
                    descriptions.Add(description);
                }
            }

            return descriptions;
        }

        private static List<LoggerScripts.Image> GetImages(List<DataModels.Image> imagesData)
        {
            var images = new List<LoggerScripts.Image>();

            if (imagesData != null)
            {
                foreach (var imageData in imagesData)
                {
                    var image = new LoggerScripts.Image()
                    {
                        Dimension = imageData.Dimension,
                        Height = imageData.Height,
                        HorizontalResolution = imageData.HorizontalResolution,
                        ImageCaption = imageData.ImageCaption,
                        ImageType = imageData.ImageType,
                        IsEnabled = imageData.IsEnabled,
                        IsSupplierHeroImage = imageData.IsSupplierHeroImage,
                        IsThumbNail = imageData.IsThumbNail,
                        QualityRating = (string)imageData.QualityRating,
                        RoomCode = imageData.RoomCode,
                        SupplierCode = imageData.SupplierCode,
                        SupplierImageUrl = imageData.SupplierImageUrl,
                        UniqueImageFileName = (string)imageData.UniqueImageFileName,
                        VerticalResolution = imageData.VerticalResolution,
                        Width = imageData.Width
                    };

                    images.Add(image);
                }
            }

            return images;
        }

        private static LoggerScripts.ClarifiModel GetClarifiModel(DataModels.ClarifiModel clarifiModelData)
        {
            if (clarifiModelData == null)
                return null;

            return new LoggerScripts.ClarifiModel()
            {
                ClarifiHotelId = clarifiModelData.HotelClarifiId,
                SupplierHotelId = clarifiModelData.SupplierId,
                RoomsData = ParseRooms(clarifiModelData.RoomsData)
            };
        }
        #endregion

        #region Public Methods
        public static LoggerScripts.Hotel GetHotel(DataModels.Hotel hotelData)
        {
            if (hotelData == null)
                return null;

            return new LoggerScripts.Hotel()
            {
                ClarifiHotelId = hotelData.clarifyHotelId,
                SupplierHotelId = hotelData.supplierHotelId,
                rooms = ParseRooms(hotelData.rooms)
            };
        }
        #endregion
    }
}
