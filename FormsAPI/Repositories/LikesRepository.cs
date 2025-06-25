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
    public class LikesRepository : BaseRepository<Like>
    {
        public LikesRepository(FormsDbContext context) : base(context)
        {
        }

        public override async Task Create(Like entity)
        {
            _context.Likes.Add(entity);
            await _context.SaveChangesAsync();
        }

        public override async Task Delete(Like entity)
        {
            _context.Likes.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<Like?> isLiked(int formId, int userid)
        {             
            return await _context.Likes.FirstOrDefaultAsync(l => l.FormId == formId && l.UserId == userid);
        }

        public async Task<IEnumerable<Like>?> GetLikesByFormId(int formId)
        {
            return await _context.Likes.Where(l=>l.FormId==formId).ToListAsync();
        }

        public override Task<IEnumerable<Like>?> GetAll()
        {
            throw new NotImplementedException();
        }

        public override Task<Like?> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public override Task Update(Like entity)
        {
            throw new NotImplementedException();
        }
    }
}
