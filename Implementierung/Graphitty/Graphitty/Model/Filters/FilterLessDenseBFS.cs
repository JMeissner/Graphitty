using Graphitty.Model.Graphs;

namespace Graphitty.Model.Filters
{
    class FilterLessDenseBFS : FilterCompareBase
    {
        #region Public Constructors

        public FilterLessDenseBFS()
        {
            Id = "FilterLessDenseBFS";
            Displayname = "Less dense graphs by means of BFS";
        }

        #endregion Public Constructors

        #region Predicate

        protected override bool OnlyPredicate(GraphEntity g)
        {
            BFSCode gBFS = new BFSCode(g.BFSCode);
            BFSCode compareEntityBFS = new BFSCode(compareEntity.BFSCode);
            if (gBFS.CompareTo(compareEntityBFS) == 1)
            {
                return true;
            }
            return false;
        }

        #endregion Predicate
    }
}