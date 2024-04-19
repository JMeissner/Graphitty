using Prism.Events;

namespace Graphitty.ViewModel.Events
{
    /// <summary>
    /// Raised, when a log text has to be written. The payload contains the text.
    /// </summary>
    public class WriteLogEvent : PubSubEvent<string>
    {
    }
}