using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Clarify.FuzzyMatchingTest.Data.Models;
using Newtonsoft.Json;

namespace Clarify.FuzzyMatchingTest
{
    public abstract class BaseRoomMappingStrategy : IRoomMappingStrategy
    {
        public List<InputFile> InputFiles { get; set; }
        public List<ClarifiModel> EpsSupplierData { get; set; }

        public List<ClarifiModel> HotelBedSupplierData { get; set; }
        public IMatchingAlgorithm RoomMatchingAlgo { get; set; }
        private IMatchingAlgorithm matchingAlgorithm;

        public BaseRoomMappingStrategy(IMatchingAlgorithm matchingAlgorithm)
        {
            RoomMatchingAlgo = matchingAlgorithm;
        }

        public void Initialize()
        {
            InputFiles = new List<InputFile>();
            EpsSupplierData = new List<ClarifiModel>();
            HotelBedSupplierData = new List<ClarifiModel>();
            PopulateInputData();
            PopulateSupplierData();
        }

        private void PopulateSupplierData()
        {
            foreach (var inputFile in InputFiles)
            {
                EpsSupplierData.Add(GetClarifiModel(inputFile.EpsDataFileName, "EPSRapid"));
                HotelBedSupplierData.Add(GetClarifiModel(inputFile.HbDataFileName, "HotelBeds"));
            }
        }

        private ClarifiModel GetClarifiModel(string fileName, string supplier)
        {
            ClarifiModel model = null;
            using (StreamReader r = new StreamReader(fileName))
            {
                string json = r.ReadToEnd();
                model = JsonConvert.DeserializeObject<ClarifiModel>(json);
                model.RoomsData.ForEach(room => room.UpdateNameIfAccessible());
            }

            //var hotelData = _elasticSearchProvider.GetHotelBySupplierIdFamily(x.SupplierId, "EPSRapid");
            return model;
        }

        private void PopulateInputData()
        {
            string[] filePaths = Directory.GetFiles(@"C:\Clarifi\RoomMapping\ExportedRooms");
            foreach (var epsFileName in filePaths.Where(n => n.Contains("EPS")))
            {
                string[] words = epsFileName.Split('_');

                if (words != null && words.Length > 0)
                {
                    string hotelBedFileName = filePaths.FirstOrDefault(n => n.Contains(words[0]) && n.Contains("HB"));
                    InputFiles.Add(new InputFile(Path.GetFileName(words[0]), epsFileName, hotelBedFileName));
                }
            }
        }

        public abstract List<RoomMappingResult> ExecuteHotelBedEanRoomMapping(List<string> matchingFields);
    }
}
