using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PASchools.Google.Connector.Models.Response
{
    public class DirectionsResponse
    {
        public List<Route> routes { get; set; }
        public string status { get; set; }

        public class Distance
        {
            public string text { get; set; }
            public int value { get; set; }
        }

        public class Duration
        {
            public string text { get; set; }
            public int value { get; set; }
        }

        public class EndLocation
        {
            public double lat { get; set; }
            public double lng { get; set; }
        }

        public class Leg
        {
            public Distance distance { get; set; }
            public Duration duration { get; set; }
            public List<Step> steps { get; set; }
        }

        public class Route
        {
            public List<Leg> legs { get; set; }
        }

        public class Step
        {
            public Distance distance { get; set; }
            public Duration duration { get; set; }
            public string html_instructions { get; set; }
            public string travel_mode { get; set; }
        }
    }
}
