using PASchools.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PASchools.Domain.Entities
{
    public class Address : BaseEntity
    {
        public string? Street { get; set; }
        public int Number { get; set; }
        public string? District { get; set; }
        public string? City { get; set; }
        public int PostalCode { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string? State { get; set; }
    }
}
