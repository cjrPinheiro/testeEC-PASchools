using PASchools.Google.Connector.Models;
using PASchools.Google.Connector.Models.Requests;
using PASchools.Google.Connector.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PASchools.Google.Connector.Interfaces
{
    public interface IGoogleApiClient
    {
        Task<DistanceResponse> GetDistanceBetweenTwoCoordinatesAsync(AddressDTO origin, Coordinate destination );
        Task<RouteResponse> GetRouteBetweenTwoCoordinatesAsync(AddressDTO origin, Coordinate destination );
    }
}
