using Graphitty.Model.Graphs;

namespace Graphitty.Model.Filters
{
    /// <summary>
    /// Filter for the average vertex degree
    /// </summary>
    class FilterAvgDegree : FilterNumBase
    {
        #region Public Constructors

        /// <summary>
        /// Sets the Id and the Displayname of the Filter
        /// </summary>
        public FilterAvgDegree()
        {
            Id = "AvgDegree";
            Displayname = "Average Vertex Degree";
        }

        #endregion Public Constructors

        #region predicates

        override protected bool PredicateEqual(GraphEntity g)
        {
            if (g.AverageVertexDegree == float.Parse(splitArgs[1]))
            {
                return true;
            }
            return false;
        }

        override protected bool PredicateGreater(GraphEntity g)
        {
            if (g.AverageVertexDegree > float.Parse(splitArgs[1]))
            {
                return true;
            }
            return false;
        }

        override protected bool PredicateLess(GraphEntity g)
        {
            if (g.AverageVertexDegree < float.Parse(splitArgs[1]))
            {
                return true;
            }
            return false;
        }

        override protected bool PredicateNotEqual(GraphEntity g)
        {
            if (g.AverageVertexDegree != float.Parse(splitArgs[1]))
            {
                return true;
            }
            return false;
        }

        #endregion predicates
    }
}