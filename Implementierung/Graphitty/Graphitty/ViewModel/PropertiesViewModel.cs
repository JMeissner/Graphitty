using Graphitty.Model.Graphs;
using Graphitty.ViewModel.Events;
using Prism.Events;
using Prism.Mvvm;

namespace Graphitty.ViewModel
{
    /// <summary>
    /// Offers the Properties of a currently selected GraphEntity for consumption from the PropertiesView.
    /// Subscribes to the SelectionChangedEvent to get the selected Graph.
    /// </summary>
    /// <see cref="View.PropertiesView"/>
    /// <see cref="ViewModel.Events.SelectionChangedEvent"/>
    public class PropertiesViewModel : BindableBase
    {
        #region Private Fields

        private IEventAggregator eventAggregator;
        private GraphEntity graph;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// Initializes the ViewModel with a eventAggregator to subscribe to the SelectionChangedEvent.
        /// </summary>
        /// <param name="eventAggregator"></param>
        public PropertiesViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            this.eventAggregator.GetEvent<SelectionChangedEvent>().Subscribe(g => Graph = g);
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// The currently selected graph, whose properties can be accessed.
        /// </summary>
        public GraphEntity Graph
        {
            get { return graph; }
            private set
            {
                SetProperty(ref graph, value);
                RaisePropertyChanged("Graph");
            }
        }

        #endregion Public Properties
    }
}