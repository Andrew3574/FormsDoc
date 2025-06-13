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
    public class TagsRepository : BaseRepository<Tag>
    {
        public TagsRepository(FormsDbContext context) : base(context)
        {
        }

        public override Task Create(Tag entity)
        {
            throw new NotImplementedException();
        }

        public override Task Delete(Tag entity)
        {
            throw new NotImplementedException();
        }

        public override async Task<IEnumerable<Tag>?> GetAll()
        {
            return await _context.Tags.ToListAsync();
        }
        public async Task<IEnumerable<Tag>?> GetByName(string name)
        {
            return await _context.Tags.Where(t => EF.Functions.Like(t.Name, $"%{name}%")).ToListAsync();
        }

        public override async Task<Tag?> GetById(int id)
        {
            return await _context.Tags.FirstOrDefaultAsync(x => x.Id == id);
        }

        public override Task Update(Tag entity)
        {
            throw new NotImplementedException();
        }
    }
}
