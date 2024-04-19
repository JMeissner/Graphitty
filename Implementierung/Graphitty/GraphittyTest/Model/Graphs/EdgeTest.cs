using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Graphitty.Model.Graphs;

namespace GraphittyTest.Model.Graphs
{
    [TestClass]
    public class EdgeTest
    {
        #region Private Fields

        private Vertex vertex1;
        private Vertex vertex2;
        private Vertex vertex3;

        #endregion Private Fields

        #region Public Methods

        [TestCleanup]
        public void Cleanup()
        {
            vertex1 = null;
            vertex2 = null;
            vertex3 = null;
        }

        /// <summary>
        /// Tests the ContainsVertex method.
        /// </summary>
        [TestMethod]
        public void ContainsVertexTest()
        {
            Edge edge = new Edge(vertex1, vertex2);

            Assert.IsTrue(edge.ContainsVertex(vertex1));
            Assert.IsTrue(edge.ContainsVertex(vertex2));
            Assert.IsFalse(edge.ContainsVertex(vertex3));
        }

        /// <summary>
        /// Tests the edge constructor.
        /// </summary>
        [TestMethod]
        public void EdgeConstructorTest()
        {
            Edge edge1 = new Edge(vertex1, vertex2);

            Assert.IsTrue(edge1.ContainsVertex(vertex1));
            Assert.IsTrue(edge1.ContainsVertex(vertex2));
        }

        /// <summary>
        /// Tests the Equals method.
        /// </summary>
        [TestMethod]
        public void EqualsTest()
        {
            Edge edge1 = new Edge(vertex1, vertex2);
            Edge edge2 = new Edge(vertex1, vertex2);
            Edge edge3 = new Edge(vertex2, vertex3);

            Assert.IsTrue(edge1.Equals(edge2));
            Assert.IsTrue(edge2.Equals(edge1));
            Assert.IsFalse(edge1.Equals(edge3));
        }

        /// <summary>
        /// Tests the GetVertices method.
        /// </summary>
        [TestMethod]
        public void GetVerticesTest()
        {
            Vertex[] ver = new Vertex[2];
            ver[0] = vertex1;
            ver[1] = vertex2;

            Edge edge = new Edge(vertex1, vertex2);

            CollectionAssert.AreEqual(ver, edge.GetVertices());
        }

        /// <summary>
        /// Tests the IncrementDegrees method.
        /// </summary>
        [TestMethod]
        public void IncrementDegreeTest()
        {
            Edge edge = new Edge(vertex1, vertex2);

            edge.IncrementDegrees();

            Assert.AreEqual(1, vertex1.Degree);
            Assert.AreEqual(1, vertex2.Degree);

            edge.IncrementDegrees();

            Assert.AreEqual(2, vertex1.Degree);
            Assert.AreEqual(2, vertex2.Degree);
        }

        [TestInitialize]
        public void SetUp()
        {
            vertex1 = new Vertex(1);
            vertex2 = new Vertex(2);
            vertex3 = new Vertex(3);
        }

        #endregion Public Methods
    }
}