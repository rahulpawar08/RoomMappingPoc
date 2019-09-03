using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.FuzzyAlgo.Models;

namespace WebApp.FuzzyAlgo.Interfaces
{
    public interface IAlgoRunService
    {
        Task<AlgoRunResponse> RunAlgo(AlgoRunRequest request);
    }
}
