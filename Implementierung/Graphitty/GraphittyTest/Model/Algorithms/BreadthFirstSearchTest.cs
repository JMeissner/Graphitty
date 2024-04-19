using Graphitty.Model.Graphs;
using Graphitty.Model.Algorithms;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GraphittyTest.Model.Algorithms
{
    [TestClass]
    public class BreadthFirstSearchTest
    {
        #region Public Methods

        [TestMethod]
        public void BFSRunCodeTest()
        {
            Graph graph = createGraph();
            BreadthFirstSearch bfs = new BreadthFirstSearch();
            bfs.Run(graph);
            string bfsCode = "1,1,2;1,1,3;-1,2,3;1,1,4;1,1,5;-1,4,5;1,1,6;1,2,7;-1,4,7;-1,6,7,0";

            Assert.IsTrue(graph.BFSCode.Equals(bfsCode));
        }

        [TestMethod]
        public void BFSRunProfileTest()
        {
            Graph graph = createGraph();
            string profile = "1,1,2?1,1,3;-1,2,3?1,1,4?1,1,5;-1,4,5?1,1,6?1,2,7;-1,4,7;-1,6,7,0?" +
                    "1,1,2?1,1,3;-1,2,3?1,1,4?1,2,5;-1,4,5?1,2,6;-1,4,6?1,2,7;-1,5,7,0?" +
                    "1,1,2?1,1,3;-1,2,3?1,1,4?1,2,5;-1,4,5?1,2,6;-1,4,6?1,2,7;-1,5,7,0?" +
                    "1,1,2?1,1,3;-1,2,3?1,2,4?1,2,5;-1,4,5?1,2,6?1,3,7;-1,4,7;-1,6,7,0?" +
                    "1,1,2?1,1,3;-1,2,3?1,2,4?1,2,5;-1,4,5?1,2,6?1,3,7;-1,4,7;-1,6,7,0?" +
                    "1,1,2?1,1,3?1,1,4?1,2,5;-1,3,5;-1,4,5?1,2,6;-1,5,6?1,3,7;-1,5,7,0?" +
                    "1,1,2?1,1,3?1,2,4;-1,3,4?1,2,5;-1,3,5?1,2,6;-1,4,6?1,2,7;-1,5,7,0";

            BreadthFirstSearch bfs = new BreadthFirstSearch();
            bfs.Run(graph);

            Assert.IsTrue(graph.Profile.Equals(profile));
        }

        [TestMethod]
        public void BoehmTest01()
        {
            Graph graph = new Graph();
            for (int i = 0; i < 8; i++)
            {
                graph.Vertices.Add(new Vertex(i + 1));
            }
            graph.Edges.Add(new Edge(graph.FindVertex(1), graph.FindVertex(2)));
            graph.Edges.Add(new Edge(graph.FindVertex(2), graph.FindVertex(3)));
            graph.Edges.Add(new Edge(graph.FindVertex(3), graph.FindVertex(6)));
            graph.Edges.Add(new Edge(graph.FindVertex(3), graph.FindVertex(4)));
            graph.Edges.Add(new Edge(graph.FindVertex(4), graph.FindVertex(5)));
            graph.Edges.Add(new Edge(graph.FindVertex(5), graph.FindVertex(6)));
            graph.Edges.Add(new Edge(graph.FindVertex(5), graph.FindVertex(7)));
            graph.Edges.Add(new Edge(graph.FindVertex(6), graph.FindVertex(7)));
            graph.Edges.Add(new Edge(graph.FindVertex(7), graph.FindVertex(8)));
            graph.Edges.Add(new Edge(graph.FindVertex(8), graph.FindVertex(2)));
            graph.Edges.Add(new Edge(graph.FindVertex(8), graph.FindVertex(1)));
            graph.Edges.Add(new Edge(graph.FindVertex(7), graph.FindVertex(1)));
            graph.Edges.Add(new Edge(graph.FindVertex(4), graph.FindVertex(1)));

            BreadthFirstSearch bfs = new BreadthFirstSearch();
            bfs.Run(graph);
            string[] profileOut = graph.Profile.Split('?');
            //Debug.WriteLine();
        }

        [TestMethod]
        public void BoehmTest02()
        {
            Graph graph = new Graph();
            for (int i = 0; i < 8; i++)
            {
                graph.Vertices.Add(new Vertex(i + 1));
            }
            graph.Edges.Add(new Edge(graph.FindVertex(1), graph.FindVertex(2)));
            graph.Edges.Add(new Edge(graph.FindVertex(2), graph.FindVertex(3)));
            graph.Edges.Add(new Edge(graph.FindVertex(2), graph.FindVertex(4)));
            graph.Edges.Add(new Edge(graph.FindVertex(3), graph.FindVertex(4)));
            graph.Edges.Add(new Edge(graph.FindVertex(3), graph.FindVertex(1)));
            graph.Edges.Add(new Edge(graph.FindVertex(4), graph.FindVertex(5)));
            graph.Edges.Add(new Edge(graph.FindVertex(5), graph.FindVertex(6)));
            graph.Edges.Add(new Edge(graph.FindVertex(4), graph.FindVertex(6)));
            graph.Edges.Add(new Edge(graph.FindVertex(6), graph.FindVertex(7)));
            graph.Edges.Add(new Edge(graph.FindVertex(7), graph.FindVertex(8)));
            graph.Edges.Add(new Edge(graph.FindVertex(8), graph.FindVertex(1)));
            graph.Edges.Add(new Edge(graph.FindVertex(7), graph.FindVertex(1)));
            graph.Edges.Add(new Edge(graph.FindVertex(6), graph.FindVertex(1)));

            BreadthFirstSearch bfs = new BreadthFirstSearch();
            bfs.Run(graph);
            string[] profileOut = graph.Profile.Split('?');
            //Debug.WriteLine();
        }

        [TestMethod]
        public void BoehmTest03()
        {
            Graph graph = new Graph();
            for (int i = 0; i < 8; i++)
            {
                graph.Vertices.Add(new Vertex(i + 1));
            }
            graph.Edges.Add(new Edge(graph.FindVertex(1), graph.FindVertex(3)));
            graph.Edges.Add(new Edge(graph.FindVertex(2), graph.FindVertex(3)));
            graph.Edges.Add(new Edge(graph.FindVertex(3), graph.FindVertex(5)));
            graph.Edges.Add(new Edge(graph.FindVertex(3), graph.FindVertex(4)));
            graph.Edges.Add(new Edge(graph.FindVertex(4), graph.FindVertex(5)));
            graph.Edges.Add(new Edge(graph.FindVertex(5), graph.FindVertex(6)));
            graph.Edges.Add(new Edge(graph.FindVertex(6), graph.FindVertex(7)));
            graph.Edges.Add(new Edge(graph.FindVertex(7), graph.FindVertex(8)));
            graph.Edges.Add(new Edge(graph.FindVertex(8), graph.FindVertex(2)));
            graph.Edges.Add(new Edge(graph.FindVertex(4), graph.FindVertex(1)));
            graph.Edges.Add(new Edge(graph.FindVertex(8), graph.FindVertex(1)));
            graph.Edges.Add(new Edge(graph.FindVertex(7), graph.FindVertex(1)));
            graph.Edges.Add(new Edge(graph.FindVertex(6), graph.FindVertex(1)));

            BreadthFirstSearch bfs = new BreadthFirstSearch();
            bfs.Run(graph);
            string[] profileOut = graph.Profile.Split('?');
            //Debug.WriteLine();
        }

        [TestMethod]
        public void BoehmTest04()
        {
            Graph graph = new Graph();
            for (int i = 0; i < 8; i++)
            {
                graph.Vertices.Add(new Vertex(i + 1));
            }
            graph.Edges.Add(new Edge(graph.FindVertex(1), graph.FindVertex(2)));
            graph.Edges.Add(new Edge(graph.FindVertex(2), graph.FindVertex(3)));
            graph.Edges.Add(new Edge(graph.FindVertex(3), graph.FindVertex(1)));
            graph.Edges.Add(new Edge(graph.FindVertex(4), graph.FindVertex(8)));
            graph.Edges.Add(new Edge(graph.FindVertex(4), graph.FindVertex(5)));
            graph.Edges.Add(new Edge(graph.FindVertex(5), graph.FindVertex(6)));
            graph.Edges.Add(new Edge(graph.FindVertex(5), graph.FindVertex(1)));
            graph.Edges.Add(new Edge(graph.FindVertex(6), graph.FindVertex(7)));
            graph.Edges.Add(new Edge(graph.FindVertex(7), graph.FindVertex(8)));
            graph.Edges.Add(new Edge(graph.FindVertex(8), graph.FindVertex(2)));
            graph.Edges.Add(new Edge(graph.FindVertex(7), graph.FindVertex(1)));
            graph.Edges.Add(new Edge(graph.FindVertex(6), graph.FindVertex(1)));

            BreadthFirstSearch bfs = new BreadthFirstSearch();
            bfs.Run(graph);
            string[] profileOut = graph.Profile.Split('?');
            //Debug.WriteLine();
        }

        [TestMethod]
        public void BoehmTest05()
        {
            Graph graph = new Graph();
            for (int i = 0; i < 8; i++)
            {
                graph.Vertices.Add(new Vertex(i + 1));
            }
            graph.Edges.Add(new Edge(graph.FindVertex(1), graph.FindVertex(2)));
            graph.Edges.Add(new Edge(graph.FindVertex(2), graph.FindVertex(3)));
            graph.Edges.Add(new Edge(graph.FindVertex(3), graph.FindVertex(1)));
            graph.Edges.Add(new Edge(graph.FindVertex(2), graph.FindVertex(4)));
            graph.Edges.Add(new Edge(graph.FindVertex(3), graph.FindVertex(4)));
            graph.Edges.Add(new Edge(graph.FindVertex(4), graph.FindVertex(5)));
            graph.Edges.Add(new Edge(graph.FindVertex(5), graph.FindVertex(6)));
            graph.Edges.Add(new Edge(graph.FindVertex(4), graph.FindVertex(6)));
            graph.Edges.Add(new Edge(graph.FindVertex(6), graph.FindVertex(1)));
            graph.Edges.Add(new Edge(graph.FindVertex(7), graph.FindVertex(1)));
            graph.Edges.Add(new Edge(graph.FindVertex(7), graph.FindVertex(8)));
            graph.Edges.Add(new Edge(graph.FindVertex(8), graph.FindVertex(5)));
            graph.Edges.Add(new Edge(graph.FindVertex(8), graph.FindVertex(2)));

            BreadthFirstSearch bfs = new BreadthFirstSearch();
            bfs.Run(graph);
            string[] profileOut = graph.Profile.Split('?');
            //Debug.WriteLine();
        }

        [TestMethod]
        public void BoehmTest06()
        {
            Graph graph = new Graph();
            for (int i = 0; i < 8; i++)
            {
                graph.Vertices.Add(new Vertex(i + 1));
            }
            graph.Edges.Add(new Edge(graph.FindVertex(1), graph.FindVertex(2)));
            graph.Edges.Add(new Edge(graph.FindVertex(2), graph.FindVertex(3)));
            graph.Edges.Add(new Edge(graph.FindVertex(3), graph.FindVertex(1)));
            graph.Edges.Add(new Edge(graph.FindVertex(2), graph.FindVertex(4)));
            graph.Edges.Add(new Edge(graph.FindVertex(3), graph.FindVertex(4)));
            graph.Edges.Add(new Edge(graph.FindVertex(4), graph.FindVertex(5)));
            graph.Edges.Add(new Edge(graph.FindVertex(5), graph.FindVertex(6)));
            graph.Edges.Add(new Edge(graph.FindVertex(4), graph.FindVertex(6)));
            graph.Edges.Add(new Edge(graph.FindVertex(6), graph.FindVertex(1)));
            graph.Edges.Add(new Edge(graph.FindVertex(7), graph.FindVertex(1)));
            graph.Edges.Add(new Edge(graph.FindVertex(7), graph.FindVertex(8)));
            graph.Edges.Add(new Edge(graph.FindVertex(8), graph.FindVertex(5)));
            graph.Edges.Add(new Edge(graph.FindVertex(8), graph.FindVertex(2)));

            BreadthFirstSearch bfs = new BreadthFirstSearch();
            bfs.Run(graph);
            string[] profileOut = graph.Profile.Split('?');
            //Debug.WriteLine();
        }

        [TestMethod]
        public void BoehmTest07()
        {
            Graph graph = new Graph();
            for (int i = 0; i < 8; i++)
            {
                graph.Vertices.Add(new Vertex(i + 1));
            }
            graph.Edges.Add(new Edge(graph.FindVertex(1), graph.FindVertex(2)));
            graph.Edges.Add(new Edge(graph.FindVertex(2), graph.FindVertex(3)));
            graph.Edges.Add(new Edge(graph.FindVertex(3), graph.FindVertex(4)));
            graph.Edges.Add(new Edge(graph.FindVertex(3), graph.FindVertex(5)));
            graph.Edges.Add(new Edge(graph.FindVertex(4), graph.FindVertex(5)));
            graph.Edges.Add(new Edge(graph.FindVertex(5), graph.FindVertex(6)));
            graph.Edges.Add(new Edge(graph.FindVertex(6), graph.FindVertex(7)));
            graph.Edges.Add(new Edge(graph.FindVertex(7), graph.FindVertex(8)));
            graph.Edges.Add(new Edge(graph.FindVertex(8), graph.FindVertex(4)));
            graph.Edges.Add(new Edge(graph.FindVertex(7), graph.FindVertex(1)));
            graph.Edges.Add(new Edge(graph.FindVertex(6), graph.FindVertex(1)));
            graph.Edges.Add(new Edge(graph.FindVertex(5), graph.FindVertex(1)));

            BreadthFirstSearch bfs = new BreadthFirstSearch();
            bfs.Run(graph);
            string[] profileOut = graph.Profile.Split('?');
            //Debug.WriteLine();
        }

        [TestMethod]
        public void BoehmTest08()
        {
            Graph graph = new Graph();
            for (int i = 0; i < 8; i++)
            {
                graph.Vertices.Add(new Vertex(i + 1));
            }
            graph.Edges.Add(new Edge(graph.FindVertex(1), graph.FindVertex(2)));
            graph.Edges.Add(new Edge(graph.FindVertex(2), graph.FindVertex(3)));
            graph.Edges.Add(new Edge(graph.FindVertex(3), graph.FindVertex(1)));
            graph.Edges.Add(new Edge(graph.FindVertex(2), graph.FindVertex(4)));
            graph.Edges.Add(new Edge(graph.FindVertex(3), graph.FindVertex(4)));
            graph.Edges.Add(new Edge(graph.FindVertex(4), graph.FindVertex(5)));
            graph.Edges.Add(new Edge(graph.FindVertex(5), graph.FindVertex(8)));
            graph.Edges.Add(new Edge(graph.FindVertex(6), graph.FindVertex(1)));
            graph.Edges.Add(new Edge(graph.FindVertex(6), graph.FindVertex(7)));
            graph.Edges.Add(new Edge(graph.FindVertex(7), graph.FindVertex(1)));
            graph.Edges.Add(new Edge(graph.FindVertex(7), graph.FindVertex(8)));
            graph.Edges.Add(new Edge(graph.FindVertex(8), graph.FindVertex(2)));

            BreadthFirstSearch bfs = new BreadthFirstSearch();
            bfs.Run(graph);
            string[] profileOut = graph.Profile.Split('?');
            //Debug.WriteLine();
        }

        #endregion Public Methods

        #region Private Methods

        private Graph createGraph()
        {
            Vertex one = new Vertex(1);
            Vertex two = new Vertex(2);
            Vertex three = new Vertex(3);
            Vertex four = new Vertex(4);
            Vertex five = new Vertex(5);
            Vertex six = new Vertex(6);
            Vertex seven = new Vertex(7);

            Graph graph = new Graph();

            graph.Vertices.Add(one);
            graph.Vertices.Add(two);
            graph.Vertices.Add(three);
            graph.Vertices.Add(four);
            graph.Vertices.Add(five);
            graph.Vertices.Add(six);
            graph.Vertices.Add(seven);

            //graph.Edges.Add(new Edge(one, two));
            //graph.Edges.Add(new Edge(one, three));
            //graph.Edges.Add(new Edge(one, four));
            //graph.Edges.Add(new Edge(five, four));
            //graph.Edges.Add(new Edge(two, six));
            //graph.Edges.Add(new Edge(one, seven));
            //graph.Edges.Add(new Edge(six, seven));
            //graph.Edges.Add(new Edge(five, six));
            //graph.Edges.Add(new Edge(six, one));
            //graph.Edges.Add(new Edge(four, seven));

            graph.Edges.Add(new Edge(one, two));
            graph.Edges.Add(new Edge(one, three));
            graph.Edges.Add(new Edge(two, three));
            graph.Edges.Add(new Edge(one, four));
            graph.Edges.Add(new Edge(one, five));
            graph.Edges.Add(new Edge(four, five));
            graph.Edges.Add(new Edge(one, six));
            graph.Edges.Add(new Edge(two, seven));
            graph.Edges.Add(new Edge(six, seven));
            graph.Edges.Add(new Edge(seven, four));

            //Graph graph = new Graph();

            //for (int i = 1; i < 21; i++)
            //{
            //    graph.Vertices.Add(new Vertex(i));
            //}

            //graph.Edges.Add(new Edge(graph.FindVertex(1), graph.FindVertex(2)));
            //graph.Edges.Add(new Edge(graph.FindVertex(1), graph.FindVertex(3)));
            //graph.Edges.Add(new Edge(graph.FindVertex(1), graph.FindVertex(4)));
            //graph.Edges.Add(new Edge(graph.FindVertex(4), graph.FindVertex(5)));
            //graph.Edges.Add(new Edge(graph.FindVertex(5), graph.FindVertex(6)));
            //graph.Edges.Add(new Edge(graph.FindVertex(6), graph.FindVertex(7)));
            //graph.Edges.Add(new Edge(graph.FindVertex(6), graph.FindVertex(9)));
            //graph.Edges.Add(new Edge(graph.FindVertex(7), graph.FindVertex(9)));
            //graph.Edges.Add(new Edge(graph.FindVertex(9), graph.FindVertex(8)));
            //graph.Edges.Add(new Edge(graph.FindVertex(5), graph.FindVertex(11)));
            //graph.Edges.Add(new Edge(graph.FindVertex(11), graph.FindVertex(10)));
            //graph.Edges.Add(new Edge(graph.FindVertex(11), graph.FindVertex(12)));
            //graph.Edges.Add(new Edge(graph.FindVertex(4), graph.FindVertex(17)));
            //graph.Edges.Add(new Edge(graph.FindVertex(17), graph.FindVertex(13)));
            //graph.Edges.Add(new Edge(graph.FindVertex(13), graph.FindVertex(14)));
            //graph.Edges.Add(new Edge(graph.FindVertex(17), graph.FindVertex(15)));
            //graph.Edges.Add(new Edge(graph.FindVertex(15), graph.FindVertex(16)));
            //graph.Edges.Add(new Edge(graph.FindVertex(3), graph.FindVertex(20)));
            //graph.Edges.Add(new Edge(graph.FindVertex(20), graph.FindVertex(18)));
            //graph.Edges.Add(new Edge(graph.FindVertex(18), graph.FindVertex(19)));
            //graph.Edges.Add(new Edge(graph.FindVertex(20), graph.FindVertex(19)));

            return graph;
        }

        #endregion Private Methods
    }
}