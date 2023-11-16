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
        Task<DistanceResponse> GetDistanceBetweenTwoCoordinatesAsync(Coordinate origin, Coordinate destination );
        Task<DirectionsResponse> GetRouteBetweenTwoCoordinatesAsync(Coordinate origin, Coordinate destination );
        Task<DistanceResponse> GetDistanceBetweenOneOriginManyDestinationAsync(Coordinate origin, Coordinate[] destinations);
        Task<GeolocationReponse> GetGeocodingAsync(AddressDTO origin);
    }
}
