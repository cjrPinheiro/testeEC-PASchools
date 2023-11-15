using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PASchools.Google.Connector.Models.Response
{
    public class GeolocationReponse
    {
        public List<Result> results { get; set; }
        public string status { get; set; }

        public class Geometry
        {
            public Location location { get; set; }
        }

        public class Location
        {
            public double lat { get; set; }
            public double lng { get; set; }
        }

        public class Result
        {
            public Geometry geometry { get; set; }

        }
    }
}
