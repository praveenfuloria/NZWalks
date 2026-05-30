using System.Linq.Expressions;

namespace NZWalks.API.Repositories
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll(string? includeproperty = null);
        T GetFirstOrDefault(Expression<Func<T, bool>> filter, string? includeproperty = null);

        void Add(T entity);

        void Remove(T entity);

        void RemoveRange(IEnumerable<T> entity);
    }
}
