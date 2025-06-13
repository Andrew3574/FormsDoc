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
    public class CheckBoxesRepository : BaseRepository<CheckboxAnswer>
    {
        public CheckBoxesRepository(FormsDbContext context) : base(context)
        {
        }

        public override async Task Create(CheckboxAnswer entity)
        {
            _context.CheckboxAnswers.Add(entity);
            await _context.SaveChangesAsync();
        }

        public override async Task Delete(CheckboxAnswer entity)
        {
            _context.CheckboxAnswers.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public override Task<IEnumerable<CheckboxAnswer>?> GetAll()
        {
            throw new NotImplementedException();
        }

        public override Task<CheckboxAnswer?> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<CheckboxAnswer>> GetByQuestionId(int id)
        {
            return await _context.CheckboxAnswers.Where(a => a.FormQuestionId == id).ToListAsync();
        }

        public override async Task Update(CheckboxAnswer entity)
        {
            _context.CheckboxAnswers.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
