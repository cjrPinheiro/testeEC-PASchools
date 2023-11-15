using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PASchools.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PASchools.Persistence
{
    public class PASchoolsContext : DbContext
    {
        public PASchoolsContext(DbContextOptions<PASchoolsContext> dbContextOptions) : base(dbContextOptions)
        {
        }

        public DbSet<School> Schools { get; set; }
        public DbSet<Address> Addresses { get; set; }
    }
}
