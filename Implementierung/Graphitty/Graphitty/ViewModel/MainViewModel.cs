using Graphitty.Model.DataAccessLayer;
using Graphitty.ViewModel.Events;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace Graphitty.ViewModel
{
    /// <summary>
    /// This is the MainViewModel, it is the parent of all existing ViewModels. It creates and initializes all its sub-ViewModels.
    /// It also creates a EventAggregator and passes it to the sub-ViewModels. The EventAggregator provides a communication canal for all the ViewModels.
    /// This ViewModel publishes the DatabaseChangedEvent and the WriteLogEvent.
    /// </summary>
    /// <see cref="View.MainView"/>
    /// <see cref="ViewModel.Events.DatabaseChangedEvent"/>
    /// <see cref="ViewModel.Events.WriteLogEvent"/>
    public class MainViewModel : BindableBase
    {
        #region Private Fields

        private ObservableCollection<string> availableDatabases;
        private ConsoleViewModel consoleViewModel;
        private CorrelationsTableViewModel correlationsTableViewModel;
        private IDialogCoordinator dialogCoordinator;
        private IEventAggregator eventAggregator;
        private FiltersViewModel filtersViewModel;
        private GeneratorViewModel generatorViewModel;
        private GraphsViewModel graphsViewModel;
        private GraphVisualizerViewModel graphVisualizerViewModel;
        private PropertiesViewModel propertiesViewModel;
        private string selectedDatabase;
        private IUnitOfWork unitOfWork = UnitOfWork.Instance;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// Creates the MainViewModel and sets a DialogCoordinator to enable this ViewModel to create dialogs for login.
        /// </summary>
        /// <param name="dialogCoordinator">Used to create/ show a login dialog.</param>
        public MainViewModel()
        {
            dialogCoordinator = DialogCoordinator.Instance;
            eventAggregator = new EventAggregator();
            //ViewModel
            propertiesViewModel = new PropertiesViewModel(eventAggregator);
            consoleViewModel = new ConsoleViewModel(eventAggregator, Directory.GetCurrentDirectory() + "/TestLog.txt");
            graphsViewModel = new GraphsViewModel(eventAggregator);
            generatorViewModel = new GeneratorViewModel(eventAggregator);
            filtersViewModel = new FiltersViewModel(eventAggregator);
            graphVisualizerViewModel = new GraphVisualizerViewModel(eventAggregator);
            correlationsTableViewModel = new CorrelationsTableViewModel(eventAggregator);
            //Commands
            CloseCommand = new DelegateCommand(onClose, canClose);
            RefreshAvailableDatabasesCommand = new DelegateCommand(onRefreshAvailableDatabases);
            ConnectDatabaseCommand = new DelegateCommand(onConnectDatabase, canConnectDatabase).ObservesProperty(() => SelectedDatabase);
            DeleteDatabaseCommand = new DelegateCommand(onDeleteDatabase, canDeleteDatabase).ObservesProperty(() => SelectedDatabase);
            LoginCommand = new DelegateCommand(onLogin, canLogin);
            ChangePathCommand = new DelegateCommand(onChangePath);
        }

        public MainViewModel(IEventAggregator eventAggregator, IUnitOfWork unitOfWork) : this()
        {
            this.unitOfWork = unitOfWork;
            this.eventAggregator = eventAggregator;
            //ViewModel
            propertiesViewModel = new PropertiesViewModel(eventAggregator);
            consoleViewModel = new ConsoleViewModel(eventAggregator, Directory.GetCurrentDirectory() + "/TestLog.txt");
            graphsViewModel = new GraphsViewModel(eventAggregator, unitOfWork);
            generatorViewModel = new GeneratorViewModel(eventAggregator);
            filtersViewModel = new FiltersViewModel(eventAggregator, unitOfWork);
            graphVisualizerViewModel = new GraphVisualizerViewModel(eventAggregator, unitOfWork);
            correlationsTableViewModel = new CorrelationsTableViewModel(eventAggregator, unitOfWork);
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// A collection of all the available databases of the current server.
        /// </summary>
        [ExcludeFromCodeCoverageAttribute]
        public ObservableCollection<string> AvailableDatabases
        {
            get
            {
                return new ObservableCollection<string>(unitOfWork.AvailableDatabases);
            }
            private set
            {
                SetProperty(ref availableDatabases, value);
                RaisePropertyChanged("AvailableDatabases");
            }
        }

        /// <summary>
        /// Changes the path of the current log file.
        /// </summary>
        public ICommand ChangePathCommand { get; private set; }

        /// <summary>
        /// Called when the user closes the application.
        /// </summary>
        public ICommand CloseCommand { get; private set; }

        /// <summary>
        /// Called when the user wants to connect to a database contained in th AvailableDatabases collection.
        /// </summary>
        /// <see cref="SelectedDatabase"/>
        public ICommand ConnectDatabaseCommand { get; private set; }

        /// <summary>
        /// Provides a log file.
        /// </summary>
        /// <see cref="ViewModel.ConsoleViewModel"/>
        public ConsoleViewModel ConsoleViewModel { get => consoleViewModel; private set => consoleViewModel = value; }

        /// <summary>
        /// Provides a collection of correlation coefficients of graph attributes with numeric values.
        /// </summary>
        /// <see cref="ViewModel.CorrelationsTableViewModel"/>
        public CorrelationsTableViewModel CorrelationsTableViewModel
        {
            get => correlationsTableViewModel;
            private set => correlationsTableViewModel = value;
        }

        /// <summary>
        /// Deletes the selected database.
        /// </summary>
        /// <remarks>It also deletes it from the host not only from the collection.</remarks>
        public ICommand DeleteDatabaseCommand { get; private set; }

        /// <summary>
        /// Offers collections of filters for filtering.
        /// </summary>
        /// <see cref="ViewModel.FiltersViewModel"/>
        public FiltersViewModel FiltersViewModel { get => filtersViewModel; private set => filtersViewModel = value; }

        /// <summary>
        /// Offers properties and a command to configure a generator and to start it.
        /// </summary>
        /// <see cref="ViewModel.GeneratorViewModel"/>
        public GeneratorViewModel GeneratorViewModel { get => generatorViewModel; private set => generatorViewModel = value; }

        /// <summary>
        /// Offers a collection of graphs which match the filter criteria.
        /// </summary>
        /// <see cref="ViewModel.GraphsViewModel"/>
        public GraphsViewModel GraphsViewModel { get => graphsViewModel; private set => graphsViewModel = value; }

        /// <summary>
        /// Responsible for displaying a selected graph.
        /// </summary>
        /// <see cref="ViewModel.GraphVisualizerViewModel"/>
        public GraphVisualizerViewModel GraphVisualizerViewModel { get => graphVisualizerViewModel; private set => graphVisualizerViewModel = value; }

        /// <summary>
        /// Command which shows a dialog so the user can login to a database server.
        /// </summary>
        public ICommand LoginCommand { get; private set; }

        /// <summary>
        /// The path to the selected log file used to configure it in the settings.
        /// </summary>
        public string Path
        {
            get => consoleViewModel.Path;
            set
            {
                consoleViewModel.Path = value;
                RaisePropertyChanged("Path");
            }
        }

        /// <summary>
        /// Offers the currently selected graph, so a view can access its properties.
        /// </summary>
        public PropertiesViewModel PropertiesViewModel { get => propertiesViewModel; private set => propertiesViewModel = value; }

        /// <summary>
        /// Refreshes the available database listing.
        /// </summary>
        public ICommand RefreshAvailableDatabasesCommand { get; private set; }

        /// <summary>
        /// The currently selected database from the AvailableDatabase collection..
        /// </summary>
        /// <see cref="AvailableDatabases"/>
        public string SelectedDatabase
        {
            get { return selectedDatabase; }
            set
            {
                SetProperty(ref selectedDatabase, value);
                RaisePropertyChanged("SelectedDatabase");
            }
        }

        #endregion Public Properties

        #region Private Methods

        /// <summary>
        /// The application can be closed if the generator is not running.
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private bool canClose()
        {
            return GeneratorViewModel.NotGenerating;
        }

        /// <summary>
        /// Connection can only be started if a database is selected and if the GeneratorViewModel is not generating.
        /// </summary>
        /// <param name="arg"></param>
        /// <returns>If it can connect.</returns>
        private bool canConnectDatabase()
        {
            return !string.IsNullOrEmpty(SelectedDatabase) && GeneratorViewModel.NotGenerating;
        }

        private bool canDeleteDatabase()
        {
            return selectedDatabase != null && selectedDatabase != unitOfWork.ConnectionStringBuilder.Database;
        }

        /// <summary>
        /// Login to a database is only possible if there is no current user set.
        /// </summary>
        /// <returns></returns>
        [ExcludeFromCodeCoverage]
        private bool canLogin()
        {
            return !string.IsNullOrEmpty(unitOfWork.ConnectionStringBuilder.UserID);
        }

        [ExcludeFromCodeCoverage]
        private void onChangePath()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                InitialDirectory = Directory.GetCurrentDirectory(),
                DefaultExt = ".txt",
                Filter = "Text|*.txt|All|*.*"
            };
            openFileDialog.ShowDialog();
            if (!string.IsNullOrEmpty(openFileDialog.FileName) && openFileDialog.CheckFileExists)
            {
                Path = openFileDialog.FileName;
            }
        }

        [ExcludeFromCodeCoverage]
        private void onClose()
        {
            eventAggregator.GetEvent<WriteLogEvent>().Publish("Disconnected. Application closed.");
            unitOfWork.Save();
            Environment.Exit(Environment.ExitCode);
        }

        /// <summary>
        /// Saves and disposes the current database and connects to a new one.
        /// </summary>
        /// <param name="arg"></param>
        private void onConnectDatabase()
        {
            eventAggregator.GetEvent<WriteLogEvent>().Publish("Saved and disconnected database " + unitOfWork.ConnectionStringBuilder.Database);
            unitOfWork.Connect(SelectedDatabase);
            eventAggregator.GetEvent<DatabaseChangedEvent>().Publish();
            eventAggregator.GetEvent<WriteLogEvent>().Publish("Connected with database " + SelectedDatabase);
        }

        private void onDeleteDatabase()
        {
            unitOfWork.DeleteDatabase(selectedDatabase);
            eventAggregator.GetEvent<WriteLogEvent>().Publish("deleted " + selectedDatabase);
            RaisePropertyChanged("AvailableDatabases");
        }

        /// <summary>
        /// Displays a LoginDialog and asks the user to enter login data until a connection was successful.
        /// </summary>
        [ExcludeFromCodeCoverage]
        private async void onLogin()
        {
            while (!unitOfWork.IsConnected())
            {
                var result = await dialogCoordinator.ShowLoginAsync(this, "Database Login", "Please enter your username and password (if neccessary).");
                unitOfWork.ConnectionStringBuilder.UserID = result.Username;
                unitOfWork.ConnectionStringBuilder.Password = result.Password;
                if (!unitOfWork.IsConnected())
                {
                    //reset input
                    unitOfWork.ConnectionStringBuilder.UserID = string.Empty;
                    unitOfWork.ConnectionStringBuilder.Password = null;
                    var answer = await dialogCoordinator.ShowMessageAsync(this, "Error", "Login has failed! " +
                        "Check your login data and make sure you have installed MariaDB. " +
                        "Press OK to try again or CANCEL to exit Graphitty.", MessageDialogStyle.AffirmativeAndNegative);
                    if (answer == MessageDialogResult.Negative)
                    {
                        Application.Current.Shutdown();
                    }
                }
            }
            eventAggregator.GetEvent<WriteLogEvent>().Publish("Connected to " + unitOfWork.ConnectionStringBuilder.UserID + "@" + unitOfWork.ConnectionStringBuilder.Server);
            AvailableDatabases = new ObservableCollection<string>(unitOfWork.AvailableDatabases);
            unitOfWork.Connect("graphitty");
            eventAggregator.GetEvent<DatabaseChangedEvent>().Publish();
        }

        private void onRefreshAvailableDatabases()
        {
            RaisePropertyChanged("AvailableDatabases");
        }

        #endregion Private Methods
    }
}