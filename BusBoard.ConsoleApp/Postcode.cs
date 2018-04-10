using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusBoard.ConsoleApp
{
    class PostCodeAPIResponse
    {
        public PostcodePosition Result { get; set; }
    }

    class PostcodePosition
    {
        public string Postcode { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
    }
}
