using Clarify.FuzzyMatchingTest.Data.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace ConsoleApp.Tool.EPSRoomNameCleaner
{
    class Program
    {
        static void Main(string[] args)
        {
            List<ClarifiModel> inputData = new List<ClarifiModel>();
            PopulateInputData(inputData);

            Dictionary<string, List<RoomsData>> hotelRooms = new Dictionary<string, List<RoomsData>>();
            inputData.ForEach(i => hotelRooms.Add(i.HotelClarifiId, i.RoomsData));

            CleanRoomNames(hotelRooms);
        }

        public static void PopulateInputData(List<ClarifiModel> inputData)
        {
            string[] filePaths = Directory.GetFiles(@"C:\Clarifi\RoomMapping\ExportedRooms");
            foreach (var epsFileName in filePaths.Where(n => n.Contains("EPS")))
            {
                inputData.Add(GetClarifiModel(epsFileName));
            }
        }

        public static ClarifiModel GetClarifiModel(string fileName)
        {
            ClarifiModel model = null;
            using (StreamReader r = new StreamReader(fileName))
            {
                string json = r.ReadToEnd();
                model = JsonConvert.DeserializeObject<ClarifiModel>(json);
                model.RoomsData.ForEach(room => room.UpdateNameIfAccessible());
            }
            return model;
        }

        public static void CleanRoomNames(Dictionary<string, List<RoomsData>> hotelRooms)
        {
            List<string> lines = new List<string>();
            lines.Add("HotelId,SupplierHotelId,RoomId,SupplierRoomId,RoomName,CleanedRoomName");
            List<EpsRoomTypeMapping> epsRoomTypeMapping = new List<EpsRoomTypeMapping>();

            foreach (var pair in hotelRooms)
            {
                if (pair.Value != null)
                {
                    foreach (var room in pair.Value)
                    {
                        string cleaneRoomName = GetCleanedRoomName(room);
                        lines.Add($"{pair.Key},{room.SupplierId},{room.ClarifiRoomId},{room.SupplierRoomId},\"{room.Name}\",\"{cleaneRoomName}\"");
                        epsRoomTypeMapping.Add(new EpsRoomTypeMapping { SupplierRoomId = room.SupplierRoomId, CleanRoomName = cleaneRoomName });
                    }
                }
            }

            File.WriteAllLines(@"C:\Clarifi\RoomMapping\EpsCleanedRoomNames.csv", lines);
            File.WriteAllText(@"C:\Clarifi\RoomMapping\EPSRoomNames.txt", JsonConvert.SerializeObject(epsRoomTypeMapping));
        }

        public static string GetCleanedRoomName(RoomsData room)
        {
            string cleanedRoomName = string.Empty;

            if (string.IsNullOrEmpty(room.Name) == false)
            {
                cleanedRoomName = RemoveBracketsData(room.Name);
                cleanedRoomName = cleanedRoomName.Split(',')[0];

                cleanedRoomName = RemoveBeddingInfo(cleanedRoomName, room);
                cleanedRoomName = RemoveViewsInfo(cleanedRoomName, room);

                cleanedRoomName = RemoveSpecialCharacters(cleanedRoomName);
                cleanedRoomName = cleanedRoomName.Trim();
            }

            return cleanedRoomName;
        }

        public static string RemoveBeddingInfo(string name, RoomsData room)
        {
            if (string.IsNullOrEmpty(name))
                return string.Empty;

            string cleanedName = name;
            string[] splits = name.Split(' ');
 
            if (room.BedDetails != null && room.BedDetails.Count > 0 && !room.BedDetails.Any(x => x.Desc == null))
            {
                var beddingDetails = room.BedDetails.Select(x => x.Desc)//select description
                                                             .Aggregate((x, y) => x + " " + y)//concat description
                                                             .Split(' ')
                                                             .Distinct().ToList();
                beddingDetails.ForEach(bd => { if (splits.Contains(bd, StringComparer.InvariantCultureIgnoreCase)) splits = splits.Where(s => !s.Equals(bd, StringComparison.InvariantCultureIgnoreCase)).ToArray(); });
                cleanedName = string.Join(' ', splits);
            }

            return cleanedName;
        }

        public static string RemoveViewsInfo(string name, RoomsData room)
        {
            if (string.IsNullOrEmpty(name))
                return string.Empty;

            string cleanedName = name;
            string[] splits = name.Split(' ');

            if (room.RoomViews != null && room.RoomViews.Count > 0 && !room.RoomViews.Any(x => x.Value == null))
            {
                var viewDetails = room.RoomViews.Select(x => x.Value)   //select description
                                                             .Aggregate((x, y) => x + " " + y)//concat description
                                                             .Split(' ')
                                                             .Distinct().ToList();//remove duplicate

                viewDetails.ForEach(vd => { if (splits.Contains(vd, StringComparer.InvariantCultureIgnoreCase)) splits = splits.Where(s => !s.Equals(vd, StringComparison.InvariantCultureIgnoreCase)).ToArray(); });
                cleanedName = string.Join(' ', splits);
            }

            return cleanedName;
        }

        public static string RemoveBracketsData(string name)
        {
            if (string.IsNullOrEmpty(name))
                return string.Empty;

            string cleanedName = name;

            string regex = "(\\[.*\\])|(\\(.*\\))";
            cleanedName = Regex.Replace(cleanedName, regex, "");

            return cleanedName;
        }

        public static string RemoveSpecialCharacters(string name)
        {
            if (string.IsNullOrEmpty(name))
                return string.Empty;

            string cleanedName = name;
            string[] chars = new string[] { ",", ".", "/", "!", "@", "#", "$", "%", "^", "&", "*", "'", "\"", ";", "_", "(", ")", ":", "|", "[", "]" };

            for(int i = 0; i < chars.Length; i++)
            {
                cleanedName = cleanedName.Replace(chars[i], "");
            }

            return cleanedName;
        }

        public static string RemoveSubString(string sourceString, string removeString)
        {
            int index = sourceString.IndexOf(removeString);
            return (index < 0) ? sourceString : sourceString.Remove(index, removeString.Length);
        }
    }
}
