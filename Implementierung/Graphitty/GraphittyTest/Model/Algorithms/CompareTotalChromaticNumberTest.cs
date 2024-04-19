using Microsoft.VisualStudio.TestTools.UnitTesting;
using Graphitty.Model.Algorithms;
using Graphitty.Model.Graphs;

namespace GraphittyTest.Model.Algorithms
{
    [TestClass]
    public class CompareTotalChromaticNumberTest
    {
        #region Public Methods

        [TestMethod]
        public void CompareTotalChromaticNumberAlgorithTest()
        {
            Graph graph1 = graphOne();
            Graph graph2 = graphTwo();
            Graph graph3 = graphThree();
            Graph graph4 = graphFour();

            TotalColoring tc = new TotalColoring();
            CompareTotalChromaticNumber tcn = new CompareTotalChromaticNumber();
            VertexDegrees vd = new VertexDegrees();

            vd.Run(graph1);
            vd.Run(graph2);
            vd.Run(graph3);
            vd.Run(graph4);

            tc.Run(graph2);
            tc.Run(graph1);
            tc.Run(graph3);
            tc.Run(graph4);

            tcn.Run(graph1, graph2);
            tcn.Run(graph1, graph2);
            tcn.Run(graph1, graph3);
            tcn.Run(graph2, graph3);
            tcn.Run(graph2, graph4);
            tcn.Run(graph3, graph4);

            Assert.AreEqual(3, graph1.NumGraphsWithSmallerEqualChromaticNumber);
            Assert.AreEqual(2, graph2.NumGraphsWithSmallerEqualChromaticNumber);
            Assert.AreEqual(2, graph3.NumGraphsWithSmallerEqualChromaticNumber);
            Assert.AreEqual(0, graph4.NumGraphsWithSmallerEqualChromaticNumber);
        }

        #endregion Public Methods

        #region Private Methods

        private Graph graphFour()
        {
            Vertex one = new Vertex(1);
            Vertex two = new Vertex(2);
            Vertex three = new Vertex(3);
            Vertex four = new Vertex(4);

            Graph graph = new Graph();

            graph.Vertices.Add(one);
            graph.Vertices.Add(two);
            graph.Vertices.Add(three);
            graph.Vertices.Add(four);

            graph.Edges.Add(new Edge(one, two));
            graph.Edges.Add(new Edge(one, four));
            graph.Edges.Add(new Edge(two, three));

            foreach (Edge e in graph.Edges)
            {
                e.IncrementDegrees();
            }

            return graph;
        }

        private Graph graphOne()
        {
            Vertex one = new Vertex(1);
            Vertex two = new Vertex(2);
            Vertex three = new Vertex(3);
            Vertex four = new Vertex(4);

            Graph graph = new Graph();

            graph.Vertices.Add(one);
            graph.Vertices.Add(two);
            graph.Vertices.Add(three);
            graph.Vertices.Add(four);

            graph.Edges.Add(new Edge(one, two));
            graph.Edges.Add(new Edge(one, four));
            graph.Edges.Add(new Edge(one, three));
            graph.Edges.Add(new Edge(two, three));
            graph.Edges.Add(new Edge(two, four));
            graph.Edges.Add(new Edge(three, four));

            foreach (Edge e in graph.Edges)
            {
                e.IncrementDegrees();
            }

            return graph;
        }

        private Graph graphThree()
        {
            Vertex one = new Vertex(1);
            Vertex two = new Vertex(2);
            Vertex three = new Vertex(3);
            Vertex four = new Vertex(4);

            Graph graph = new Graph();

            graph.Vertices.Add(one);
            graph.Vertices.Add(two);
            graph.Vertices.Add(three);
            graph.Vertices.Add(four);

            graph.Edges.Add(new Edge(one, two));
            graph.Edges.Add(new Edge(one, three));
            graph.Edges.Add(new Edge(one, four));
            graph.Edges.Add(new Edge(two, three));

            foreach (Edge e in graph.Edges)
            {
                e.IncrementDegrees();
            }

            return graph;
        }

        private Graph graphTwo()
        {
            Vertex one = new Vertex(1);
            Vertex two = new Vertex(2);
            Vertex three = new Vertex(3);
            Vertex four = new Vertex(4);

            Graph graph = new Graph();

            graph.Vertices.Add(one);
            graph.Vertices.Add(two);
            graph.Vertices.Add(three);
            graph.Vertices.Add(four);

            graph.Edges.Add(new Edge(one, two));
            graph.Edges.Add(new Edge(one, four));
            graph.Edges.Add(new Edge(one, three));
            graph.Edges.Add(new Edge(two, three));
            graph.Edges.Add(new Edge(two, four));

            foreach (Edge e in graph.Edges)
            {
                e.IncrementDegrees();
            }

            return graph;
        }

        #endregion Private Methods
    }
}