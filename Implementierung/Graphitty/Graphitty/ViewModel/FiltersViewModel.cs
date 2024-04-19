using Graphitty.Model.Filters;
using Graphitty.Model.Graphs;
using Graphitty.ViewModel.Events;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Input;
using Graphitty.Model.DataAccessLayer;
using System.Linq;
using System.Windows;
using System.Diagnostics.CodeAnalysis;

namespace Graphitty.ViewModel
{
    /// <summary>
    /// Offers properties to filter the graph database. The FiltersView consumes these properties.
    /// Subscribes to the DatabaseChangedEvent.
    /// Publishes the FilterAppliedEvent.
    /// </summary>
    /// <see cref="View.FiltersView"/>
    /// <see cref="ViewModel.Events.FilterAppliedEvent"/>
    /// <see cref="ViewModel.Events.DatabaseChangedEvent"/>
    public class FiltersViewModel : BindableBase
    {
        #region Private Fields

        private ObservableCollection<FilterItemViewModel> activeFilters;
        private List<serializedFilter> activeSerialized;
        private List<IFilter> allFilters;
        private ObservableCollection<FilterItemViewModel> booleanFilters;
        private ObservableCollection<FilterItemViewModel> compareFilters;
        private IEventAggregator eventAggregator;
        private ObservableCollection<string> filterSet;
        private ObservableCollection<FilterItemViewModel> numericFilters;
        private GraphEntity selectedGraph;
        private IUnitOfWork unitOfWork = UnitOfWork.Instance;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// Public constructor of FiltersViewModel
        /// </summary>
        /// <param name="eventAggregator">the eventaggregator</param>
        public FiltersViewModel(IEventAggregator eventAggregator)
        {
            SaveFilterCommand = new DelegateCommand(onSaveFilter);
            LoadFilterCommand = new DelegateCommand<string>(onLoadFilter);
            AddFilterCommand = new DelegateCommand<string>(onAddFilter);
            RemoveFilterCommand = new DelegateCommand<string>(onRemoveFilter);
            ApplyFilterCommand = new DelegateCommand(onApplyFilter);

            this.eventAggregator = eventAggregator;
            this.eventAggregator.GetEvent<SelectionChangedEvent>().Subscribe(g => selectedGraph = g);
            this.eventAggregator.GetEvent<DatabaseChangedEvent>().Subscribe(() =>
            {
                ApplyFilterCommand.Execute(null);
            });

            activeFilters = new ObservableCollection<FilterItemViewModel>();
            numericFilters = new ObservableCollection<FilterItemViewModel>();
            booleanFilters = new ObservableCollection<FilterItemViewModel>();
            compareFilters = new ObservableCollection<FilterItemViewModel>();

            allFilters = new List<IFilter>();
            activeSerialized = new List<serializedFilter>();

            filterSet = new ObservableCollection<string> { "Set 1", "Set 2", "Set 3", "Set 4", "Set 5" };

            createAllFilter();
        }

        public FiltersViewModel(IEventAggregator eventAggregator, IUnitOfWork pUoW) : this(eventAggregator)
        {
            unitOfWork = pUoW;
        }

        #endregion Public Constructors

        #region Public Properties

        public ObservableCollection<FilterItemViewModel> ActiveFilters
        {
            get { return activeFilters; }
            [ExcludeFromCodeCoverageAttribute]
            set { SetProperty(ref activeFilters, value); RaisePropertyChanged("ActiveFilters"); }
        }

        public ICommand AddFilterCommand { get; private set; }
        public ICommand ApplyFilterCommand { get; private set; }

        public ObservableCollection<FilterItemViewModel> BooleanFilters
        {
            get { return booleanFilters; }
            [ExcludeFromCodeCoverageAttribute]
            set { SetProperty(ref booleanFilters, value); RaisePropertyChanged("BooleanFilters"); }
        }

        public ObservableCollection<FilterItemViewModel> CompareFilters
        {
            get { return compareFilters; }
            [ExcludeFromCodeCoverageAttribute]
            set { SetProperty(ref compareFilters, value); RaisePropertyChanged("CompareFilters"); }
        }

        public ObservableCollection<string> FilterSet
        {
            [ExcludeFromCodeCoverageAttribute]
            get { return filterSet; }
            [ExcludeFromCodeCoverageAttribute]
            set { SetProperty(ref filterSet, value); RaisePropertyChanged("FilterSet"); }
        }

        public ICommand LoadFilterCommand { get; private set; }

        public ObservableCollection<FilterItemViewModel> NumericFilters
        {
            get { return numericFilters; }
            [ExcludeFromCodeCoverageAttribute]
            set { SetProperty(ref numericFilters, value); RaisePropertyChanged("NumericFilters"); }
        }

        public ICommand RemoveFilterCommand { get; private set; }
        public ICommand SaveFilterCommand { get; private set; }
        public string SelectedFilterSet { get; set; }

        //Datatype for serialized List
        public struct serializedFilter
        {
            #region Public Fields

            public string Args;
            public string ID;

            #endregion Public Fields
        }

        #endregion Public Properties

        #region Private Methods

        /// <summary>
        /// Method for easy extensibility. Drops all filters into a list and then calls the populate function
        /// </summary>
        private void createAllFilter()
        {
            allFilters.Add(new FilterNumEdges());
            allFilters.Add(new FilterAvgDegree());
            allFilters.Add(new FilterMaxDegree());
            allFilters.Add(new FilterMaxClique());
            allFilters.Add(new FilterNumCliques());
            allFilters.Add(new FilterIsTC());
            allFilters.Add(new FilterTCN());
            allFilters.Add(new FilterRunTime());
            allFilters.Add(new FilterDenserEqTCN());
            allFilters.Add(new FilterDenserHigherTCN());
            allFilters.Add(new FilterDenserSmallerTCN());
            allFilters.Add(new FilterLessDenseEqTCN());
            allFilters.Add(new FilterLessDenseHigherTCN());
            allFilters.Add(new FilterLessDenseSmallerTCN());
            allFilters.Add(new FilterDenserBFS());
            allFilters.Add(new FilterLessDenseBFS());
            allFilters.Add(new FilterDenserProfile());
            allFilters.Add(new FilterLessDenseProfile());
            //Add your Filter here

            populateFilterList();
        }

        /// <summary>
        /// Clears all active filters and all serialized filters
        /// </summary>
        private void disposeActiveFilter()
        {
            ActiveFilters.Clear();
            activeSerialized.Clear();
        }

        /// <summary>
        /// Returns a Filter with the according ID
        /// </summary>
        /// <param name="id">ID of the filter</param>
        /// <returns>FilterInstance</returns>
        private IFilter getFilterById(string id)
        {
            foreach (IFilter filter in allFilters)
            {
                if (filter.Id.Equals(id)) { return filter; }
            }
            return null;
        }

        /// <summary>
        /// Returns a FilterItemViewModel with the according ID
        /// </summary>
        /// <param name="id">ID of the filter</param>
        /// <returns>FilterItemViewModel</returns>
        private FilterItemViewModel getFilterItemById(string id)
        {
            foreach (FilterItemViewModel filter in NumericFilters)
            {
                if (filter.Id.Equals(id)) { return filter; }
            }
            foreach (FilterItemViewModel filter in BooleanFilters)
            {
                if (filter.Id.Equals(id)) { return filter; }
            }
            foreach (FilterItemViewModel filter in CompareFilters)
            {
                if (filter.Id.Equals(id)) { return filter; }
            }
            return null;
        }

        /// <summary>
        /// Adds the selected filter to active filters
        /// </summary>
        /// <param name="Args">filter to add</param>
        private void onAddFilter(string Args)
        {
            if (!Args.Contains(":"))
            {
                showErrorPopUp();
                return;
            }
            string[] splitArgs = Args.Split(':');
            FilterItemViewModel _filterToReset = getFilterItemById(splitArgs[0]);
            _filterToReset.Args = "";

            if (!splitArgs[1].Contains(" ") && typeof(FilterNumBase).IsAssignableFrom(getFilterById(_filterToReset.Id).GetType()))
            {
                showErrorPopUp();
                return;
            }
            else if ((!splitArgs[1].Contains("=") && !splitArgs[1].Contains(">") && !splitArgs[1].Contains("<") && !splitArgs[1].Contains("!="))
                && typeof(FilterNumBase).IsAssignableFrom(getFilterById(_filterToReset.Id).GetType()))
            {
                showErrorPopUp();
                return;
            }

            //Add Filter to ActiveFilterItems
            FilterItemViewModel _filterItemViewModel = new FilterItemViewModel(getFilterById(splitArgs[0]), splitArgs[1]);
            ActiveFilters.Add(_filterItemViewModel);
            //Add Filter to serialized List
            serializedFilter _serializedFilter = new serializedFilter
            {
                ID = splitArgs[0],
                Args = splitArgs[1]
            };
            activeSerialized.Add(_serializedFilter);
        }

        /// <summary>
        /// Applies the active filters
        /// </summary>
        private void onApplyFilter()
        {
            Debug.WriteLine("Applying Filter...");
            IEnumerable<GraphEntity> gList = unitOfWork.GraphRepository.GetAll();
            foreach (serializedFilter f in activeSerialized)
            {
                IFilter filter = getFilterById(f.ID);
                filter.Args = f.Args;
                if (CompareFilters.Contains(getFilterItemById(f.ID)))
                {
                    if (selectedGraph == null)
                    {
                        MessageBox.Show("No Graph Selected! \nSelect a Graph in the Graphs-Tab to compare to", "Error: No Graph Selected", MessageBoxButton.OK);
                        return;
                    }
                    FilterCompareBase Cfilter = filter as FilterCompareBase;
                    Debug.WriteLine("Compare Update: " + Cfilter.Id + ", with Graph: " + selectedGraph.BFSCode);
                    Cfilter.UpdateCompareEntity(selectedGraph);
                }

                if (filter.Predicate() != null)
                {
                    Debug.WriteLine("Predicate from " + f.ID + " is valid.");
                    gList = gList.Where(filter.Predicate());
                }
            }
            var tmp = new ObservableCollection<GraphEntity>(gList);
            //Done filtering -> Take list to GraphsViewModel
            eventAggregator.GetEvent<FilterAppliedEvent>().Publish(tmp);
        }

        /// <summary>
        /// Loads the specified filterset
        /// </summary>
        /// <param name="Args">Filterset to Load. Args must be "Set X" X : int</param>
        private void onLoadFilter(string Args)
        {
            Debug.WriteLine("LoadFilterCommand executed");
            if (SelectedFilterSet == null || SelectedFilterSet == "")
            {
                MessageBox.Show("Error: No Filter Set specified.", "No Set selected", MessageBoxButton.OK);
                return;
            }
            string[] set = SelectedFilterSet.Split(' ');
            int setID = int.Parse(set[1]);
            IEnumerable<FilterEntity> feList = unitOfWork.FilterRepository.Get(f => f.GroupID == setID);

            //Clear old filters
            disposeActiveFilter();

            foreach (FilterEntity _fe in feList)
            {
                //Add Filter to ActiveFilterItems
                FilterItemViewModel _filterItemViewModel = new FilterItemViewModel(getFilterById(_fe.ID), _fe.Args);
                ActiveFilters.Add(_filterItemViewModel);
                //Add Filter to serialized List
                serializedFilter _serializedFilter = new serializedFilter
                {
                    ID = _fe.ID,
                    Args = _fe.Args
                };
                activeSerialized.Add(_serializedFilter);
            }
        }

        /// <summary>
        /// Deletes the specified filter from active and serialized Filters
        /// </summary>
        /// <param name="Args">Filter to remove. Cant go wrong since its called from the view</param>
        private void onRemoveFilter(string Args)
        {
            Debug.WriteLine("RemoveFilter executed");
            string[] splitArgs = Args.Split(':');

            //remove from visual ActiveFilters
            foreach (FilterItemViewModel _filterItemViewModel in ActiveFilters)
            {
                if (_filterItemViewModel.Id.Equals(splitArgs[0]) && _filterItemViewModel.Args.Equals(splitArgs[1]))
                {
                    ActiveFilters.Remove(_filterItemViewModel);
                    break;
                }
            }
            //remove from serialized ActiveFilters
            foreach (serializedFilter _serializedFilter in activeSerialized)
            {
                if (_serializedFilter.ID.Equals(splitArgs[0]) && _serializedFilter.Args.Equals(splitArgs[1]))
                {
                    activeSerialized.Remove(_serializedFilter);
                    break;
                }
            }
            if (!activeSerialized.Any())
            {
                onApplyFilter();
            }
        }

        /// <summary>
        /// Saves active Filters into the database
        /// </summary>
        private void onSaveFilter()
        {
            Debug.WriteLine("SaveFilterCommand executed");
            Debug.WriteLine("Chosen Set: " + SelectedFilterSet);
            if (SelectedFilterSet == null || SelectedFilterSet == "")
            {
                MessageBox.Show("Error: No Filter Set specified.", "No Set selected", MessageBoxButton.OK);
                return;
            }
            string[] set = SelectedFilterSet.Split(' ');

            foreach (serializedFilter _serializedFilter in activeSerialized)
            {
                FilterEntity _filterEntity = new FilterEntity
                {
                    ID = _serializedFilter.ID,
                    Args = _serializedFilter.Args,
                    GroupID = int.Parse(set[1])
                };

                if (unitOfWork.FilterRepository.Exists(f => f.ID == _filterEntity.ID && f.GroupID == _filterEntity.GroupID && f.Args == _filterEntity.Args))
                {
                    unitOfWork.FilterRepository.Update(_filterEntity);
                }
                else
                {
                    unitOfWork.FilterRepository.Create(_filterEntity);
                }
            }
            unitOfWork.Save();
        }

        /// <summary>
        /// Sorts all filter into the according lists
        /// </summary>
        private void populateFilterList()
        {
            foreach (IFilter f in allFilters)
            {
                if (typeof(FilterNumBase).IsAssignableFrom(f.GetType()))
                {
                    NumericFilters.Add(new FilterItemViewModel(f));
                }
                if (typeof(FilterBoolBase).IsAssignableFrom(f.GetType()))
                {
                    BooleanFilters.Add(new FilterItemViewModel(f));
                }
                if (typeof(FilterCompareBase).IsAssignableFrom(f.GetType()))
                {
                    CompareFilters.Add(new FilterItemViewModel(f));
                }
            }
        }

        /// <summary>
        /// Shows an Error Pop-up on illegal arguments
        /// </summary>
        private void showErrorPopUp()
        {
            MessageBox.Show("The entered arguments are incorrect. \n Please use '=|<|>|!=' SPACE YOURNUMBER.", "Incorrect Filter Arguments", MessageBoxButton.OK);
        }

        #endregion Private Methods
    }
}