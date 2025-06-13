using Microsoft.EntityFrameworkCore;
using Models;
using Repositories.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class AccessFormUsersRepository : BaseRepository<AccessformUser>
    {
        public AccessFormUsersRepository(FormsDbContext context) : base(context)
        {
        }

        public override async Task Create(AccessformUser entity)
        {
            _context.AccessformUsers.Add(entity);
            await _context.SaveChangesAsync();
        }

        public override async Task Delete(AccessformUser entity)
        {
            _context.AccessformUsers.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public override Task<IEnumerable<AccessformUser>?> GetAll()
        {
            throw new NotImplementedException();
        }

        public override async Task<AccessformUser?> GetById(int id)
        {
            return await _context.AccessformUsers.FirstOrDefaultAsync(a=>a.Id==id);
        }

        public override Task Update(AccessformUser entity)
        {
            throw new NotImplementedException();
        }
    }
}
