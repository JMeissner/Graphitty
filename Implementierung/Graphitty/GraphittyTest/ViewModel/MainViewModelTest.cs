using System.IO;
using Graphitty.ViewModel;
using GraphittyTest.Model.DataAccessLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Prism.Events;

namespace GraphittyTest.ViewModel
{
    [TestClass]
    public class MainViewModelTest
    {
        #region Private Fields

        private EventAggregator eventAggregator;
        private MainViewModel mainViewModel;
        private UnitOfWorkMock unitOfWork;

        #endregion Private Fields

        #region Public Methods

        [TestMethod]
        public void AvailableDBTest()
        {
            Assert.IsTrue(mainViewModel.AvailableDatabases.Contains("MockDatabase"));
        }

        [TestCleanup]
        public void CleanUp()
        {
            unitOfWork = null;
            mainViewModel = null;
        }

        [TestMethod]
        public void CloseCommandTest()
        {
            //only allowed to close when not generating
            Assert.IsTrue(mainViewModel.CloseCommand.CanExecute(null));
        }

        [TestMethod]
        public void ConnectDatabaseCommand()
        {
            //dont allow changing if no db is selected
            Assert.IsFalse(mainViewModel.ConnectDatabaseCommand.CanExecute(null));

            mainViewModel.SelectedDatabase = "MockDatabase";
            Assert.IsTrue(mainViewModel.ConnectDatabaseCommand.CanExecute(null));

            //after connection the uow db should be the selected one
            mainViewModel.Path = Directory.GetCurrentDirectory() + @"\ConnectionTest.txt";

            mainViewModel.ConnectDatabaseCommand.Execute(null);
            Assert.AreEqual(mainViewModel.SelectedDatabase, unitOfWork.ConnectionStringBuilder.Database);
        }

        [TestMethod]
        public void DeleteDatabaseCommandTest()
        {
            //cannot delete database if none is selected
            Assert.IsFalse(mainViewModel.DeleteDatabaseCommand.CanExecute(null));

            //and if the selected database == connected database
            mainViewModel.SelectedDatabase = "MockDatabase";
            mainViewModel.ConnectDatabaseCommand.Execute(null);
            Assert.IsFalse(mainViewModel.DeleteDatabaseCommand.CanExecute(null));

            //select a different databse
            mainViewModel.SelectedDatabase = "MockDatabase2";
            Assert.IsTrue(mainViewModel.DeleteDatabaseCommand.CanExecute(null));

            //select another databse
            mainViewModel.DeleteDatabaseCommand.Execute(null);

            Assert.IsFalse(mainViewModel.AvailableDatabases.Contains("MockDatabase2"));
        }

        [TestInitialize]
        public void Initialize()
        {
            eventAggregator = new EventAggregator();
            unitOfWork = new UnitOfWorkMock();
            mainViewModel = new MainViewModel(eventAggregator, unitOfWork);
        }

        #endregion Public Methods
    }
}