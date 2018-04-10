using System.Collections.Generic;

namespace BusBoard.Api
{
    public class StopsResponse
    {
        public string smsCode { get; set; }
        public string commonName { get; set; }
        public List<LineGroup> lineGroup { get; set; }
    }

    public class LineGroup
    {
        public string naptanIdReference { get; set; }
    }
}
