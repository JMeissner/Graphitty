using Graphitty.Model.Graphs;

namespace Graphitty.Model.Algorithms
{
    public class CompareTotalChromaticNumber : Comparison
    {
        #region Public Constructors

        /// <summary>
        /// Constructor of CompareTotalChromaticNumber. It was intentionally left empty.
        /// </summary>
        public CompareTotalChromaticNumber()
        {
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// Compares the total chromatic numbers of two graphs
        /// and increments the NumGraphWithSmallerEqualChromaticNumber property of the graph,
        /// that has the bigger or equal chromatic number.
        /// </summary>
        /// <param name="graph">The current graph</param>
        /// <param name="dbGraph">A graph from the databank</param>
        public override void Run(Graph graph, GraphEntity dbGraph)
        {
            if (graph.TotalChromaticNumber < dbGraph.TotalChromaticNumber)
            {
                dbGraph.NumGraphsWithSmallerEqualChromaticNumber++;
            }
            else if (graph.TotalChromaticNumber > dbGraph.TotalChromaticNumber)
            {
                graph.NumGraphsWithSmallerEqualChromaticNumber++;
            }
            else
            {
                graph.NumGraphsWithSmallerEqualChromaticNumber++;
                dbGraph.NumGraphsWithSmallerEqualChromaticNumber++;
            }
        }

        #endregion Public Methods
    }
}