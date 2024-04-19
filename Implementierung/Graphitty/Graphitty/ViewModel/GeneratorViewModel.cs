using Graphitty.Model.DataAccessLayer;
using Graphitty.Model.GraphGeneration;
using Graphitty.Model.Graphs;
using Graphitty.ViewModel.Events;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Windows.Input;

namespace Graphitty.ViewModel
{
    /// <summary>
    /// Offers properties to configure the graph generation. The GeneratorView consumes these properties.
    /// Subscribes to the SelectionChangedEvent.
    /// Publishes the DatabaseChangedEvent and WriteLogEvent.
    /// </summary>
    /// <see cref="View.GeneratorView"/>
    /// <see cref="ViewModel.Events.DatabaseChangedEvent"/>
    /// <see cref="ViewModel.Events.WriteLogEvent"/>
    public class GeneratorViewModel : BindableBase
    {
        #region Private Fields

        //used to define default database name
        private string databaseBaseName = UnitOfWork.DatabasePrefix;

        private ObservableCollection<IEdgeFactoryViewModel> edgeFactories;
        private IEventAggregator eventAggregator;
        private bool generateTuple;
        private bool notGenerating;
        private int numberOfGraphs;
        private int numVerticesSelectedVertexFactory;
        private IEdgeFactoryViewModel selectedEdgeFactory;
        private GraphEntity selectedGraph;
        private IVertexFactoryViewModel selectedVertexFactory;

        private int tryGenerateThreshold;

        private UnitOfWork unitOfWork = UnitOfWork.Instance;

        private ObservableCollection<IVertexFactoryViewModel> vertexFactories;

        private GraphEntity SelectedGraph
        {
            get => selectedGraph;
            set
            {
                selectedGraph = value;
                raiseCanExecuteChanged();
            }
        }

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// Creates a new instance of the GeneratorViewModel and initializes it.
        /// </summary>
        /// <param name="eventAggregator">The EventAggregator reference from which this ViewModel gets notified about events and publishes to.</param>
        public GeneratorViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            this.eventAggregator.GetEvent<SelectionChangedEvent>().Subscribe(g => SelectedGraph = g);
            //addDefaultGraphs();

            // command initialization
            GenerateCommand = new DelegateCommand(onGenerate, canGenerate);
            GenerateNextDenserSelectedCommand = new DelegateCommand(onGenerateNextDenserSelected, canGenerateNextDenserSelected);
            GenerateSuccessiveNextDenserCommand = new DelegateCommand(onGenerateSuccessiveNextDenser, canGenerateSuccessiveNextDenser);
            NotGenerating = true;

            //initialize vertex factories
            vertexFactories = new ObservableCollection<IVertexFactoryViewModel>
            {
                new MaxVerticesFactoryViewModel(),
                new FixedNumVerticesFactoryViewModel()
                //add constructor call from a new vertex factory here
            };

            //initialize edge factories
            edgeFactories = new ObservableCollection<IEdgeFactoryViewModel>
            {
                new DegreeRangeFactoryViewModel()
                //add constructor call from a new edge factory here
            };

            NumVerticesSelectedVertexFactory = 20;
            TryGenerateThreshold = 50;
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// The edge factories from which the user can select from.
        /// </summary>
        public ObservableCollection<IEdgeFactoryViewModel> EdgeFactories
        {
            get
            {
                return edgeFactories;
            }
        }

        /// <summary>
        /// The command to start the graph generation.
        /// </summary>
        public ICommand GenerateCommand { get; private set; }

        /// <summary>
        /// Generates the next denser graph of the currently selected one.
        /// </summary>
        public ICommand GenerateNextDenserSelectedCommand { get; private set; }

        /// <summary>
        /// Successively generate the next denser graph of the currently selected one.
        /// </summary>
        public ICommand GenerateSuccessiveNextDenserCommand { get; private set; }

        /// <summary>
        /// Defines if the user wants to generate a tuple of a graph and its next denser instance.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public bool GenerateTuple
        {
            get { return generateTuple; }
            set
            {
                SetProperty(ref generateTuple, value);
                RaisePropertyChanged("GenerateTuple");
            }
        }

        /// <summary>
        /// The inverted state of the generator.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public bool NotGenerating
        {
            get => notGenerating;
            private set
            {
                SetProperty(ref notGenerating, value);
                RaisePropertyChanged("NotGenerating");
                // notify the GenerateCommand to evaluate if the generation can be started.
                raiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// The number of graphs the user wants to generate.
        /// </summary>
        public int NumberOfGraphs
        {
            get { return numberOfGraphs; }
            set
            {
                SetProperty(ref numberOfGraphs, value);
                RaisePropertyChanged("NumberOfGraphs");
                // notify the GenerateCommand to evaluate if the generation can be started.
                raiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// The number of vertices which the user configured in the currrently selected vertex factory.
        /// </summary>
        /// <remarks>
        /// Every vertex factory offers a number of vertices which can be configured due to the IVertexFactoryViewModel
        /// </remarks>
        /// <see cref="IVertexFactoryViewModel"/>
        /// <use>
        /// The template of the FactoryViewModel needs to bind to this (the GeneratorViewModels) property.
        /// E.g.: TextBox Text="{Binding DataContext.NumVerticesSelectedVertexFactory, RelativeSource={RelativeSource FindAncestor, AncestorType=UserControl}, Mode=TwoWay, UpdateSourceTrigger=PropertyChanged}"
        /// </use>
        public int NumVerticesSelectedVertexFactory
        {
            get => numVerticesSelectedVertexFactory;
            set
            {
                SetProperty(ref numVerticesSelectedVertexFactory, value);
                RaisePropertyChanged("NumVerticesSelectedVertexFactory");
                // restrict the maximal possible number of edges, that the user can select
                if (selectedVertexFactory != null)
                {
                    selectedVertexFactory.NumVertices = value;
                    if (selectedEdgeFactory != null)
                    {
                        selectedEdgeFactory.MaxPossibleNumEdges = selectedVertexFactory.NumVertices - 1;
                    }
                }
            }
        }

        /// <summary>
        /// The currently selected edge factory.
        /// </summary>
        public IEdgeFactoryViewModel SelectedEdgeFactory
        {
            get
            {
                return selectedEdgeFactory;
            }
            set
            {
                SetProperty(ref selectedEdgeFactory, value);
                RaisePropertyChanged("SelectedEdgeFactory");
                // notify the GenerateCommand to evaluate if the generation can be started.
                raiseCanExecuteChanged();
                // restrict the maximal possible number of edges, that the user can select
                if (selectedVertexFactory != null)
                {
                    selectedEdgeFactory.MaxPossibleNumEdges = selectedVertexFactory.NumVertices - 1;
                }
            }
        }

        /// <summary>
        /// The currently selected vertex factory.
        /// </summary>
        public IVertexFactoryViewModel SelectedVertexFactory
        {
            get
            {
                return selectedVertexFactory;
            }
            set
            {
                SetProperty(ref selectedVertexFactory, value);
                RaisePropertyChanged("SelectedVertexFactory");
                // notify the GenerateCommand to evaluate if the generation can be started.
                raiseCanExecuteChanged();
                // restrict the maximal possible number of edges, that the user can select
                if (selectedEdgeFactory != null)
                {
                    selectedEdgeFactory.MaxPossibleNumEdges = selectedVertexFactory.NumVertices - 1;
                }
            }
        }

        [ExcludeFromCodeCoverage]
        public int TryGenerateThreshold
        {
            get
            {
                return tryGenerateThreshold;
            }
            set
            {
                SetProperty(ref tryGenerateThreshold, value);
                RaisePropertyChanged("TryGenerateThreshold");
            }
        }

        /// <summary>
        /// The available vertex factories from which the user can choose from.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public ObservableCollection<IVertexFactoryViewModel> VertexFactories
        {
            get
            {
                return vertexFactories;
            }
        }

        #endregion Public Properties

        #region Private Methods

        /// <summary>
        /// Evaluates if the generation can be started with the current setup.
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private bool canGenerate()
        {
            return (NotGenerating && SelectedVertexFactory != null && selectedEdgeFactory != null && NumberOfGraphs > 0);
        }

        private bool canGenerateNextDenserSelected()
        {
            if (SelectedGraph == null) return false;
            bool isComplete = SelectedGraph.NumEdges == (SelectedGraph.NumVertices * (SelectedGraph.NumVertices - 1)) / 2;
            return NotGenerating && !isComplete;
        }

        private bool canGenerateSuccessiveNextDenser()
        {
            if (SelectedGraph == null) return false;
            bool isComplete = SelectedGraph.NumEdges == (SelectedGraph.NumVertices * (SelectedGraph.NumVertices - 1)) / 2;
            return NotGenerating && NumberOfGraphs > 1 && !isComplete;
        }

        [ExcludeFromCodeCoverage]
        private void onGenerate()
        {
            //change state to: generating
            NotGenerating = false;
            eventAggregator.GetEvent<WriteLogEvent>().Publish("Generating " + NumberOfGraphs + " Graphs...");
            unitOfWork.Connect(databaseBaseName + selectedVertexFactory.NumVertices);

            Thread t = new Thread(new ThreadStart(() =>
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                //setup Graphbuilder
                GraphBuilder graphBuilder = new GraphBuilder(SelectedVertexFactory.VertexFactory, SelectedEdgeFactory.EdgeFactory);
                Generator generator = new Generator(graphBuilder)
                {
                    TupelGeneration = GenerateTuple,
                    TryGenerateThreshold = TryGenerateThreshold
                };
                generator.StartGenerating(numberOfGraphs);

                //Generation ended
                NotGenerating = true;
                stopwatch.Stop();
                eventAggregator.GetEvent<WriteLogEvent>().Publish("Generation of " + NumberOfGraphs + " Graphs finished! Took " + stopwatch.ElapsedMilliseconds + "ms.");
                eventAggregator.GetEvent<DatabaseChangedEvent>().Publish();
            }))
            {
                IsBackground = true
            };
            t.Start();
        }

        [ExcludeFromCodeCoverage]
        private void onGenerateNextDenserSelected()
        {
            //change state to: generating
            NotGenerating = false;
            eventAggregator.GetEvent<WriteLogEvent>().Publish("Generating next denser from selected graph...");

            Thread t = new Thread(new ThreadStart(() =>
            {
                //generate
                NextDenserGenerator nextDenserGenerator = new NextDenserGenerator();
                nextDenserGenerator.GenerateNextDenser(selectedGraph);

                //generation ended
                NotGenerating = true;
                if (!nextDenserGenerator.GeneratedDuplicate)
                {
                    eventAggregator.GetEvent<WriteLogEvent>().Publish("Successfully generated next denser graph!");
                }
                else
                {
                    eventAggregator.GetEvent<WriteLogEvent>().Publish("Next denser graph already exists in database!");
                }
                eventAggregator.GetEvent<DatabaseChangedEvent>().Publish();
            }))
            {
                IsBackground = true
            };
            t.Start();
        }

        [ExcludeFromCodeCoverage]
        private void onGenerateSuccessiveNextDenser()
        {
            //change state to: generating
            NotGenerating = false;
            eventAggregator.GetEvent<WriteLogEvent>().Publish("Generating successively next denser graph from selected...");

            Thread t = new Thread(new ThreadStart(() =>
            {
                //generate
                NextDenserGenerator nextDenserGenerator = new NextDenserGenerator();
                nextDenserGenerator.GenerateSuccessiveNextDenser(NumberOfGraphs, selectedGraph);

                //generation ended
                NotGenerating = true;
                if (!nextDenserGenerator.GeneratedDuplicate)
                {
                    eventAggregator.GetEvent<WriteLogEvent>().Publish("Successfully generated " + NumberOfGraphs + " next denser graphs!");
                }
                else
                {
                    int numDuplicates = nextDenserGenerator.NumDuplicates;
                    int actualNumberOfGraphsGenerated = NumberOfGraphs - numDuplicates;
                    eventAggregator.GetEvent<WriteLogEvent>().Publish("Generated " + actualNumberOfGraphsGenerated + " next denser graphs! " + numDuplicates + " graph(s) already existed.");
                }
                eventAggregator.GetEvent<DatabaseChangedEvent>().Publish();
            }))
            {
                IsBackground = true
            };
            t.Start();
        }

        private void raiseCanExecuteChanged()
        {
            ((DelegateCommand)GenerateCommand).RaiseCanExecuteChanged();
            ((DelegateCommand)GenerateSuccessiveNextDenserCommand).RaiseCanExecuteChanged();
            ((DelegateCommand)GenerateNextDenserSelectedCommand).RaiseCanExecuteChanged();
        }

        #endregion Private Methods
    }
}