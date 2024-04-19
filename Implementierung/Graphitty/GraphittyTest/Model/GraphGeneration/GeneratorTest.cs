using Microsoft.VisualStudio.TestTools.UnitTesting;
using Graphitty.Model.GraphGeneration;
using Graphitty.Model.GraphGeneration.Factories;
using GraphittyTest.Model.DataAccessLayer;
using Graphitty.Model.Graphs;

namespace GraphittyTest.Model.GraphGeneration
{
    [TestClass]
    public class GeneratorTest
    {
        #region Private Fields

        private DegreeRangeFactory edgeFactory;
        private Generator generator;
        private GraphBuilder graphBuilder;
        private UnitOfWorkMock unitOfWork;
        private FixedNumVerticesFactory vertexFactory;

        #endregion Private Fields

        #region Public Methods

        [TestCleanup]
        public void Cleanup()
        {
            unitOfWork = null;
            generator = null;
            graphBuilder = null;
            edgeFactory = null;
            vertexFactory = null;
            System.GC.Collect();
        }

        [TestMethod]
        public void GenerationThresholdTest()
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
                Id = graph1.Id,
                BFSCode = graph1.BFSCode,
                BFSCodeBitvector = graph1.BFSCodeBitvector
            });
            unitOfWork.GraphRepository.Create(new GraphEntity
            {
                Id = graph2.Id,
                BFSCode = graph2.BFSCode,
                BFSCodeBitvector = graph2.BFSCodeBitvector
            });
            vertexFactory.NumVertices = 3;
            edgeFactory.MinDegree = 1;
            edgeFactory.MaxDegree = 2;
            graphBuilder = new GraphBuilder(vertexFactory, edgeFactory);
            generator = new Generator(graphBuilder, unitOfWork)
            {
                TryGenerateThreshold = 100
            };
            Assert.IsTrue(generator.TryGenerateThreshold == 100);
            generator.StartGenerating(1);
            //make sure that no more graph has been generated
            Assert.IsTrue(unitOfWork.GraphRepository.GetCount() == 2);
        }

        [TestMethod]
        public void GraphGenerationProcedureTest()
        {
            vertexFactory.NumVertices = 3;
            edgeFactory.MinDegree = 2;
            edgeFactory.MaxDegree = 2;
            graphBuilder = new GraphBuilder(vertexFactory, edgeFactory);
            generator = new Generator(graphBuilder, unitOfWork);
            generator.StartGenerating(1);
            //make sure that only one graph has been generated
            Assert.IsTrue(unitOfWork.GraphRepository.GetCount() == 1);
        }

        [TestInitialize]
        public void SetUp()
        {
            unitOfWork = new UnitOfWorkMock();
            vertexFactory = new FixedNumVerticesFactory();
            edgeFactory = new DegreeRangeFactory();
        }

        [TestMethod]
        public void TupelGenerationTest()
        {
            vertexFactory.NumVertices = 3;
            edgeFactory.MinDegree = 1;
            edgeFactory.MaxDegree = 2;
            graphBuilder = new GraphBuilder(vertexFactory, edgeFactory);
            generator = new Generator(graphBuilder, unitOfWork)
            {
                TupelGeneration = true
            };
            Assert.IsTrue(generator.TupelGeneration);
            generator.StartGenerating(1);
            //make sure that one graph an the next denser one has been generated
            Assert.IsTrue(unitOfWork.GraphRepository.GetCount() == 2);
        }

        #endregion Public Methods
    }
}