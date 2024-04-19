using Graphitty.Model.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GraphittyTest.Model.DataAccessLayer
{
    class RepositoryMock<TEntity> : IRepository<TEntity> where TEntity : class
    {
        #region Private Fields

        private List<TEntity> mockCollection;

        #endregion Private Fields

        #region Public Constructors

        public RepositoryMock()
        {
            MockCollection = new List<TEntity>();
        }

        public RepositoryMock(List<TEntity> collection)
        {
            mockCollection = collection;
        }

        #endregion Public Constructors

        #region Public Properties

        public List<TEntity> MockCollection
        {
            get => mockCollection;
            set => mockCollection = value;
        }

        #endregion Public Properties

        #region Public Methods

        public void Create(TEntity entity)
        {
            if (mockCollection.Contains(entity))
            {
                return;
            }
            mockCollection.Add(entity);
        }

        public void Delete(TEntity entity)
        {
            if (mockCollection.Contains(entity))
            {
                mockCollection.Remove(entity);
            }
        }

        public void Delete(object id)
        {
            mockCollection.RemoveAt((int)id - 1);
        }

        public bool Exists(System.Linq.Expressions.Expression<Func<TEntity, bool>> filter)
        {
            return Get(filter).Any();
        }

        public IEnumerable<TEntity> Get(System.Linq.Expressions.Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, int? skip = null, int? take = null)
        {
            IQueryable<TEntity> query = mockCollection.AsQueryable();
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

        public IEnumerable<TEntity> GetAll(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, int? skip = null, int? take = null)
        {
            return mockCollection.AsEnumerable<TEntity>();
        }

        public TEntity GetById(object id)
        {
            throw new NotImplementedException();
        }

        public int GetCount(System.Linq.Expressions.Expression<Func<TEntity, bool>> filter = null)
        {
            return mockCollection.Count;
        }

        public TEntity GetOne(System.Linq.Expressions.Expression<Func<TEntity, bool>> filter = null)
        {
            return Get(filter).SingleOrDefault();
        }

        public void Update(TEntity entity)
        {
        }

        #endregion Public Methods
    }
}