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
            _apikey = "AIzaSyAZzkj7oDuyax7KYLL1QDXlGPe9dIefqZk";//config.GetSection("GoogleApiKey").Value;
            var uri = "https://maps.googleapis.com"; //config.GetSection("GoogleApiURI").Value;
            _httpClient = new HttpClient();
            //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            _httpClient.BaseAddress = new Uri(uri + "/maps/api");
            base.SetHttpClient(_httpClient);

        }

        public async Task<DistanceResponse> GetDistanceBetweenTwoCoordinatesAsync(Coordinate origin, Coordinate destination)
        {
            DistanceResponse response = null;
            try
            {
                Dictionary<string, string> @params = new Dictionary<string, string>() {
                    { "origins", $"{origin.Lat} {origin.Lng}"},
                    { "destinations", $"{destination.Lat} {destination.Lng}"},
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
        /// <summary>
        /// Supports max 100 destinations per request
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="destinations"></param>
        /// <returns></returns>
        public async Task<DistanceResponse> GetDistanceBetweenOneOriginManyDestinationAsync(Coordinate origin, Coordinate[] destinations)
        {
            DistanceResponse response = null;
            try
            {
                string[] destinationsString = destinations.Select(q => $"{q.Lat},{q.Lng}").ToArray();
                string dests = String.Join("|", destinationsString);
                Dictionary<string, string> @params = new Dictionary<string, string>() {
                    { "origins", $"{origin.Lat} {origin.Lng}"},
                    { "destinations", $"{dests}"},
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
        public async Task<DirectionsResponse> GetRouteBetweenTwoCoordinatesAsync(Coordinate origin, Coordinate destination)
        {
            DirectionsResponse response = null;
            try
            {
                Dictionary<string, string> @params = new Dictionary<string, string>() {
                    { "destination", $"{destination.Lat},{destination.Lng}"},
                    { "origin", $"{origin.Lat},{origin.Lng}"},
                    { "language", "pt-BR"},
                    { "mode", "DRIVING"},
                    { "key", $"{_apikey}"},
                    };
                var res = await GetAsync("/directions/json", @params);
                string stringres = await res.Content.ReadAsStringAsync();

                response = JsonSerializer.Deserialize<DirectionsResponse>(stringres);
            }
            catch (Exception)
            {

                throw;
            }
            return response;
        }
        public async Task<GeolocationReponse> GetGeocodingAsync(AddressDTO origin)
        {
            GeolocationReponse response = null;
            try
            {
                Dictionary<string, string> @params = new Dictionary<string, string>() {
                    { "address", $"{origin.Street}, {origin.Number} - {origin.District}, {origin.City} - {origin.State}, Brazil"},
                    { "key", $"{_apikey}"}};

                var res = await GetAsync("/geocode/json", @params);
                string stringres = await res.Content.ReadAsStringAsync();

                response = JsonSerializer.Deserialize<GeolocationReponse>(stringres);
            }
            catch (Exception)
            {
                throw;
            }
            return response;
        }

    }
}
