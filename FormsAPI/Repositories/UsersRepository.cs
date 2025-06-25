using Microsoft.EntityFrameworkCore;
using Models;
using Models.Enums;
using Repositories.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class UsersRepository : BaseRepository<User>
    {
        public UsersRepository(FormsDbContext context) : base(context)
        {
        }

        public override async Task Create(User entity)
        {
            _context.Users.Add(entity);
            await _context.SaveChangesAsync();
        }

        public override async Task Delete(User entity)
        {
            _context.Users.Remove(entity);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteRange(IEnumerable<User> entities)
        {
            _context.Users.RemoveRange(entities);
            await _context.SaveChangesAsync();
        }

        public override Task<IEnumerable<User>?> GetAll()
        {
            throw new NotImplementedException();
        }

        public override async Task<User?> GetById(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(u=>u.Id==id);
        }
        public async Task<IEnumerable<User>?> GetById(IEnumerable<int> indexes)
        {
            return await _context.Users.Where(u => indexes.Contains(u.Id)).ToListAsync();
        }

        public async Task<IEnumerable<User>?> FilterByEmail(string email)
        {
            return await _context.Users.Where(u=>EF.Functions.Like(u.Email,$"%{email}%")).Take(5).ToListAsync();
        }

        public async Task<User?> GetByEmail(string? email)
        {
            if (!string.IsNullOrEmpty(email))
            {
                return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            }
            return null;
        }

        public async Task<IEnumerable<User>?> GetByEmail(IEnumerable<string> emails)
        {
            return await _context.Users.AsNoTracking().Where(u => emails.Contains(u.Email)).ToListAsync();
        }

        public async Task<IEnumerable<User>?> GetByBatch(int batch,int batchSize=20)
        {
            return await _context.Users.Skip(batch*batchSize).Take(batchSize).ToListAsync();
        }

        public override async Task Update(User entity)
        {
            _context.Users.Update(entity);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateRange(IEnumerable<User> entities)
        {
            _context.Users.UpdateRange(entities);
            await _context.SaveChangesAsync();
        }
    }
}
