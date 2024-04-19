using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Graphitty.Model.DataAccessLayer
{
    public interface IRepository<TEntity>
    {
        #region CRUD

        void Create(TEntity entity);

        void Delete(TEntity entity);

        void Delete(object id);

        void Update(TEntity entity);

        #endregion CRUD

        #region Get

        /// <summary>
        /// Returns true if a Entity matching the filter criteria exists.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="filter">Specifies the filter criteria, the default value null means that no filtering is wished.</param>
        /// <returns></returns>
        bool Exists(Expression<Func<TEntity, bool>> filter);

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
        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null,
                                          Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                          int? skip = null,
                                          int? take = null);

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
        IEnumerable<TEntity> GetAll(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                            int? skip = null,
                                            int? take = null);

        /// <summary>
        /// Fetches the Entity matching the given ID.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="id">The wanted ID.</param>
        /// <returns></returns>
        TEntity GetById(object id);

        /// <summary>
        /// Returns the number of Entities matching the criteria.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="filter">Specifies the filter criteria, the default value null means that no filtering is wished.</param>
        /// <returns></returns>
        int GetCount(Expression<Func<TEntity, bool>> filter = null);

        /// <summary>
        /// Fetches one Entity matching the filter criteria.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="filter">Specifies the filter criteria, the default value null means that no filtering is wished.</param>
        /// <returns></returns>
        TEntity GetOne(Expression<Func<TEntity, bool>> filter = null);

        #endregion Get
    }
}