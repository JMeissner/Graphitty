using Microsoft.VisualStudio.TestTools.UnitTesting;
using Prism.Events;
using Graphitty.ViewModel;
using Graphitty.ViewModel.Events;
using Graphitty.Model.Graphs;

namespace GraphittyTest.ViewModel
{
    [TestClass]
    public class GeneratorViewModelTest
    {
        #region Private Fields

        private Graphitty.ViewModel.DegreeRangeFactoryViewModel degreeRangeFactoryViewModel;
        private EventAggregator eventAggregator;
        private FixedNumVerticesFactoryViewModel fixedNumVerticesFactoryViewModel;
        private GeneratorViewModel generatorViewModel;
        private MaxVerticesFactoryViewModel maxVerticesFactoryViewModel;

        #endregion Private Fields

        #region Public Methods

        /// <summary>
        /// Generation of Next denser graph should only be possible if a graph is selected
        /// </summary>
        [TestMethod]
        public void CanGenerateNextDenserTest()
        {
            Assert.IsFalse(generatorViewModel.GenerateNextDenserSelectedCommand.CanExecute(null));
            var bfs = new BFSCode("1,1,2;1,1,3;-1,2,3,0");
            var graph = new GraphEntity
            {
                Id = 1,
                BFSCode = "1,1,2;1,1,3;-1,2,3,0",
                Profile = "1,1,2?1,1,3;-1,2,3?1,1,4?1,4,5,0?1,1,2?1,1,3;-1,2,3?1,2,4?1,4,5,0?1,1,2?1,1,3;-1,2,3?1,2,4?1,4,5,0?1,1,2?1,1,3?1,2,4?1,2,5;-1,4,5,0?1,1,2?1,2,3?1,3,4?1,3,5;-1,4,5,0",
                NumEdges = 3,
                NumVertices = 10
            };
            eventAggregator.GetEvent<SelectionChangedEvent>().Publish(graph);
            Assert.IsTrue(generatorViewModel.GenerateNextDenserSelectedCommand.CanExecute(null));
        }

        /// <summary>
        /// Successive generation should only be possible when a graph is selected and the number of graphs is greater 1.
        /// </summary>
        [TestMethod]
        public void canGenerateSuccessiveNextDenser()
        {
            Assert.IsFalse(generatorViewModel.GenerateSuccessiveNextDenserCommand.CanExecute(null));
            var bfs = new BFSCode("1,1,2;1,1,3;-1,2,3,0");
            var graph = new GraphEntity
            {
                Id = 1,
                BFSCode = "1,1,2;1,1,3;-1,2,3,0",
                Profile = "1,1,2?1,1,3;-1,2,3?1,1,4?1,4,5,0?1,1,2?1,1,3;-1,2,3?1,2,4?1,4,5,0?1,1,2?1,1,3;-1,2,3?1,2,4?1,4,5,0?1,1,2?1,1,3?1,2,4?1,2,5;-1,4,5,0?1,1,2?1,2,3?1,3,4?1,3,5;-1,4,5,0",
                NumEdges = 3,
                NumVertices = 10
            };
            eventAggregator.GetEvent<SelectionChangedEvent>().Publish(graph);
            generatorViewModel.NumberOfGraphs = 2;
            Assert.IsTrue(generatorViewModel.GenerateSuccessiveNextDenserCommand.CanExecute(null));
        }

        /// <summary>
        /// Generation should only be possible if
        /// </summary>
        [TestMethod]
        public void CanGenerateTest()
        {
            //assert cannot generate if no vertex factory and no edgefactory are set
            Assert.IsFalse(generatorViewModel.GenerateCommand.CanExecute(null));

            generatorViewModel.SelectedVertexFactory = fixedNumVerticesFactoryViewModel;
            //assert cannot generate if no edgefactory is set
            Assert.IsFalse(generatorViewModel.GenerateCommand.CanExecute(null));

            generatorViewModel.SelectedEdgeFactory = degreeRangeFactoryViewModel;
            //assert cannot generate if the number of graphs is zero
            Assert.IsFalse(generatorViewModel.GenerateCommand.CanExecute(null));

            generatorViewModel.NumberOfGraphs = 1;
            Assert.IsTrue(generatorViewModel.GenerateCommand.CanExecute(null));
        }

        [TestCleanup]
        public void CleanUp()
        {
            eventAggregator = null;
            maxVerticesFactoryViewModel = null;
            degreeRangeFactoryViewModel = null;
            fixedNumVerticesFactoryViewModel = null;
            generatorViewModel = null;
            System.GC.Collect();
        }

        [TestInitialize]
        public void Initialize()
        {
            eventAggregator = new EventAggregator();
            generatorViewModel = new GeneratorViewModel(eventAggregator);
            fixedNumVerticesFactoryViewModel = new FixedNumVerticesFactoryViewModel();
            degreeRangeFactoryViewModel = new Graphitty.ViewModel.DegreeRangeFactoryViewModel();
            maxVerticesFactoryViewModel = new MaxVerticesFactoryViewModel();
        }

        #endregion Public Methods
    }
}