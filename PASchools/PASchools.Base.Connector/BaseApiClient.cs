﻿using PASchools.Base.Connector.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace PASchools.Base.Connector
{
    public class BaseApiClient : IBaseApiClient
    {
        public HttpClient _httpClient;

        public async Task<HttpResponseMessage> GetAsync(string endpoint, Dictionary<string, string> @params = null)
        {
            try
            {
                HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, endpoint);
                NameValueCollection query = HttpUtility.ParseQueryString("");

                if (@params != null)
                {
                    foreach (var param in @params)
                    {
                        query[param.Key] = param.Value;
                    }
                }
                var http = new HttpClient();
                var builder = new UriBuilder(new Uri(string.Concat("https://dadosabertos.poa.br/api/3/action/datastore_search?resource_id=5579bc8e-1e47-47ef-a06e-9f08da28dec8&limit=400")));
                builder.Query = query.ToString();

                return await http.GetAsync("https://dadosabertos.poa.br/api/3/action/datastore_search?resource_id=5579bc8e-1e47-47ef-a06e-9f08da28dec8&limit=400");
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<HttpResponseMessage> PostAsync(object body, string endpoint)
        {
            try
            {
                HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, string.Concat(_httpClient.BaseAddress, endpoint));

                message.Content = new StringContent(JsonSerializer.Serialize(body));
                message.Headers.Add("Content-Type", "application/json");

                return await _httpClient.SendAsync(message);

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        protected void SetHttpClient(HttpClient client) => _httpClient = client;
    }

}
