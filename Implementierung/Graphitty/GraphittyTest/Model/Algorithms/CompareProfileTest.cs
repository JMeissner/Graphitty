﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Graphitty.Model.Algorithms;
using Graphitty.Model.Graphs;

namespace GraphittyTest.Model.Algorithms
{
    [TestClass]
    public class CompareProfileTest
    {
        #region Public Methods

        [TestMethod]
        public void CompareProfileAlgorithm()
        {
            Graph graph1 = graphOne();
            Graph graph2 = graphTwo();
            Graph graph3 = graphThree();
            Graph graph4 = graphFour();

            BreadthFirstSearch bfs = new BreadthFirstSearch();
            CompareProfile cp = new CompareProfile();

            bfs.Run(graph1);
            bfs.Run(graph2);
            bfs.Run(graph3);
            bfs.Run(graph4);

            cp.Run(graph1, graph2);
            cp.Run(graph4, graph1);
            cp.Run(graph1, graph3);
            cp.Run(graph2, graph3);
            cp.Run(graph4, graph2);
            cp.Run(graph4, graph3);

            Assert.AreEqual(3, graph4.NumDenserGraphsProfile);
            Assert.AreEqual(2, graph3.NumDenserGraphsProfile);
            Assert.AreEqual(1, graph2.NumDenserGraphsProfile);
            Assert.AreEqual(0, graph1.NumDenserGraphsProfile);
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
            graph.Edges.Add(new Edge(one, three));

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

            return graph;
        }

        #endregion Private Methods
    }
}