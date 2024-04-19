using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Graphitty.Model.GraphVisualizer;
using Graphitty.Model.Graphs;
using System.Windows.Media;

namespace GraphittyTest.Model.GraphVisualizer
{
    [TestClass]
    public class GraphVisualizerTest
    {
        #region Public Methods

        [TestMethod]
        public void TestConverterConnectedTest()
        {
            GraphToGUIConverter converter = new GraphToGUIConverter();

            GUIGraph guigraph = new GUIGraph();
            guigraph.AddVerticesAndEdge(new GUIEdge(new GUIVertex(Brushes.Green, 1), new GUIVertex(Brushes.Silver, 2), Brushes.Pink));
            guigraph.AddVerticesAndEdge(new GUIEdge(new GUIVertex(Brushes.Blue, 3), new GUIVertex(Brushes.Purple, 4), Brushes.Yellow));
            GUIVertex v1 = guigraph.Vertices.ToList().ElementAt(0);
            GUIVertex v2 = guigraph.Vertices.ToList().ElementAt(3);
            guigraph.AddEdge(new GUIEdge(v1, v2));

            Graph g = converter.Convert(guigraph);

            Assert.IsNotNull(g);
        }

        [TestMethod]
        public void TestConverterGraphToGUI()
        {
            GraphToGUIConverter converter = new GraphToGUIConverter();

            Graph graph = new Graph();
            for (int i = 1; i <= 5; i++)
            {
                graph.Vertices.Add(new Vertex(i));
            }
            graph.Edges.Add(new Edge(graph.FindVertex(1), graph.FindVertex(2)));
            graph.Edges.Add(new Edge(graph.FindVertex(2), graph.FindVertex(3)));
            graph.Edges.Add(new Edge(graph.FindVertex(1), graph.FindVertex(4)));
            graph.Edges.Add(new Edge(graph.FindVertex(3), graph.FindVertex(5)));
            graph.Edges.Add(new Edge(graph.FindVertex(3), graph.FindVertex(4)));
            graph.Edges.Add(new Edge(graph.FindVertex(1), graph.FindVertex(5)));

            GUIGraph guigraph = converter.Convert(graph);

            Assert.IsTrue(guigraph.VertexCount == 5);
            Assert.IsTrue(guigraph.EdgeCount == 6);
        }

        [TestMethod]
        public void TestConverterGUIGraphToGraph()
        {
            GraphToGUIConverter converter = new GraphToGUIConverter();

            GUIGraph guigraph = new GUIGraph();
            guigraph.AddVerticesAndEdge(new GUIEdge(new GUIVertex(Brushes.Green, 1), new GUIVertex(Brushes.Silver, 2), Brushes.Pink));
            guigraph.AddVerticesAndEdge(new GUIEdge(new GUIVertex(Brushes.Blue, 3), new GUIVertex(Brushes.Purple, 4), Brushes.Yellow));
            GUIVertex v1 = guigraph.Vertices.ToList().ElementAt(0);
            GUIVertex v2 = guigraph.Vertices.ToList().ElementAt(3);
            guigraph.AddEdge(new GUIEdge(v1, v2));

            Graph g = converter.Convert(guigraph);
            Assert.IsTrue(g.Vertices.Count == 4);
            Assert.IsTrue(g.Edges.Count == 3);
        }

        [TestMethod]
        public void TestConverterNotConnectedTest()
        {
            GraphToGUIConverter converter = new GraphToGUIConverter();
            GUIGraph guigraph = new GUIGraph();
            guigraph.AddVerticesAndEdge(new GUIEdge(new GUIVertex(Brushes.Green, 1), new GUIVertex(Brushes.Silver, 2), Brushes.Pink));
            guigraph.AddVerticesAndEdge(new GUIEdge(new GUIVertex(Brushes.Blue, 3), new GUIVertex(Brushes.Purple, 4), Brushes.Yellow));

            Graph g = converter.Convert(guigraph);
            Assert.IsNull(g);
        }

        #endregion Public Methods
    }
}