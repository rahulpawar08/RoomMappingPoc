namespace Clarify.FuzzyMatchingTest
{
    public class InputFile
    {
        public InputFile(string clarifiHotelId, string epsDataFileName, string hbDataFieName)
        {
            ClarifiHotelId = clarifiHotelId;
            EpsDataFileName = epsDataFileName;
            HbDataFileName = hbDataFieName;
        }
        public string ClarifiHotelId { get; set; }

        public string EpsDataFileName { get; set; }

        public string HbDataFileName { get; set; }
    }
}