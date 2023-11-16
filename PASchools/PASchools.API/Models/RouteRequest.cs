using PASchools.Application.DTOs;
using PASchools.Google.Connector.Models.Requests;

namespace PASchools.API.Models
{
    public class RouteRequest
    {
        public Coordinate Origin { get; set; }
        public Coordinate Destination { get; set; }
    }
}
