using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PASchools.Domain.Models
{
    public class Settings
    {
        public ApiSettings GoogleApiSettings { get; set; }
        public ApiSettings SIEApiSettings { get; set; }
    }

    public class ApiSettings {
        public string Key { get; set; }
        public string BaseUri { get; set; }
    }
}
