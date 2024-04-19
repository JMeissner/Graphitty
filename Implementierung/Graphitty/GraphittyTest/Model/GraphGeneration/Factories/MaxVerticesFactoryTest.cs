using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Graphitty.Model.GraphGeneration.Factories;
using System.Collections.Generic;
using Graphitty.Model.Graphs;

namespace GraphittyTest.Model.GraphGeneration.Factories
{
    [TestClass]
    public class MaxVerticesFactoryTest
    {
        #region Private Fields

        private MaxVerticesFactory factory;

        #endregion Private Fields

        #region Public Methods

        [TestMethod]
        public void CreateMax100VerticesTest()
        {
            factory.MaxVertices = 100;
            List<ICollection<Vertex>> createdCollections = new List<ICollection<Vertex>>();
            for (int i = 0; i < 5; i++)
            {
                createdCollections.Add(factory.CreateVertices());
            }
            foreach (List<Vertex> vertices in createdCollections)
            {
                Assert.IsTrue(vertices.Count <= 100);
                Assert.IsTrue(vertices.Count > 0);
            }
        }

        [TestMethod]
        public void CreateMax5VerticesTest()
        {
            factory.MaxVertices = 5;
            List<ICollection<Vertex>> createdCollections = new List<ICollection<Vertex>>();
            for (int i = 0; i < 5; i++)
            {
                createdCollections.Add(factory.CreateVertices());
            }
            foreach (List<Vertex> vertices in createdCollections)
            {
                Assert.IsTrue(vertices.Count <= 5);
                Assert.IsTrue(vertices.Count > 0);
            }
        }

        [TestMethod]
        public void CreateMaxRandomVerticesTest()
        {
            Random random = new Random();
            for (int i = 0; i < 5; i++)
            {
                int vertexCount = random.Next(2, 51);
                factory.MaxVertices = vertexCount;
                List<ICollection<Vertex>> createdCollections = new List<ICollection<Vertex>>();
                for (int j = 0; j < 5; j++)
                {
                    createdCollections.Add(factory.CreateVertices());
                }
                foreach (List<Vertex> vertices in createdCollections)
                {
                    Assert.IsTrue(vertices.Count <= vertexCount);
                    Assert.IsTrue(vertices.Count > 0);
                }
            }
        }

        [TestInitialize]
        public void SetUp()
        {
            factory = new MaxVerticesFactory();
        }

        #endregion Public Methods
    }
}