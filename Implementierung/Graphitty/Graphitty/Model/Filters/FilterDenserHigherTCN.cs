using Graphitty.Model.Graphs;

namespace Graphitty.Model.Filters
{
    class FilterDenserHigherTCN : FilterCompareBase
    {
        #region Public Constructors

        public FilterDenserHigherTCN()
        {
            Id = "DenserHigherTCN";
            Displayname = "Denser Graphs with higher Total Chromatic Number";
        }

        #endregion Public Constructors

        #region Predicates

        override protected bool OnlyPredicate(GraphEntity g)
        {
            BFSCode gBFS = new BFSCode(g.BFSCode);
            BFSCode compareEntityBFS = new BFSCode(compareEntity.BFSCode);
            if ((gBFS.CompareTo(compareEntityBFS) == -1) && g.TotalChromaticNumber > compareEntity.TotalChromaticNumber)
            {
                return true;
            }
            return false;
        }

        #endregion Predicates
    }
}