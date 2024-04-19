using System;
using System.Windows.Media;

namespace Graphitty.Model.Graphs
{
    public class Edge : IEquatable<Edge>
    {
        #region Public Properties

        public Brush EColor { get; set; }
        public Vertex Vertex1 { get; set; }
        public Vertex Vertex2 { get; set; }
        public bool Visited { get; set; }

        #endregion Public Properties

        #region Public Constructors

        /// <summary>
        /// Creates a edge with the given vertices.
        /// </summary>
        /// <param name="vertex1">First vertex of the Edge.</param>
        /// <param name="vertex2">Second vertex of the edge.</param>
        public Edge(Vertex vertex1, Vertex vertex2)
        {
            Vertex1 = vertex1;
            Vertex2 = vertex2;
            EColor = Brushes.Gray;
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// Checks if the edge includes the given vertex.
        /// </summary>
        /// <param name="vertex">The vertex to check.</param>
        /// <returns>Returns true if the edge contains the vertex, else false.</returns>
        public bool ContainsVertex(Vertex vertex)
        {
            return Vertex1.Equals(vertex) || Vertex2.Equals(vertex);
        }

        /// <summary>
        /// Compares two Edges.
        /// </summary>
        /// <param name="edge">The edge to compare with.</param>
        /// <returns>Returns true if the edges are equal, else false.</returns>
        public bool Equals(Edge other)
        {
            return (other.Vertex1.Equals(Vertex1) || other.Vertex1.Equals(Vertex2)) && (other.Vertex2.Equals(Vertex1) || other.Vertex2.Equals(Vertex2));
        }

        /// <summary>
        /// Returns the other vertex of the edge
        /// </summary>
        /// <param name="vertex">The vertex to check.</param>
        /// <returns>Returns the other vertex of the edge. If it doesnt exists, it returns null</returns>
        public Vertex GetOtherVertex(Vertex vertex)
        {
            if (Vertex1.Equals(vertex)) { return Vertex2; }
            if (Vertex2.Equals(vertex)) { return Vertex1; }
            return null;
        }

        /// <summary>
        /// Creates an array of vertices from the edge.
        /// </summary>
        /// <returns>Returns an array of vertices.</returns>
        public Vertex[] GetVertices()
        {
            Vertex[] vertices = new Vertex[2];
            vertices[0] = Vertex1;
            vertices[1] = Vertex2;
            return vertices;
        }

        /// <summary>
        /// Increments the degree of the vertices from the edge.
        /// </summary>
        public void IncrementDegrees()
        {
            Vertex1.Degree++;
            Vertex2.Degree++;
        }

        #endregion Public Methods
    }
}