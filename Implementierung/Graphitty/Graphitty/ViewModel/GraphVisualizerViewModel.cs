using Graphitty.Model.GraphVisualizer;
using Graphitty.ViewModel.Events;
using Prism.Events;
using Prism.Mvvm;
using System.Windows.Input;
using GraphX.PCL.Common.Enums;
using GraphX.PCL.Logic.Algorithms.LayoutAlgorithms;
using GraphX.Controls;
using GraphX.Controls.Models;
using System.Windows;
using System;
using Prism.Commands;
using Graphitty.Model.Graphs;
using Graphitty.Model.Algorithms;
using Graphitty.Model.DataAccessLayer;

namespace Graphitty.ViewModel
{
    /// <summary>
    /// This ViewModel offers commands to manipulate and display the currently selected graph.
    /// Publishes the GraphEditedEvent.
    /// Subscribes the SelectionChangedEvent.
    /// </summary>
    /// <see cref="View.GraphVisualizerView"/>
    /// <see cref="ViewModel.Events.GraphEditedEvent"/>
    /// <see cref="ViewModel.Events.SelectionChangedEvent"/>
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class GraphVisualizerViewModel : BindableBase
    {
        #region Commands

        public ICommand EdgeMCommand { get; private set; }

        public ICommand RelayoutCommand { get; private set; }

        public ICommand SaveCommand { get; private set; }

        //SM = Set Mode Commands
        public ICommand SetModeCommand { get; private set; }

        public ICommand UpdateCommand { get; private set; }

        public ICommand VertexMCommand { get; private set; }

        public ICommand VertexMouseUpCommand { get; private set; }

        public ICommand VertexSelectedCommand { get; private set; }

        //Basic Commands
        public ICommand ZCClickCommand { get; private set; }

        #endregion Commands

        #region Variables

        public GUIGraphArea Area;
        public GUIGraph GUIGraphFromGraph;
        public GUIGXLogicCore GUILogicCore;
        private GVMode _mode;
        private GraphToGUIConverter converter;         //Eventaggregator

        private IEventAggregator eventAggregator;

        private bool isGraphEdited;
        private IUnitOfWork unitOfWork = UnitOfWork.Instance;

        //Modes of the GUI -> 0=idle, 1=addVertex, 2=removeVertex, 3=addEdge, 4=removeEdge, 5=vertexDrag
        public enum GVMode { idle, addVertex, removeVertex, addEdge, removeEdge, vertexDrag };

        /// <summary>
        /// Used to track if the displayed graph was edited.
        /// </summary>
        public bool IsGraphEdited
        {
            get => isGraphEdited;
            private set
            {
                SetProperty(ref isGraphEdited, value);
                RaisePropertyChanged("IsGraphEdited");
            }
        }

        //Current GUI Mode
        public GVMode mode
        {
            get => _mode;
            set { SetProperty(ref _mode, value); RaisePropertyChanged("mode"); }
        }

        //Saves Vertices for Edgebinding
        private VertexControl vertex1 { get; set; }

        private VertexControl vertex2 { get; set; }

        #endregion Variables

        #region Public Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public GraphVisualizerViewModel(IEventAggregator eventAggregator)
        {
            //Setup Commands
            SetModeCommand = new DelegateCommand<object>(SetMode);

            ZCClickCommand = new DelegateCommand<MouseButtonEventArgs>(ZCreceivedClick);
            VertexMCommand = new DelegateCommand<VertexClickedEventArgs>(VertexMClick);
            EdgeMCommand = new DelegateCommand<EdgeClickedEventArgs>(EdgeMClick);
            RelayoutCommand = new DelegateCommand(RelayoutGraph);
            UpdateCommand = new DelegateCommand(Update);
            SaveCommand = new DelegateCommand(SaveGraph).ObservesCanExecute(() => IsGraphEdited);
            VertexSelectedCommand = new DelegateCommand<VertexSelectedEventArgs>(onVertexSelected);
            VertexMouseUpCommand = new DelegateCommand<VertexSelectedEventArgs>(onVertexMouseUp);

            //TODO THIS IS A WIP TESTGRAPH! REMOVE ASAP
            //GUIGraphFromGraph = new GUIGraph();
            //GUIGraphFromGraph.AddVerticesAndEdge(new GUIEdge(new GUIVertex(Brushes.Green, 1), new GUIVertex(Brushes.Silver, 2), Brushes.Pink));
            //GUIGraphFromGraph.AddVerticesAndEdge(new GUIEdge(new GUIVertex(Brushes.Blue, 3), new GUIVertex(Brushes.Purple, 4), Brushes.Yellow));

            //Setup the logic
            SetupLogic();

            //Setup Mode
            mode = GVMode.idle;

            //Setup Edgeconnect
            vertex1 = null;
            vertex2 = null;

            //Setup Eventaggregator
            this.eventAggregator = eventAggregator;
            this.eventAggregator.GetEvent<SelectionChangedEvent>().Subscribe(g =>
            {
                convertGraph(g);
                IsGraphEdited = false;
            });

            //Setup converter
            converter = new GraphToGUIConverter();
        }

        /// <summary>
        /// Constructor for testing
        /// </summary>
        public GraphVisualizerViewModel(IEventAggregator eventAggregator, IUnitOfWork pUoW) : this(eventAggregator)
        {
            unitOfWork = pUoW;
        }

        #endregion Public Constructors

        #region Private Methods

        //Gets called by SelectionChangedEvent. Converts a Graph to a GUIGraph and displays it.
        private void convertGraph(GraphEntity pGraph)
        {
            //assign graph, so we can write back changes
            //convert graph to GUIGraph to display it
            if (pGraph == null) { DisposeGraph(); return; }
            GUIGraphFromGraph = converter.Convert(new Graph(pGraph.BFSCode));

            Graph graph = new Graph(pGraph.BFSCode)
            {
                TotalChromaticNumber = pGraph.TotalChromaticNumber
            };
            TotalColoring totalColoring = new TotalColoring();
            graph = totalColoring.ColorGraph(graph);
            GUIGraphFromGraph = converter.Convert(graph);
            //Setup the GraphX Components
            SetupLogic();
            UpdateViewer();
            //Reset Tools
            SetMode("0");
            vertex1 = null;
            vertex2 = null;
            Area.SetVerticesDrag(false);
        }

        //Disposes current Graph
        private void DisposeGraph()
        {
            Area.Dispose();
            Area.ClearLayout(true, false, true);
        }

        //Handles all the clicking on Edges functionality
        //Implements: RemoveEdge
        private void EdgeMClick(EdgeClickedEventArgs e)
        {
            if (mode.Equals(GVMode.removeEdge))
            {
                GUILogicCore.Graph.RemoveEdge(e.Control.GetDataEdge<GUIEdge>());
                GUIGraphFromGraph.RemoveEdge(e.Control.GetDataEdge<GUIEdge>());
                Area.RemoveEdge(e.Control.GetDataEdge<GUIEdge>());
                IsGraphEdited = true;
            }
        }

        private bool hasGraph()
        {
            if (GUIGraphFromGraph != null) { return true; }
            return false;
        }

        private void onVertexMouseUp(VertexSelectedEventArgs args)
        {
            // add edge
            if (mode.Equals(GVMode.addEdge))
            {
                //Vertex1 is saved and a different vertex selected
                if (vertex1 != args.VertexControl && vertex2 == null)
                {
                    vertex2 = args.VertexControl;
                    var dataEdge = new GUIEdge(vertex1.GetDataVertex<GUIVertex>(), vertex2.GetDataVertex<GUIVertex>());
                    if (!GUILogicCore.Graph.ContainsEdge(dataEdge))
                    {
                        GUILogicCore.Graph.AddEdge(dataEdge);
                        IsGraphEdited = true;
                        Area.AddEdge(dataEdge, new EdgeControl(vertex1, vertex2, dataEdge));
                    }

                    //Clear Verts
                    vertex1 = null;
                    vertex2 = null;
                }
            }
        }

        private void onVertexSelected(VertexSelectedEventArgs args)
        {
            if (mode.Equals(GVMode.addEdge))
            {
                vertex1 = args.VertexControl;
            }
        }

        //Relayouts the Graph without recalculating
        private void RelayoutGraph()
        {
            Area.RelayoutGraph(true);
        }

        //Saves the edited Graph
        private void SaveGraph()
        {
            var confirmResult = MessageBox.Show("Do you want to save the current graph?",
                                     "Confirm Save",
                                     MessageBoxButton.YesNo);
            if (confirmResult == MessageBoxResult.No)
            {
                return;
            }
            //Check for consistency
            Graph graphToSave = converter.Convert(GUIGraphFromGraph);
            if (graphToSave == null)
            {
                MessageBox.Show("Error: The Graph is not connected.", "Error: Graph not connected", MessageBoxButton.OK); //Graph is not connected
                return;
            }
            unitOfWork.Connect(UnitOfWork.DatabasePrefix + graphToSave.NumVertices);
            AlgorithmRunner _aR = new AlgorithmRunner();
            graphToSave = _aR.RunAlgorithms(graphToSave);
            if (graphToSave == null)
            {
                MessageBox.Show("The Graph you are trying to save already exists.", "Error: Graph already exists", MessageBoxButton.OK); //Graph is not connected
                return;
            }
            GraphEntity _g = new GraphEntity
            {
                BFSCode = graphToSave.BFSCode,
                BFSCodeBitvector = graphToSave.BFSCodeBitvector,
                TotalChromaticNumber = graphToSave.TotalChromaticNumber,
                IsTCCFulfilled = graphToSave.IsTCCFulfilled,
                AverageVertexDegree = graphToSave.AverageVertexDegree,
                LargestCliqueSize = graphToSave.LargestCliqueSize,
                MaxVertexDegree = graphToSave.MaxVertexDegree,
                MinimalColouringComplexityInMiliSeconds = graphToSave.MinimalColouringComplexityInMiliSeconds,
                NumCliquesOfSizeK = graphToSave.NumCliquesOfSizeK,
                NumDenserGraphsBFS = graphToSave.NumDenserGraphsBFS,
                NumDenserGraphsProfile = graphToSave.NumDenserGraphsProfile,
                NumEdges = graphToSave.NumEdges,
                NumGraphsWithSmallerEqualChromaticNumber = graphToSave.NumGraphsWithSmallerEqualChromaticNumber,
                Profile = graphToSave.Profile,
                NumVertices = graphToSave.NumVertices,
            };
            if (unitOfWork.GraphRepository.Exists(g => g.BFSCodeBitvector == _g.BFSCodeBitvector))
            {
                unitOfWork.GraphRepository.Update(_g);
                unitOfWork.Save();
            }
            else
            {
                unitOfWork.GraphRepository.Create(_g);
                unitOfWork.Save();
            }
            IsGraphEdited = false;
            eventAggregator.GetEvent<DatabaseChangedEvent>().Publish();
            eventAggregator.GetEvent<GraphEditedEvent>().Publish(_g);
        }

        //Sets the mode of the Statemachine
        private void SetMode(object pMode)
        {
            switch ((String)pMode)
            {
                case "0": mode = GVMode.idle; Area.SetVerticesDrag(false); vertex1 = null; vertex2 = null; break;
                case "1": mode = GVMode.addVertex; break;
                case "2": mode = GVMode.removeVertex; break;
                case "3": mode = GVMode.addEdge; break;
                case "4": mode = GVMode.removeEdge; break;
                case "5": mode = GVMode.vertexDrag; VertexMDraggable(); break;
                default: mode = GVMode.idle; break;
            }
        }

        //Generates the Graph logic
        private void SetupLogic()
        {
            if (GUIGraphFromGraph == null) { return; }
            //Create logic core and set the graph to the logic
            var logicCore = new GUIGXLogicCore() { Graph = GUIGraphFromGraph };
            //This property sets layout algorithm that will be used to calculate vertices positions
            //Different algorithms uses different values and params
            logicCore.DefaultLayoutAlgorithm = LayoutAlgorithmTypeEnum.KK;
            //Now we can set parameters for selected algorithm using AlgorithmFactory property. This property provides methods for
            //creating all available algorithms and algo parameters.
            logicCore.DefaultLayoutAlgorithmParams = logicCore.AlgorithmFactory.CreateLayoutParameters(LayoutAlgorithmTypeEnum.KK);
            //Unfortunately to change algo parameters you need to specify params type which is different for every algorithm.
            ((KKLayoutParameters)logicCore.DefaultLayoutAlgorithmParams).Seed = 30;

            //This property sets vertex overlap removal algorithm.
            //Such algorithms help to arrange vertices in the layout so no one overlaps each other.
            logicCore.DefaultOverlapRemovalAlgorithm = OverlapRemovalAlgorithmTypeEnum.FSA;
            //Default parameters are created automaticaly when new default algorithm is set and previous params were NULL
            logicCore.DefaultOverlapRemovalAlgorithmParams.HorizontalGap = 50;
            logicCore.DefaultOverlapRemovalAlgorithmParams.VerticalGap = 50;

            //No need for edgerouting
            logicCore.DefaultEdgeRoutingAlgorithm = EdgeRoutingAlgorithmTypeEnum.None;

            //This property sets async algorithms computation so methods like: Area.RelayoutGraph() and Area.GenerateGraph()
            //will run async with the UI thread. Completion of the specified methods can be catched by corresponding events:
            //Area.RelayoutFinished and Area.GenerateGraphFinished.
            logicCore.AsyncAlgorithmCompute = false;

            //Finally assign logic core
            GUILogicCore = logicCore;
            RaisePropertyChanged("GUILogicCore");
        }

        //Invokes the UpdateViewer() Method (Used with Commands)
        private void Update()
        {
            Console.Out.WriteLine("UpdateCommand");
            UpdateViewer();
        }

        //Updates the Viewer(Area)
        private void UpdateViewer()
        {
            Area.LogicCore = GUILogicCore;
            if (Area.LogicCore != null)
            {
                Area.GenerateGraph(true, true);
                //Sets Edgestyle to solid
                Area.SetEdgesDashStyle(EdgeDashStyle.Solid);
                //Sets Vertexshape to circle
                Area.SetVerticesMathShape(VertexShape.Circle);
                //Make Vertices non-movable
                Area.SetVerticesDrag(false);
            }
        }

        //Handles all the clicking on Vertices functionality
        //Implements: RemoveVertex, AddEdge
        private void VertexMClick(VertexClickedEventArgs e)
        {
            if (mode.Equals(GVMode.removeVertex))
            {
                var vert = e.Control.GetDataVertex<GUIVertex>();
                if (vertex1 != null && vertex1.Vertex.Equals(vert))
                {
                    vertex1 = null;
                }
                Area.RemoveVertexAndEdges(e.Control.GetDataVertex<GUIVertex>());
                GUILogicCore.Graph.RemoveVertex(vert);
                GUIGraphFromGraph.RemoveVertex(vert);
                IsGraphEdited = true;
                return;
            }
        }

        //Makes Vertices Draggable
        //Implements: DraggableVertex
        private void VertexMDraggable()
        {
            mode = GVMode.vertexDrag;
            Area.SetVerticesDrag(true, true);
        }

        //Handles all the clicking on ZoomControl functionality
        //Implements: Break, AddVertex
        private void ZCreceivedClick(MouseButtonEventArgs e)
        {
            //Rightclick detected -> Break from all modes
            if (e.ChangedButton.ToString().Equals("Right"))
            {
                mode = GVMode.idle;
                vertex1 = null;
                vertex2 = null;
                Area.SetVerticesDrag(false);
                return;
            }

            //If Mode allows to add a Vertex, add one
            if (mode.Equals(GVMode.addVertex))
            {
                if (GUIGraphFromGraph == null) { return; }

                var offset = 0.0d;
                if (Area.LogicCore.Graph.VertexCount != 0)
                {
                    offset = Area.GetAllVertexControls()[0].ActualHeight / 2;
                }

                //Create Vertex and point it to its place
                Point mousePosition = e.GetPosition(Area);
                mousePosition.Offset(-offset, -offset);

                var vert = new GUIVertex();

                Area.LogicCore.Graph.AddVertex(vert);
                Area.AddVertex(vert, new VertexControl(vert), true);
                IsGraphEdited = true;
                Area.VertexList[vert].SetPosition(mousePosition);
            }
        }

        #endregion Private Methods
    }
}