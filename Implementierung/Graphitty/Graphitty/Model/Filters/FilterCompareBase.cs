using System;
using Graphitty.Model.Graphs;

namespace Graphitty.Model.Filters
{
    public class FilterCompareBase : IFilter
    {
        public string Id { get; protected set; }

        public string Displayname { get; protected set; }

        public string Args { get; set; }

        protected GraphEntity compareEntity { get; set; }

        /// <summary>
        /// Updates the GraphEntity to compare against
        /// </summary>
        /// <param name="g">GraphEntity to compare against: The selected graph</param>
        public void UpdateCompareEntity(GraphEntity g)
        {
            compareEntity = g;
        }

        /// <summary>
        /// checks Args and selects the right predicte
        /// </summary>
        /// <returns>Returns a Function (GraphEntity -> bool) to filter the UoW</returns>
        public Func<GraphEntity, bool> Predicate()
        {
            return OnlyPredicate;
        }

        #region Predicates
        virtual protected bool OnlyPredicate(GraphEntity g)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
