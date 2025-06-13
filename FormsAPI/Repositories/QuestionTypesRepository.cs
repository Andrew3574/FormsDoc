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
    public class QuestionTypesRepository : BaseRepository<QuestionType>
    {
        public QuestionTypesRepository(FormsDbContext context) : base(context)
        {
        }

        public override async Task Create(QuestionType entity)
        {
            _context.QuestionTypes.Add(entity);
            await _context.SaveChangesAsync();
        }

        public override async Task Delete(QuestionType entity)
        {
            _context.QuestionTypes.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public override async Task<IEnumerable<QuestionType>?> GetAll()
        {
            return await _context.QuestionTypes.ToListAsync();
        }

        public override async Task<QuestionType?> GetById(int id)
        {
            return await _context.QuestionTypes.FirstOrDefaultAsync(x => x.Id == id);
        }

        public override async Task Update(QuestionType entity)
        {
            _context.QuestionTypes.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
