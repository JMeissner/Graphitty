using Graphitty.Model.Graphs;
using System.Collections.Generic;

namespace Graphitty.Model.GraphGeneration
{
    public interface IEdgeFactory
    {
        #region Public Methods

        Graph AddEdges(ICollection<Vertex> vertices);

        #endregion Public Methods
    }
}