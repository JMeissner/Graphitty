using System.Collections.Generic;
using Graphitty.Model.Graphs;

namespace Graphitty.Model.GraphGeneration.Factories
{
    public class FixedNumVerticesFactory : IVertexFactory
    {
        #region Private Fields

        private int numVertices;

        #endregion Private Fields

        #region Public Constructors

        public FixedNumVerticesFactory()
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public int NumVertices { get => numVertices; set => numVertices = value; }

        #endregion Public Properties

        #region Public Methods

        public ICollection<Vertex> CreateVertices()
        {
            List<Vertex> _vertices = new List<Vertex>();
            for (int i = 0; i < numVertices; i++)
            {
                Vertex _vert = new Vertex(i + 1);
                _vertices.Add(_vert);
            }
            return _vertices;
        }

        #endregion Public Methods
    }
}