using Repositories.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public abstract class BaseRepository<T> where T : class
    {
        protected FormsDbContext _context;

        public BaseRepository(FormsDbContext context)
        {
            _context = context;
        }
        public abstract Task Create(T entity);
        public abstract Task Update(T entity);
        public abstract Task Delete(T entity);
        public abstract Task<T?> GetById(int id);
        public abstract Task<IEnumerable<T>?> GetAll();
    }
}
