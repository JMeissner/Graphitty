using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using Graphitty.Model.Graphs;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Graphitty.Model.Algorithms
{
    public class TotalColoring : Algorithm
    {
        #region Private Fields

        private bool foundColoring;
        private Graph g;
        private Dictionary<Vertex, Edge> originalEdgeDictionary;
        private int TCN;

        #endregion Private Fields

        #region Public Constructors

        public TotalColoring()
        {
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// Colors a graph with the already calculated total chromatic number and returns it.
        /// </summary>
        /// <param name="graphToColor"></param>
        /// <returns></returns>
        [ExcludeFromCodeCoverageAttribute]
        public Graph ColorGraph(Graph graphToColor)
        {
            Graph editableGraph = graphToColor;
            //If graph is a line, we need to fix the numbering
            if (graphToColor.MaxVertexDegree == 2 && graphToColor.AverageVertexDegree < 2)
            {
                editableGraph = FixLineNumbering(graphToColor);
            }
            //Set the number of possible retrys to get a valid coloring
            int _numberOfRetrys = 25;
            //Try to color, if it fails, reshuffle vertex IDs and begin again
            while (_numberOfRetrys > 0)
            {
                g = ConvertEdgesToVertices(editableGraph);
                VertexColor(1, graphToColor.TotalChromaticNumber);

                bool isColored = true;
                foreach (Vertex vertex in g.Vertices)
                {
                    if (vertex.Color == 0)
                    {
                        isColored = false;
                    }
                }
                if (isColored) { break; }
                Debug.WriteLine("shuffleing vertices");
                editableGraph = ShuffleVertexIDs(graphToColor);
                _numberOfRetrys--;
                Debug.WriteLine("Coloring incorrect. Try again... Retrys: " + _numberOfRetrys);
            }
            Debug.WriteLine("found coloring");
            foreach (Vertex vertex in g.Vertices)
            {
                if (vertex.ID <= graphToColor.Vertices.Count)
                {
                    Vertex originalVertex = editableGraph.FindVertex(vertex.ID);
                    originalVertex.VColor = IntToBrush(vertex.Color);
                }
                else
                {
                    Edge originalEdge = originalEdgeDictionary[vertex];
                    graphToColor.Edges.Remove(originalEdge);
                    originalEdge.EColor = IntToBrush(vertex.Color);
                    graphToColor.Edges.Add(originalEdge);
                }
            }
            return graphToColor;
        }

        public override void Run(Graph graph)
        {
            if (graph.MaxVertexDegree == 2 && graph.AverageVertexDegree < 2)
            {
                //write data into graph
                graph.TotalChromaticNumber = 3;
                graph.MinimalColouringComplexityInMiliSeconds = 0;
                if (TCN <= (graph.MaxVertexDegree + 2)) { graph.IsTCCFulfilled = true; } else { graph.IsTCCFulfilled = false; }
                return;
            }
            //save elapsed time
            var watch = System.Diagnostics.Stopwatch.StartNew();
            g = ConvertEdgesToVertices(graph);
            TCN = -1;
            foundColoring = false;

            int colorsToUse = graph.MaxVertexDegree;
            //try different colorings and increment amount of colors
            while (!foundColoring)
            {
                VertexColor(1, colorsToUse);
                //Checks for illegal colorings
                for (int i = 0; i < g.Vertices.Count; i++)
                {
                    if (g.Vertices.ElementAt(i).Color == 0) { foundColoring = false; }
                }
                colorsToUse++;
            }
            //found coloring
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            var elapsedSec = elapsedMs / 1000;

            //write data into graph
            graph.TotalChromaticNumber = TCN;
            graph.MinimalColouringComplexityInMiliSeconds = (int)elapsedMs;
            if (TCN <= (graph.MaxVertexDegree + 2)) { graph.IsTCCFulfilled = true; } else { graph.IsTCCFulfilled = false; }
            //debugColoring();
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Checks whether a given vertex can be colored with a given color
        /// </summary>
        /// <param name="id"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        private bool CanColor(int id, int color)
        {
            Vertex temp = new Vertex(id);
            List<Edge> adjacentEdges = g.FindEdges(temp);
            foreach (Edge e in adjacentEdges)
            {
                if (!e.Vertex1.Equals(temp))
                {
                    if (e.Vertex1.Color == color) { return false; }
                }
                if (!e.Vertex2.Equals(temp))
                {
                    if (e.Vertex2.Color == color) { return false; }
                }
            }
            return true;
        }

        /// <summary>
        /// Takes all edges from a graph and converts them into a new vertex, which is connected with the two vertices from the edge.
        /// Then every vertex representing an edge is connected with all adjacent edges, respectively with the vertices representing them.
        /// This is done, so that a graph can be total colored by just coloring the vertices.
        /// </summary>
        /// <param name="graph"></param>
        /// <returns></returns>
        private Graph ConvertEdgesToVertices(Graph graph)
        {
            Graph convertedGraph = new Graph()
            {
                Vertices = new List<Vertex>(graph.Vertices),
                Edges = new List<Edge>(graph.Edges),
                BFSCode = graph.BFSCode,
                BFSCodeBitvector = graph.BFSCodeBitvector,
                MaxVertexDegree = graph.MaxVertexDegree,
                AverageVertexDegree = graph.AverageVertexDegree,
            };
            originalEdgeDictionary = new Dictionary<Vertex, Edge>();
            List<Edge> edges = new List<Edge>(convertedGraph.Edges);
            int originalNumVerts = graph.Vertices.Count;
            int index = originalNumVerts;
            //every edge is replaced with a vertex
            foreach (Edge originalEdge in edges)
            {
                Vertex tempVertex = new Vertex(++index);
                Edge tempEdge1 = new Edge(tempVertex, originalEdge.Vertex1);
                Edge tempEdge2 = new Edge(tempVertex, originalEdge.Vertex2);
                originalEdgeDictionary.Add(tempVertex, originalEdge);
                convertedGraph.Vertices.Add(tempVertex);
                convertedGraph.Edges.Add(tempEdge1);
                convertedGraph.Edges.Add(tempEdge2);
            }

            //all edges represented by vertices are being connected with all adjacent edges
            List<Vertex> vertices = new List<Vertex>(graph.Vertices);
            foreach (Vertex originalVertex in vertices)
            {
                List<Edge> adjacentEdges = convertedGraph.FindEdges(originalVertex);
                List<Edge> adjacentNewEdges = new List<Edge>(adjacentEdges);
                foreach (Edge edge in adjacentEdges)
                {
                    if (edge.Vertex1.ID <= originalNumVerts && edge.Vertex2.ID <= originalNumVerts)
                    {
                        adjacentNewEdges.Remove(edge);
                    }
                }
                List<Edge> unsatisfiedEdges = new List<Edge>(adjacentNewEdges);
                adjacentNewEdges.Remove(adjacentNewEdges.Last()); //at the end this edge will be passively connected to all other edges
                foreach (Edge edge in adjacentNewEdges)
                {
                    Edge sourceEdge = edge;
                    Vertex sourceVertex = sourceEdge.Vertex1.ID > originalNumVerts ? sourceEdge.Vertex1 : sourceEdge.Vertex2;
                    unsatisfiedEdges.Remove(sourceEdge);
                    foreach (Edge targetEdge in unsatisfiedEdges)
                    {
                        Vertex targetVertex = targetEdge.Vertex1.ID > originalNumVerts ? targetEdge.Vertex1 : targetEdge.Vertex2;
                        Edge traversingEdge = new Edge(sourceVertex, targetVertex);
                        convertedGraph.Edges.Add(traversingEdge);
                    }
                }
            }
            return convertedGraph;
        }

        [ExcludeFromCodeCoverageAttribute]
        private void debugColoring()
        {
            foreach (Edge e in g.Edges)
            {
                Debug.WriteLine(e.Vertex1.ID + "|" + e.Vertex1.Color + "<->" + e.Vertex2.ID + "|" + e.Vertex2.Color);
            }
        }

        /// <summary>
        /// Adjusts vertex ids to be able to color line graphs
        /// </summary>
        [ExcludeFromCodeCoverageAttribute]
        private Graph FixLineNumbering(Graph GraphToFix)
        {
            int _startVertexID = -1;
            foreach (Vertex v in GraphToFix.Vertices)
            {
                if (v.Degree == 1) { _startVertexID = v.ID; }
            }

            for (int i = 1; i <= GraphToFix.Vertices.Count; i++)
            {
                Vertex vertexToNumber = GraphToFix.FindVertex(_startVertexID);
                _startVertexID = GraphToFix.FindEdges(vertexToNumber).First().GetOtherVertex(vertexToNumber).ID;
                vertexToNumber.ID = i;
            }
            return GraphToFix;
        }

        //TODO: distinct color algorithmus
        [ExcludeFromCodeCoverageAttribute]
        private Brush IntToBrush(int c)
        {
            switch (c)
            {
                case 1: return Brushes.Red;
                case 2: return Brushes.Blue;
                case 3: return Brushes.Gold;
                case 4: return Brushes.Green;
                case 5: return Brushes.LawnGreen;
                case 6: return Brushes.Orange;
                case 7: return Brushes.LightGreen;
                case 8: return Brushes.LightSkyBlue;
                case 9: return Brushes.MediumOrchid;
                case 10: return Brushes.Violet;
                case 11: return Brushes.Maroon;
                case 12: return Brushes.LightGoldenrodYellow;
                case 13: return Brushes.HotPink;
                case 14: return Brushes.DarkSeaGreen;
                case 15: return Brushes.PowderBlue;
                case 16: return Brushes.DodgerBlue;
                case 17: return Brushes.Thistle;
                case 18: return Brushes.MistyRose;
                case 19: return Brushes.DimGray;
                case 20: return Brushes.Crimson;
                case 21: return Brushes.SandyBrown;
                case 22: return Brushes.DarkKhaki;
                case 23: return Brushes.Olive;
                case 24: return Brushes.LightCyan;
                case 25: return Brushes.DarkTurquoise;
                case 26: return Brushes.Purple;
                case 27: return Brushes.DarkGray;
                case 28: return Brushes.Chocolate;
                case 29: return Brushes.Khaki;
                case 30: return Brushes.DarkSlateGray;
                case 31: return Brushes.Indigo;
                case 32: return Brushes.Fuchsia;
                case 33: return Brushes.LightCoral;
                case 34: return Brushes.LightGray;
                case 35: return Brushes.Sienna;
                case 36: return Brushes.DarkGoldenrod;
                case 37: return Brushes.SpringGreen;
                default: return Brushes.Black;
            }
        }

        /// <summary>
        /// Randomly assigns new numbers to vertices, so we may find a coloring
        /// </summary>
        [ExcludeFromCodeCoverageAttribute]
        private Graph ShuffleVertexIDs(Graph GraphToShuffle)
        {
            List<int> unShuffledIDs = new List<int>();
            for (int i = 1; i <= GraphToShuffle.Vertices.Count; i++)
            {
                unShuffledIDs.Add(i);
            }
            Random random = new Random();
            foreach (Vertex v in GraphToShuffle.Vertices)
            {
                int randomID = random.Next(1, GraphToShuffle.Vertices.Count);
                while (!unShuffledIDs.Contains(randomID))
                {
                    Debug.WriteLine("RandomID: " + randomID);
                    randomID = random.Next(1, GraphToShuffle.Vertices.Count + 1);
                }
                v.ID = randomID;
                unShuffledIDs.Remove(randomID);
            }
            return GraphToShuffle;
        }

        /// <summary>
        /// Colors the vertices using backtracking
        /// </summary>
        /// <param name="id"></param>
        /// <param name="maxColors"></param>
        private void VertexColor(int id, int maxColors)
        {
            for (int c = 1; c <= maxColors; c++)
            {
                if (CanColor(id, c))
                {
                    g.FindVertex(id).Color = c;
                    if (((id + 1) < g.Vertices.Count + 1))
                    {
                        VertexColor(id + 1, maxColors);
                    }
                    else
                    {
                        foundColoring = true;
                        TCN = maxColors;
                        return;
                    }
                }
            }
        }

        #endregion Private Methods
    }
}