using Graphitty.Model.Graphs;
using Prism.Events;
using System.Collections.ObjectModel;

namespace Graphitty.ViewModel.Events
{
    /// <summary>
    /// Raised, when filters have been applied. The payload contains a collection of GraphEntities which match the filter criterias.
    /// </summary>
    public class FilterAppliedEvent : PubSubEvent<ObservableCollection<GraphEntity>>
    {
    }
}