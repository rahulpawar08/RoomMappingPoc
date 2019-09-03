using Clarify.FuzzyMatchingTest.Data.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Clarify.FuzzyMatchingTest
{
    public class HotelBedsKeywordExtractor
    {
        private ConcurrentBag<HotelBedsKeywordMapping> _mapping = new ConcurrentBag<HotelBedsKeywordMapping>();

        public HotelBedsKeywordExtractor()
        {
            InitializeMapping();
        }

        public string GetExtractedString(string roomCode, string keywordType)
        {
            var roomkeywords = GetRoomKeywords(roomCode);
            try
            {
                var extractedMeaning = _mapping
                              .Where(x => roomkeywords.Contains(x.Keyword))
                              .Where(y => y.Type == keywordType)
                              .Select(x => x.Meaning)
                              .Aggregate((m, n) => m + " " + n);
                return extractedMeaning;
            }
            catch (Exception ex)
            {
                return "";
            }

        }

        private List<string> GetRoomKeywords(string roomCode)
        {
            var roomInitialSplit = roomCode.Split('.');
            var secondaryKeywords = roomInitialSplit[1].Split('-');
            var result = new List<string> { roomInitialSplit[0] };
            result.AddRange(secondaryKeywords);
            return result;
        }

        private void InitializeMapping()
        {
            _mapping = new ConcurrentBag<HotelBedsKeywordMapping>(File.ReadAllLines(Directory.GetCurrentDirectory() + "\\SupplierKeywords\\HBKeywords.csv")
                           .Skip(1)
                           .Select(x => GetMapping(x)));
        }

        internal Dictionary<string, string> GetKeywordMapping(string roomCode)
        {
            //Identify the type of room
            var roomkeywords = GetRoomKeywords(roomCode);
            var result = _mapping
           .Where(x => roomkeywords.Contains(x.Keyword))
           //Group the room by type, then create a dictionary with keyword and its meaning
           .GroupBy(x => x.Type).ToDictionary(x => x.Key, y => GetMeaning(y));

            //return only the valid keywords
            return result.Where(x => x.Key != "unknown").ToDictionary(x => x.Key, y => y.Value);

        }

        private string GetMeaning(IGrouping<string, HotelBedsKeywordMapping> group)
        {
            return group.Select(z => z.Meaning).Aggregate((x, y) => x + " " + y);
        }

        private HotelBedsKeywordMapping GetMapping(string csvLine)
        {
            var splits = csvLine.Split(',');
            return new HotelBedsKeywordMapping
            {
                Keyword = splits[0].Trim(),
                Meaning = splits[1],
                Type = splits[2]
            };
        }
    }
}
