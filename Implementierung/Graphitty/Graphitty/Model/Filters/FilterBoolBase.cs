using System;
using Graphitty.Model.Graphs;

namespace Graphitty.Model.Filters
{
    class FilterBoolBase : IFilter
    {
        #region Public Properties

        public string Args { get; set; }
        public string Displayname { get; protected set; }
        public string Id { get; protected set; }

        #endregion Public Properties

        #region Public Methods

        public Func<GraphEntity, bool> Predicate()
        {
            //Check Args consistency
            if (!CheckArgs()) { return null; }

            Func<GraphEntity, bool> pred = null;

            //return the correct function based on the Args
            switch (Args)
            {
                case "true": pred = PredicateTrue; break;
                case "false": pred = PredicateFalse; break;
                default: return null;
            }

            return pred;
        }

        #endregion Public Methods

        #region Protected Methods

        /// <summary>
        /// Checks whether the Args are correct and can be parsed
        /// </summary>
        /// <returns>true if Args are correct</returns>
        protected bool CheckArgs()
        {
            if (Args.Equals("true") || Args.Equals("false"))
            {
                return true;
            }
            return false;
        }

        #endregion Protected Methods

        #region Predicates

        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        virtual protected bool PredicateFalse(GraphEntity g)
        {
            throw new NotImplementedException();
        }

        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        virtual protected bool PredicateTrue(GraphEntity g)
        {
            throw new NotImplementedException();
        }

        #endregion Predicates
    }
}