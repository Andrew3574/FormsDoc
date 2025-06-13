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
    public class IntegerAnswersRepository : BaseRepository<IntegerAnswer>
    {
        public IntegerAnswersRepository(FormsDbContext context) : base(context)
        {
        }

        public override async Task Create(IntegerAnswer entity)
        {
            _context.IntegerAnswers.Add(entity);
            await _context.SaveChangesAsync();
        }

        public override async Task Delete(IntegerAnswer entity)
        {
            _context.IntegerAnswers.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public override Task<IEnumerable<IntegerAnswer>?> GetAll()
        {
            throw new NotImplementedException();
        }

        public override async Task<IntegerAnswer?> GetById(int id)
        {
            return await _context.IntegerAnswers.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<IntegerAnswer>> GetByQuestionId(int id)
        {
            return await _context.IntegerAnswers.Where(a=>a.FormQuestionId==id).ToListAsync();
        }

        public override async Task Update(IntegerAnswer entity)
        {
            _context.IntegerAnswers.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
