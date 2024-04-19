using Graphitty.Model.Graphs;
using System.Collections.Generic;
using System.Linq;
using Graphitty.Model.Algorithms;
using Graphitty.Model.DataAccessLayer;
using System.Diagnostics.CodeAnalysis;

namespace Graphitty.Model.GraphGeneration
{
    public class NextDenserGenerator : Generator
    {
        #region Private Fields

        private bool generatedDuplicate;
        private int numDuplicates;

        #endregion Private Fields

        #region Public Constructors

        [ExcludeFromCodeCoverage]
        public NextDenserGenerator() : base(null)
        {
        }

        public NextDenserGenerator(IUnitOfWork unitOfWork) : base(null, unitOfWork)
        {
        }

        #endregion Public Constructors

        #region Public Properties

        [ExcludeFromCodeCoverage]
        public bool GeneratedDuplicate { get => generatedDuplicate; private set => generatedDuplicate = value; }

        [ExcludeFromCodeCoverage]
        public int NumDuplicates { get => numDuplicates; private set => numDuplicates = value; }

        #endregion Public Properties

        #region Public Methods

        ///<summary>
        /// Helping method to generate and store just the next denser graph of the given graph.
        /// </summary>
        public void GenerateNextDenser(GraphEntity startingGraph) => GenerateSuccessiveNextDenser(1, startingGraph);

        ///<summary>
        /// Generates and stores a graph, which is the next denser one to the given graph.
        /// Continues this procedure outgoing from the resulting graph as often as the given integer states.
        /// </summary>
        public void GenerateSuccessiveNextDenser(int numGraphs, GraphEntity startingGraph)
        {
            GeneratedDuplicate = false;
            NumDuplicates = 0;
            int _iterations = numGraphs;
            BFSCode bFSCode = new BFSCode(startingGraph.BFSCode);
            string currentBitvector = bFSCode.GetBitVector();
            while (_iterations > 0)
            {
                bool validNextDenser = false;
                bool connected = true;
                while (!validNextDenser)
                {
                    BFSCode currentBFSCode = new BFSCode(currentBitvector);
                    Graph tempGraph = new Graph(currentBFSCode.BFSToString());
                    if (tempGraph.NumEdges == (tempGraph.NumVertices * (tempGraph.NumVertices - 1)) / 2 && connected)
                    {
                        return;
                    }
                    //1 is added to the bitvector of the given graph and the resulting BFSCode is computed
                    string newBitVector = "";
                    bool previous = true;
                    for (int i = currentBitvector.Length - 1; i >= 0; i--)
                    {
                        if (!previous)
                        {
                            newBitVector = currentBitvector[i].ToString() + newBitVector;
                            continue;
                        }
                        previous = currentBitvector[i] == '1' ? true : false;
                        newBitVector = (!previous ? "1" : "0") + newBitVector;
                    }
                    currentBitvector = newBitVector;
                    currentBFSCode = new BFSCode(currentBitvector);
                    tempGraph = new Graph(currentBFSCode.BFSToString());
                    //connectivity requirement is tested
                    if (tempGraph.NumVertices != startingGraph.NumVertices)
                    {
                        connected = false;
                        continue;
                    }
                    connected = true;
                    //database is checked if the graph belonging to computed bfscode already exists
                    BreadthFirstSearch breadthFirstSearchRunner = new BreadthFirstSearch();
                    breadthFirstSearchRunner.Run(tempGraph);
                    IEnumerable<GraphEntity> duplicate = unitOfWork.GraphRepository.Get(g => g.BFSCode.Equals(tempGraph.BFSCode));
                    if (duplicate.Any())
                    {
                        _iterations--;
                        GeneratedDuplicate = true;
                        NumDuplicates++;
                        break;
                    }
                    //tests if the newly computed BFSCode was minimal
                    if ((new BFSCode(currentBitvector)).CompareTo(new BFSCode(tempGraph.BFSCode)) == 0)
                    {
                        SaveGraph(CalculateTraits(tempGraph));
                        validNextDenser = true;
                        _iterations--;
                    }
                }
            }
        }

        #endregion Public Methods
    }
}