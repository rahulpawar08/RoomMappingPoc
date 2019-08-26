using Clarify.FuzzyMatchingTest.Data.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Clarify.FuzzyMatchingTest
{
    public class HotelBedsKeywordExtractor
    {
        private List<HotelBedsKeywordMapping> _mapping = new List<HotelBedsKeywordMapping>();

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
                              .FindAll(x => roomkeywords.Contains(x.Keyword))
                              .Where(y => y.Type == keywordType)
                              .Select(x => x.Meaning)
                              .Aggregate((m, n) => m + " " + n);
                return extractedMeaning;
            }
            catch(Exception ex)
            {
                return "";
            }
            
        }

        private List<string> GetRoomKeywords(string roomCode)
        {
            var roomInitialSplit = roomCode.Split('.');
            var secondaryKeywords =  roomInitialSplit[1].Split('-');
            var result =  new List<string> { roomInitialSplit[0] };
            result.AddRange(secondaryKeywords);
            return result;
        }

        private void InitializeMapping()
        {
            _mapping = File.ReadAllLines(Directory.GetCurrentDirectory() + "\\SupplierKeywords\\HBKeywords.csv")
                           .Skip(1)
                           .Select(x => GetMapping(x))
                           .ToList();
        }

        internal Dictionary<string, string> GetKeywordMapping(string roomCode)
        {
            var roomkeywords = GetRoomKeywords(roomCode);
             var result = _mapping
            .FindAll(x => roomkeywords.Contains(x.Keyword))
            .GroupBy(x => x.Type).ToDictionary(x => x.Key, y => GetMeaning(y));

            return result.Where(x=>x.Key != "unknown").ToDictionary(x=>x.Key, y=>y.Value);

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
