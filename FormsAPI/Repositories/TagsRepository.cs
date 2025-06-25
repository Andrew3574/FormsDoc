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
    public class TagsRepository : BaseRepository<Tag>
    {
        public TagsRepository(FormsDbContext context) : base(context)
        {
        }

        public override Task Create(Tag entity)
        {
            throw new NotImplementedException();
        }

        public override Task Delete(Tag entity)
        {
            throw new NotImplementedException();
        }

        public override async Task<IEnumerable<Tag>?> GetAll()
        {
            return await _context.Tags.ToListAsync();
        }
        public async Task<IEnumerable<Tag>?> FilterByName(string name)
        {
            return await _context.Tags.Where(t => EF.Functions.Like(t.Name, $"%{name}%")).Take(5).ToListAsync();
        }

        public async Task<IEnumerable<Tag>> GetOrCreateByName(IEnumerable<string> tags)
        {
            var existingTags = _context.Tags.Where(t => tags.Contains(t.Name));
            if(existingTags.Count() != tags.Count())
            {
                var notExistingTags = GetNotExistingTags(existingTags, tags);
                _context.Tags.AddRange(notExistingTags);
                await _context.SaveChangesAsync();
                existingTags.Concat(notExistingTags);
            }
            return existingTags;
        }

        public override async Task<Tag?> GetById(int id)
        {
            return await _context.Tags.FirstOrDefaultAsync(x => x.Id == id);
        }

        public override Task Update(Tag entity)
        {
            throw new NotImplementedException();
        }

        private IEnumerable<Tag> GetNotExistingTags(IEnumerable<Tag> existingTags, IEnumerable<string> tags)
        {
            var notExistingTagNames = tags.Except(existingTags.Select(et => et.Name));
            return notExistingTagNames.Select(t => new Tag { Name = t });
        }

    }
}
