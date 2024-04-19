using Graphitty.Model.Graphs;

namespace Graphitty.Model.Algorithms
{
    public abstract class Comparison
    {
        #region Public Methods

        /// <summary>
        /// Compares two graphs and does something with them afterwards.
        /// </summary>
        /// <param name="actualGraph">The current graph</param>
        /// <param name="dbGraph">A graph from the databank</param>
        public abstract void Run(Graph actualGraph, GraphEntity dbGraph);

        #endregion Public Methods
    }
}