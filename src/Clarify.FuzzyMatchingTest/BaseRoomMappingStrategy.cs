using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Clarify.FuzzyMatchingTest.Data.Models;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Clarify.FuzzyMatchingTest
{
    public abstract class BaseRoomMappingStrategy : IRoomMappingStrategy
    {
        protected readonly string StrategyName = null;
        protected readonly string VersionId = null;
        protected readonly string MatchingAlgo = null;

        public ConcurrentBag<InputFile> InputFiles { get; set; }
        public ConcurrentBag<ClarifiModel> EpsSupplierData { get; set; }

        public ConcurrentBag<ClarifiModel> HotelBedSupplierData { get; set; }
        public IMatchingAlgorithm RoomMatchingAlgo { get; set; }
        private IMatchingAlgorithm matchingAlgorithm;

        public BaseRoomMappingStrategy(IMatchingAlgorithm matchingAlgorithm, string strategyName, string versionId, string matchingAlgo)
        {
            RoomMatchingAlgo = matchingAlgorithm;
            StrategyName = strategyName;
            VersionId = versionId;
            MatchingAlgo = matchingAlgo;
        }

        public void Initialize()
        {
            InputFiles = new ConcurrentBag<InputFile>();
            EpsSupplierData = new ConcurrentBag<ClarifiModel>();
            HotelBedSupplierData = new ConcurrentBag<ClarifiModel>();
            PopulateInputData();
            PopulateSupplierData();
        }

        private void PopulateSupplierData()
        {
            Parallel.ForEach(InputFiles, inputFile =>
            {
                EpsSupplierData.Add(GetClarifiModel(inputFile.EpsDataFileName, "EPSRapid"));
                HotelBedSupplierData.Add(GetClarifiModel(inputFile.HbDataFileName, "HotelBeds"));
            });
        }

        private ClarifiModel GetClarifiModel(string fileName, string supplier)
        {
            ClarifiModel model = null;
            using (StreamReader r = new StreamReader(fileName))
            {
                string json = r.ReadToEnd();
                model = JsonConvert.DeserializeObject<ClarifiModel>(json);
                model.RoomsData.ForEach(room => room.UpdateNameIfAccessible());
                model.SupplierFamily = supplier;
                model.HotelName = Regex.Replace((model.HotelName ?? string.Empty), "\"", string.Empty, RegexOptions.IgnoreCase);
                foreach (var roomData in model.RoomsData)
                    roomData.Name = Regex.Replace((roomData.Name ?? string.Empty), "\"", string.Empty, RegexOptions.IgnoreCase);
            }
            return model;
        }

        private void PopulateInputData()
        {
            //string[] filePaths = Directory.GetFiles(Directory.GetCurrentDirectory() + "\\Input");
            string[] filePaths = Directory.GetFiles(@"C:\logs\ExportedRooms\ExportedRooms");

            Parallel.ForEach(filePaths.Where(n => n.Contains("EPS")), epsFileName =>
            {
                string[] words = epsFileName.Split('_');

                if (words != null && words.Length > 0)
                {
                    string hotelBedFileName = filePaths.FirstOrDefault(n => n.Contains(words[0]) && n.Contains("HB"));
                    InputFiles.Add(new InputFile(Path.GetFileName(words[0]), epsFileName, hotelBedFileName));
                }
            });
        }

        public abstract ConcurrentBag<RoomMappingResult> ExecuteHotelBedEanRoomMapping(List<string> matchingFields);

        public string GetStrategyName()
        {
            return StrategyName;
        }

        public string GetMatchingAlgorithmName()
        {
            return MatchingAlgo;
        }
    }
}
