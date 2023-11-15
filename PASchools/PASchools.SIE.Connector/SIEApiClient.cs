using PASchools.Base.Connector;
using PASchools.SIE.Connector.Interfaces;
using PASchools.SIE.Connector.Models.Reponse;
using System.Net.Http;
using System.Text.Json;

namespace PASchools.SIE.Connector
{
    public class SIEApiClient : BaseApiClient, ISIEApiClient
    {
        private readonly string _resourceId = "";
        public SIEApiClient()
        {
            _resourceId = "5579bc8e-1e47-47ef-a06e-9f08da28dec8";
            var uri = "https://dadosabertos.poa.br";
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(uri + "/api/3/action");
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