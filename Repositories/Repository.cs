using System.Linq.Expressions;
using LetterboxNetCore.Models;
using LetterboxNetCore.Repositories.Database;
using LetterboxNetCore.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LetterboxNetCore.Repositories
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly ApplicationDbContext _context;

        public Repository(ApplicationDbContext context)
        {
            this._context = context;
        }

        protected ApplicationDbContext context { get { return this._context; } }

        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public async Task<bool> Exists(int id)
        {
            var exists = await _context.Set<T>().AnyAsync(x => x.Id == id);
            return exists;
        }

        public async Task<T?> Get(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> where)
        {
            return _context.Set<T>().Where(where);
        }

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }
    }
}