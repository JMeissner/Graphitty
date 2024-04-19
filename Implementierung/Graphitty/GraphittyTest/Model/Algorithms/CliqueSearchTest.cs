using Microsoft.VisualStudio.TestTools.UnitTesting;
using Graphitty.Model.Graphs;
using Graphitty.Model.Algorithms;

namespace GraphittyTest.Model.Algorithms
{
    [TestClass]
    public class CliqueSearchTest
    {
        #region Public Methods

        [TestMethod]
        public void CliqueGraphTest()
        {
            Graph graph = createCliqueGraph();
            CliqueSearch cs = new CliqueSearch();

            cs.Run(graph);

            string cliquesOfSizeK = "\'3\': 4, \'4\': 1";

            Assert.IsTrue(graph.NumCliquesOfSizeK.Equals(cliquesOfSizeK));
        }

        [TestMethod]
        public void CliqueSizeOfSizeKTest()
        {
            Graph graph = createGraph();
            CliqueSearch cs = new CliqueSearch();

            cs.Run(graph);

            string cliquesOfSizeK = "\'3\': 15, \'4\': 6, \'5\': 1";

            Assert.IsTrue(graph.NumCliquesOfSizeK.Equals(cliquesOfSizeK));
        }

        [TestMethod]
        public void CliqueSizeTwoTest()
        {
            Graph graph = createHorseshoeGraph();
            CliqueSearch cs = new CliqueSearch();

            cs.Run(graph);

            string cliqueSizeTwo = "# Edges";

            Assert.IsTrue(graph.NumCliquesOfSizeK.Equals(cliqueSizeTwo));
        }

        [TestMethod]
        public void MaxCliqueSizeTest()
        {
            Graph graph = createGraph();
            CliqueSearch cs = new CliqueSearch();

            cs.Run(graph);

            Assert.AreEqual(5, graph.LargestCliqueSize);
        }

        #endregion Public Methods

        #region Private Methods

        private Graph createCliqueGraph()
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
            graph.Edges.Add(new Edge(two, four));
            graph.Edges.Add(new Edge(three, four));

            graph.BFSCodeBitvector = "111111";

            return graph;
        }

        private Graph createGraph()
        {
            Vertex one = new Vertex(1);
            Vertex two = new Vertex(2);
            Vertex three = new Vertex(3);
            Vertex four = new Vertex(4);
            Vertex five = new Vertex(5);
            Vertex six = new Vertex(6);
            Vertex seven = new Vertex(7);
            Vertex eight = new Vertex(8);
            Vertex nine = new Vertex(9);

            Graph graph = new Graph();

            graph.Vertices.Add(one);
            graph.Vertices.Add(two);
            graph.Vertices.Add(three);
            graph.Vertices.Add(four);
            graph.Vertices.Add(five);
            graph.Vertices.Add(six);
            graph.Vertices.Add(seven);
            graph.Vertices.Add(eight);
            graph.Vertices.Add(nine);

            graph.Edges.Add(new Edge(one, seven));
            graph.Edges.Add(new Edge(one, four));
            graph.Edges.Add(new Edge(one, nine));
            graph.Edges.Add(new Edge(one, three));
            graph.Edges.Add(new Edge(one, five));
            graph.Edges.Add(new Edge(two, five));
            graph.Edges.Add(new Edge(two, three));
            graph.Edges.Add(new Edge(two, eight));
            graph.Edges.Add(new Edge(two, six));
            graph.Edges.Add(new Edge(four, seven));
            graph.Edges.Add(new Edge(four, nine));
            graph.Edges.Add(new Edge(nine, seven));
            graph.Edges.Add(new Edge(five, six));
            graph.Edges.Add(new Edge(five, eight));
            graph.Edges.Add(new Edge(five, three));
            graph.Edges.Add(new Edge(three, six));
            graph.Edges.Add(new Edge(three, eight));
            graph.Edges.Add(new Edge(eight, six));

            graph.BFSCodeBitvector = "111111111111000000001000001100000111";
            return graph;
        }

        private Graph createHorseshoeGraph()
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

            graph.Edges.Add(new Edge(one, two));
            graph.Edges.Add(new Edge(two, three));
            graph.Edges.Add(new Edge(one, four));
            graph.Edges.Add(new Edge(four, five));

            graph.BFSCodeBitvector = "1100100010";

            return graph;
        }

        #endregion Private Methods
    }
}