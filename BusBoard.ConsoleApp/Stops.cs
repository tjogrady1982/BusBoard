using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusBoard.ConsoleApp
{
    class StopsResponse
    {
        public Stops lineGroup { get; set; }
    }
    class Stops
    {
        public string naptanIdReference { get; set; }
        public string smsCode { get; set; }
        public string commonName { get; set; }
    }
}
