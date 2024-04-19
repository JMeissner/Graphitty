using System;
using System.Collections.Generic;
using Graphitty.Model.DataAccessLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;

namespace GraphittyTest
{
    [TestClass]
    public class UnitOfWorkTest
    {
        #region Private Fields

        private MySqlConnectionStringBuilder connectionStringBuilder;
        private List<string> createdDatabases;
        private string databaseBaseName;
        private UnitOfWork unitOfWork;

        #endregion Private Fields

        #region Private Properties

        private List<string> availableDatabases
        {
            get
            {
                List<string> res = new List<string>();
                connectionStringBuilder.UserID = "root";
                connectionStringBuilder.Server = "localhost";
                MySqlConnection connection = new MySqlConnection(connectionStringBuilder.ConnectionString);
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "show databases;";
                connection.Open();

                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string row = "";
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        row += reader.GetValue(i).ToString();
                    }
                    if (row.Contains(databaseBaseName))
                    {
                        res.Add(row);
                    }
                }
                connection.Close();
                return res;
            }
        }

        #endregion Private Properties

        #region Public Methods

        [TestCleanup]
        public void CleanUp()
        {
            unitOfWork.ConnectionStringBuilder.Database = "";
            foreach (var database in createdDatabases)
            {
                unitOfWork.DeleteDatabase(database);
            }
            unitOfWork = null;
            createdDatabases = null;
        }

        [TestMethod]
        public void ConnectDatabaseTest()
        {
            //act
            unitOfWork.Connect(databaseBaseName + "_60");
            createdDatabases.Add(databaseBaseName + "_60");
            unitOfWork.Connect(databaseBaseName + "_50");
            unitOfWork.Connect(databaseBaseName + "_50");
            createdDatabases.Add(databaseBaseName + "_50");

            //assert
            Assert.IsTrue(availableDatabases.Contains(databaseBaseName + "_60"));
            Assert.IsTrue(availableDatabases.Contains(databaseBaseName + "_50"));
        }

        [TestMethod]
        public void DeleteDatabaseTest()
        {
            unitOfWork.Connect(databaseBaseName + "_60");
            unitOfWork.Connect(databaseBaseName + "_50");

            //act
            unitOfWork.DeleteDatabase(databaseBaseName + "_60");
            unitOfWork.DeleteDatabase(databaseBaseName + "_50");

            //assert
            Assert.IsFalse(availableDatabases.Contains(databaseBaseName + "_60"));
            //has to check if true because the currently connected database shouldn't be deleted
            Assert.IsTrue(availableDatabases.Contains(databaseBaseName + "_50"));
        }

        [TestMethod]
        public void DisposeContextTest()
        {
            unitOfWork.Dispose();
            bool errorOnAccess = false;
            try
            {
                unitOfWork.GraphRepository.Create(new Graphitty.Model.Graphs.GraphEntity());
            }
            catch (Exception e)
            {
                errorOnAccess = true;
            }
            Assert.IsTrue(errorOnAccess);
        }

        [TestInitialize]
        public void Initialize()
        {
            connectionStringBuilder = new MySqlConnectionStringBuilder();
            databaseBaseName = "test";
            unitOfWork = UnitOfWork.Instance;
            unitOfWork.ConnectionStringBuilder.UserID = "root";
            createdDatabases = new List<string>();
        }

        [TestMethod]
        public void IsConnectedTest()
        {
            //not connected if connection string is corrupted
            unitOfWork.ConnectionStringBuilder.Server = "wrongserver";
            Assert.IsFalse(unitOfWork.IsConnected());

            unitOfWork.ConnectionStringBuilder.Server = "localhost";
            unitOfWork.Connect(databaseBaseName + "IsConnected");
            createdDatabases.Add(databaseBaseName + "IsConnected");

            Assert.IsTrue(unitOfWork.IsConnected());
        }

        #endregion Public Methods
    }
}