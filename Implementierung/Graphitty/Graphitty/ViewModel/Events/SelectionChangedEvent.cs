using Graphitty.Model.Graphs;
using Prism.Events;

namespace Graphitty.ViewModel.Events
{
    /// <summary>
    /// Raised, when a graph gets selected. Contains the selected graph as in the payload.
    /// </summary>
    public class SelectionChangedEvent : PubSubEvent<GraphEntity>
    {
    }
}