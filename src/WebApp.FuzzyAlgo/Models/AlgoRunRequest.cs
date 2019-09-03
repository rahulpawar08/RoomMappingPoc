using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.FuzzyAlgo.Models
{
    public class AlgoRunRequest
    {
        public string Strategy { get; set; }

        public List<string> Fields { get; set; }
    }
}
