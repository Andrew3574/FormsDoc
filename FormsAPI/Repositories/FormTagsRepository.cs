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
    public class FormTagsRepository : BaseRepository<FormTag>
    {
        public FormTagsRepository(FormsDbContext context) : base(context)
        {
        }

        public override async Task Create(FormTag entity)
        {
            _context.FormTags.Add(entity);
            await _context.SaveChangesAsync();
        }

        public override async Task Delete(FormTag entity)
        {
            _context.FormTags.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<FormTag>> GetByFormId(int formId)
        {
            return await _context.FormTags.Where(t=>t.FormId==formId).ToListAsync();
        }

        public override Task<IEnumerable<FormTag>?> GetAll()
        {
            throw new NotImplementedException();
        }

        public override async Task<FormTag?> GetById(int id)
        {
            return await _context.FormTags.FirstOrDefaultAsync(x => x.Id == id);
        }

        public override Task Update(FormTag entity)
        {
            throw new NotImplementedException();
        }
    }
}
