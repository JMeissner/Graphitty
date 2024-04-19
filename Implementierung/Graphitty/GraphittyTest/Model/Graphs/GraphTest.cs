using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Graphitty.Model.Graphs;
using System.Collections.Generic;
using System.Diagnostics;

namespace GraphittyTest
{
    [TestClass]
    public class GraphTest
    {
        #region Private Fields

        private String bfs1;
        private String bfs2;

        private List<Edge> edg;
        private Edge edge1;
        private Edge edge2;
        private Edge edge3;
        private Vertex vertex1;
        private Vertex vertex2;
        private Vertex vertex3;
        private Vertex vertex4;
        private List<Vertex> verti;

        #endregion Private Fields

        #region Public Methods

        [TestCleanup]
        public void Cleanup()
        {
            bfs1 = "";
            bfs2 = "";

            vertex1 = null;
            vertex2 = null;
            vertex3 = null;
            vertex4 = null;

            edge1 = null;
            edge2 = null;
            edge3 = null;

            verti = null;
            edg = null;
        }

        /// <summary>
        /// Tests if BFSCode compare works correctly
        /// </summary>
        [TestMethod]
        public void CompareBFSTest()
        {
            Graph graph1 = new Graph(bfs1);
            Graph graph2 = new Graph(bfs2);

            Assert.AreEqual(-1, graph1.CompareBFS(graph2));
            Assert.AreEqual(1, graph2.CompareBFS(graph1));
            Assert.AreEqual(0, graph1.CompareBFS(graph1));
        }

        /// <summary>
        /// Tests if Graph compare works correctly
        /// </summary>
        [TestMethod]
        public void CompareGraphsTest()
        {
            Graph graph1 = new Graph(bfs1);
            Graph graph2 = new Graph(bfs2);

            Assert.AreEqual(false, graph1.Compare(graph2));
            Assert.AreEqual(true, graph1.Compare(graph1));
        }

        /// <summary>
        /// Tests if Contains Vertex Method works correctly
        /// </summary>
        [TestMethod]
        public void CompareProfilTest()
        {
            String profil1 = "1,1,2?1,1,3;-1,2,3?1,1,4;-1,2,4;-1,3,4?1,1,5;-1,2,5?1,1,6;-1,5,6?1,2,7;-1,5,7?1,2,8;-1,7,8?" +
                "1,1,2?1,1,3;-1,2,3?1,1,4;-1,2,4?1,1,5?1,1,6;-1,5,6?1,3,7;-1,5,7;-1,6,7?1,3,8;-1,7,8?" +
                "1,1,2?1,1,3;-1,2,3?1,1,4;-1,2,4?1,2,5?1,2,6;-1,5,6?1,3,7;-1,5,7;-1,6,7?1,3,8;-1,7,8?" +
                "1,1,2?1,1,3;-1,2,3?1,1,4;-1,2,4?1,3,5?1,3,6;-1,4,6;-1,5,6?1,3,7;-1,5,7?1,4,8;-1,6,8?" +
                "1,1,2?1,1,3;-1,2,3?1,1,4;-1,2,4?1,3,5?1,3,6;-1,4,6;-1,5,6?1,3,7;-1,5,7?1,4,8;-1,6,8?" +
                "1,1,2?1,1,3;-1,2,3?1,1,4?1,1,5;-1,4,5?1,2,6;-1,4,6?1,2,7;-1,4,7;-1,6,7?1,2,8;-1,3,8?" +
                "1,1,2?1,1,3;-1,2,3?1,2,4;-1,3,4?1,2,5?1,2,6;-1,5,6?1,4,7;-1,5,7;-1,6,7?1,4,8;-1,7,8?" +
                "1,1,2?1,1,3;-1,2,3?1,2,4?1,2,5;-1,4,5?1,3,6;-1,4,6?1,3,7;-1,4,7;-1,6,7?1,4,8;-1,5,8";

            String profil2 = "1,1,2?1,1,3;-1,2,3?1,1,4;-1,2,4;-1,3,4?1,1,5;-1,2,5?1,1,6;-1,5,6?1,2,7;-1,5,7?1,2,8;-1,7,8?" +
                "1,1,2?1,1,3;-1,2,3?1,1,4;-1,2,4?1,1,5?1,1,6;-1,5,6?1,3,7;-1,5,7;-1,6,7?1,3,8;-1,7,8?" +
                "1,1,2?1,1,3;-1,2,3?1,1,4;-1,2,4?1,2,5?1,2,6;-1,5,6?1,3,7;-1,5,7;-1,6,7?1,3,8;-1,7,8?" +
                "1,1,2?1,1,3;-1,2,3?1,1,4;-1,2,4?1,4,5?1,3,6;-1,4,6;-1,5,6?1,3,7;-1,5,7?1,4,8;-1,6,8?" +
                "1,1,2?1,1,3;-1,2,3?1,1,4;-1,2,4?1,3,5?1,3,6;-1,4,6;-1,5,6?1,3,7;-1,5,7?1,4,8;-1,6,8?" +
                "1,1,2?1,1,3;-1,2,3?1,1,4?1,1,5;-1,4,5?1,2,6;-1,4,6?1,2,7;-1,4,7;-1,6,7?1,2,8;-1,3,8?" +
                "1,1,2?1,1,3;-1,2,3?1,2,4;-1,3,4?1,2,5?1,2,6;-1,5,6?1,4,7;-1,5,7;-1,6,7?1,4,8;-1,7,8?" +
                "1,1,2?1,1,3;-1,2,3?1,2,4?1,2,5;-1,4,5?1,3,6;-1,4,6?1,3,7;-1,4,7;-1,6,7?1,4,8;-1,5,8";

            Graph graph1 = new Graph();
            Graph graph2 = new Graph();
            graph1.Profile = profil1;
            graph2.Profile = profil2;

            Assert.IsFalse(graph1.CompareProfile(graph2));
            Assert.IsTrue(graph2.CompareProfile(graph1));
        }

        /// <summary>
        /// Tests if Contains Edge Method works correctly
        /// </summary>
        [TestMethod]
        public void ContainsEdgeTest()
        {
            edg.Add(edge1);
            edg.Add(edge2);

            Graph graph = new Graph(verti, edg);

            Assert.AreEqual(true, graph.ContainsEdge(edge1));
            Assert.AreEqual(true, graph.ContainsEdge(edge2));
            Assert.AreEqual(false, graph.ContainsEdge(edge3));
        }

        /// <summary>
        /// Tests if Contains Vertex Method works correctly
        /// </summary>
        [TestMethod]
        public void ContainsVertexTest()
        {
            verti.Add(vertex1);
            verti.Add(vertex2);

            Graph graph = new Graph(verti, edg);

            Assert.AreEqual(true, graph.ContainsVertex(vertex1));
            Assert.AreEqual(true, graph.ContainsVertex(vertex2));
            Assert.AreEqual(false, graph.ContainsVertex(vertex3));
        }

        /// <summary>
        /// Tests the empty Graph Constructor
        /// </summary>
        [TestMethod]
        public void EmptyGraphConstructorTest()
        {
            Graph graph = new Graph();

            Assert.AreEqual(0, graph.Edges.Count);
            Assert.AreEqual(0, graph.Vertices.Count);
        }

        /// <summary>
        /// Tests if Find Edges Method works correctly
        /// </summary>
        [TestMethod]
        public void FindEdgesTest()
        {
            List<Edge> edgListToCompare = new List<Edge>();

            verti.Add(vertex1);
            verti.Add(vertex2);
            verti.Add(vertex3);
            verti.Add(vertex4);

            edg.Add(edge1);
            edg.Add(edge2);
            edg.Add(edge3);

            edgListToCompare.Add(edge1);
            edgListToCompare.Add(edge2);

            Graph graph = new Graph(verti, edg);

            CollectionAssert.AreEqual(graph.FindEdges(vertex1), edgListToCompare);
        }

        /// <summary>
        /// Tests if Contains Vertex Method works correctly
        /// </summary>
        [TestMethod]
        public void FindVertexTest()
        {
            verti.Add(vertex1);
            verti.Add(vertex2);

            Graph graph = new Graph(verti, edg);

            Assert.IsTrue(vertex1.Equals(graph.FindVertex(1)));
            Assert.IsTrue(vertex2.Equals(graph.FindVertex(2)));
            Assert.IsNull(graph.FindVertex(3));
        }

        /// <summary>
        /// Tests if the Graph gets constructed properly from a BFS Code
        /// </summary>
        [TestMethod]
        public void GraphFromBFSConstructorTest()
        {
            Graph graph = new Graph(bfs1);
            Assert.IsTrue(graph.ContainsVertex(vertex1));
            Assert.IsTrue(graph.ContainsVertex(vertex2));
            Assert.IsTrue(graph.ContainsVertex(vertex3));
            Assert.IsTrue(graph.ContainsVertex(vertex4));
            Assert.IsTrue(graph.ContainsEdge(edge1));
            Assert.IsTrue(graph.ContainsEdge(edge2));
            Assert.IsTrue(graph.ContainsEdge(edge3));

            Assert.AreEqual(bfs1, graph.BFSCode);
            Assert.AreEqual(4, graph.NumVertices);
            Assert.AreEqual(3, graph.NumEdges);
        }

        /// <summary>
        /// Test the Creation of Graph from a List of Vertices and Edges
        /// </summary>
        public void GraphListConstructorTest()
        {
            verti.Add(vertex1);
            verti.Add(vertex2);
            verti.Add(vertex3);
            verti.Add(vertex4);

            edg.Add(edge1);
            edg.Add(edge2);

            Graph graph = new Graph(verti, edg);
            CollectionAssert.AreEqual(verti, graph.Vertices);
            CollectionAssert.AreEqual(edg, graph.Edges);
        }

        [TestInitialize]
        public void SetUp()
        {
            bfs1 = "1,1,2;1,1,3;1,2,4,0";
            bfs2 = "1,1,2;1,1,3;1,3,4,0";

            vertex1 = new Vertex(1);
            vertex2 = new Vertex(2);
            vertex3 = new Vertex(3);
            vertex4 = new Vertex(4);

            edge1 = new Edge(vertex1, vertex2);
            edge2 = new Edge(vertex1, vertex3);
            edge3 = new Edge(vertex2, vertex4);

            verti = new List<Vertex>();
            edg = new List<Edge>();
        }

        #endregion Public Methods
    }
}