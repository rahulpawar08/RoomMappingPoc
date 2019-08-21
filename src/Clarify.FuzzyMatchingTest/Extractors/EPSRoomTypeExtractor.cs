using Clarify.FuzzyMatchingTest.Data.Models;
using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Clarify.FuzzyMatchingTest
{
    public class EPSRoomTypeExtractor
    {
        private List<EpsRoomTypeMapping> _mapping = new List<EpsRoomTypeMapping>();

        public EPSRoomTypeExtractor()
        {
            InitializeMapping();
        }

        public string GetRoomType(string supplierRoomId)
        {
            return _mapping.FirstOrDefault(x => x.SupplierRoomId == supplierRoomId)?.CleanRoomName;
            
        }

        private void InitializeMapping()
        {
            _mapping = JsonConvert.DeserializeObject<List<EpsRoomTypeMapping>>(File.ReadAllText("please fix me"));
        }

        internal string GetFields(RoomsData targetRoom, List<string> keys)
        {
            string result = string.Empty;

            foreach(var key in keys)
            {
                switch (key)
                {
                    case "roomtype":
                        result += GetRoomType(targetRoom.SupplierRoomId) + " ";
                        break;
                    case "bedding":
                        result += GetBedding(targetRoom) + " ";
                        break;
                    case "amenity":
                        break; ///TBD
                    case "bathrooms":
                        result += GetBathroomInfo(targetRoom) + " ";
                        break;
                    case "views":
                        result += GetViewsInfo(targetRoom) + " ";
                        break;
                    default:
                        break;
                }

            }

            return result;
        }

        private string GetViewsInfo(RoomsData targetRoom)
        {
            if (targetRoom.RoomViews != null)
            {
                if (targetRoom.RoomViews.Count > 0 && !targetRoom.RoomViews.Any(x => x.Value == null))
                {
                    var result = targetRoom.RoomViews.Select(x => x.Value)   //select description
                                                                 .Aggregate((x, y) => x + " " + y)//concat description
                                                                 .Split(' ')
                                                                 .Distinct().ToList();//remove duplicate

                    return result.Aggregate((x, y) => x + " " + y);
                }
            }
            return string.Empty;
        }

        private string GetBathroomInfo(RoomsData targetRoom)
        {
            var description = targetRoom.Descriptions.FirstOrDefault(x => x.Type == "Overview")?.Value;
            var splits = description.Replace(@",", "").Split(' ').ToList();
            
            if(splits.Contains("bathroom"))
            {
                return "1 bathroom";
            }

            if(splits.Contains("bathrooms", StringComparer.OrdinalIgnoreCase))
            {
                return splits[splits.IndexOf("bathrooms") - 1] + "bathrooms";
            }
            return string.Empty;
        }

        private string GetBedding(RoomsData room)
        {
            if (room.BedDetails.Count > 0 && !room.BedDetails.Any(x => x.Desc == null))
            {
                var result = room.BedDetails.Select(x => x.Desc)//select description
                                                             .Aggregate((x, y) => x + " " + y)//concat description
                                                             .Split(' ')
                                                             .Distinct().ToList();//remove duplicate

                return result.Aggregate((x, y) => x + " " + y);
            }
            return ParseBeddingKeywordsFromDescription(room.Descriptions.FirstOrDefault(x => x.Type == "Overview")?.Value);
        }

        private string ParseBeddingKeywordsFromDescription(string value)
        {
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(value);
            return htmlDocument.DocumentNode.ChildNodes.First().ChildNodes.First().InnerHtml;
            
        }
    }
}
