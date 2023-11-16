using PASchools.Domain.Base;

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