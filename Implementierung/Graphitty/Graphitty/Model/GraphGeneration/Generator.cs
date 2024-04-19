using Graphitty.Model.DataAccessLayer;
using Graphitty.Model.Graphs;
using Graphitty.Model.Algorithms;
using System;
using System.Reflection;
using System.Diagnostics.CodeAnalysis;

namespace Graphitty.Model.GraphGeneration
{
    public class Generator
    {
        #region Protected Fields

        protected IUnitOfWork unitOfWork;

        #endregion Protected Fields

        #region Private Fields

        private bool _testing;
        private GraphBuilder graphBuilder;
        private int tryGenerateThreshold;
        private bool tupelGeneration;

        #endregion Private Fields

        #region Public Constructors

        [ExcludeFromCodeCoverage]
        public Generator(GraphBuilder graphBuilder)
        {
            this.graphBuilder = graphBuilder;
            unitOfWork = UnitOfWork.Instance;
            _testing = false;
        }

        /// <summary>
        /// Creates a generator which accepts a custom IUnitOfWork instnce for testing purposes.
        /// </summary>
        /// <param name="unitOfWorkMock"></param>
        public Generator(GraphBuilder graphBuilder, IUnitOfWork unitOfWork)
        {
            this.graphBuilder = graphBuilder;
            this.unitOfWork = unitOfWork;
            _testing = true;
        }

        #endregion Public Constructors

        #region Public Properties

        public int TryGenerateThreshold { get => tryGenerateThreshold; set => tryGenerateThreshold = value; }
        public bool TupelGeneration { get => tupelGeneration; set => tupelGeneration = value; }

        #endregion Public Properties

        #region Public Methods

        ///<summary>
        /// Creates a data set of graphs and their inherent traits.
        /// </summary>
        /// <param name="numGraphs">The number of graphs to be generated.</param>
        public void StartGenerating(int numGraphs)
        {
            int _iterations = numGraphs;
            int _tries = 0;
            while (_iterations > 0)
            {
                Graph builtGraph = graphBuilder.BuildGraph();
                if (builtGraph != null)
                {
                    unitOfWork.Connect(UnitOfWork.DatabasePrefix + builtGraph.NumVertices);
                }
                Graph treatedGraph = CalculateTraits(builtGraph);
                if (treatedGraph != null)
                {
                    SaveGraph(treatedGraph);
                    _iterations--;
                    if (tupelGeneration)
                    {
                        if (_testing)
                        {
                            NextDenserGenerator ndgTest = new NextDenserGenerator(unitOfWork);
                            ndgTest.GenerateNextDenser(builtGraph);
                        }
                        else
                        {
                            NextDenserGenerator ndg = new NextDenserGenerator();
                            ndg.GenerateNextDenser(builtGraph);
                        }
                    }
                }
                else
                {
                    if (++_tries >= tryGenerateThreshold)
                    {
                        return;
                    }
                }
            }
        }

        #endregion Public Methods

        #region Protected Methods

        [ExcludeFromCodeCoverage]
        ///<summary>
        /// Induces the calculation of traits for a given graph.
        /// </summary>
        protected Graph CalculateTraits(Graph graph)
        {
            if (_testing)
            {
                AlgorithmRunner runner = new AlgorithmRunner(unitOfWork);
                return runner.RunAlgorithms(graph);
            }
            else
            {
                AlgorithmRunner runner = new AlgorithmRunner();
                return runner.RunAlgorithms(graph);
            }
        }

        [ExcludeFromCodeCoverage]
        ///<summary>
        /// Uses the repository to save a given graph in to data base.
        /// </summary>
        protected void SaveGraph(Graph graph)
        {
            GraphEntity graphEntity = new GraphEntity();
            Type graphEntityType = graphEntity.GetType();
            PropertyInfo[] properties = graphEntityType.GetProperties();
            foreach (PropertyInfo property in properties)
            {
                property.SetValue(graphEntity, property.GetValue(graph));
            }
            if (!(unitOfWork.GraphRepository.Exists(g => g.BFSCodeBitvector == graphEntity.BFSCodeBitvector)))
            {
                unitOfWork.GraphRepository.Create(graphEntity);
            }
            else
            {
                unitOfWork.GraphRepository.Update(graphEntity);
            }
            unitOfWork.Save();
        }

        #endregion Protected Methods
    }
}