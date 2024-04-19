using Graphitty.Model.Graphs;

namespace Graphitty.Model.Algorithms
{
    public class CompareBFS : Comparison
    {
        #region Public Constructors

        /// <summary>
        /// The constructor of CompareBFS. It is intentionally left empty.
        /// </summary>
        public CompareBFS() { }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// Compares two BFS-Codes and increments the NumDenserGraphsBFS property of the graph that is denser.
        /// </summary>
        /// <param name="graph">The current graph</param>
        /// <param name="dbGraph">A graph from the databank</param>
        public override void Run(Graph graph, GraphEntity dbGraph)
        {
            if (graph.CompareBFS(dbGraph) == 1)
            {
                graph.NumDenserGraphsBFS++;
            }
            else if (graph.CompareBFS(dbGraph) == -1)
            {
                dbGraph.NumDenserGraphsBFS++;
            }
        }

        #endregion Public Methods
    }
}