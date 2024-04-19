using Microsoft.VisualStudio.TestTools.UnitTesting;
using Prism.Events;
using Graphitty.ViewModel;
using Graphitty.ViewModel.Events;
using Graphitty.Model.Graphs;

namespace GraphittyTest.ViewModel
{
    [TestClass]
    public class PropertiesViewModelTest
    {
        #region Private Fields

        private EventAggregator eventAggregator;
        private PropertiesViewModel propertiesViewModel;

        #endregion Private Fields

        #region Public Methods

        [TestCleanup]
        public void CleanUp()
        {
            eventAggregator = null;
            propertiesViewModel = null;
            System.GC.Collect();
        }

        [TestMethod]
        public void GraphPropertyTest()
        {
            var bfs = new BFSCode("1,1,2;1,1,3;-1,2,3,0");
            var graph = new GraphEntity
            {
                Id = 1,
                BFSCode = "1,1,2;1,1,3;-1,2,3,0",
                Profile = "1,1,2?1,1,3;-1,2,3?1,1,4?1,4,5,0?1,1,2?1,1,3;-1,2,3?1,2,4?1,4,5,0?1,1,2?1,1,3;-1,2,3?1,2,4?1,4,5,0?1,1,2?1,1,3?1,2,4?1,2,5;-1,4,5,0?1,1,2?1,2,3?1,3,4?1,3,5;-1,4,5,0",
                BFSCodeBitvector = bfs.GetBitVector(),
                TotalChromaticNumber = 4,
                IsTCCFulfilled = false,
            };
            eventAggregator.GetEvent<SelectionChangedEvent>().Publish(graph);

            //assert
            Assert.AreEqual(graph.BFSCode, propertiesViewModel.Graph.BFSCode);
            Assert.AreEqual(graph.Profile, propertiesViewModel.Graph.Profile);
            Assert.AreEqual(graph.BFSCodeBitvector, propertiesViewModel.Graph.BFSCodeBitvector);
            Assert.AreEqual(graph.TotalChromaticNumber, propertiesViewModel.Graph.TotalChromaticNumber);
            Assert.AreEqual(graph.IsTCCFulfilled, propertiesViewModel.Graph.IsTCCFulfilled);
        }

        [TestInitialize]
        public void Initialize()
        {
            eventAggregator = new EventAggregator();
            propertiesViewModel = new PropertiesViewModel(eventAggregator);
        }

        #endregion Public Methods
    }
}