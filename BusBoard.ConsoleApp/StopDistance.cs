using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusBoard.ConsoleApp
{
    class StopDistanceAPIReponse
    {

        public List<StopDistance> stopPoints { get; set; }

    }

    class StopDistance
    {
        public string naptanIdReference { get; set; }
        public string distance { get; set; }
        public string commonName { get; set; }
    }
}

