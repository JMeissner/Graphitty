using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Graphitty.Model.Graphs;

namespace GraphittyTest.Model.Graphs
{
    [TestClass]
    public class BFSCodeTest
    {
        #region Private Fields

        private String bfs1;
        private String bfs2;
        private String bitVector;

        #endregion Private Fields

        #region Public Methods

        /// <summary>
        /// tests the method BFSToString.
        /// </summary>
        [TestMethod]
        public void BFSCodeBFSToStringTest()
        {
            BFSCode bfsCode = new BFSCode(bfs1);

            Assert.AreEqual(bfs1, bfsCode.BFSToString());
        }

        /// <summary>
        /// Tests the method to compare two BFSCodes.
        /// </summary>
        [TestMethod]
        public void BFSCodeCompareToTest()
        {
            BFSCode bfsCode1 = new BFSCode(bfs1);

            BFSCode bfsCode2 = new BFSCode(bfs2);

            Assert.AreEqual(-1, bfsCode1.CompareTo(bfsCode2));
            Assert.AreEqual(1, bfsCode2.CompareTo(bfsCode1));
            Assert.AreEqual(0, bfsCode2.CompareTo(bfsCode2));
        }

        /// <summary>
        /// Tests the method to compare two BFSCodes.
        /// </summary>
        [TestMethod]
        public void BFSCodeIntArrayListConstructorTest()
        {
            List<int[]> edgeList = new List<int[]>();

            int[] edge1 = new int[3];
            edge1[0] = 1;
            edge1[1] = 1;
            edge1[2] = 2;

            int[] edge2 = new int[3];
            edge2[0] = 1;
            edge2[1] = 1;
            edge2[2] = 3;

            edgeList.Add(edge1);
            edgeList.Add(edge2);

            BFSCode bfsCode = new BFSCode(edgeList);

            CollectionAssert.AreEqual(edgeList, bfsCode.BFSEdges);
        }

        /// <summary>
        /// Tests the constructor to create a bfscode object with a string.
        /// </summary>
        [TestMethod]
        public void BFSCodeStringConstructorTest()
        {
            BFSCode bfsCode1 = new BFSCode(bfs1);
            BFSCode bfsCode2 = new BFSCode(bitVector);

            Assert.AreEqual(bfs1, bfsCode1.BFSToString());
            Assert.AreEqual(bitVector, bfsCode2.GetBitVector());
        }

        /// <summary>
        /// Tests the GetBitVector method.
        /// </summary>
        [TestMethod]
        public void BitVectorTest()
        {
            BFSCode bfsCode = new BFSCode(bfs1);

            Assert.AreEqual(bitVector, bfsCode.GetBitVector());
        }

        [TestCleanup]
        public void Cleanup()
        {
            bfs1 = "";
            bfs2 = "";
            bitVector = "";
        }

        [TestInitialize]
        public void SetUp()
        {
            bfs1 = "1,1,2;1,2,3;1,1,4;-1,2,4,0";
            bfs2 = "1,1,2;1,2,3;1,1,4;-1,3,4,0";
            bitVector = "101110";
        }

        #endregion Public Methods
    }
}