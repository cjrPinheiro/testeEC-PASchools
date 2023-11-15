using PASchools.Application.DTOs;
using PASchools.Google.Connector.Models;
using PASchools.Google.Connector.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PASchools.Application.Interfaces
{
    public interface ISchoolService
    {
        Task<List<SchoolDTO>> FindSchoolsByAddressOrderByDistance(AddressDTO address);
        Task<RouteDTO> GenerateRoute(AddressDTO origin, Coordinate destination);
        Task UpdateSchoolDatabase();
    }
}
