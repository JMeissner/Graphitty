using Graphitty.Model.Graphs;
using System;
using System.Collections.Generic;

namespace Graphitty.Model.GraphGeneration.Factories
{
    public class MaxVerticesFactory : IVertexFactory
    {
        #region Private Fields

        private static Random random;
        private int maxVertices;

        #endregion Private Fields

        #region Public Constructors

        public MaxVerticesFactory()
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public int MaxVertices { get => maxVertices; set => maxVertices = value; }

        #endregion Public Properties

        #region Public Methods

        ///<summary>
        /// Creates a collection of vertices which doesn't contain more vertices
        /// than what the property MaxVertices is set to and returns that collection.
        /// </summary>
        public ICollection<Vertex> CreateVertices()
        {
            random = new Random();
            int _numVertices = random.Next(2, maxVertices + 1);
            List<Vertex> _vertices = new List<Vertex>();
            for (int i = 0; i < _numVertices; i++)
            {
                Vertex _vert = new Vertex(i + 1);
                _vertices.Add(_vert);
            }
            return _vertices;
        }

        #endregion Public Methods
    }
}