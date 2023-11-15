using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PASchools.Base.Connector.Interfaces
{
    public interface IBaseApiClient
    {
        Task<HttpResponseMessage> PostAsync(object body, string endpoint);
        Task<HttpResponseMessage> GetAsync(string endpoint, Dictionary<string, string> @params = null);
    }
}
