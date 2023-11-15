using PASchools.SIE.Connector.Models.Reponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PASchools.SIE.Connector.Interfaces
{
    public interface ISIEApiClient
    {
        Task<SchoolsResponse> GetAllPASchoolsListAsync(int limit);
    }
}
