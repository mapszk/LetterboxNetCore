using LetterboxNetCore.Models;

namespace LetterboxNetCore.Repositories.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<T?> Get(int id);
        Task<IEnumerable<T>> GetAll();
        void Add(T entity);
        void Delete(T entity);
        void Update(T entity);
        Task<bool> Exists(int id);
    }
}