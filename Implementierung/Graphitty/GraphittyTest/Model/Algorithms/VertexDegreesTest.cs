using System.Collections.Generic;
using Graphitty.Model.Algorithms;
using Graphitty.Model.Graphs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GraphittyTest.Model.Algorithms
{
    [TestClass]
    public class VertexDegreesTest
    {
        #region Public Methods

        [TestMethod]
        public void AverageVertexDegreeTest()
        {
            Graph graph = createGraph();
            VertexDegrees deg = new VertexDegrees();

            deg.Run(graph);

            Assert.AreEqual(graph.AverageVertexDegree, 2.4);
        }

        [TestMethod]
        public void MaxVertexDegreeTest()
        {
            Graph graph = createGraph();
            VertexDegrees deg = new VertexDegrees();

            deg.Run(graph);

            Assert.AreEqual(graph.MaxVertexDegree, 4);
        }

        #endregion Public Methods

        #region Private Methods

        private Graph createGraph()
        {
            Graph graph = new Graph();

            Vertex one = new Vertex(1);
            Vertex two = new Vertex(2);
            Vertex three = new Vertex(3);
            Vertex four = new Vertex(4);
            Vertex five = new Vertex(5);

            graph.Vertices.Add(one);
            graph.Vertices.Add(two);
            graph.Vertices.Add(three);
            graph.Vertices.Add(four);
            graph.Vertices.Add(five);

            Edge oneTwo = new Edge(one, two);
            Edge oneThree = new Edge(one, three);
            Edge twoThree = new Edge(two, three);
            Edge fiveFour = new Edge(five, four);
            Edge fiveThree = new Edge(five, three);
            Edge fourThree = new Edge(four, three);

            List<Edge> edges = new List<Edge>();
            edges.Add(oneThree);
            edges.Add(oneTwo);
            edges.Add(twoThree);
            edges.Add(fourThree);
            edges.Add(fiveFour);
            edges.Add(fiveThree);

            foreach (Edge edge in edges)
            {
                graph.Edges.Add(edge);
                edge.IncrementDegrees();
            }

            return graph;
        }

        #endregion Private Methods
    }
}