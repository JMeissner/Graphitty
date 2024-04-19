using Graphitty.Model.DataAccessLayer;
using Graphitty.Model.Filters;
using Graphitty.Model.Graphs;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace GraphittyTest.Model.DataAccessLayer
{
    public class UnitOfWorkMock : IUnitOfWork
    {
        #region Private Fields

        private MySqlConnectionStringBuilder connectionStringBuilderMock = new MySqlConnectionStringBuilder();
        private IRepository<FilterEntity> filterRepository;

        private IRepository<GraphEntity> graphRepository;

        #endregion Private Fields

        #region Public Methods

        public void Connect(string database)
        {
            connectionStringBuilderMock.Database = database;
            if (!AvailableDatabases.Exists(s => s == database))
            {
                AvailableDatabases.Add(database);
            }
        }

        public void DeleteDatabase(string database)
        {
            AvailableDatabases.Remove(database);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public bool IsConnected()
        {
            return true;
        }

        public void Save()
        {
        }

        #endregion Public Methods

        #region Public Properties

        public List<string> AvailableDatabases
        {
            get => new List<string> { "MockDatabase" };
        }

        public MySqlConnectionStringBuilder ConnectionStringBuilder
        {
            get => connectionStringBuilderMock;
        }

        public IRepository<FilterEntity> FilterRepository
        {
            get
            {
                return filterRepository;
            }
        }

        public IRepository<GraphEntity> GraphRepository
        {
            get
            {
                return graphRepository;
            }
        }

        #endregion Public Properties

        #region Public Constructors

        /// <summary>
        /// Instantiates the UnitOfWork with a RepositoryMock for GraphEntity and FilterEntity.
        /// </summary>
        public UnitOfWorkMock()
        {
            graphRepository = new RepositoryMock<GraphEntity>();
            filterRepository = new RepositoryMock<FilterEntity>();
        }

        public UnitOfWorkMock(IRepository<GraphEntity> graphRepository, IRepository<FilterEntity> filterRepository)
        {
            this.graphRepository = graphRepository;
            this.filterRepository = filterRepository;
        }

        #endregion Public Constructors
    }
}