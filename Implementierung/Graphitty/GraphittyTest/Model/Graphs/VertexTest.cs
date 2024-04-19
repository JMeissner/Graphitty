using Microsoft.VisualStudio.TestTools.UnitTesting;
using Graphitty.Model.Graphs;

namespace GraphittyTest.Model.Graphs
{
    [TestClass]
    public class VertexTest
    {
        #region Private Fields

        private Vertex vertex1;
        private Vertex vertex2;

        #endregion Private Fields

        #region Public Methods

        [TestCleanup]
        public void Cleanup()
        {
            vertex1 = null;
            vertex2 = null;
        }

        /// <summary>
        /// Tests the Equals method.
        /// </summary>
        [TestMethod]
        public void EqualsTest()
        {
            Assert.IsTrue(vertex1.Equals(vertex1));
            Assert.IsFalse(vertex1.Equals(vertex2));
        }

        [TestInitialize]
        public void SetUp()
        {
            vertex1 = new Vertex(1);
            vertex2 = new Vertex(2);
        }

        /// <summary>
        /// Tests the constructor of the Vertex.
        /// </summary>
        [TestMethod]
        public void VertexConstructorTest()
        {
            Assert.AreEqual(1, vertex1.ID);
            Assert.AreEqual(0, vertex1.Degree);
        }

        #endregion Public Methods
    }
}