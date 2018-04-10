using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusBoard.Api
{
    public class StopDistanceAPIReponse
    {

        public List<StopDistance> stopPoints { get; set; }

    }

    public class StopDistance
    {
        public List<DistanceLineGroup> lineGroup { get; set; }
        public string distance { get; set; }
        public string commonName { get; set; }
    }

    public class DistanceLineGroup
    {
        public string naptanIdReference { get; set; }
    }
}

