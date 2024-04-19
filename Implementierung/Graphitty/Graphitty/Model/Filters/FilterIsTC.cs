using Graphitty.Model.Graphs;

namespace Graphitty.Model.Filters
{
    class FilterIsTC : FilterBoolBase
    {
        #region Public Constructors

        public FilterIsTC()
        {
            Id = "IsTCC";
            Displayname = "Is TCC Fulfilled?";
        }

        #endregion Public Constructors

        #region Predicates

        protected override bool PredicateFalse(GraphEntity g)
        {
            if (!g.IsTCCFulfilled)
            {
                return true;
            }
            return false;
        }

        protected override bool PredicateTrue(GraphEntity g)
        {
            if (g.IsTCCFulfilled)
            {
                return true;
            }
            return false;
        }

        #endregion Predicates
    }
}