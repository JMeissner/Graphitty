using System.Collections.Generic;
using System.Collections.ObjectModel;
using Graphitty.Model.DataAccessLayer;
using Graphitty.Model.Filters;
using Graphitty.Model.Graphs;
using Graphitty.ViewModel;
using Graphitty.ViewModel.Events;
using GraphittyTest.Model.DataAccessLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Prism.Events;

namespace GraphittyTest.ViewModel
{
    [TestClass]
    public class FiltersViewModelTest
    {
        #region Private Fields

        private ObservableCollection<GraphEntity> filteredCollection;
        private FiltersViewModel fVM;
        private IUnitOfWork uoW;

        #endregion Private Fields

        #region Public Methods

        [TestCleanup]
        public void Cleanup()
        {
            uoW = null;
            fVM = null;
            filteredCollection = null;
        }

        [TestInitialize]
        public void SetUp()
        {
            IRepository<GraphEntity> grepo = new RepositoryMock<GraphEntity>(GenerateGraphEntitys());
            IRepository<FilterEntity> frepo = new RepositoryMock<FilterEntity>(new List<FilterEntity>());
            uoW = new UnitOfWorkMock(grepo, frepo);
            IEventAggregator eventAggregator = new EventAggregator();
            fVM = new FiltersViewModel(eventAggregator, uoW);
            eventAggregator.GetEvent<FilterAppliedEvent>().Subscribe(c => filteredCollection = c);
            eventAggregator.GetEvent<SelectionChangedEvent>().Publish(GenerateGraphEntitys().ToArray()[1]);
        }

        [TestMethod]
        public void TestFilterAvgDegree()
        {
            //< Branch
            fVM.AddFilterCommand.Execute("AvgDegree:< 3");
            fVM.ApplyFilterCommand.Execute(null);

            Assert.IsTrue(filteredCollection.Count == 2);

            fVM.RemoveFilterCommand.Execute("AvgDegree:< 3");

            //> Branch
            fVM.AddFilterCommand.Execute("AvgDegree:> 3");
            fVM.ApplyFilterCommand.Execute(null);

            Assert.IsTrue(filteredCollection.Count == 1);

            fVM.RemoveFilterCommand.Execute("AvgDegree:> 3");

            //= Branch
            fVM.AddFilterCommand.Execute("AvgDegree:= 5");
            fVM.ApplyFilterCommand.Execute(null);

            Assert.IsTrue(filteredCollection.Count == 1);

            fVM.RemoveFilterCommand.Execute("AvgDegree:= 5");

            //!= Branch
            fVM.AddFilterCommand.Execute("AvgDegree:!= 5");
            fVM.ApplyFilterCommand.Execute(null);

            Assert.IsTrue(filteredCollection.Count == 3);

            fVM.RemoveFilterCommand.Execute("AvgDegree:!= 5");
        }

        [TestMethod]
        public void TestFilterDenserBFS()
        {
            fVM.AddFilterCommand.Execute("FilterDenserBFS:");
            fVM.ApplyFilterCommand.Execute(null);

            Assert.IsTrue(filteredCollection.Count == 2);
        }

        [TestMethod]
        public void TestFilterDenserEqTCN()
        {
            fVM.AddFilterCommand.Execute("DenserEqTCN:");
            fVM.ApplyFilterCommand.Execute(null);

            Assert.IsTrue(filteredCollection.Count == 1);
        }

        [TestMethod]
        public void TestFilterDenserHigherTCN()
        {
            fVM.AddFilterCommand.Execute("DenserHigherTCN:");
            fVM.ApplyFilterCommand.Execute(null);

            Assert.IsTrue(filteredCollection.Count == 1);
        }

        [TestMethod]
        public void TestFilterDenserProfile()
        {
            fVM.AddFilterCommand.Execute("FilterDenserProfile:");
            fVM.ApplyFilterCommand.Execute(null);

            Assert.IsTrue(filteredCollection.Count == 2);
        }

        [TestMethod]
        public void TestFilterDenserSmallerTCN()
        {
            fVM.AddFilterCommand.Execute("DenserSmallerTCN:");
            fVM.ApplyFilterCommand.Execute(null);

            Assert.IsTrue(filteredCollection.Count == 0);
        }

        [TestMethod]
        public void TestFilterFail()
        {
            fVM.AddFilterCommand.Execute("AvgDegree:a 3");

            Assert.IsTrue(fVM.ActiveFilters.Count == 0);

            fVM.AddFilterCommand.Execute("AvgDegree:< b");
            fVM.ApplyFilterCommand.Execute(null);
            Assert.IsTrue(filteredCollection.Count == 4);
        }

        [TestMethod]
        public void TestFilterIsTC()
        {
            //false Branch
            fVM.AddFilterCommand.Execute("IsTCC:false");
            fVM.ApplyFilterCommand.Execute(null);

            Assert.IsTrue(filteredCollection.Count == 1);

            fVM.RemoveFilterCommand.Execute("IsTCC:false");

            //true Branch
            fVM.AddFilterCommand.Execute("IsTCC:true");
            fVM.ApplyFilterCommand.Execute(null);

            Assert.IsTrue(filteredCollection.Count == 3);

            fVM.RemoveFilterCommand.Execute("IsTCC:true");
        }

        [TestMethod]
        public void TestFilterLessDenseBFS()
        {
            fVM.AddFilterCommand.Execute("FilterLessDenseBFS:");
            fVM.ApplyFilterCommand.Execute(null);

            Assert.IsTrue(filteredCollection.Count == 1);
        }

        [TestMethod]
        public void TestFilterLessDenseEqTCN()
        {
            fVM.AddFilterCommand.Execute("LessDenseEqTCN:");
            fVM.ApplyFilterCommand.Execute(null);

            Assert.IsTrue(filteredCollection.Count == 0);
        }

        [TestMethod]
        public void TestFilterLessDenseHigherTCN()
        {
            fVM.AddFilterCommand.Execute("LessDenseHigherTCN:");
            fVM.ApplyFilterCommand.Execute(null);

            Assert.IsTrue(filteredCollection.Count == 0);
        }

        [TestMethod]
        public void TestFilterLessDenseProfile()
        {
            fVM.AddFilterCommand.Execute("FilterLessDenseProfile:");
            fVM.ApplyFilterCommand.Execute(null);

            Assert.IsTrue(filteredCollection.Count == 2);
        }

        [TestMethod]
        public void TestFilterLessDenseSmallerTCN()
        {
            fVM.AddFilterCommand.Execute("LessDenseSmallerTCN:");
            fVM.ApplyFilterCommand.Execute(null);

            Assert.IsTrue(filteredCollection.Count == 1);
        }

        [TestMethod]
        public void TestFilterMaxClique()
        {
            //< Branch
            fVM.AddFilterCommand.Execute("MaxClique:< 4");
            fVM.ApplyFilterCommand.Execute(null);

            Assert.IsTrue(filteredCollection.Count == 1);
            fVM.RemoveFilterCommand.Execute("MaxClique:< 4");

            //> Branch
            fVM.AddFilterCommand.Execute("MaxClique:> 1");
            fVM.ApplyFilterCommand.Execute(null);

            Assert.IsTrue(filteredCollection.Count == 4);
            fVM.RemoveFilterCommand.Execute("MaxClique:> 1");

            //= Branch
            fVM.AddFilterCommand.Execute("MaxClique:= 4");
            fVM.ApplyFilterCommand.Execute(null);

            Assert.IsTrue(filteredCollection.Count == 2);
            fVM.RemoveFilterCommand.Execute("MaxClique:= 4");

            //!= Branch
            fVM.AddFilterCommand.Execute("MaxClique:!= 4");
            fVM.ApplyFilterCommand.Execute(null);

            Assert.IsTrue(filteredCollection.Count == 2);
            fVM.RemoveFilterCommand.Execute("MaxClique:!= 4");
        }

        [TestMethod]
        public void TestFilterMaxDegree()
        {
            //< Branch
            fVM.AddFilterCommand.Execute("MaxDegree:< 2");
            fVM.ApplyFilterCommand.Execute(null);

            Assert.IsTrue(filteredCollection.Count == 1);
            fVM.RemoveFilterCommand.Execute("MaxDegree:< 2");

            //> Branch
            fVM.AddFilterCommand.Execute("MaxDegree:> 2");
            fVM.ApplyFilterCommand.Execute(null);

            Assert.IsTrue(filteredCollection.Count == 2);
            fVM.RemoveFilterCommand.Execute("MaxDegree:> 2");

            //= Branch
            fVM.AddFilterCommand.Execute("MaxDegree:= 4");
            fVM.ApplyFilterCommand.Execute(null);

            Assert.IsTrue(filteredCollection.Count == 2);
            fVM.RemoveFilterCommand.Execute("MaxDegree:= 4");

            //!= Branch
            fVM.AddFilterCommand.Execute("MaxDegree:!= 1");
            fVM.ApplyFilterCommand.Execute(null);

            Assert.IsTrue(filteredCollection.Count == 3);
            fVM.RemoveFilterCommand.Execute("MaxDegree:!= 1");
        }

        [TestMethod]
        public void TestFilterNumCliques()
        {
            //< Branch
            fVM.AddFilterCommand.Execute("NumCliques:< 15");
            fVM.ApplyFilterCommand.Execute(null);

            Assert.IsTrue(filteredCollection.Count == 3);

            fVM.RemoveFilterCommand.Execute("NumCliques:< 15");

            //> Branch
            fVM.AddFilterCommand.Execute("NumCliques:> 15");
            fVM.ApplyFilterCommand.Execute(null);

            Assert.IsTrue(filteredCollection.Count == 1);

            fVM.RemoveFilterCommand.Execute("NumCliques:> 15");

            //= Branch
            fVM.AddFilterCommand.Execute("NumCliques:= 5");
            fVM.ApplyFilterCommand.Execute(null);

            Assert.IsTrue(filteredCollection.Count == 1);

            fVM.RemoveFilterCommand.Execute("NumCliques:= 5");

            //!= Branch
            fVM.AddFilterCommand.Execute("NumCliques:!= 5");
            fVM.ApplyFilterCommand.Execute(null);

            Assert.IsTrue(filteredCollection.Count == 3);

            fVM.RemoveFilterCommand.Execute("NumCliques:!= 5");
        }

        [TestMethod]
        public void TestFilterNumEdges()
        {
            //< Branch
            fVM.AddFilterCommand.Execute("NumEdges:< 7");
            fVM.ApplyFilterCommand.Execute(null);

            Assert.IsTrue(filteredCollection.Count == 2);
            fVM.RemoveFilterCommand.Execute("NumEdges:< 7");

            //> Branch
            fVM.AddFilterCommand.Execute("NumEdges:> 2");
            fVM.ApplyFilterCommand.Execute(null);

            Assert.IsTrue(filteredCollection.Count == 3);
            fVM.RemoveFilterCommand.Execute("NumEdges:> 2");

            //= Branch
            fVM.AddFilterCommand.Execute("NumEdges:= 6");
            fVM.ApplyFilterCommand.Execute(null);

            Assert.IsTrue(filteredCollection.Count == 1);
            fVM.RemoveFilterCommand.Execute("NumEdges:= 6");

            //!= Branch
            fVM.AddFilterCommand.Execute("NumEdges:!= 6");
            fVM.ApplyFilterCommand.Execute(null);

            Assert.IsTrue(filteredCollection.Count == 3);
            fVM.RemoveFilterCommand.Execute("NumEdges:!= 6");
        }

        [TestMethod]
        public void TestFilterRemove()
        {
            fVM.AddFilterCommand.Execute("AvgDegree:< 3");
            fVM.RemoveFilterCommand.Execute("AvgDegree:< 3");

            Assert.IsTrue(fVM.ActiveFilters.Count == 0);
        }

        [TestMethod]
        public void TestFilterRuntime()
        {
            //< Branch
            fVM.AddFilterCommand.Execute("RunTime:< 300");
            fVM.ApplyFilterCommand.Execute(null);

            Assert.IsTrue(filteredCollection.Count == 3);

            fVM.RemoveFilterCommand.Execute("RunTime:< 300");

            //> Branch
            fVM.AddFilterCommand.Execute("RunTime:> 300");
            fVM.ApplyFilterCommand.Execute(null);

            Assert.IsTrue(filteredCollection.Count == 1);

            fVM.RemoveFilterCommand.Execute("RunTime:> 300");

            //= Branch
            fVM.AddFilterCommand.Execute("RunTime:= 420");
            fVM.ApplyFilterCommand.Execute(null);

            Assert.IsTrue(filteredCollection.Count == 1);

            fVM.RemoveFilterCommand.Execute("RunTime:= 420");

            //!= Branch
            fVM.AddFilterCommand.Execute("RunTime:!= 420");
            fVM.ApplyFilterCommand.Execute(null);

            Assert.IsTrue(filteredCollection.Count == 3);

            fVM.RemoveFilterCommand.Execute("RunTime:!= 420");
        }

        [TestMethod]
        public void TestFilterSaveAndLoad()
        {
            fVM.SelectedFilterSet = "Set 1";

            fVM.AddFilterCommand.Execute("AvgDegree:< 3");
            fVM.AddFilterCommand.Execute("IsTCC:true");
            fVM.AddFilterCommand.Execute("NumEdges:< 7");
            fVM.AddFilterCommand.Execute("MaxDegree:< 4");

            fVM.SaveFilterCommand.Execute(null);

            fVM.RemoveFilterCommand.Execute("NumEdges:< 7");

            Assert.IsTrue(fVM.ActiveFilters.Count == 3);

            fVM.LoadFilterCommand.Execute("Set 1");

            Assert.IsTrue(fVM.ActiveFilters.Count == 4);
        }

        [TestMethod]
        public void TestFilterSetup()
        {
            fVM.AddFilterCommand.Execute("AvgDegree:< 3");

            Assert.IsTrue(fVM.ActiveFilters.Count > 0);
        }

        [TestMethod]
        public void TestFilterTCN()
        {
            //< Branch
            fVM.AddFilterCommand.Execute("TCN:< 4");
            fVM.ApplyFilterCommand.Execute(null);

            Assert.IsTrue(filteredCollection.Count == 1);

            fVM.RemoveFilterCommand.Execute("TCN:< 4");

            //> Branch
            fVM.AddFilterCommand.Execute("TCN:> 4");
            fVM.ApplyFilterCommand.Execute(null);

            Assert.IsTrue(filteredCollection.Count == 1);

            fVM.RemoveFilterCommand.Execute("TCN:> 4");

            //= Branch
            fVM.AddFilterCommand.Execute("TCN:= 4");
            fVM.ApplyFilterCommand.Execute(null);

            Assert.IsTrue(filteredCollection.Count == 2);

            fVM.RemoveFilterCommand.Execute("TCN:= 4");

            // != Branch
            fVM.AddFilterCommand.Execute("TCN:!= 4");
            fVM.ApplyFilterCommand.Execute(null);

            Assert.IsTrue(filteredCollection.Count == 2);

            fVM.RemoveFilterCommand.Execute("TCN:!= 4");
        }

        #endregion Public Methods

        #region Private Methods

        private List<GraphEntity> GenerateGraphEntitys()
        {
            //All NumberOfCliquesOfSizeK strings are random, because these graphs do not have any cliques in them
            List<GraphEntity> GraphEntityList = new List<GraphEntity>();

            GraphEntityList.Add(new GraphEntity
            {
                AverageVertexDegree = 2,
                BFSCode = "1,1,2;1,1,3;1,1,4;1,2,5;-1,3,5;1,2,6;-1,4,6;1,3,7,0",
                IsTCCFulfilled = true,
                LargestCliqueSize = 3,
                MaxVertexDegree = 4,
                MinimalColouringComplexityInMiliSeconds = 420,
                NumCliquesOfSizeK = "\'3\': 20, \'4\': 5, \'5\': 1",
                NumEdges = 10,
                NumVertices = 7,
                Profile = "1,1,2?1,1,3?1,1,4?1,2,5;-1,3,5?1,2,6;-1,4,6?1,3,7,0?1,1,2?1,1,3?1,1,4?1,2,5;-1,3,5?1,2,6;-1,4,6?1,5,7,0?1,1,2?1,1,3?1,1,4?1,2,5;-1,3,5?1,2,6?1,5,7;-1,6,7,0?1,1,2?1,1,3?1,2,4;-1,3,4?1,2,5?1,3,6?1,4,7;-1,5,7,0?1,1,2?1,1,3?1,2,4;-1,3,4?1,2,5?1,4,6;-1,5,6?1,5,7,0?1,1,2?1,1,3?1,2,4;-1,3,4?1,2,5?1,4,6;-1,5,6?1,6,7,0?1,1,2?1,2,3?1,2,4?1,3,5;-1,4,5?1,3,6?1,5,7;-1,6,7,0",
                TotalChromaticNumber = 3,
            });

            GraphEntityList.Add(new GraphEntity
            {
                AverageVertexDegree = 3,
                BFSCode = "1,1,2;1,1,3;1,1,4;1,2,5;-1,3,5;1,2,6;-1,4,6;1,3,7;-1,6,7,0",
                IsTCCFulfilled = true,
                LargestCliqueSize = 4,
                MaxVertexDegree = 1,
                MinimalColouringComplexityInMiliSeconds = 120,
                NumCliquesOfSizeK = "\'3\': 4",
                NumEdges = 6,
                NumVertices = 7,
                Profile = "1,1,2?1,1,3?1,1,4?1,2,5;-1,3,5?1,2,6;-1,4,6?1,3,7;-1,6,7,0?1,1,2?1,1,3?1,1,4?1,2,5;-1,3,5?1,2,6;-1,4,6?1,3,7;-1,6,7,0?1,1,2?1,1,3?1,1,4?1,2,5;-1,3,5?1,2,6?1,4,7;-1,5,7;-1,6,7,0?1,1,2?1,1,3?1,1,4?1,2,5;-1,3,5?1,2,6?1,4,7;-1,5,7;-1,6,7,0?1,1,2?1,1,3?1,2,4;-1,3,4?1,2,5?1,3,6;-1,5,6?1,4,7;-1,5,7,0?1,1,2?1,1,3?1,2,4;-1,3,4?1,2,5?1,3,6;-1,5,6?1,4,7;-1,5,7,0?1,1,2?1,1,3?1,2,4?1,2,5?1,3,6;-1,4,6;-1,5,6?1,3,7;-1,4,7,0",
                TotalChromaticNumber = 4,
            });

            GraphEntityList.Add(new GraphEntity
            {
                AverageVertexDegree = 2,
                BFSCode = "1,1,2;1,1,3;1,1,4;1,2,5;-1,3,5;1,2,6;-1,4,6;1,3,7;-1,4,7,0",
                IsTCCFulfilled = true,
                LargestCliqueSize = 4,
                MaxVertexDegree = 2,
                MinimalColouringComplexityInMiliSeconds = 120,
                NumCliquesOfSizeK = "\'3\': 8, \'4\': 2, \'5\': 1",
                NumEdges = 7,
                NumVertices = 7,
                Profile = "1,1,2?1,1,3?1,1,4?1,2,5;-1,3,5?1,2,6;-1,4,6?1,3,7;-1,4,7,0?1,1,2?1,1,3?1,1,4?1,2,5;-1,3,5?1,2,6;-1,4,6?1,5,7;-1,6,7,0?1,1,2?1,1,3?1,1,4?1,2,5;-1,3,5?1,2,6;-1,4,6?1,5,7;-1,6,7,0?1,1,2?1,1,3?1,1,4?1,2,5;-1,3,5?1,2,6;-1,4,6?1,5,7;-1,6,7,0?1,1,2?1,1,3?1,2,4;-1,3,4?1,2,5?1,3,6?1,4,7;-1,5,7;-1,6,7,0?1,1,2?1,1,3?1,2,4;-1,3,4?1,2,5?1,3,6?1,4,7;-1,5,7;-1,6,7,0?1,1,2?1,1,3?1,2,4;-1,3,4?1,2,5?1,3,6?1,4,7;-1,5,7;-1,6,7,0",
                TotalChromaticNumber = 4,
            });

            GraphEntityList.Add(new GraphEntity
            {
                AverageVertexDegree = 5,
                BFSCode = "1,1,2;1,1,3;1,1,4;1,2,5;-1,3,5;-1,4,5;1,2,6;1,6,7,0",
                IsTCCFulfilled = false,
                LargestCliqueSize = 6,
                MaxVertexDegree = 4,
                MinimalColouringComplexityInMiliSeconds = 120,
                NumCliquesOfSizeK = "\'3\': 4, \'4\': 1",
                NumEdges = 2,
                NumVertices = 7,
                Profile = "1,1,2?1,1,3?1,1,4?1,2,5;-1,3,5;-1,4,5?1,2,6?1,6,7,0?1,1,2?1,1,3?1,1,4?1,2,5;-1,3,5;-1,4,5?1,2,6?1,6,7,0?1,1,2?1,1,3?1,1,4?1,2,5;-1,3,5?1,2,6;-1,3,6?1,4,7,0?1,1,2?1,1,3?1,2,4;-1,3,4?1,2,5;-1,3,5?1,4,6?1,6,7,0?1,1,2?1,1,3?1,2,4;-1,3,4?1,2,5;-1,3,5?1,4,6?1,6,7,0?1,1,2?1,1,3?1,2,4?1,2,5?1,4,6;-1,5,6?1,4,7;-1,5,7,0?1,1,2?1,2,3?1,3,4?1,3,5?1,4,6;-1,5,6?1,4,7;-1,5,7,0",
                TotalChromaticNumber = 31,
            });

            return GraphEntityList;
        }

        #endregion Private Methods
    }
}