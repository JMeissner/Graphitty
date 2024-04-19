using System.Collections.Generic;
using System.Linq;
using Graphitty.Model.DataAccessLayer;
using Graphitty.Model.Graphs;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Graphitty.Model.Algorithms
{
    [ExcludeFromCodeCoverageAttribute]
    public class AlgorithmRunner
    {
        #region Private Fields

        private BreadthFirstSearch BFS;
        private CliqueSearch cliqueSearch;
        private CompareBFS compareBFS;
        private CompareProfile compareProfile;
        private CompareTotalChromaticNumber compareTCC;
        private VertexDegrees degrees;
        private TotalColoring totalColoring;
        private IUnitOfWork unitOfWork;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// Initializes instances of all Algorithm and Comparison classes.
        /// Also gets an instance of the UnitOfWork.
        /// </summary>
        public AlgorithmRunner()
        {
            BFS = new BreadthFirstSearch();
            cliqueSearch = new CliqueSearch();
            totalColoring = new TotalColoring();
            degrees = new VertexDegrees();
            compareBFS = new CompareBFS();
            compareProfile = new CompareProfile();
            compareTCC = new CompareTotalChromaticNumber();
            unitOfWork = UnitOfWork.Instance;
        }

        /// <summary>
        /// Constructor for tests. It extends the other constructor.
        /// </summary>
        /// <param name="unitOfWork">Mock UnitOfWork</param>
        public AlgorithmRunner(IUnitOfWork unitOfWork) : this()
        {
            this.unitOfWork = unitOfWork;
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// Calculates the properties of a given graph using algorithms.
        /// </summary>
        /// <param name="graph">The current graph.</param>
        /// <returns>The same graph it got, or null if given graph was null or a duplicate.</returns>
        public Graph RunAlgorithms(Graph graph)
        {
            if (graph == null)
            {
                return null;
            }
            string debug = "";
            foreach (Edge e in graph.Edges)
            {
                debug = debug + " |" + e.Vertex1.ID + "<->" + e.Vertex2.ID;
            }
            Debug.WriteLine(debug);

            BFS.Run(graph);
            IEnumerable<GraphEntity> duplicate = unitOfWork.GraphRepository.Get(g => g.BFSCodeBitvector.Equals(graph.BFSCodeBitvector));
            if (duplicate.Any())
            {
                return null;
            }
            degrees.Run(graph);
            totalColoring.Run(graph);
            cliqueSearch.Run(graph);

            IEnumerable<GraphEntity> graphList = unitOfWork.GraphRepository.GetAll();
            foreach (GraphEntity compare in graphList)
            {
                compareBFS.Run(graph, compare);
                compareProfile.Run(graph, compare);
                compareTCC.Run(graph, compare);
                unitOfWork.GraphRepository.Update(compare);
            }

            return graph;
        }

        #endregion Public Methods
    }
}