using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Graphitty.Model.GraphGeneration.Factories;
using Graphitty.Model.Graphs;

namespace GraphittyTest.Model.GraphGeneration.Factories
{
    [TestClass]
    public class FixedNumVerticesFactoryTest
    {
        #region Private Fields

        private FixedNumVerticesFactory factory;

        #endregion Private Fields

        #region Public Methods

        [TestMethod]
        public void Create100VerticesTest()
        {
            factory.NumVertices = 100;
            List<ICollection<Vertex>> createdCollections = new List<ICollection<Vertex>>();
            for (int i = 0; i < 5; i++)
            {
                createdCollections.Add(factory.CreateVertices());
            }
            foreach (List<Vertex> vertices in createdCollections)
            {
                Assert.IsTrue(vertices.Count == 100);
                Assert.IsTrue(vertices.Count > 0);
            }
        }

        [TestMethod]
        public void Create5VerticesTest()
        {
            factory.NumVertices = 5;
            List<ICollection<Vertex>> createdCollections = new List<ICollection<Vertex>>();
            for (int i = 0; i < 5; i++)
            {
                createdCollections.Add(factory.CreateVertices());
            }
            foreach (List<Vertex> vertices in createdCollections)
            {
                Assert.IsTrue(vertices.Count == 5);
                Assert.IsTrue(vertices.Count > 0);
            }
        }

        [TestMethod]
        public void CreateRandomVerticesTest()
        {
            Random random = new Random();
            for (int i = 0; i < 5; i++)
            {
                int vertexCount = random.Next(2, 51);
                factory.NumVertices = vertexCount;
                List<ICollection<Vertex>> createdCollections = new List<ICollection<Vertex>>();
                for (int j = 0; j < 5; j++)
                {
                    createdCollections.Add(factory.CreateVertices());
                }
                foreach (List<Vertex> vertices in createdCollections)
                {
                    Assert.IsTrue(vertices.Count == vertexCount);
                    Assert.IsTrue(vertices.Count > 0);
                }
            }
        }

        [TestInitialize]
        public void SetUp()
        {
            factory = new FixedNumVerticesFactory();
        }

        #endregion Public Methods
    }
}