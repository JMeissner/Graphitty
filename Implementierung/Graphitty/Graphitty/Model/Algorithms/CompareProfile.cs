using Graphitty.Model.Graphs;

namespace Graphitty.Model.Algorithms
{
    public class CompareProfile : Comparison
    {
        #region Public Constructors

        /// <summary>
        /// Constructor of CompareProfile. It was intentionally left empty.
        /// </summary>
        public CompareProfile()
        {
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// Compares the profiles of two graphs and increments the numberDenserGraphsProfile of the graph that is less dense.
        /// </summary>
        /// <param name="graph">The current graph</param>
        /// <param name="dbGraph">A graph from the database</param>
        public override void Run(Graph graph, GraphEntity dbGraph)
        {
            if (!graph.CompareProfile(dbGraph))
            {
                dbGraph.NumDenserGraphsProfile++;
            }
            else
            {
                graph.NumDenserGraphsProfile++;
            }
        }

        #endregion Public Methods
    }
}