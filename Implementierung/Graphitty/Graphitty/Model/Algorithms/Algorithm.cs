using Graphitty.Model.Graphs;

namespace Graphitty.Model.Algorithms
{
    public abstract class Algorithm
    {
        #region Public Methods

        /// <summary>
        /// Runs calculations over a graph.
        /// </summary>
        /// <param name="graph">The current graph</param>
        public abstract void Run(Graph graph);

        #endregion Public Methods
    }
}