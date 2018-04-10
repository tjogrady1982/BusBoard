using System.Collections.Generic;

namespace BusBoard.ConsoleApp
{
    class StopsResponse
    {
        public string smsCode { get; set; }
        public string commonName { get; set; }
        public List<LineGroup> lineGroup { get; set; }
    }

    class LineGroup
    {
        public string naptanIdReference { get; set; }
    }
}
