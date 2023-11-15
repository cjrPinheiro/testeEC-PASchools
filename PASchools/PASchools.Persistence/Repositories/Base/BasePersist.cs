using PASchools.Persistence.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PASchools.Persistence.Repositories.Base
{
    public class BasePersist<R> : IBasePersist<R> where R : class
    {
        protected readonly PASchoolsContext _context;

        public BasePersist(PASchoolsContext context)
        {
            this._context = context;
        }

        public async Task AddAsync(R entity)
        {
            await _context.AddAsync(entity);
        }

        public void Update(R entity)
        {
            _context.Update(entity);
        }

        public void Delete(R entity)
        {
            _context.Remove(entity);
        }

        public void DeleteRange(R[] entityArray)
        {
            _context.RemoveRange(entityArray);
        }
        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
