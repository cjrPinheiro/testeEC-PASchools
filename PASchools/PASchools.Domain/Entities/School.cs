using PASchools.Domain.Base;
using PASchools.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace PASchools.Domain.Entities
{
    public class School : BaseEntity
    {
        public string? Name { get; set; }
        public bool? PublicDepartment { get; set; }
        public string EducationType { get; set; }
        public short Code { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? WebSite { get; set; }
        public bool ActiveOrganization { get; set; }
        public int AddressId { get; set; }
        public Address Address { get; set; }
        public int InepCode { get; set; }
    }
}