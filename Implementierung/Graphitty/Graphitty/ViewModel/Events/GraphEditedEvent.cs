using Graphitty.Model.Graphs;
using Prism.Events;

namespace Graphitty.ViewModel.Events
{
    /// <summary>
    /// Raised when a graph was edited. The payload contains the new GraphEntity.
    /// </summary>
    public class GraphEditedEvent : PubSubEvent<GraphEntity>
    {
    }
}