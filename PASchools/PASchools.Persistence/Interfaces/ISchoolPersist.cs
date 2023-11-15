using PASchools.Domain.Entities;
using PASchools.Persistence.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PASchools.Persistence.Interfaces
{
    public interface ISchoolPersist : IBasePersist<School>
    {
        Task<School[]> GetAllSchoolsAsync();
        Task<School[]> GetAllSchoolsByStreetAsync(string street);
        Task<School> GetSchoolsByInepIdAsync(int inepId);
    }
}
