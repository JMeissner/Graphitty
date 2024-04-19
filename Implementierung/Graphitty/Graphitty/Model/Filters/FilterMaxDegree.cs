using Graphitty.Model.Graphs;

namespace Graphitty.Model.Filters
{
    /// <summary>
    /// Filter by the maximum vertex degree of the graph
    /// </summary>
    class FilterMaxDegree : FilterNumBase
    {
        #region Public Constructors

        /// <summary>
        /// Sets the Id and the Displayname of the Filter
        /// </summary>
        public FilterMaxDegree()
        {
            Id = "MaxDegree";
            Displayname = "Maximum Vertex Degree";
        }

        #endregion Public Constructors

        #region predicates

        override protected bool PredicateEqual(GraphEntity g)
        {
            if (g.MaxVertexDegree == float.Parse(splitArgs[1]))
            {
                return true;
            }
            return false;
        }

        override protected bool PredicateGreater(GraphEntity g)
        {
            if (g.MaxVertexDegree > float.Parse(splitArgs[1]))
            {
                return true;
            }
            return false;
        }

        override protected bool PredicateLess(GraphEntity g)
        {
            if (g.MaxVertexDegree < float.Parse(splitArgs[1]))
            {
                return true;
            }
            return false;
        }

        override protected bool PredicateNotEqual(GraphEntity g)
        {
            if (g.MaxVertexDegree != float.Parse(splitArgs[1]))
            {
                return true;
            }
            return false;
        }

        #endregion predicates
    }
}