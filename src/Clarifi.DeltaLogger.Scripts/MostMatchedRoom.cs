using System;
using System.Collections.Generic;
using System.Text;

namespace Clarifi.DeltaLogger.Scripts
{
    public class MostMatchedRoom
    {
        public string MostMatchedRoomId { get; set; }

        public int HighestMatchedScore { get; set; }

        public string FieldsUsedForHighestMatch { get; set; }
    }
}
