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
    public class ShortTextAnswersRepository : BaseRepository<ShortTextAnswer>
    {
        public ShortTextAnswersRepository(FormsDbContext context) : base(context)
        {
        }

        public override async Task Create(ShortTextAnswer entity)
        {
            _context.ShortTextAnswers.Add(entity);
            await _context.SaveChangesAsync();
        }

        public override async Task Delete(ShortTextAnswer entity)
        {
            _context.ShortTextAnswers.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public override Task<IEnumerable<ShortTextAnswer>?> GetAll()
        {
            throw new NotImplementedException();
        }

        public override async Task<ShortTextAnswer?> GetById(int id)
        {
            return await _context.ShortTextAnswers.FirstOrDefaultAsync(x => x.Id == id);
        }

        public override async Task Update(ShortTextAnswer entity)
        {
            _context.ShortTextAnswers.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
