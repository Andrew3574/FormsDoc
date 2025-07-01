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
            entity.Version++;
            _context.Forms.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Form>> GetByBatch(int batch, int batchSize = 20)
        {
            return await _context.Forms.Skip(batch * batchSize).Take(batchSize).ToListAsync();
        }

        public async Task<IEnumerable<Form>> FilterByBatch_FormTagId(int tagId, int batch, int batchSize = 20)
        {
            return await _context.Forms.Where(f => f.FormTags.Any(ft => ft.Id == tagId)).Skip(batch * batchSize).Take(batchSize).ToListAsync();
        }

        public async Task<IEnumerable<Form>> FilterByBatch_TopicId(int topicId, int batch, int batchSize = 20)
        {
            return await _context.Forms.Where(f => f.TopicId == topicId).Skip(batch * batchSize).Take(batchSize).ToListAsync();
        }

        public async Task<IEnumerable<Form>> FilterByBatch_Title(string title, int batch, int batchSize = 20)
        {
            return await _context.Forms.Where(f => EF.Functions.Like(f.Title, $"%{title}%")).Skip(batch * batchSize).Take(batchSize).ToListAsync();
        }

        public async Task<IEnumerable<Form>> FilterByUserId(int userId)
        {
            return await _context.Forms.Where(f => f.UserId == userId).ToListAsync();
        }
        /// <summary>
        /// elastic modif
        /// </summary>
        /// <returns></returns>
        public override async Task<IEnumerable<Form>?> GetAll()
        {
            return await _context.Forms.ToListAsync();
        }

        public override async Task<Form?> GetById(int id)
        {
            return await _context.Forms.FirstOrDefaultAsync(x => x.Id == id);
        }

    }
}
