using System.Collections.Generic;
using System.Linq;
using Graphitty.Model.DataAccessLayer;
using Graphitty.Model.Graphs;
using Graphitty.ViewModel;
using Graphitty.ViewModel.Events;
using GraphittyTest.Model.DataAccessLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Prism.Events;

namespace GraphittyTest.ViewModel
{
    [TestClass]
    public class CorrelationsTableViewModelTest
    {
        #region Private Fields

        private CorrelationsTableViewModel correlationsTableViewModel;
        private IEventAggregator eventAggregator;
        private IUnitOfWork uow;

        #endregion Private Fields

        #region Public Methods

        [TestMethod]
        public void CalculateCorrelationsTest()
        {
            //arrange
            List<int> averageVertexDegreeList = new List<int> { 4, 21, 2, 11, 14, 2, 6 };
            List<int> numVerticesList = new List<int> { 70, 63, 82, 65, 61, 74, 84 };

            foreach (var i in Enumerable.Range(0, numVerticesList.Count))
            {
                if (!uow.GraphRepository.Exists(g => g.Id == i))
                {
                    uow.GraphRepository.Create(new GraphEntity
                    {
                        Id = i,
                        BFSCode = i.ToString(),
                        AverageVertexDegree = averageVertexDegreeList.ElementAt(i),
                        NumVertices = numVerticesList.ElementAt(i),
                    });
                }
            }
            uow.Save();

            //act
            eventAggregator.GetEvent<DatabaseChangedEvent>().Publish();
            var resCorrelations = correlationsTableViewModel.Correlations;

            //assert
            var correlation = resCorrelations.Where(elem =>
            {
                return (elem.FirstProperty == "AverageVertexDegree" && elem.SecondProperty == "NumVertices")
                || (elem.FirstProperty == "NumVertices" && elem.SecondProperty == "AverageVertexDegree");
            }).First();
            Assert.IsTrue(-0.74 == correlation.Coefficient);
        }

        [TestMethod]
        public void CalculateCorrelationsWithEmptyRepositoryTest()
        {
            //act
            eventAggregator.GetEvent<DatabaseChangedEvent>().Publish();
            var resCorrelations = correlationsTableViewModel.Correlations;

            //assert
            foreach (var cell in correlationsTableViewModel.Correlations)
            {
                Assert.IsTrue(cell.Coefficient == 0);
            }
        }

        [TestCleanup]
        public void CleanUp()
        {
            uow = null;
            eventAggregator = null;
            correlationsTableViewModel = null;
            System.GC.Collect();
        }

        [TestInitialize]
        public void Initialize()
        {
            eventAggregator = new EventAggregator();
            uow = new UnitOfWorkMock();
            correlationsTableViewModel = new CorrelationsTableViewModel(eventAggregator, uow);
        }

        /// <summary>
        /// Tests if the possible property combinations are calculated.
        /// </summary>
        [TestMethod]
        public void PropertyCombinationTest()
        {
            int numCombinations;
            foreach (var property1 in (typeof(GraphEntity).GetProperties()))
            {
                foreach (var property2 in (typeof(GraphEntity).GetProperties()))
                {
                    numCombinations = correlationsTableViewModel.Correlations.Where(elem =>
                    {
                        return (elem.FirstProperty == property1.Name && elem.SecondProperty == property2.Name)
                        || (elem.FirstProperty == property2.Name && elem.SecondProperty == property1.Name);
                    }).Count();
                    Assert.IsTrue(numCombinations <= 1);
                }
            }
        }

        #endregion Public Methods
    }
}