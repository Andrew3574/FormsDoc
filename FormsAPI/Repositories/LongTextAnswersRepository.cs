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
    public class LongTextAnswersRepository : BaseRepository<LongTextAnswer>
    {
        public LongTextAnswersRepository(FormsDbContext context) : base(context)
        {
        }

        public override async Task Create(LongTextAnswer entity)
        {
            _context.LongTextAnswers.Add(entity);
            await _context.SaveChangesAsync();
        }

        public override async Task Delete(LongTextAnswer entity)
        {
            _context.LongTextAnswers.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public override Task<IEnumerable<LongTextAnswer>?> GetAll()
        {
            throw new NotImplementedException();
        }

        public override async Task<LongTextAnswer?> GetById(int id)
        {
            return await _context.LongTextAnswers.FirstOrDefaultAsync(x => x.Id == id);
        }

        public override async Task Update(LongTextAnswer entity)
        {
            _context.LongTextAnswers.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
