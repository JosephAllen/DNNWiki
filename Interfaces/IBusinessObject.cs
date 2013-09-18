using System.Collections.Generic;

namespace DotNetNuke.Wiki.Interfaces
{
    public interface IBusinessObject<T, I> where T : class
    {
        T Add(T entity);

        T Delete(T entity);

        T Update(T entity);

        IEnumerable<T> GetAll();

        T Get(I id);

        IEnumerable<T> Find(string sql, params object[] args);
    }
}