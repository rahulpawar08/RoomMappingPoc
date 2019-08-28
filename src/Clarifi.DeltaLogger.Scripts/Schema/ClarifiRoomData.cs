using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Clarifi.RoomMappingLogger.Internal;

namespace Clarifi.RoomMappingLogger.Scripts
{
    [TypeInfo("descriptions")]
    public class Description : LogEntryBase
    {
        [SimpleField("type")]
        public string Type { get; set; }

        [SimpleField("value")]
        public string Value { get; set; }
    }

    [TypeInfo("images")]
    public class Image : LogEntryBase
    {
        [SimpleField("is_thumbnail")]
        public bool IsThumbNail { get; set; }

        [SimpleField("unique_image_file_name")]
        public string UniqueImageFileName { get; set; }

        [SimpleField("image_type")]
        public string ImageType { get; set; }

        [SimpleField("image_caption")]
        public string ImageCaption { get; set; }

        [SimpleField("supplier_code")]
        public string SupplierCode { get; set; }

        [SimpleField("supplier_image_url")]
        public string SupplierImageUrl { get; set; }

        [SimpleField("is_enabled")]
        public bool IsEnabled { get; set; }

        [SimpleField("quality_rating")]
        public string QualityRating { get; set; }

        [SimpleField("dimension")]
        public string Dimension { get; set; }

        [SimpleField("height")]
        public int Height { get; set; }

        [SimpleField("width")]
        public int Width { get; set; }

        [SimpleField("vertical_resolution")]
        public double VerticalResolution { get; set; }

        [SimpleField("horizontal_resolution")]
        public double HorizontalResolution { get; set; }

        [SimpleField("room_code")]
        public string RoomCode { get; set; }

        [SimpleField("is_supplier_hero_image")]
        public bool IsSupplierHeroImage { get; set; }
    }

    [TypeInfo("amenities")]
    public class Amenity : LogEntryBase
    {
        [SimpleField("name")]
        public string Name { get; set; }

        [SimpleField("supplier_image_url")]
        public string SupplierImageUrl { get; set; }

        [SimpleField("image_name")]
        public string ImageName { get; set; }

        [SimpleField("description")]
        public string Description { get; set; }

        [SimpleField("type")]
        public string Type { get; set; }

        [SimpleField("amenity_groupid")]
        public int AmenityGroupId { get; set; }
    }

    [TypeInfo("bed_details")]
    public class BedDetail : LogEntryBase
    {
        [SimpleField("type")]
        public string Type { get; set; }

        [SimpleField("description")]
        public string Description { get; set; }

        [SimpleField("count")]
        public int Count { get; set; }
    }

    [TypeInfo("room_views")]
    public class RoomView : LogEntryBase
    {
        [SimpleField("type")]
        public string Type { get; set; }

        [SimpleField("value")]
        public string Value { get; set; }
    }

    [TypeInfo("rooms_data")]
    public class RoomsData : LogEntryBase
    {
        [SimpleField("clarifi_roomid")]
        public int ClarifiRoomId { get; set; }

        [SimpleField("supplier_name")]
        public string SupplierName { get; set; }

        [SimpleField("supplier_hotelid")]
        public string SupplierHotelId { get; set; }

        [SimpleField("supplier_roomid")]
        public string SupplierRoomId { get; set; }

        [SimpleField("name")]
        public string Name { get; set; }

        [NestedField()]
        public List<Description> Descriptions { get; set; }

        [NestedField()]
        public List<Image> Images { get; set; }

        [NestedField()]
        public List<Amenity> Amenities { get; set; }

        [NestedField()]
        public List<BedDetail> BedDetails { get; set; }

        [SimpleField("square_footage")]
        public string SquareFootage { get; set; }

        [NestedField()]
        public List<RoomView> RoomViews { get; set; }

        [SimpleField("smoking_indicator")]
        public string SmokingIndicator { get; set; }

        [SimpleField("is_disabled")]
        public bool IsDisabled { get; set; }

        [SimpleField("added_date")]
        public DateTime AddedDate { get; set; }

        [SimpleField("modified_date")]
        public DateTime ModifiedDate { get; set; }

        [SimpleField("is_enable_for_display")]
        public bool IsEnableForDisplay { get; set; }
    }

    [TypeInfo("hotels")]
    public class Hotel : LogEntryBase
    {
        [SimpleField("clarifi_hotelid")]
        public string ClarifiHotelId { get; set; }

        [SimpleField("supplier_hotelid")]
        public string SupplierHotelId { get; set; }

        [NestedField()]
        public List<RoomsData> rooms { get; set; }
    }

    [TypeInfo("clarifi_model")]
    public class ClarifiModel : LogEntryBase
    {
        [SimpleField("clarifi_hotelid")]
        public string ClarifiHotelId { get; set; }

        [SimpleField("supplier_hotelid")]
        public string SupplierHotelId { get; set; }

        [NestedField()]
        public List<RoomsData> RoomsData { get; set; }

        [SimpleField("supplier_name")]
        public string SupplierFamily { get; set; }

        [SimpleField("property_type")]
        public string PropertyType { set; get; }

        [SimpleField("hotel_name")]
        public string HotelName { get; set; }

        [SimpleField("address_line1")]
        public string AddressLine1 { set; get; }

        [SimpleField("address_line2")]
        public string AddressLine2 { set; get; }

        [SimpleField("city_code")]
        public string CityCode { set; get; }

        [SimpleField("city_name")]
        public string CityName { set; get; }

        [SimpleField("state_code")]
        public string StateCode { set; get; }

        [SimpleField("state_name")]
        public string StateName { set; get; }

        [SimpleField("country_code")]
        public string CountryCode { set; get; }

        [SimpleField("zip_code")]
        public string ZipCode { get; set; }

        [SimpleField("latitude")]
        public double Latitude { get; set; }

        [SimpleField("longitude")]
        public double Longitude { get; set; }

        [SimpleField("star_rating")]
        public double StarRating { get; set; }
    }
}
