using Graphitty.Migrations;
using Graphitty.Model.Filters;
using Graphitty.Model.Graphs;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;

namespace Graphitty.Model.DataAccessLayer
{
    /// <summary>
    /// Manages the current DbContext and the repositories.
    /// Implements the singleton pattern.
    /// </summary>
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        #region Private Fields

        private static UnitOfWork instance;
        private MySqlConnectionStringBuilder connectionStringBuilder;
        private GraphittyContext context = new GraphittyContext();
        private bool disposed = false;
        private IRepository<FilterEntity> filterRepository;
        private IRepository<GraphEntity> graphRepository;

        #endregion Private Fields

        #region Private Constructors

        private UnitOfWork()
        {
            filterRepository = new Repository<FilterEntity>(context);
            graphRepository = new Repository<GraphEntity>(context);
            connectionStringBuilder = new MySqlConnectionStringBuilder
            {
                UserID = "root",
                Server = "localhost",
                Password = string.Empty,
                Database = "graphitty"
            };
        }

        #endregion Private Constructors

        #region Public Properties

        /// <summary>
        /// Standartized database name, which is used when a new database is created.
        /// </summary>
        public static string DatabasePrefix = "graphitty_numvertices_";

        /// <summary>
        /// Provides the singletorn instance reference of the UnitOfWork.
        /// </summary>
        public static UnitOfWork Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new UnitOfWork();
                }
                return instance;
            }
        }

        /// <summary>
        /// List of all available databases of the current connection.
        /// </summary>
        public List<string> AvailableDatabases
        {
            get
            {
                List<string> res = new List<string>();
                MySqlConnection connection = new MySqlConnection(ConnectionStringBuilder.ConnectionString);
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "show databases;";
                try
                {
                    connection.Open();
                }
                catch (Exception e)
                {
                    connection.Clone();
                    return res;
                }

                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string row = "";
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        row += reader.GetValue(i).ToString();
                    }
                    if (row.Contains(DatabasePrefix))
                    {
                        res.Add(row);
                    }
                }
                connection.Close();
                return res;
            }
        }

        /// <summary>
        /// The current connection string.
        /// </summary>
        public MySqlConnectionStringBuilder ConnectionStringBuilder
        {
            get => connectionStringBuilder;
        }

        /// <summary>
        /// Offers CRUD operations for filters in the database.
        /// </summary>
        public IRepository<FilterEntity> FilterRepository
        {
            get => filterRepository;
            private set => filterRepository = value;
        }

        /// <summary>
        /// Offers CRUD operations for graphs in the database.
        /// </summary>
        public IRepository<GraphEntity> GraphRepository
        {
            get => graphRepository;
            private set => graphRepository = value;
        }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Connects to the given Database and sets the context.
        /// If the Database does not exist this method will save the current context and additionally create the Database.
        /// </summary>
        /// <param name="Database">Name of the Database to connect to</param>
        public void Connect(string database)
        {
            if (ConnectionStringBuilder.Database != database || ConnectionStringBuilder.ConnectionString != context.Database.Connection.ConnectionString)
            {
                Save();

                //open connection and create if not exists
                MySqlConnection connection = new MySqlConnection(ConnectionStringBuilder.ConnectionString);
                MySqlCommand command = connection.CreateCommand();
                connection.Open();
                command.CommandText = "create database if not exists " + database + ";";
                command.ExecuteNonQuery();

                //change the current database and setup the schema
                ConnectionStringBuilder.Database = database;
                var configuration = new Configuration
                {
                    TargetDatabase = new DbConnectionInfo(ConnectionStringBuilder.ConnectionString, "MySql.Data.MySqlClient")
                };
                var migrator = new DbMigrator(configuration);
                migrator.Update();
                connection.Close();

                //update the context and repositories
                context = new GraphittyContext(ConnectionStringBuilder.ConnectionString);
                GraphRepository = new Repository<GraphEntity>(context);
                FilterRepository = new Repository<FilterEntity>(context);
            }
        }

        /// <summary>
        /// Drops the database with the given name.
        /// </summary>
        /// <param name="database"></param>
        public void DeleteDatabase(string database)
        {
            if (ConnectionStringBuilder.Database != database)
            {
                string currentDatabase = ConnectionStringBuilder.Database;
                ConnectionStringBuilder.Database = null;
                MySqlConnection connection = new MySqlConnection(ConnectionStringBuilder.ConnectionString);
                MySqlCommand command = connection.CreateCommand();
                connection.Open();
                command.CommandText = "drop database " + database + ";";
                command.ExecuteNonQuery();
                connection.Close();
                ConnectionStringBuilder.Database = currentDatabase;
            }
        }

        /// <summary>
        /// Disposes the context.
        /// </summary>
        /// <see cref="Dispose(bool)"/>
        public void Dispose()
        {
            Dispose(true);
            //Garbage Collector wird aufgefordert, dass die Unit of Work nicht im Finalizer aufgerufen wird.
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Returns whether the UnitOfWork is connected to a database.
        /// </summary>
        /// <returns></returns>
        public bool IsConnected()
        {
            MySqlConnection connection = new MySqlConnection(ConnectionStringBuilder.ConnectionString);
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "show databases;";
            try
            {
                connection.Open();
            }
            catch (Exception e)
            {
                connection.Clone();
                return false;
            }
            connection.Close();
            return true;
        }

        /// <summary>
        /// Saves the current context.
        /// </summary>
        public void Save()
        {
            context.SaveChanges();
        }

        #endregion Public Methods

        #region Protected Methods

        /// <summary>
        /// Disposes the context if it's not already disposed.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                context.Dispose();
            }
            disposed = true;
        }

        #endregion Protected Methods
    }
}