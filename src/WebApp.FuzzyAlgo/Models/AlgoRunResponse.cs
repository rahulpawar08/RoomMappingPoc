using Clarify.FuzzyMatchingTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.FuzzyAlgo.Models
{
    public class AlgoRunResponse
    {
        public string Message { get; set; }

        public RoomMappingSummary Summary { get; set; }
    }
}
