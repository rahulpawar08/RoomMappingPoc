using System;
using System.Collections.Generic;
using System.Text;

namespace Clarify.FuzzyMatchingTest
{
    public interface IRoomMatchingAlgo
    {
        int GetMatchingScore(string room1, string room2);
    }
}
