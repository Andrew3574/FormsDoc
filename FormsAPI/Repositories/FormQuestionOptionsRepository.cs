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
    public class FormQuestionOptionsRepository : BaseRepository<FormQuestionOption>
    {
        public FormQuestionOptionsRepository(FormsDbContext context) : base(context)
        {
        }

        public override async Task Create(FormQuestionOption entity)
        {
            _context.FormQuestionOptions.Add(entity);
            await _context.SaveChangesAsync();
        }

        public override async Task Delete(FormQuestionOption entity)
        {
            _context.FormQuestionOptions.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public override Task<IEnumerable<FormQuestionOption>?> GetAll()
        {
            throw new NotImplementedException();
        }

        public override async Task<FormQuestionOption?> GetById(int id)
        {
            return await _context.FormQuestionOptions.FirstOrDefaultAsync(x => x.Id == id);
        }

        public override async Task Update(FormQuestionOption entity)
        {
            _context.FormQuestionOptions.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
