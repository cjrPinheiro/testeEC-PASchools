using Microsoft.Extensions.Options;
using PASchools.Base.Connector;
using PASchools.Domain.Models;
using PASchools.SIE.Connector.Interfaces;
using PASchools.SIE.Connector.Models.Reponse;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace PASchools.SIE.Connector
{
    public class SIEApiClient : BaseApiClient, ISIEApiClient
    {
        private readonly string _resourceId = "";
        public SIEApiClient(IOptions<Settings> settings)
        {
            _resourceId = settings.Value.SIEApiSettings.Key;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(settings.Value.SIEApiSettings.BaseUri);
            base.SetHttpClient(_httpClient);

        }
        public async Task<SchoolsResponse> GetAllPASchoolsListAsync(int limit)
        {
            SchoolsResponse response = null;
            try
            {
                Dictionary<string, string> @params = new Dictionary<string, string>() {
                    { "resource_id", _resourceId},
                    { "limit", $"{limit}"}};

                var res = await GetAsync("/datastore_search", @params);
                string stringres = await res.Content.ReadAsStringAsync();

                response = JsonSerializer.Deserialize<SchoolsResponse>(stringres);
            }
            catch (Exception)
            {
                throw;
            }
            return response;
        }
    }

}