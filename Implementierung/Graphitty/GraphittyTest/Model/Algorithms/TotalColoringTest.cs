using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GraphittyTest.Model.Algorithms
{
    [TestClass]
    public class TotalColoringTest
    {
        #region Public Methods

        //[TestMethod]
        //public void TCTestV20E21()
        //{
        //    Graph graph = new Graph();

        //    for (int i = 1; i < 21; i++)
        //    {
        //        graph.Vertices.Add(new Vertex(i));
        //    }

        //    graph.Edges.Add(new Edge(graph.FindVertex(1), graph.FindVertex(2)));
        //    graph.Edges.Add(new Edge(graph.FindVertex(1), graph.FindVertex(3)));
        //    graph.Edges.Add(new Edge(graph.FindVertex(1), graph.FindVertex(4)));
        //    graph.Edges.Add(new Edge(graph.FindVertex(4), graph.FindVertex(5)));
        //    graph.Edges.Add(new Edge(graph.FindVertex(5), graph.FindVertex(6)));
        //    graph.Edges.Add(new Edge(graph.FindVertex(6), graph.FindVertex(7)));
        //    graph.Edges.Add(new Edge(graph.FindVertex(6), graph.FindVertex(9)));
        //    graph.Edges.Add(new Edge(graph.FindVertex(7), graph.FindVertex(9)));
        //    graph.Edges.Add(new Edge(graph.FindVertex(9), graph.FindVertex(8)));
        //    graph.Edges.Add(new Edge(graph.FindVertex(5), graph.FindVertex(11)));
        //    graph.Edges.Add(new Edge(graph.FindVertex(11), graph.FindVertex(10)));
        //    graph.Edges.Add(new Edge(graph.FindVertex(11), graph.FindVertex(12)));
        //    graph.Edges.Add(new Edge(graph.FindVertex(4), graph.FindVertex(17)));
        //    graph.Edges.Add(new Edge(graph.FindVertex(17), graph.FindVertex(13)));
        //    graph.Edges.Add(new Edge(graph.FindVertex(13), graph.FindVertex(14)));
        //    graph.Edges.Add(new Edge(graph.FindVertex(17), graph.FindVertex(15)));
        //    graph.Edges.Add(new Edge(graph.FindVertex(15), graph.FindVertex(16)));
        //    graph.Edges.Add(new Edge(graph.FindVertex(3), graph.FindVertex(20)));
        //    graph.Edges.Add(new Edge(graph.FindVertex(20), graph.FindVertex(18)));
        //    graph.Edges.Add(new Edge(graph.FindVertex(18), graph.FindVertex(19)));
        //    graph.Edges.Add(new Edge(graph.FindVertex(20), graph.FindVertex(19)));

        //    graph.MaxVertexDegree = 3;

        //    TotalColoring _tc = new TotalColoring();
        //    _tc.Run(graph);

        //    Debug.WriteLine("Colored Graph with: " + graph.TotalChromaticNumber + " Colors.");
        //    Debug.WriteLine("Coloring took: " + graph.MinimalColouringComplexityInMiliSeconds + "ms");

        //    Assert.IsTrue(graph.TotalChromaticNumber < 5, "Colored Graph with: " + graph.TotalChromaticNumber + " Colors.");
        //}

        //[TestMethod]
        //public void TCTestV3E3()
        //{
        //    Graph graph = new Graph("1,1,2;1,1,3;-1,2,3,0");

        //    graph.MaxVertexDegree = 2;

        //    TotalColoring _tc = new TotalColoring();
        //    _tc.Run(graph);

        //    Debug.WriteLine("Colored Graph with: " + graph.TotalChromaticNumber + " Colors.");
        //    Debug.WriteLine("Coloring took: " + graph.MinimalColouringComplexityInMiliSeconds + "ms");

        //    Assert.IsTrue(graph.TotalChromaticNumber < 4, "Colored Graph with: " + graph.TotalChromaticNumber + " Colors.");
        //}

        //[TestMethod]
        //public void TCTestV5E5()
        //{
        //    Graph graph = new Graph();
        //    graph.Vertices.Add(new Vertex(1));
        //    graph.Vertices.Add(new Vertex(2));
        //    graph.Vertices.Add(new Vertex(3));
        //    graph.Vertices.Add(new Vertex(4));
        //    graph.Vertices.Add(new Vertex(5));

        //    graph.Edges.Add(new Edge(graph.FindVertex(1), graph.FindVertex(2)));
        //    graph.Edges.Add(new Edge(graph.FindVertex(2), graph.FindVertex(3)));
        //    graph.Edges.Add(new Edge(graph.FindVertex(3), graph.FindVertex(4)));
        //    graph.Edges.Add(new Edge(graph.FindVertex(4), graph.FindVertex(5)));
        //    graph.Edges.Add(new Edge(graph.FindVertex(5), graph.FindVertex(3)));

        //    graph.MaxVertexDegree = 3;

        //    TotalColoring _tc = new TotalColoring();
        //    _tc.Run(graph);

        //    Debug.WriteLine("Colored Graph with: " + graph.TotalChromaticNumber + " Colors.");
        //    Debug.WriteLine("Coloring took: " + graph.MinimalColouringComplexityInMiliSeconds + "ms");

        //    Assert.IsTrue(graph.TotalChromaticNumber < 5, "Colored Graph with: " + graph.TotalChromaticNumber + " Colors.");
        //}

        //[TestMethod]
        //public void TCTestV6E8()
        //{
        //    Graph graph = new Graph();
        //    graph.Vertices.Add(new Vertex(1));
        //    graph.Vertices.Add(new Vertex(2));
        //    graph.Vertices.Add(new Vertex(3));
        //    graph.Vertices.Add(new Vertex(4));
        //    graph.Vertices.Add(new Vertex(5));
        //    graph.Vertices.Add(new Vertex(6));

        //    graph.Edges.Add(new Edge(graph.FindVertex(1), graph.FindVertex(2)));
        //    graph.Edges.Add(new Edge(graph.FindVertex(2), graph.FindVertex(3)));
        //    graph.Edges.Add(new Edge(graph.FindVertex(3), graph.FindVertex(4)));
        //    graph.Edges.Add(new Edge(graph.FindVertex(4), graph.FindVertex(5)));
        //    graph.Edges.Add(new Edge(graph.FindVertex(5), graph.FindVertex(3)));
        //    graph.Edges.Add(new Edge(graph.FindVertex(3), graph.FindVertex(6)));
        //    graph.Edges.Add(new Edge(graph.FindVertex(6), graph.FindVertex(1)));
        //    graph.Edges.Add(new Edge(graph.FindVertex(2), graph.FindVertex(5)));

        //    graph.MaxVertexDegree = 4;

        //    TotalColoring _tc = new TotalColoring();
        //    _tc.Run(graph);

        //    Debug.WriteLine("Colored Graph with: " + graph.TotalChromaticNumber + " Colors.");
        //    Debug.WriteLine("Coloring took: " + graph.MinimalColouringComplexityInMiliSeconds + "ms");

        //    Assert.IsTrue(graph.TotalChromaticNumber < 6, "Colored Graph with: " + graph.TotalChromaticNumber + " Colors.");
        //}

        //[TestMethod]
        //public void TCTestV7E10()
        //{
        //    Graph graph = new Graph("1,1,2;1,1,3;-1,2,3;1,1,4;1,1,5;-1,4,5;1,1,6;1,2,7;1,4,7;-1,6,7,0");

        //    graph.MaxVertexDegree = 5;

        //    TotalColoring _tc = new TotalColoring();
        //    _tc.Run(graph);

        //    Debug.WriteLine("Colored Graph with: " + graph.TotalChromaticNumber + " Colors.");
        //    Debug.WriteLine("Coloring took: " + graph.MinimalColouringComplexityInMiliSeconds + "ms");

        //    Assert.IsTrue(graph.TotalChromaticNumber < 7, "Colored Graph with: " + graph.TotalChromaticNumber + " Colors.");
        //}

        #endregion Public Methods
    }
}