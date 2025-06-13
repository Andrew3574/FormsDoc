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
    public class FormsRepository : BaseRepository<Form>
    {
        public FormsRepository(FormsDbContext context) : base(context)
        {
        }

        public override async Task Create(Form entity)
        {
            _context.Forms.Add(entity);
            await _context.SaveChangesAsync();
        }

        public override async Task Delete(Form entity)
        {
            _context.Forms.Remove(entity);
            await _context.SaveChangesAsync();
        }
        public override async Task Update(Form entity)
        {
            _context.Forms.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Form>> GetByBatch(int batch, int batchSize = 20)
        {
            return await _context.Forms.Skip(batch * batchSize).Take(batchSize).ToListAsync();
        }

        public async Task<IEnumerable<Form>> GetByBatch_FormTagId(int id, int batch, int batchSize = 20)
        {
            return await _context.Forms.Skip(batch * batchSize).Where(f=>f.FormTags.Any(ft=>ft.Id==id)).Take(batchSize).ToListAsync();
        }

        public async Task<IEnumerable<Form>> GetByBatch_TopicId(int id, int batch, int batchSize = 20)
        {
            return await _context.Forms.Skip(batch * batchSize).Where(f => f.TopicId== id).Take(batchSize).ToListAsync();
        }

        public async Task<IEnumerable<Form>> GetByBatch_Title(string title, int batch, int batchSize = 20)
        {
            return await _context.Forms.Skip(batch * batchSize).Where(f=>EF.Functions.Like(f.Title,$"%{title}%")).Take(batchSize).ToListAsync();
        }

        public override Task<IEnumerable<Form>?> GetAll()
        {
            throw new NotImplementedException();
        }

        public override async Task<Form?> GetById(int id)
        {
            return await _context.Forms.FirstOrDefaultAsync(x => x.Id == id);
        }

    }
}
