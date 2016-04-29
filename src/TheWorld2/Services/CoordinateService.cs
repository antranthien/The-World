using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace TheWorld2.Services
{
    public class CoordinateService
    {
        private ILogger<CoordinateService> _logger;

        public CoordinateService(ILogger<CoordinateService> logger)
        {
            _logger = logger;
        }

        public async Task<CoordinateServiceResult> Lookup(string location)
        {
            var result = new CoordinateServiceResult
            {
                IsSuccess = false,
                Message = $"Unable to look up the coordinates for: {location}"
            };

            var encodeAddress = WebUtility.UrlEncode(location);

            //Alternative: Environment.GetEnvironmentVariable("AppSettings:APIKey");
            var apiKey = Startup.Configuration["AppSettings:APIKey"];

            var url = $"https://maps.googleapis.com/maps/api/geocode/json?address={encodeAddress}&key={apiKey}";

            var client = new HttpClient();

            var json = await client.GetStringAsync(url);

            var jsonObject = JObject.Parse(json);
            var results = jsonObject["results"];

            if (!results.HasValues)
            {
                result.Message = $"Could not find {location} as a location";
            }
            else
            {
                var locationCoordinate = results[0]["geometry"]["location"];
                result.Latitude = (double)locationCoordinate["lat"];
                result.Longitude = (double)locationCoordinate["lng"];
                result.IsSuccess = true;
                result.Message = "Success";
            }
            return result;
        }
    }
}
