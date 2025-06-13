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

        public async Task<bool> isLiked(int formId, int userid)
        {
            return await _context.Likes.ContainsAsync(new Like{FormId=formId,UserId=userid});
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
