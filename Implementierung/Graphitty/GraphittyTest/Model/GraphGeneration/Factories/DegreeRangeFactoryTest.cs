using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Graphitty.Model.GraphGeneration.Factories;
using Graphitty.Model.Graphs;
using System.Collections.Generic;

namespace GraphittyTest.Model.GraphGeneration.Factories
{
    [TestClass]
    public class DegreeRangeFactoryTest
    {
        #region Private Fields

        private DegreeRangeFactory factory;
        private Random random;
        private ICollection<Vertex> vertices;

        #endregion Private Fields

        #region Public Methods

        [TestMethod]
        public void FixedDegreeRangeTest()
        {
            int minDegree, maxDegree;
            factory.MinDegree = minDegree = 3;
            factory.MaxDegree = maxDegree = 5;
            Graph graph = factory.AddEdges(this.vertices);
            List<Vertex> vertices = graph.Vertices;
            foreach (Vertex vertex in vertices)
            {
                Assert.IsTrue(vertex.Degree >= minDegree && vertex.Degree <= maxDegree);
            }
        }

        [TestMethod]
        public void LargeDegreeRangeTest()
        {
            int minDegree, maxDegree;
            factory.MinDegree = minDegree = random.Next(1, 4);
            factory.MaxDegree = maxDegree = 9;
            Graph graph = factory.AddEdges(this.vertices);
            List<Vertex> vertices = graph.Vertices;
            foreach (Vertex vertex in vertices)
            {
                Assert.IsTrue(vertex.Degree >= minDegree && vertex.Degree <= maxDegree);
            }
        }

        [TestInitialize]
        public void SetUp()
        {
            factory = new DegreeRangeFactory();
            FixedNumVerticesFactory fixedNumVerticesFactory = new FixedNumVerticesFactory { NumVertices = 10 };
            vertices = fixedNumVerticesFactory.CreateVertices();
            random = new Random();
        }

        [TestMethod]
        public void SmallDegreeRangeTest()
        {
            int minDegree, maxDegree;
            factory.MinDegree = minDegree = random.Next(1, 9);
            factory.MaxDegree = maxDegree = factory.MinDegree + 1;
            Graph graph = factory.AddEdges(this.vertices);
            List<Vertex> vertices = graph.Vertices;
            foreach (Vertex vertex in vertices)
            {
                Assert.IsTrue(vertex.Degree >= minDegree && vertex.Degree <= maxDegree);
            }
        }

        #endregion Public Methods
    }
}