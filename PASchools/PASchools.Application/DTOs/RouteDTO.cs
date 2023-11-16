using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PASchools.Application.DTOs
{
    public class RouteDTO
    {
        public string DurationText { get; set; }
        public string DistanceText { get; set; }
        public List<string> Steps { get; set; }
    }
}
