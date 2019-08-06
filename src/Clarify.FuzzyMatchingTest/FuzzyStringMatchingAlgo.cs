using BoomTown.FuzzySharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clarify.FuzzyMatchingTest
{
   public class FuzzyStringMatchingAlgo : IMatchingAlgorithm
    {
        public int GetMatchingScore(string room1, string room2)
        {
            return Fuzzy.TokenSetRatio(room1, room2);
        }
    }
}
