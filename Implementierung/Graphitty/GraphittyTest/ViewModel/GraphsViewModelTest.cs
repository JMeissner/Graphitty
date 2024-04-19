using Microsoft.VisualStudio.TestTools.UnitTesting;
using Prism.Events;
using Graphitty.ViewModel;
using Graphitty.ViewModel.Events;
using Graphitty.Model.Graphs;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using GraphittyTest.Model.DataAccessLayer;
using System.Windows;

namespace GraphittyTest.ViewModel
{
    [TestClass]
    public class GraphsViewModelTest
    {
        #region Private Fields

        private EventAggregator eventAggregator;
        private GraphsViewModel graphsViewModel;
        private UnitOfWorkMock uow;

        #endregion Private Fields

        #region Public Methods

        [TestMethod]
        public void BfsCopyTest()
        {
            //arrange
            var filteredCollection = new ObservableCollection<GraphEntity>(getDefaultGraphEntities());
            eventAggregator.GetEvent<FilterAppliedEvent>().Publish(filteredCollection);

            var itemToCopyFrom = graphsViewModel.Graphs.First();
            graphsViewModel.SelectedItem = itemToCopyFrom;

            //act
            graphsViewModel.CopyBFSToClipBoardCommand.Execute(null);

            //assert
            Assert.AreEqual(Clipboard.GetText(), itemToCopyFrom.BFSCode);
        }

        [TestMethod]
        public void CanDeleteTest()
        {
            Assert.IsFalse(graphsViewModel.DeleteGraphCommand.CanExecute(null));
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
            eventAggregator.GetEvent<FilterAppliedEvent>().Publish(new ObservableCollection<GraphEntity>(new List<GraphEntity> { graph }));
            graphsViewModel.SelectedItem = graph;

            Assert.IsTrue(graphsViewModel.DeleteGraphCommand.CanExecute(null));
        }

        [TestCleanup]
        public void CleanUp()
        {
            uow = null;
            eventAggregator = null;
            graphsViewModel = null;
            System.GC.Collect();
        }

        [TestMethod]
        public void DeleteGraphTest()
        {
            //add graphs to ViewModel
            eventAggregator.GetEvent<FilterAppliedEvent>().Publish(new ObservableCollection<GraphEntity>(getDefaultGraphEntities()));

            //arrange item to delete
            var itemToDelete = graphsViewModel.Graphs.First();
            graphsViewModel.SelectedItem = itemToDelete;

            //act
            graphsViewModel.DeleteGraphCommand.Execute(null);

            //Assert
            Assert.IsFalse(uow.GraphRepository.Exists(entity => entity.BFSCode == itemToDelete.BFSCode));
        }

        [TestInitialize]
        public void Initialize()
        {
            eventAggregator = new EventAggregator();
            uow = new UnitOfWorkMock();
            graphsViewModel = new GraphsViewModel(eventAggregator, uow);

            //add graphs to database
            var tmpGraphs = getDefaultGraphEntities();
            foreach (var item in tmpGraphs)
            {
                if (!uow.GraphRepository.Exists(entity => entity.BFSCode == item.BFSCode))
                {
                    uow.GraphRepository.Create(item);
                }
            }
            uow.Save();
        }

        [TestMethod]
        public void NumberOfGraphsInDBTest()
        {
            Assert.AreEqual(graphsViewModel.NumGraphsDatabase, 4);

            //add non existant graph
            uow.GraphRepository.Create(new GraphEntity
            {
                BFSCode = "1,1,2;1,1,3;-1,2,4,0",
                Id = 5
            });
            Assert.AreEqual(5, graphsViewModel.NumGraphsDatabase);
        }

        [TestMethod]
        public void ProfileCopyTest()
        {
            //arrange
            var filteredCollection = new ObservableCollection<GraphEntity>(getDefaultGraphEntities());
            eventAggregator.GetEvent<FilterAppliedEvent>().Publish(filteredCollection);

            var itemToCopyFrom = graphsViewModel.Graphs.First();
            graphsViewModel.SelectedItem = itemToCopyFrom;

            //act
            graphsViewModel.CopyProfileToClipBoardCommand.Execute(null);

            //assert
            Assert.AreEqual(Clipboard.GetText(), itemToCopyFrom.Profile);
        }

        [TestMethod]
        public void ShowGraphsAfterFilterAppliedEvent()
        {
            //add graphs to the ViewModel
            var filteredCollection = new ObservableCollection<GraphEntity>(getDefaultGraphEntities());
            //act
            eventAggregator.GetEvent<FilterAppliedEvent>().Publish(filteredCollection);

            //assert
            Assert.IsTrue(graphsViewModel.Graphs.Count == filteredCollection.Count);
            foreach (var graph in graphsViewModel.Graphs)
            {
                Assert.IsTrue(filteredCollection.Contains(graph));
            }
        }

        private List<GraphEntity> getDefaultGraphEntities()
        {
            List<GraphEntity> res = new List<GraphEntity>();
            var bfsCodes = new List<BFSCode>();
            bfsCodes.Add(new BFSCode("1,1,2;1,1,3;-1,2,3,0"));
            bfsCodes.Add(new BFSCode("1,1,2;1,1,3;-1,2,3;1,1,4;1,4,5,0"));
            bfsCodes.Add(new BFSCode("1,1,2;1,1,3;1,1,4;-1,3,4;1,2,5;-1,4,5;1,2,6;-1,4,6;1,4,7;-1,6,7,0"));
            bfsCodes.Add(new BFSCode("1,1,2;1,1,3;-1,2,3;1,1,4;1,1,5;-1,4,5;1,1,6;1,2,7;1,4,7;-1,6,7,0"));
            int i = 1;
            foreach (var bfs in bfsCodes)
            {
                res.Add(new GraphEntity
                {
                    Id = i,
                    BFSCode = bfs.BFSToString(),
                    Profile = "1,1,2?1,1,3;-1,2,3?1,1,4?1,4,5,0?1,1,2?1,1,3;-1,2,3?1,2,4?1,4,5,0?1,1,2?1,1,3;-1,2,3?1,2,4?1,4,5,0?1,1,2?1,1,3?1,2,4?1,2,5;-1,4,5,0?1,1,2?1,2,3?1,3,4?1,3,5;-1,4,5,0",
                    BFSCodeBitvector = bfs.GetBitVector(),
                    TotalChromaticNumber = 4,
                    IsTCCFulfilled = (i++ % 2 == 0),
                });
            }

            return res;
        }

        #endregion Public Methods
    }
}