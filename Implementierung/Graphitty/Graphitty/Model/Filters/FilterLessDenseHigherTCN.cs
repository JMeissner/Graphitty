using Graphitty.Model.Graphs;

namespace Graphitty.Model.Filters
{
    class FilterLessDenseHigherTCN : FilterCompareBase
    {
        #region Public Constructors

        public FilterLessDenseHigherTCN()
        {
            Id = "LessDenseHigherTCN";
            Displayname = "Less dense Graphs with higher Total Chromatic Number";
        }

        #endregion Public Constructors

        #region Predicates

        override protected bool OnlyPredicate(GraphEntity g)
        {
            BFSCode gBFS = new BFSCode(g.BFSCode);
            BFSCode compareEntityBFS = new BFSCode(compareEntity.BFSCode);
            if ((gBFS.CompareTo(compareEntityBFS) == 1) && g.TotalChromaticNumber > compareEntity.TotalChromaticNumber)
            {
                return true;
            }
            return false;
        }

        #endregion Predicates
    }
}