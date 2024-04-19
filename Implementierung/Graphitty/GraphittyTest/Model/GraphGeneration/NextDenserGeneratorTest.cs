using Microsoft.VisualStudio.TestTools.UnitTesting;
using Graphitty.Model.GraphGeneration;
using Graphitty.Model.Graphs;
using Graphitty.Model.DataAccessLayer;
using System.Collections.Generic;
using System.Diagnostics;
using GraphittyTest.Model.DataAccessLayer;

namespace GraphittyTest.Model.GraphGeneration
{
    [TestClass]
    public class NextDenserGeneratorTest
    {
        #region Private Fields

        private NextDenserGenerator generator;
        private UnitOfWorkMock unitOfWork;

        #endregion Private Fields

        #region Public Methods

        [TestCleanup]
        public void CleanUp()
        {
            unitOfWork = null;
            generator = null;
            System.GC.Collect();
        }

        [TestMethod]
        public void DuplicateGenerationTest()
        {
            Graph graph1 = new Graph("1,1,2;1,1,3,0")
            {
                Id = 1
            };
            Graph graph2 = new Graph("1,1,2;1,1,3;-1,2,3,0")
            {
                Id = 2
            };
            unitOfWork.GraphRepository.Create(new GraphEntity
            {
                Id = graph2.Id,
                BFSCode = graph2.BFSCode,
                BFSCodeBitvector = graph2.BFSCodeBitvector
            });
            generator.GenerateNextDenser(new GraphEntity
            {
                Id = graph1.Id,
                BFSCode = graph1.BFSCode,
                BFSCodeBitvector = graph1.BFSCodeBitvector,
                NumVertices = graph1.NumVertices
            });
            Assert.IsTrue(generator.GeneratedDuplicate);
            Assert.AreEqual(generator.NumDuplicates, 1);
        }

        [TestMethod]
        public void GenerateDenserToTriangleTest()
        {
            Graph graph = new Graph("1,1,2;1,1,3,0");
            generator.GenerateNextDenser(graph);
            graph.Id = 1;
            unitOfWork.GraphRepository.Create(new GraphEntity
            {
                Id = graph.Id,
                BFSCode = graph.BFSCode,
                BFSCodeBitvector = graph.BFSCodeBitvector
            });
            string correctBitVector = "111";
            Assert.IsTrue(unitOfWork.GraphRepository.GetCount() == 2);
            List<GraphEntity> graphs = new List<GraphEntity>(unitOfWork.GraphRepository.Get(g => g.BFSCodeBitvector == correctBitVector));
            Assert.AreEqual(graphs[0].BFSCodeBitvector, correctBitVector);
        }

        [TestMethod]
        public void GenerateNonTrivialNextDenserTest()
        {
            Graph graph = new Graph("1,1,2;1,1,3;-1,2,3;1,1,4;1,4,5,0");
            generator.GenerateNextDenser(graph);
            graph.Id = 1;
            unitOfWork.GraphRepository.Create(new GraphEntity
            {
                Id = graph.Id,
                BFSCode = graph.BFSCode,
                BFSCodeBitvector = graph.BFSCodeBitvector
            });
            string correctBitVector = "1111000100";
            Assert.IsTrue(unitOfWork.GraphRepository.GetCount() == 2);
            List<GraphEntity> graphs = new List<GraphEntity>(unitOfWork.GraphRepository.Get(g => g.BFSCodeBitvector == correctBitVector));
            Assert.IsTrue(graphs.Count > 0);
            if (graphs.Count > 0)
            {
                Assert.AreEqual(graphs[0].BFSCodeBitvector, correctBitVector);
            }
        }

        [TestInitialize]
        public void SetUp()
        {
            unitOfWork = new UnitOfWorkMock();
            generator = new NextDenserGenerator(unitOfWork);
        }

        #endregion Public Methods
    }
}