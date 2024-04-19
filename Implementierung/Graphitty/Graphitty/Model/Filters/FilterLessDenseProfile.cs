using Graphitty.Model.Graphs;

namespace Graphitty.Model.Filters
{
    class FilterLessDenseProfile : FilterCompareBase
    {
        #region Public Constructors

        public FilterLessDenseProfile()
        {
            Id = "FilterLessDenseProfile";
            Displayname = "Less dense graphs by means of Profile";
        }

        #endregion Public Constructors

        #region Predicates

        protected override bool OnlyPredicate(GraphEntity g)
        {
            Graph gGraph = new Graph(g.BFSCode)
            {
                Profile = g.Profile
            };
            if (gGraph.CompareProfile(compareEntity))
            {
                return true;
            }
            return false;
        }

        #endregion Predicates
    }
}