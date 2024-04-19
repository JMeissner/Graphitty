using Graphitty.Model.Graphs;

namespace Graphitty.Model.Filters
{
    /// <summary>
    /// Filter by the totalchromatic number
    /// </summary>
    class FilterTCN : FilterNumBase
    {
        #region Public Constructors

        /// <summary>
        /// Sets the Id and the Displayname of the Filter
        /// </summary>
        public FilterTCN()
        {
            Id = "TCN";
            Displayname = "Total Chromatic Number";
        }

        #endregion Public Constructors

        #region predicates

        override protected bool PredicateEqual(GraphEntity g)
        {
            if (g.TotalChromaticNumber == float.Parse(splitArgs[1]))
            {
                return true;
            }
            return false;
        }

        override protected bool PredicateGreater(GraphEntity g)
        {
            if (g.TotalChromaticNumber > float.Parse(splitArgs[1]))
            {
                return true;
            }
            return false;
        }

        override protected bool PredicateLess(GraphEntity g)
        {
            if (g.TotalChromaticNumber < float.Parse(splitArgs[1]))
            {
                return true;
            }
            return false;
        }

        override protected bool PredicateNotEqual(GraphEntity g)
        {
            if (g.TotalChromaticNumber != float.Parse(splitArgs[1]))
            {
                return true;
            }
            return false;
        }

        #endregion predicates
    }
}