using Microsoft.VisualStudio.TestTools.UnitTesting;
using Graphitty.Model.GraphGeneration;
using Graphitty.Model.GraphGeneration.Factories;
using Graphitty.Model.Graphs;
using System.Collections.Generic;

namespace GraphittyTest.Model.GraphGeneration
{
    [TestClass]
    public class GraphBuilderTest
    {
        #region Private Fields

        private IEdgeFactory edgeFactory;
        private GraphBuilder graphBuilder;
        private IVertexFactory vertexFactory;

        #endregion Private Fields

        #region Public Methods

        [TestMethod]
        public void BuildWithFixedAndDegreeTest()
        {
            vertexFactory = new FixedNumVerticesFactory() { NumVertices = 5 };
            edgeFactory = new DegreeRangeFactory() { MinDegree = 4, MaxDegree = 4 };
            graphBuilder = new GraphBuilder(vertexFactory, edgeFactory);
            Graph graph = graphBuilder.BuildGraph();
            Assert.IsTrue(graph.Vertices.Count == 5);
            Assert.IsTrue(graph.Edges.Count == 10);
        }

        #endregion Public Methods
    }
}