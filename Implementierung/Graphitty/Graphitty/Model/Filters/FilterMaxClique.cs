using Graphitty.Model.Graphs;

namespace Graphitty.Model.Filters
{
    /// <summary>
    /// Filter by the number of vertices in the largest clique
    /// </summary>
    internal class FilterMaxClique : FilterNumBase
    {
        #region Public Constructors

        /// <summary>
        /// Sets the ID and the DisplayName of the Filter
        /// </summary>
        public FilterMaxClique()
        {
            Id = "MaxClique";
            Displayname = "Number of Vertices in the largest clique";
        }

        #endregion Public Constructors

        #region predicates

        override protected bool PredicateEqual(GraphEntity g)
        {
            if (g.LargestCliqueSize == float.Parse(splitArgs[1]))
            {
                return true;
            }
            return false;
        }

        override protected bool PredicateGreater(GraphEntity g)
        {
            if (g.LargestCliqueSize > float.Parse(splitArgs[1]))
            {
                return true;
            }
            return false;
        }

        override protected bool PredicateLess(GraphEntity g)
        {
            if (g.LargestCliqueSize < float.Parse(splitArgs[1]))
            {
                return true;
            }
            return false;
        }

        override protected bool PredicateNotEqual(GraphEntity g)
        {
            if (g.LargestCliqueSize != float.Parse(splitArgs[1]))
            {
                return true;
            }
            return false;
        }

        #endregion predicates
    }
}