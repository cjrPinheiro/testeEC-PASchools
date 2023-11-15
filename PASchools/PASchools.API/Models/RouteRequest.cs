using PASchools.Application.DTOs;
using PASchools.Google.Connector.Models.Requests;

namespace PASchools.API.Models
{
    public class RouteRequest
    {
        public AddressDTO Origin { get; set; }
        public Coordinate Destination { get; set; }
    }
}
