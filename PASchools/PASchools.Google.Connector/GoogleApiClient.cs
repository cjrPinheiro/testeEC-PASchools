using Microsoft.Extensions.Configuration;
using PASchools.Base.Connector;
using PASchools.Google.Connector.Interfaces;
using PASchools.Google.Connector.Models;
using PASchools.Google.Connector.Models.Requests;
using PASchools.Google.Connector.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PASchools.Google.Connector
{
    public class GoogleApiClient : BaseApiClient, IGoogleApiClient
    {
        public string? _apikey { get; set; }
        public GoogleApiClient()
        {
            _apikey = "";//config.GetSection("GoogleApiKey").Value;
            var uri = "https://maps.googleapis.com"; //config.GetSection("GoogleApiURI").Value;
            _httpClient = new HttpClient();
            //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            _httpClient.BaseAddress = new Uri(uri + "/maps/api");
            base.SetHttpClient(_httpClient);

        }

        public async Task<DistanceResponse> GetDistanceBetweenTwoCoordinatesAsync(AddressDTO origin, Coordinate destination)
        {
            DistanceResponse response = null;
            try
            {
                Dictionary<string, string> @params = new Dictionary<string, string>() {
                    { "destinations", $"{origin.Street}, {origin.Number} - {origin.District}, {origin.City} - SP, Brasil"},
                    { "origins", $"{destination.Latitude}{destination.Longitude}"},
                    { "units", "kilometers"},
                    { "key", $"{_apikey}"},
                    };
                var res = await GetAsync("/distancematrix/json", @params);
                string stringres = await res.Content.ReadAsStringAsync();

                response = JsonSerializer.Deserialize<DistanceResponse>(stringres);
            }
            catch (Exception)
            {

                throw;
            }
            return response;
        }

        public async Task<RouteResponse> GetRouteBetweenTwoCoordinatesAsync(AddressDTO origin, Coordinate destination)
        {
            RouteResponse response = null;
            try
            {
                Dictionary<string, string> @params = new Dictionary<string, string>() {
                    { "destinations", $"{origin.Street}, {origin.Number} - {origin.District}, {origin.City} - SP, Brasil"},
                    { "origins", $"{destination.Latitude}{destination.Longitude}"},
                    { "units", "kilometers"},
                    { "key", $"{_apikey}"},
                    };
                var res = await GetAsync("/distancematrix/json", @params);
                string stringres = await res.Content.ReadAsStringAsync();

                response = JsonSerializer.Deserialize<RouteResponse>(stringres);
            }
            catch (Exception)
            {

                throw;
            }
            return response;
        }
    }
}
