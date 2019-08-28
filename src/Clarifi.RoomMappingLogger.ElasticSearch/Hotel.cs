using Nest;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clarifi.RoomMappingLogger.ElasticSearch
{
    [Serializable]
    [ElasticType(IdProperty = "MongoId")]
    public class Hotel
    {
        private string _mongoId;

        public string MongoId
        {
            get; set;
        }
        public string ClarifiId { get; set; }
        public string SupplierId { get; set; }
        public string SupplierFamily { get; set; }
        public PrimaryInfo PrimaryContent { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsVerified { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public Summary Summary { get; set; }
    }

    public class PrimaryInfo
    {
        public HotelName Name { get; set; }
        public Currency CurrencyInfo { get; set; }
        public Address Address { get; set; }
        public Contact Contact { get; set; }
        public Chain Chain { get; set; }

        public GeoCode GeoCode { get; set; }

        public Rating Rating { get; set; }
        public PropertyType PropertyType { get; set; }

        public GeoPoint GeoPoint
        {
            get
            {
                return GeoCode == null
                    ? null
                    : new GeoPoint
                    {
                        Lat = GeoCode.Latitude,
                        Lon = GeoCode.Longitude,

                    };
            }
            set
            {

            }
        }
    }

    public class PropertyType
    {
        public string Type { set; get; }
    }

    public class HotelName
    {
        public string Name { get; set; }
    }

    public class Currency
    {
        public string Code { get; set; }
    }

    public class Address
    {
        public string AddressLine1 { set; get; }
        public string AddressLine2 { set; get; }
        public string CityCode { set; get; }
        public string CityName { set; get; }
        public string StateCode { set; get; }
        public string StateName { set; get; }
        public string CountryCode { set; get; }
        public string ZipCode { get; set; }
    }

    public class Contact
    {
        public string PhoneNumber { get; set; }
        public string FaxNumber { get; set; }
        public string WebsiteUrl { get; set; }
    }

    public class Chain
    {
        public string ChainCode { get; set; }
        public string ChainName { get; set; }
    }

    public class GeoCode
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        [ElasticProperty(Index = FieldIndexOption.NotAnalyzed, Store = true)]
        public string GeoCodeSource { get; set; }
    }

    public class Rating
    {
        public string RatingType { get; set; }
        public double RatingValue { get; set; }
    }

    public class Summary
    {
        public int ImagesCount { get; set; }
    }

    public class GeoPoint
    {
        public double Lat { get; set; }
        public double Lon { get; set; }
    }
}
