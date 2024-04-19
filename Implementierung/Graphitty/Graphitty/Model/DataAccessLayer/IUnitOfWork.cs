using Graphitty.Model.Filters;
using Graphitty.Model.Graphs;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace Graphitty.Model.DataAccessLayer
{
    public interface IUnitOfWork
    {
        #region Public Properties

        /// <summary>
        /// Offers CRUD operations for filters in the database.
        /// </summary>
        IRepository<FilterEntity> FilterRepository { get; }

        /// <summary>
        /// Offers CRUD operations for graphs in the database.
        /// </summary>
        IRepository<GraphEntity> GraphRepository { get; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Connects to the given Database and sets the context.
        /// If the Database does not exist this method will save the current context and additionally create the Database.
        /// </summary>
        /// <param name="Database">Name of the Database to connect to</param>
        void Connect(string database);

        /// <summary>
        /// Drops the database with the given name.
        /// </summary>
        /// <param name="database"></param>
        void DeleteDatabase(string database);

        /// <summary>
        /// Disposes the context.
        /// </summary>
        /// <see cref="Dispose(bool)"/>
        void Dispose();

        /// <summary>
        /// Returns whether the UnitOfWork is connected to a database.
        /// </summary>
        /// <returns></returns>
        bool IsConnected();

        /// <summary>
        /// Saves the current context.
        /// </summary>
        void Save();

        #endregion Public Methods

        /// <summary>
        /// List of all available databases of the current connection.
        /// </summary>
        List<string> AvailableDatabases { get; }

        /// <summary>
        /// The current connection string.
        /// </summary>
        MySqlConnectionStringBuilder ConnectionStringBuilder { get; }
    }
}