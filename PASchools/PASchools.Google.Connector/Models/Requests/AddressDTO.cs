using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PASchools.Google.Connector.Models.Requests
{
    public class AddressDTO
    {
        public AddressDTO()
        {
            State = "RS";
            City = "Porto Alegre";
        }
        public string? Street { get; set; }
        public int Number { get; set; }
        public string? District { get; set; }
        public string? City { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string? State { get; set; }
    }
}
