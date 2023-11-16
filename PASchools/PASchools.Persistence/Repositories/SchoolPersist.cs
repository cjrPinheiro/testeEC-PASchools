using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PASchools.Domain.Entities;
using PASchools.Persistence.Interfaces;
using PASchools.Persistence.Repositories.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PASchools.Persistence.Repositories
{
    public class SchoolPersist : BasePersist<School>, ISchoolPersist
    {
        public SchoolPersist(PASchoolsContext context) : base(context) { }

        public async Task<School[]> GetAllSchoolsAsync()
        {
            IQueryable<School> query = _context.Schools
                .Include(e => e.Address);

            query = query.OrderBy(e => e.Id);

            return await query.ToArrayAsync();
        }

        public async Task<School[]> GetAllSchoolsByStreetAsync(string street)
        {
            IQueryable<School> query = _context.Schools
                .Include(e => e.Address);

            query = query.OrderBy(e => e.Id).Where(q => q.Address.Street.ToLower().Contains(street.ToLower()));

            return await query.ToArrayAsync();
        }

        public async Task<School> GetSchoolsByInepIdAsync(int inepId)
        {
            IQueryable<School> query = _context.Schools
             .Include(e => e.Address);

            query = query.OrderBy(e => e.Id).Where(q => q.Code== inepId);

            return await query.FirstOrDefaultAsync();
        }
    }
}
