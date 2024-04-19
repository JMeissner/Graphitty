using Graphitty.Model.Graphs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Graphitty.Model.GraphGeneration
{
    public class GraphBuilder
    {
        #region Private Fields

        private static Random random;

        #endregion Private Fields

        #region Public Constructors

        public GraphBuilder(IVertexFactory vertexFactory, IEdgeFactory edgeFactory)
        {
            VertexFactory = vertexFactory;
            EdgeFactory = edgeFactory;
            random = new Random();
        }

        #endregion Public Constructors

        #region Public Properties

        public IEdgeFactory EdgeFactory { get; set; }
        public IVertexFactory VertexFactory { get; set; }

        #endregion Public Properties

        #region Public Methods

        ///<summary>
        /// Returns a graph by retrieving a collection of vertices from VertexFactory
        /// and then adding edges to those vertices.
        /// </summary>
        public Graph BuildGraph()
        {
            IEdgeFactory edgeFactory = EdgeFactory;
            IVertexFactory vertexFactory = VertexFactory;
            List<Vertex> _vertexList = vertexFactory.CreateVertices().ToList<Vertex>();
            Graph newGraph = edgeFactory.AddEdges(_vertexList);
            if (newGraph != null)
            {
                newGraph.NumVertices = newGraph.Vertices.Count;
                newGraph.NumEdges = newGraph.Edges.Count;
            }
            return newGraph;
        }

        #endregion Public Methods
    }
}