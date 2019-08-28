using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clarify.FuzzyMatchingTest.Data.Models
{
    public class Description
    {
        public string Type { get; set; }
        public string Value { get; set; }
    }

    public class Image
    {
        public bool IsThumbNail { get; set; }
        public object UniqueImageFileName { get; set; }
        public string ImageType { get; set; }
        public string ImageCaption { get; set; }
        public string SupplierCode { get; set; }
        public string SupplierImageUrl { get; set; }
        public bool IsEnabled { get; set; }
        public object QualityRating { get; set; }
        public string Dimension { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public double VerticalResolution { get; set; }
        public double HorizontalResolution { get; set; }
        public string RoomCode { get; set; }
        public bool IsSupplierHeroImage { get; set; }
    }

    public class Amenity
    {
        public string Name { get; set; }
        public object SupplierImageUrl { get; set; }
        public object ImageName { get; set; }
        public object Description { get; set; }
        public string Type { get; set; }
        public int AmenityGroupId { get; set; }
    }

    public class BedDetail
    {
        public string Type { get; set; }
        public string Desc { get; set; }
        public int Count { get; set; }
    }
    public class RoomView
    {
        public string Type { get; set; }

        public string Value { get; set; }
    }
    public class RoomsData
    {
        public int ClarifiRoomId { get; set; }
        public string SupplierFamily { get; set; }
        public string SupplierId { get; set; }
        public string SupplierRoomId { get; set; }
        public string Name { get; set; }
        public List<Description> Descriptions { get; set; }
        public List<Image> Images { get; set; }
        public List<Amenity> Amenities { get; set; }
        public List<BedDetail> BedDetails { get; set; }
        public string SquareFootage { get; set; }
        public List<RoomView> RoomViews { get; set; }
        public string SmokingIndicator { get; set; }
        public bool IsDisabled { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public string GetMappingString(string fields)
        {
            StringBuilder strBuilder = new StringBuilder();

            if(fields.Contains("SQF"))
            strBuilder.Append(SquareFootage);

            if (fields.Contains("TY"))
                strBuilder.Append(" " + Name);

            if (fields.Contains("BD"))
            {
                if (BedDetails != null && BedDetails.Count > 0)
                {
                    BedDetails.ForEach(b => strBuilder.Append(" " +b.Desc));
                }
            }

            if (fields.Contains("RV"))
            {
                if (RoomViews != null && RoomViews.Count > 0)
                {
                    RoomViews.ForEach(r => strBuilder.Append(" " + r.Value));
                }
            }

            if (fields.Contains("DESC"))
            {
                if (Descriptions != null && Descriptions.Count > 0)
                {
                    Descriptions.ForEach(d => strBuilder.Append(" " + d.Value));
                }
            }

            return strBuilder.ToString();
        }

        public void UpdateNameIfAccessible()
        {
            if (Name != null)
            {
                if (Amenities != null && Amenities.Count > 0 && !Name.ToLower().Contains("accessible"))
                {
                    if (Amenities.Any(a => a.Name.ToLower().Contains("wheelchair") || a.Name.ToLower().Contains("accessible")))
                    {
                        Name += ", Accessible";
                    }
                }
            }
        }
    }

    public class Hotel
    {
        public string clarifyHotelId { get; set; }
        public string supplierHotelId { get; set; }
        public List<RoomsData> rooms { get; set; }
    }

    public class ClarifiModel
    {
        public string HotelClarifiId { get; set; }
        public string SupplierId { get; set; }
        public List<RoomsData> RoomsData { get; set; }

        public string SupplierFamily { get; set; }
        public string PropertyType { set; get; }
        public string HotelName { get; set; }
        public string AddressLine1 { set; get; }
        public string AddressLine2 { set; get; }
        public string CityCode { set; get; }
        public string CityName { set; get; }
        public string StateCode { set; get; }
        public string StateName { set; get; }
        public string CountryCode { set; get; }
        public string ZipCode { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double RatingValue { get; set; }
        // public Hotel hotel { get; set; }
    }
}
