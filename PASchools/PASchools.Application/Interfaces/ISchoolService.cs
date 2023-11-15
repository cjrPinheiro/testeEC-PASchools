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
        Task<List<SchoolDTO>> FindSchoolsByAddressOrderByDistance(Coordinate coordinate);
        Task<RouteDTO> GenerateRoute(Coordinate coordinate, Coordinate destination);
        Task<int> UpdateSchoolDatabase(int limit);
        Task<Coordinate> GetCoordinatesAsync(AddressDTO origin);
    }
}
