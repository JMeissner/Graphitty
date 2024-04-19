using Prism.Events;

namespace Graphitty.ViewModel.Events
{
    /// <summary>
    /// Raised, when the database itself or the entries change.
    /// </summary>
    public class DatabaseChangedEvent : PubSubEvent
    {
    }
}