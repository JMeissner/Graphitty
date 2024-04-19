using System;
using System.IO;
using Graphitty.ViewModel;
using Graphitty.ViewModel.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Prism.Events;

namespace GraphittyTest.ViewModel
{
    [TestClass]
    public class ConsoleViewModelTest
    {
        #region Private Fields

        private ConsoleViewModel consoleViewModel;
        private IEventAggregator eventAggregator;
        private string path = Directory.GetCurrentDirectory() + @"\console_test.txt";

        #endregion Private Fields

        #region Public Methods

        [TestCleanup]
        public void CleanUp()
        {
            eventAggregator = null;
            consoleViewModel = null;

            while (true)
            {
                try
                {
                    //File.Delete(path);
                    break;
                }
                catch
                {
                    continue;
                }
            }
            System.GC.Collect();
        }

        [TestMethod]
        public void ClearLogTest()
        {
            eventAggregator.GetEvent<WriteLogEvent>().Publish("Test");
            Assert.IsTrue(File.ReadAllText(path).Trim().Contains("Test"));
            consoleViewModel.ClearCommand.Execute(null);
            Assert.AreEqual(String.Empty, File.ReadAllText(path));
        }

        [TestInitialize]
        public void Initialize()
        {
            eventAggregator = new EventAggregator();
            consoleViewModel = new ConsoleViewModel(eventAggregator, path);
        }

        [TestMethod]
        public void LogFilePathTest()
        {
            Assert.AreEqual(path, consoleViewModel.Path);
        }

        [TestMethod]
        public void WriteLogTest()
        {
            eventAggregator.GetEvent<WriteLogEvent>().Publish("Test");
            Assert.IsTrue(File.ReadAllText(path).Trim().Contains("Test"));
            Assert.AreEqual(File.ReadAllText(path), consoleViewModel.LogText);
        }

        #endregion Public Methods
    }
}