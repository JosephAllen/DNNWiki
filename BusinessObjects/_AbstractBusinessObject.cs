using DotNetNuke.Data;
using DotNetNuke.Modules.Wiki.Interfaces;
using DotNetNuke.Modules.Wiki.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DotNetNuke.Modules.Wiki.BusinessObjects
{
    public abstract class _AbstractBusinessObject<T, I> : IBusinessObject<T, I> where T : class
    {
        internal IDataContext db;
        private readonly IRepository<T> rep;

        public _AbstractBusinessObject(IDataContext _context)
        {
            if (_context == null)
                throw new ArgumentNullException("Context");

            db = _context;
            rep = db.GetRepository<T>();
        }

        #region IbusinessObject<T> Members

        /// <summary>
        /// Based on the user permissions, filters the collection of T elements to return
        /// </summary>
        /// <param name="collection">collection of T elements</param>
        /// <returns>returns collection of T elements, filtered by the user access</returns>
        public virtual T FilterByAccess(T entity)
        {
            return entity;
        }

        /// <summary>
        /// Based on the user permissions, filters the collection of T elements to return
        /// </summary>
        /// <param name="collection">collection of T elements</param>
        /// <returns>returns collection of T elements, filtered by the user access</returns>
        public virtual IEnumerable<T> FilterByAccess(IEnumerable<T> collection)
        {
            return collection;
        }

        /// <summary>
        /// Called when a insert operation is going to be done against the database, so the entity
        /// to be created, can be changed before being submitted
        /// </summary>
        /// <param name="entity"></param>
        internal virtual void OnBeforeInsertOperation(T entity)
        {
        }

        /// <summary>
        /// Called when a insert operation is going to be done against the database, so the entity
        /// to be created, can be changed before being submitted
        /// </summary>
        /// <param name="entity"></param>
        internal virtual void OnBeforeUpdateOperation(T entity)
        {
        }

        /// <summary>
        /// Called when a insert operation is going to be done against the database, so the entity
        /// to be created, can be changed before being submitted
        /// </summary>
        /// <param name="entity"></param>
        internal virtual void OnBeforeDeleteOperation(T entity)
        {
        }

        /// <summary>
        /// Creates a new entity, but before creating it, parses it by calling the
        /// ParseUserAbleToInsert method
        /// </summary>
        /// <param name="entity">entity to create</param>
        /// <returns>returns the entity that was created</returns>
        public virtual T Add(T entity)
        {
            try
            {
                ParseUserAbleToInsert(entity);

                OnBeforeInsertOperation(entity);

                RepositoryAdd(ref entity);
            }
            catch (SqlException exc)
            {
                Entity_EvaluateSqlException(exc, SharedEnum.CrudOperation.Insert);
            }
            return entity;
        }

        /// <summary>
        /// Creates a new entity using the repository interface, this method should only be
        /// overriden if the insertion mechanism in the database has to be changed
        /// </summary>
        /// <param name="entity">entity to create</param>
        internal virtual void RepositoryAdd(ref T entity)
        {
            rep.Insert(entity);
        }

        /// <summary>
        /// Deletes a entity, but before deleting it, parses it by calling the ParseUserAbleToDelete
        /// method
        /// </summary>
        /// <param name="entity"></param>
        public virtual T Delete(T entity)
        {
            try
            {
                ParseUserAbleToDelete(entity);

                OnBeforeDeleteOperation(entity);

                RepositoryDelete(ref entity);
            }
            catch (SqlException exc)
            {
                Entity_EvaluateSqlException(exc, SharedEnum.CrudOperation.Insert);
            }
            return entity;
        }

        /// <summary>
        /// Deletes an entity using the repository interface, this method should only be overridden
        /// if the deletion mechanism in the database has to be changed
        /// </summary>
        /// <param name="entity">entity to delete</param>
        internal virtual void RepositoryDelete(ref T entity)
        {
            rep.Delete(entity);
        }

        /// <summary>
        /// Updates an entity, but before updating it, parses it by calling the
        /// ParseUserAbleToUpdate method
        /// </summary>
        /// <param name="entityCollection">entity to update</param>
        /// <param name="entity">returns the entity that was updated</param>
        public virtual T Update(T entity)
        {
            try
            {
                ParseUserAbleToUpdate(entity);

                OnBeforeUpdateOperation(entity);

                RepositoryUpdate(ref entity);
            }
            catch (SqlException exc)
            {
                Entity_EvaluateSqlException(exc, SharedEnum.CrudOperation.Update);
            }
            return entity;
        }

        /// <summary>
        /// Updates an entity using the repository interface, this method should only be overridden
        /// if the update mechanism in the database has to be changed
        /// </summary>
        /// <param name="entity">entity to delete</param>
        internal virtual void RepositoryUpdate(ref T entity)
        {
            rep.Update(entity);
        }

        /// <summary>
        /// Collects all entities
        /// </summary>
        /// <returns>returns collection of entities</returns>
        public virtual IEnumerable<T> GetAll()
        {
            return FilterByAccess(rep.Get());
        }

        /// <summary>
        /// Collects a specific entity based on a condition
        /// </summary>
        /// <param name="condition">condition to evaluate</param>
        /// <returns>returns a specific entity</returns>
        public virtual T Get(I id)
        {
            return FilterByAccess(rep.GetById<I>(id));
        }

        /// <summary>
        /// Method called when a sql operation happens after a crud operation
        /// </summary>
        /// <param name="objectStateEntryChangedCollection"></param>
        internal abstract void Entity_EvaluateSqlException(SqlException exc, SharedEnum.CrudOperation crudOperation);

        internal virtual void ParseUserAbleToInsert(T entity)
        {
        }

        internal virtual void ParseUserAbleToUpdate(T entity)
        {
        }

        internal virtual void ParseUserAbleToDelete(T entity)
        {
        }

        /// <summary>
        /// Filters for specific data in the database
        /// </summary>
        /// <param name="sql">sql query</param>
        /// <param name="args">data for the sql query</param>
        /// <returns>returns ienumerable of T items</returns>
        public IEnumerable<T> Find(string sql, params object[] args)
        {
            return this.db.ExecuteQuery<T>(CommandType.Text, sql, args);
        }

        #endregion IbusinessObject<T> Members
    }
}