using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Graphitty.Model.DataAccessLayer
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        #region Private Fields

        private DbContext context;
        private DbSet<TEntity> dbSet;

        #endregion Private Fields

        #region Public Constructors

        public Repository(DbContext context)
        {
            this.context = context;
            dbSet = context.Set<TEntity>();
        }

        #endregion Public Constructors

        #region CRUD

        public void Create(TEntity entity)
        {
            dbSet.Add(entity);
        }

        public void Delete(TEntity entity)
        {
            if (context.Entry(entity).State == EntityState.Detached)
            {
                dbSet.Attach(entity);
            }
            dbSet.Remove(entity);
        }

        public void Delete(object id)
        {
            TEntity toDelete = dbSet.Find(id);
            Delete(toDelete);
        }

        public void Update(TEntity entity)
        {
            dbSet.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
        }

        #endregion CRUD

        #region Get

        /// <summary>
        /// Returns true if a Entity matching the filter criteria exists.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="filter">Specifies the filter criteria, the default value null means that no filtering is wished.</param>
        /// <returns></returns>
        public virtual bool Exists(Expression<Func<TEntity, bool>> filter = null)
        {
            return Get(filter).Any();
        }

        /// <summary>
        /// Fetches all Entities matching the filter criteria, which should be passed as a lambda.
        /// Also allows ordering and and a number of Entities which should be skipped
        /// and an number of Entities which should be taken.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="filter">Specifies the filter criteria, the default value null means that no filtering is wished.</param>
        /// <param name="orderBy">Specifies the ordering, the default value null means that no ordering is wished.</param>
        /// <param name="skip">>Number of elements which should be skipped.</param>
        /// <param name="take">Number of elements which should be taken.</param>
        /// <returns></returns>
        public virtual IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null,
                                          Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                          int? skip = null,
                                          int? take = null)
        {
            IQueryable<TEntity> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            if (skip.HasValue)
            {
                query = query.Skip(skip.Value);
            }
            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }
            return query;
        }

        /// <summary>
        /// Fetches all Entities ordered by the lambda which takes a queryable Entity and returns an ordered queryable Entity.
        /// E.g. for all Graphs ordered by number of edges, just pass: (graph => graph.OrderBy(g => g.NumEdges))
        /// Also allowsto choose a number of Entities which should be skipped
        /// and an number of Entities which should be taken.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="orderBy">Specifies the ordering, the default value null means that no ordering is wished.</param>
        /// <param name="skip">Number of elements which should be skipped.</param>
        /// <param name="take">Number of elements which should be taken.</param>
        /// <returns></returns>
        public virtual IEnumerable<TEntity> GetAll(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                             int? skip = null,
                                             int? take = null)
        {
            return Get(null, orderBy, skip, take);
        }

        /// <summary>
        /// Fetches the Entity matching the given ID.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="id">The wanted ID.</param>
        /// <returns></returns>
        public virtual TEntity GetById(object id)
        {
            return dbSet.Find(id);
        }

        /// <summary>
        /// Returns the number of Entities matching the criteria.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="filter">Specifies the filter criteria, the default value null means that no filtering is wished.</param>
        /// <returns></returns>
        public virtual int GetCount(Expression<Func<TEntity, bool>> filter = null)
        {
            return Get(filter).Count();
        }

        /// <summary>
        /// Fetches one Entity matching the filter criteria.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="filter">Specifies the filter criteria, the default value null means that no filtering is wished.</param>
        /// <returns></returns>
        public virtual TEntity GetOne(Expression<Func<TEntity, bool>> filter = null)
        {
            return Get(filter).SingleOrDefault();
        }

        #endregion Get
    }
}