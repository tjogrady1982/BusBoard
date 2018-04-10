using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;

namespace BusBoard.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            new Program().Run();
        }

        private void Run()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            Console.WriteLine("Bus Board 2018");
            Console.WriteLine("Please Insert a 5 Digit SMS Stop Code OR your Postcode:");

            string input = Console.ReadLine();


            if (input.Length < 5)
            {
                Console.Write("Please enter a 5 digit number! OR a 6 character postcode");
            }

            else if (input.Length == 5)
            {
                ProcessSMSCode(input);

            }

            else if (input.Length > 5)
            {
                GetStopsFromPostcode(input);
            }

        }
        void ConvertSMSCodeToString(string busstopid)
        {

            var client = new RestClient("https://api.tfl.gov.uk/StopPoint/" + busstopid + "/Arrivals");

            var request = new RestRequest("", Method.GET);
            request.AddParameter("app_id", "4b439dd2");
            request.AddParameter("app_key", "42d5b73ee92771eb99ea20c4b43edf24");


            var response = client.Execute<List<Arrivals>>(request);
            var buslist = new List<Arrivals>();

            foreach (var item in response.Data)
            {
                buslist.Add(item);
            }

            List<Arrivals> SortedList = buslist.OrderBy(o => o.expectedArrival).Take(5).ToList();

            foreach (var item in SortedList)
            {
                Console.WriteLine(item.vehicleId + " " + item.expectedArrival + " " + item.naptanId + " " + item.stationName);
            }

            //Console.ReadLine();
        }

        public void ProcessSMSCode(string input)
        {
            var stopclient = new RestClient("https://api.tfl.gov.uk/StopPoint/Sms/" + input);
            var stoprequest = new RestRequest("", Method.GET);
            stoprequest.AddParameter("app_id", "4b439dd2");
            stoprequest.AddParameter("app_key", "42d5b73ee92771eb99ea20c4b43edf24");
            var stopresponse = stopclient.Execute<StopsResponse>(stoprequest);
            var stop = stopresponse.Data;
            var lineGroup = stop.lineGroup[0];

            Console.WriteLine(lineGroup.naptanIdReference + " " + stop.smsCode + " " + stop.commonName);
            var busstopid = lineGroup.naptanIdReference;
            ConvertSMSCodeToString(busstopid);
            Console.ReadLine();
        }
        public void GetStopsFromPostcode(string input)
        {
            var postclient = new RestClient("https://api.postcodes.io/postcodes/" + input);
            var postrequest = new RestRequest("", Method.GET);
            var postresponse = postclient.Execute<PostCodeAPIResponse>(postrequest);
            var postlist = new List<PostcodePosition>();

            postlist.Add(postresponse.Data.Result);

            foreach (var item in postlist)
            {
                Console.WriteLine(item.Postcode + " " + item.Latitude + " " + item.Longitude);
                var lat = item.Latitude;
                var lon = item.Longitude;
                Console.WriteLine();
                GetStopsFromLatAndLon(lat, lon);
            }
        }

        public void GetStopsFromLatAndLon(string lat, string lon)
        {
            var distanceclient = new RestClient("https://api.tfl.gov.uk/StopPoint?stopTypes=NaptanOnstreetBusCoachStopPair&radius=500&lat=" + lat + "&lon=" + lon);
            var distancerequest = new RestRequest("", Method.GET);
            distancerequest.AddParameter("app_id", "4b439dd2");
            distancerequest.AddParameter("app_key", "42d5b73ee92771eb99ea20c4b43edf24");

            var distanceresponse = distanceclient.Execute<StopDistanceAPIReponse>(distancerequest);
            var distancelist = new List<StopDistance>();

            foreach (var item in distanceresponse.Data.stopPoints)
            {
                distancelist.Add(item);
            }

            List<StopDistance> SortedList = distancelist.OrderBy(o => o.distance).Take(2).ToList();

            foreach (var item in SortedList)
            {
                Console.WriteLine(item.distance + " " + item.commonName + " " + item.naptanIdReference);

                ConvertSMSCodeToString(item.naptanIdReference);


            }
            Console.ReadLine();
        }
    }
}
