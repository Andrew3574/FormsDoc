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
    public class FormQuestionRepository : BaseRepository<FormQuestion>
    {
        public FormQuestionRepository(FormsDbContext context) : base(context)
        {
        }

        public override async Task Create(FormQuestion entity)
        {
            _context.FormQuestions.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task CreateRange(IEnumerable<FormQuestion> entities)
        {
            _context.FormQuestions.AddRange(entities);
            await _context.SaveChangesAsync();
        }

        public override async Task Delete(FormQuestion entity)
        {
            _context.FormQuestions.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public override Task<IEnumerable<FormQuestion>?> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<FormQuestion>> GetByFormId(int id)
        {
            return await _context.FormQuestions.Where(f => f.FormId==id).ToListAsync();
        }

        public override Task<FormQuestion?> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void MarkDelete(IEnumerable<FormQuestion> entities)
        {
            _context.FormQuestions.RemoveRange(entities);
        }

        public override async Task Update(FormQuestion entity)
        {
            _context.FormQuestions.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateRange(IEnumerable<FormQuestion> entities)
        {
            _context.FormQuestions.UpdateRange(entities);
            await _context.SaveChangesAsync();
        }
    }
}
