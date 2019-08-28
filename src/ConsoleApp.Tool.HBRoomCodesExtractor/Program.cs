using ConsoleApp.Tool.HBRoomCodesExtractor.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ConsoleApp.Tool.HBRoomCodesExtractor
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Room> rooms = new List<Room>();
            Dictionary<string, string> primaryTypes = new Dictionary<string, string>();
            Dictionary<string, string> secondaryTypes = new Dictionary<string, string>();
            Dictionary<string, string> typesOnly = new Dictionary<string, string>();

            List<string> lines = new List<string>();

            string[] filePaths = Directory.GetFiles(@"C:\Clarifi\RoomMapping\Room_Type");

            foreach(string filePath in filePaths)
            {
                RootObject jsonObj = JsonConvert.DeserializeObject<RootObject>(File.ReadAllText(filePath));
                rooms.AddRange(jsonObj.rooms);
            }

            rooms = rooms.OrderBy(r => r.code.Length).ToList();

            foreach(Room room in rooms)
            {
                string[] firstplit = room.code.Split('.');
                string type = firstplit[0];
                string[] secondSplit = firstplit[1].Split('-');

                primaryTypes[type] = room.typeDescription.content;
                typesOnly[type] = string.Empty;

                if(secondSplit.Length == 1 || IsAllDigits(secondSplit[1]))
                {
                    secondaryTypes[secondSplit[0]] = room.characteristicDescription.content;
                    typesOnly[secondSplit[0]] = string.Empty;
                }
                else
                {
                    typesOnly[secondSplit[0]] = string.Empty;
                    typesOnly[secondSplit[1]] = string.Empty;

                    if(secondaryTypes.ContainsKey(secondSplit[0]) && (secondaryTypes.ContainsKey(secondSplit[1]) == false))
                    {
                        secondaryTypes[secondSplit[1]] = RemoveSubString(room.characteristicDescription.content, secondaryTypes[secondSplit[0]]);
                        secondaryTypes[secondSplit[1]] = secondaryTypes[secondSplit[1]].Trim();
                    }
                    else if ((secondaryTypes.ContainsKey(secondSplit[0]) == false) && secondaryTypes.ContainsKey(secondSplit[1]))
                    {
                        secondaryTypes[secondSplit[0]] = RemoveSubString(room.characteristicDescription.content, secondaryTypes[secondSplit[1]]);
                        secondaryTypes[secondSplit[0]] = secondaryTypes[secondSplit[0]].Trim();
                    }
                    else if((secondaryTypes.ContainsKey(secondSplit[0]) == false) && (secondaryTypes.ContainsKey(secondSplit[1]) == false))
                    {
                        if (secondSplit[0] == "XW")
                        {
                            secondaryTypes[secondSplit[0]] = "WITH MORE THAN THREE BATHROOMS";
                            secondaryTypes[secondSplit[1]] = RemoveSubString(room.characteristicDescription.content, secondaryTypes[secondSplit[0]]);
                            secondaryTypes[secondSplit[1]] = secondaryTypes[secondSplit[1]].Trim();
                        }
                        else if (secondSplit[1] == "XW")
                        {
                            secondaryTypes[secondSplit[1]] = "WITH MORE THAN THREE BATHROOMS";
                            secondaryTypes[secondSplit[0]] = RemoveSubString(room.characteristicDescription.content, secondaryTypes[secondSplit[1]]);
                            secondaryTypes[secondSplit[0]] = secondaryTypes[secondSplit[0]].Trim();
                        }
                        else
                        {
                            string[] spaceSplit = room.characteristicDescription.content.Split(' ');
                            int mid = spaceSplit.Length / 2;

                            secondaryTypes[secondSplit[0]] = string.Empty;
                            secondaryTypes[secondSplit[1]] = string.Empty;
                            for (int i = 0; i < mid; i++) { secondaryTypes[secondSplit[1]] += (spaceSplit[i] + " "); }
                            for (int j = mid; j < spaceSplit.Length; j++) { secondaryTypes[secondSplit[0]] += (spaceSplit[j] + " "); }
                            secondaryTypes[secondSplit[0]] = secondaryTypes[secondSplit[0]].Trim();
                            secondaryTypes[secondSplit[1]] = secondaryTypes[secondSplit[1]].Trim();
                        }
                    }
                }
            }

            lines.AddRange(primaryTypes.Select(x => x.Key + "," + x.Value));
            lines.AddRange(secondaryTypes.Select(x => x.Key + "," + x.Value));

            File.WriteAllLines(@"C:\Clarifi\RoomMapping\HBKeywords.csv", lines);
        }

        public static bool IsAllDigits(string s)
        {
            return s.All(char.IsDigit);
        }

        public static string RemoveSubString(string sourceString, string removeString)
        {
            int index = sourceString.IndexOf(removeString);
            return (index < 0) ? sourceString : sourceString.Remove(index, removeString.Length);
        }
    }
}
