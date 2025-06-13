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
    public class TopicsRepository : BaseRepository<Topic>
    {
        public TopicsRepository(FormsDbContext context) : base(context)
        {
        }

        public override Task Create(Topic entity)
        {
            throw new NotImplementedException();
        }

        public override Task Delete(Topic entity)
        {
            throw new NotImplementedException();
        }

        public override async Task<IEnumerable<Topic>?> GetAll()
        {
            return await _context.Topics.ToListAsync();
        }

        public override async Task<Topic?> GetById(int id)
        {
            return await _context.Topics.FirstOrDefaultAsync(u=>u.Id==id);
        }

        public override Task Update(Topic entity)
        {
            throw new NotImplementedException();
        }
    }
}
