using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;

namespace BusBoard.Api
{
    public class Program
    {
       

        public List<Arrivals> GetBusInfo(string input)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            // TODO return error object/message?
            if (input.Length < 5)
            {
                //    Console.Write("Please enter a 5 digit number! OR a 6 character postcode");
                return new List<Arrivals>();
            }

            else if (input.Length == 5)
            {
               return ProcessSMSCode(input);
            }

            else
            {
                return GetStopsFromPostcode(input);
            }
        }
        List<Arrivals> ConvertSMSCodeToString(string busstopid)
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

            buslist.ForEach(e => e.expectedArrival = e.expectedArrival.ToLocalTime());
            buslist.ForEach(e => e.TimeRemaining = e.expectedArrival.Subtract(DateTime.Now));


            return buslist.OrderBy(o => o.expectedArrival).Take(5).ToList();

        }

        public List<Arrivals> ProcessSMSCode(string input)
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
            return ConvertSMSCodeToString(busstopid);
        }
        public List<Arrivals> GetStopsFromPostcode(string input)
        {
            var postclient = new RestClient("https://api.postcodes.io/postcodes/" + input);
            var postrequest = new RestRequest("", Method.GET);
            var postresponse = postclient.Execute<PostCodeAPIResponse>(postrequest);

            var lat = postresponse.Data.Result.Latitude;
            var lon = postresponse.Data.Result.Longitude;
            return GetStopsFromLatAndLon(lat, lon);
        }

        public List<Arrivals> GetStopsFromLatAndLon(string lat, string lon)
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

            var stopsOutput = new List<Arrivals>();
            var SortedList = distancelist.OrderBy(o => o.distance).Take(2).ToList();
            foreach (var item in SortedList)
            {
                stopsOutput = stopsOutput.Concat(ConvertSMSCodeToString(item.lineGroup[0].naptanIdReference)).ToList();
            }
            return stopsOutput;
        }
    }
}
