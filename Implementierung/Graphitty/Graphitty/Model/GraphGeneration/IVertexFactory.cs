using Graphitty.Model.Graphs;
using System.Collections.Generic;

namespace Graphitty.Model.GraphGeneration
{
    public interface IVertexFactory
    {
        #region Public Methods

        ICollection<Vertex> CreateVertices();

        #endregion Public Methods
    }
}