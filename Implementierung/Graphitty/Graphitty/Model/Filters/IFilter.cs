using System;
using Graphitty.Model.Graphs;

namespace Graphitty.Model.Filters
{
    public interface IFilter
    {
        #region Public Properties

        string Args { get; set; }

        string Displayname { get; }
        string Id { get; }

        #endregion Public Properties

        #region Public Methods

        //readonly
        //readonly
        Func<GraphEntity, bool> Predicate();

        #endregion Public Methods
    }
}