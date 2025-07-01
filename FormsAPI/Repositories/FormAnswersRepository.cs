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
    public class FormAnswersRepository : BaseRepository<FormAnswer>
    {
        public FormAnswersRepository(FormsDbContext context) : base(context)
        {
        }

        public override async Task Create(FormAnswer entity)
        {
            try
            {
                _context.FormAnswers.Add(entity);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException pgEx)
            {
                throw new DbUpdateException("You can have only one answer for each form. You can edit existing answers in your account manager.");
            }
        }        

        public override async Task Delete(FormAnswer entity)
        {
            _context.FormAnswers.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public override Task<IEnumerable<FormAnswer>?> GetAll()
        {
            throw new NotImplementedException();
        }

        public override async Task<FormAnswer?> GetById(int id)
        {
            return await _context.FormAnswers.FirstOrDefaultAsync(f=>f.Id == id);
        }

        public override async Task Update(FormAnswer entity)
        {
            _context.FormAnswers.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<FormAnswer>> FilterByUserId(int userId)
        {
            return await _context.FormAnswers.Where(f => f.UserId == userId).ToListAsync();
        }
        public async Task<IEnumerable<FormAnswer>?> GetByFormId(int formId)
        {
            return await _context.FormAnswers.Where(f => f.FormId == formId).ToListAsync();
        }

        public async Task<FormAnswer?> GetByUserId_FormId(int userId,int formId)
        {
            return await _context.FormAnswers.FirstOrDefaultAsync(f => f.UserId == userId && f.FormId == formId);
        }
    }
}
