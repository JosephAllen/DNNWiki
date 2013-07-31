using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Christoc.Modules.DNNWikiTestVersion.Components.Interfaces
{
    internal interface IBusinessObject<T, I> where T : class
    {
        void OnBeforeInsertOperation(T entity);

        void OnBeforeUpdateOperation(T entity);

        void OnBeforeDeleteOperation(T entity);

        T Add(T entity);

        void RepositoryAdd(ref T entity);

        T Delete(T entity);

        void RepositoryDelete(ref T entity);

        T Update(T entity);

        void RepositoryUpdate(ref T entity);

        IEnumerable<T> GetAll();

        T Get(I id);

        IEnumerable<T> Find(string sql, params object[] args);
    }
}