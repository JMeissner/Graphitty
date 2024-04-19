using Graphitty.Model.DataAccessLayer;
using Graphitty.Model.Graphs;
using Graphitty.ViewModel.Events;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Input;

namespace Graphitty.ViewModel
{
    /// <summary>
    /// Offers a collection of all graphs which match the filter criteria.
    /// Subscribes to the FilterAppliedEvent, to get the graphs matching the filter criteria.
    /// Publishes the SelectionChangedEvent when a graph from the collection gets selected.
    /// And a WriteLogEvent and a DatabaseChangedEvent when a graph gets deleted.
    /// </summary>
    /// <see cref="ViewModel.Events.WriteLogEvent"/>
    /// <see cref="ViewModel.Events.DatabaseChangedEvent"/>
    /// <see cref="View.GraphsView"/>
    public class GraphsViewModel : BindableBase
    {
        #region Private Fields

        private IEventAggregator eventAggregator;
        private ObservableCollection<GraphEntity> graphs;

        private GraphEntity selectedItem;

        private IUnitOfWork uow = UnitOfWork.Instance;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// Creates a new GraphsViewModel.
        /// </summary>
        /// <param name="eventAggregator">Used to subscribe the FilterAppliedEvent and to publish the SelectionChangedEvent.</param>
        public GraphsViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            this.eventAggregator.GetEvent<FilterAppliedEvent>().Subscribe(o =>
            {
                Graphs = new ObservableCollection<GraphEntity>(o);
                RaisePropertyChanged("NumGraphsDatabase");
            });
            this.eventAggregator.GetEvent<GraphEditedEvent>().Subscribe(g => SelectedItem = g);
            DeleteGraphCommand = new DelegateCommand(onDeleteGraph, isGraphSelected).ObservesProperty(() => SelectedItem);
            CopyBFSToClipBoardCommand = new DelegateCommand(onCopyBFSToClipBoard, isGraphSelected).ObservesProperty(() => SelectedItem);
            ShowProfileCommand = new DelegateCommand(onShowProfile, isGraphSelected).ObservesProperty(() => SelectedItem);
            CopyProfileToClipBoardCommand = new DelegateCommand(onCopyProfile, isGraphSelected).ObservesProperty(() => SelectedItem);
        }

        /// <summary>
        /// Creates a GraphsViewModel and initializes the used UnitOfWork with the given reference.
        /// </summary>
        /// <param name="eventAggregator"></param>
        /// <param name="unitOfWork"></param>
        /// <see cref="GraphsViewModel.GraphsViewModel(IEventAggregator)"/>
        public GraphsViewModel(IEventAggregator eventAggregator, IUnitOfWork unitOfWork) : this(eventAggregator)
        {
            uow = unitOfWork;
        }

        #endregion Public Constructors

        #region Private Methods

        private bool isGraphSelected()
        {
            return selectedItem != null;
        }

        /// <summary>
        /// Copies the BFS code of the selected Graph into the clipboard
        /// </summary>
        private void onCopyBFSToClipBoard()
        {
            Clipboard.SetText(selectedItem.BFSCode);
        }

        /// <summary>
        /// Copies the profile into the clipboard
        /// </summary>
        private void onCopyProfile()
        {
            string profileToShow = "";
            //Old implementation with no seperator
            //Fragments are seperated by ? from each other.
            //string[] splitProfil = selectedItem.Profile.Split('?');
            //int dimension = (int)System.Math.Sqrt(splitProfil.Length);
            //int fragmentCounter = 0;

            //for (int i = 0; i < dimension; i++)
            //{
            //    for (int j = 0; j < dimension; j++)
            //    {
            //        profileToShow += (splitProfil[fragmentCounter] + ";");
            //        fragmentCounter++;
            //    }
            //    profileToShow.Trim('0');
            //    profileToShow += "\n";
            //}
            profileToShow = selectedItem.Profile;

            Clipboard.SetText(profileToShow);
        }

        /// <summary>
        /// Deletes the selected Graph and fires a DatabaseChangedEvent.
        /// </summary>
        private void onDeleteGraph()
        {
            eventAggregator.GetEvent<WriteLogEvent>().Publish("Deleted graph " + selectedItem.BFSCode);
            foreach (var graphEntity in uow.GraphRepository.GetAll())
            {
                if (selectedItem.NumDenserGraphsBFS < graphEntity.NumDenserGraphsBFS)
                {
                    graphEntity.NumDenserGraphsBFS--;
                }
                if (selectedItem.NumDenserGraphsProfile < graphEntity.NumDenserGraphsProfile)
                {
                    graphEntity.NumDenserGraphsProfile--;
                }
                if (selectedItem.NumGraphsWithSmallerEqualChromaticNumber <= graphEntity.NumGraphsWithSmallerEqualChromaticNumber)
                {
                    graphEntity.NumGraphsWithSmallerEqualChromaticNumber--;
                }
                uow.GraphRepository.Update(graphEntity);
            }
            uow.GraphRepository.Delete(selectedItem.Id);
            uow.Save();
            graphs.Remove(selectedItem);
            selectedItem = null;
            eventAggregator.GetEvent<DatabaseChangedEvent>().Publish();
        }

        /// <summary>
        /// Displays the Profile as a popup
        /// </summary>
        [ExcludeFromCodeCoverage]
        private void onShowProfile()
        {
            string profileToShow = string.Empty;
            //Fragments are seperated by ? from each other.
            string[] splitProfil = selectedItem.Profile.Split('?');
            int dimension = (int)System.Math.Sqrt(splitProfil.Length);
            int fragmentCounter = 0;

            for (int i = 0; i < dimension; i++)
            {
                for (int j = 0; j < dimension; j++)
                {
                    profileToShow += (splitProfil[fragmentCounter] + ";");
                    fragmentCounter++;
                }
                profileToShow.Trim('0');
                profileToShow += "\n";
            }

            MessageBox.Show(profileToShow, "Profile of selected Graph", MessageBoxButton.OK);
        }

        #endregion Private Methods

        #region Public Properties

        /// <summary>
        /// Copies the BFS Code of the selected Graph into the clipboard
        /// </summary>
        public ICommand CopyBFSToClipBoardCommand { get; private set; }

        /// <summary>
        /// Copies the Profile of the selected Graph into the clipboard
        /// </summary>
        public ICommand CopyProfileToClipBoardCommand { get; private set; }

        /// <summary>
        /// Deletes the selected graph from the database.
        /// </summary>
        public ICommand DeleteGraphCommand { get; private set; }

        /// <summary>
        /// Collection of graphs matching the filter criteria.
        /// </summary>
        public ObservableCollection<GraphEntity> Graphs
        {
            get => graphs;
            set
            {
                SetProperty(ref graphs, value);
                RaisePropertyChanged("Graphs");
            }
        }

        public int NumGraphsDatabase
        {
            get
            {
                if (uow.IsConnected())
                {
                    return uow.GraphRepository.GetCount();
                }
                return 0;
            }
        }

        /// <summary>
        /// Currently selected item from the Graphs collection. On set it fires a SelectionChangedEvent.
        /// </summary>
        /// <see cref="Graphs"/>
        public GraphEntity SelectedItem
        {
            get => selectedItem;
            set
            {
                if (Graphs.Contains(value))
                {
                    SetProperty(ref selectedItem, value);
                    RaisePropertyChanged("SelectedItem");
                    eventAggregator.GetEvent<SelectionChangedEvent>().Publish(selectedItem);
                }
                else
                {
                    SetProperty(ref selectedItem, null);
                    RaisePropertyChanged("SelectedItem");
                    eventAggregator.GetEvent<SelectionChangedEvent>().Publish(selectedItem);
                }
            }
        }

        /// <summary>
        /// Displays the profile of the selected Graph
        /// </summary>
        public ICommand ShowProfileCommand { get; private set; }

        #endregion Public Properties
    }
}