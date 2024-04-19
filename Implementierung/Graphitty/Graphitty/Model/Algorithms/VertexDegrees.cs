using Graphitty.Model.Graphs;
using System.Linq;

namespace Graphitty.Model.Algorithms
{
    public class VertexDegrees : Algorithm
    {
        #region Public Constructors

        /// <summary>
        /// Creates a VertexDegrees Object. Intentionally left empty.
        /// </summary>
        public VertexDegrees() { }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// Runs a graph through all the private methods. This is a template method.
        /// </summary>
        /// <param name="graph">The current graph</param>
        public override void Run(Graph graph)
        {
            graph.AverageVertexDegree = findAverageVertexDegree(graph);
            graph.MaxVertexDegree = findMaxVertexDegree(graph);
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Calculates the average vertex degree of a graph.
        /// </summary>
        /// <param name="graph">The current graph</param>
        /// <returns>Returns the average vertex degree.</returns>
        private double findAverageVertexDegree(Graph graph)
        {
            return graph.Vertices.Select(v => v.Degree).Average();
        }

        /// <summary>
        /// Finds the maximum vertex degree of a graph.
        /// </summary>
        /// <param name="graph">The current graph</param>
        /// <returns>Returns the maximum vertex degree.</returns>
        private int findMaxVertexDegree(Graph graph)
        {
            int max = 0;

            foreach (Vertex vertex in graph.Vertices)
            {
                if (vertex.Degree > max)
                {
                    max = vertex.Degree;
                }
            }

            return max;
        }

        #endregion Private Methods
    }
}