using System;
using System.Collections.Generic;
using System.Linq;

namespace Graphitty.Model.Graphs
{
    public class Graph : GraphEntity
    {
        #region Private Methods

        //To add vertex degree to vertices, used for the standart constructor with a list of vertices and edges in argument
        private void addVertexDegreeToVertices(List<Vertex> verticesOfGraph, List<Edge> edgesOfGraph)
        {
            int degree;
            foreach (Vertex ver in verticesOfGraph)
            {
                degree = 0;
                foreach (Edge ed in edgesOfGraph)
                {
                    if (ed.ContainsVertex(ver))
                    {
                        degree++;
                    }
                }
                ver.Degree = degree;
            }
        }

        //Turns a given Profil String into a BFSCode.Matrix
        private BFSCode[,] getProfilMatrix(String pro)
        {
            //Fragments are seperated by ? from each other.
            String[] splitProfil = pro.Split('?');
            int dimension = (int)Math.Sqrt(splitProfil.Length);
            BFSCode[,] profileMatrix = new BFSCode[dimension, dimension];
            int fragmentCounter = 0;

            for (int i = 0; i < dimension; i++)
            {
                for (int j = 0; j < dimension; j++)
                {
                    profileMatrix[i, j] = new BFSCode(splitProfil[fragmentCounter]);
                    fragmentCounter++;
                }
            }
            return profileMatrix;
        }

        #endregion Private Methods

        #region Private Fields

        private List<Edge> edges;
        private List<Vertex> vertices;

        #endregion Private Fields

        #region Public Properties

        public List<Edge> Edges { get => edges; set => edges = value; }
        public List<Vertex> Vertices { get => vertices; set => vertices = value; }

        #endregion Public Properties

        #region Public Constructors

        public Graph()
        {
            vertices = new List<Vertex>();
            edges = new List<Edge>();

            NumEdges = edges.Count();
            NumVertices = vertices.Count();
        }

        /// <summary>
        /// Creates a graph from BFSCode.
        /// </summary>
        /// <param name="bfsCode">The BFSCode of a Graph</param>
        public Graph(string bfsCode)
        {
            vertices = new List<Vertex>();
            edges = new List<Edge>();
            BFSCode bfs = new BFSCode(bfsCode);
            Vertex vertex1;
            Vertex vertex2;
            Edge edge;

            foreach (int[] edgeCode in bfs.BFSEdges)
            {
                vertex1 = new Vertex(edgeCode[1]);
                vertex2 = new Vertex(edgeCode[2]);
                edge = new Edge(vertex1, vertex2);
                if (!ContainsVertex(vertex1))
                {
                    vertices.Add(vertex1);
                }

                if (!ContainsVertex(vertex2))
                {
                    vertices.Add(vertex2);
                }

                if (!ContainsEdge(edge))
                {
                    Edge newEdge = new Edge(FindVertex(edgeCode[1]), FindVertex(edgeCode[2]));
                    newEdge.IncrementDegrees();
                    edges.Add(newEdge);
                }
            }

            NumEdges = edges.Count();
            NumVertices = vertices.Count();
            BFSCode = bfsCode;
            BFSCodeBitvector = bfs.GetBitVector();
        }

        /// <summary>
        /// Creates a graph with given lists of vertices and edges.
        /// </summary>
        /// <param name="vertices">List of vertices of the Graph.</param>
        /// <param name="edges">List of edges of the Graph.</param>
        public Graph(List<Vertex> vertices, List<Edge> edges)
        {
            this.vertices = vertices;
            this.edges = edges;

            addVertexDegreeToVertices(this.vertices, this.edges);

            NumEdges = this.edges.Count();
            NumVertices = this.vertices.Count();
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// Compares two graphs at edges and vertices.
        /// </summary>
        /// <param name="graph">The graph to compare with.</param>
        /// <returns>Returns true at equal graphs, else false.</returns>
        public bool Compare(Graph graph)
        {
            foreach (Vertex ver in vertices)
            {
                if (!graph.vertices.Contains(ver))
                {
                    return false;
                }
            }

            foreach (Vertex ver in graph.vertices)
            {
                if (!vertices.Contains(ver))
                {
                    return false;
                }
            }

            foreach (Edge ed in Edges)
            {
                if (!graph.edges.Contains(ed))
                {
                    return false;
                }
            }

            foreach (Edge ed in graph.Edges)
            {
                if (!edges.Contains(ed))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Compares two graphs at BFS-Density.
        /// </summary>
        /// <param name="graph">The graph to compare with.</param>
        /// <returns>Returns 1 if given graph is less dense, returns 0 if they have the same density,
        /// returns -1 if given graph is denser. </returns>
        public int CompareBFS(GraphEntity graph)
        {
            BFSCode BFSActualGraph = new BFSCode(BFSCode);
            BFSCode BFSGivenGraph = new BFSCode(graph.BFSCode);

            return BFSActualGraph.CompareTo(BFSGivenGraph);
        }

        /// <summary>
        /// Compares two graphs at Profil-Density.
        /// </summary>
        /// <param name="graph">The graph to compare with.</param>
        /// <returns>Returns true if given graph is denser, returns false if given graph is less dense.</returns>
        public bool CompareProfile(GraphEntity graph)
        {
            BFSCode[,] profile1 = getProfilMatrix(Profile);
            BFSCode[,] profile2 = getProfilMatrix(graph.Profile);
            int compareInt;

            for (int i = 0; i < profile1.GetLength(1); i++)
            {
                for (int j = 0; j < profile1.GetLength(0); j++)
                {
                    compareInt = profile1[j, i].CompareTo(profile2[j, i]);
                    if (compareInt == 0)
                    {
                    }
                    else if (compareInt == -1)
                    {
                        return false;
                    }
                    else if (compareInt == 1)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Checks if the edge is in the graph.
        /// </summary>
        /// <param name="edge">The edge to check.</param>
        /// <returns>Returns true if the graph contains the edge, else false.</returns>
        public bool ContainsEdge(Edge edge)
        {
            return edges.Contains(edge);
        }

        /// <summary>
        /// Checks if the Graph contains the Vertex.
        /// </summary>
        /// <param name="vertex">The vertex to check.</param>
        /// <returns>Returns true if the graph contains the vertex, else false.</returns>
        public bool ContainsVertex(Vertex vertex)
        {
            return vertices.Contains(vertex);
        }

        /// <summary>
        /// Creates a list of edges containing the given vertex.
        /// </summary>
        /// <param name="vertex">The vertex to search for.</param>
        /// <returns>Returns a list of edges containing given vertex.</returns>
        public List<Edge> FindEdges(Vertex vertex)
        {
            List<Edge> edgesContainsVertex = new List<Edge>();
            foreach (Edge ed in Edges)
            {
                if (ed.ContainsVertex(vertex))
                {
                    edgesContainsVertex.Add(ed);
                }
            }
            return edgesContainsVertex;
        }

        /// <summary>
        /// Checks if a vertex with given id is in the Graph.
        /// </summary>
        /// <param name="id">The id of the vertex to search for.</param>
        /// <returns>Returns the vertex if he exists, else null.</returns>
        public Vertex FindVertex(int id)
        {
            foreach (Vertex ver in Vertices)
            {
                if (ver.ID == id)
                {
                    return ver;
                }
            }
            return null;
        }

        #endregion Public Methods
    }
}