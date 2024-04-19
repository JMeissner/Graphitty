using Graphitty.Model.Graphs;

namespace Graphitty.Model.Filters
{
    class FilterDenserProfile : FilterCompareBase
    {
        #region Public Constructors

        public FilterDenserProfile()
        {
            Id = "FilterDenserProfile";
            Displayname = "Denser graphs by means of Profile";
        }

        #endregion Public Constructors

        #region Predicate

        protected override bool OnlyPredicate(GraphEntity g)
        {
            Graph gGraph = new Graph(g.BFSCode)
            {
                Profile = g.Profile
            };
            if (!gGraph.CompareProfile(compareEntity))
            {
                return true;
            }
            return false;
        }

        #endregion Predicate
    }
}