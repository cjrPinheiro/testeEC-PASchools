using PASchools.Domain.Entities;
using PASchools.Domain.Enum;
using PASchools.Google.Connector.Models;
using PASchools.Google.Connector.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PASchools.Application.DTOs
{
    public class SchoolDTO
    {
        public string? Name { get; set; }
        public bool? PublicDepartment { get; set; }
        public string EducationType { get; set; }
        public short Code { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? WebSite { get; set; }
        public bool ActiveOrganization { get; set; }
        public decimal Distance { get; set; }
        public AddressDTO? Address { get; set; }
        public string DistanceText { get; internal set; }
    }
}
