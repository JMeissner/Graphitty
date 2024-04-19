using System.Collections.Generic;
using System.Linq;
using Graphitty.Model.Graphs;

namespace Graphitty.Model.Algorithms
{
    public class BreadthFirstSearch : Algorithm
    {
        #region Public Constructors

        public BreadthFirstSearch()
        {
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// Runs the graph through all of the private methods. This is a template method.
        /// </summary>
        /// <param name="graph">The current graph</param>
        public override void Run(Graph graph)
        {
            List<BFSCode> BFSCodes = new List<BFSCode>();

            //creates all the local codes of the different vertices and adds them to a list
            foreach (Vertex vertex in graph.Vertices)
            {
                BFSCodes.Add(breadthFirstSearch(vertex, graph));
            }

            BFSCodes.Sort();

            graph.BFSCode = BFSCodes[0].BFSToString();
            graph.BFSCodeBitvector = BFSCodes[0].GetBitVector();

            graph.Profile = createProfile(BFSCodes);
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Creates a local code of a vertex
        /// </summary>
        /// <param name="start">The vertex for which the local code is going to be created</param>
        /// <param name="graph">The current graph</param>
        /// <returns>Returns a list of edges, which is the local code</returns>
        private BFSCode breadthFirstSearch(Vertex start, Graph graph)
        {
            List<BFSCode> codes = new List<BFSCode>();
            List<List<Vertex>> stages = createStages(start, graph);
            Queue<int[]> numerationQueue = new Queue<int[]>();

            int[] firstNumeration = new int[graph.Vertices.Count];
            //initializes the BFSIDs of the vertices higher than is possible for sorting purposes
            for (int i = 0; i < firstNumeration.Length; i++)
            {
                firstNumeration[i] = graph.Vertices.Count + 1;
            }
            firstNumeration[start.ID - 1] = 1;
            int BFSIDStart = 2;
            int BFSIDMax = 0;
            numerationQueue.Enqueue(firstNumeration);

            //finds the different possible numerations for each stage
            for (int stageCount = 1; stageCount < stages.Count; stageCount++)
            {
                int BFSIDCounter = BFSIDStart;
                List<int[]> currentStageNum = new List<int[]>();

                while (numerationQueue.Count != 0)
                {
                    int[] currentNumeration = numerationQueue.Dequeue();
                    setBFSID(graph, currentNumeration);
                    List<List<Vertex>> sets = createSets(stages[stageCount], graph, stages[stageCount - 1]);

                    List<int[]> permutations = permute(sets);

                    //creates the new numerations from the current numeration, by adding the permutations onto it
                    foreach (int[] permutation in permutations)
                    {
                        int[] numeration = new int[graph.Vertices.Count];
                        //copies current array to a new one and adds a permutation to it
                        System.Array.Copy(currentNumeration, numeration, graph.Vertices.Count);
                        for (int i = 0; i < permutation.Length; i++)
                        {
                            numeration[permutation[i] - 1] = BFSIDCounter;
                            BFSIDCounter++;
                        }
                        currentStageNum.Add(numeration);
                        BFSIDMax = BFSIDCounter;
                        BFSIDCounter = BFSIDStart;
                    }
                }

                BFSIDStart = BFSIDMax;

                List<BFSCode> tempBFSCode = new List<BFSCode>();
                //creates BFSCodes and sorts them lexicographically
                foreach (int[] numeration in currentStageNum)
                {
                    BFSCode code = new BFSCode(createBFSCode(graph, numeration));
                    int[] tag = new int[1] { currentStageNum.IndexOf(numeration) };
                    code.BFSEdges.Add(tag);
                    tempBFSCode.Add(code);
                }

                tempBFSCode.Sort();

                //saves all complete codes in an external list, when the last stage is done
                if (stageCount == stages.Count - 1)
                {
                    codes = tempBFSCode;
                    break;
                }
                //looks for the lowest codes and adds the associated numerations to a queue
                else
                {
                    //adds the numeration to the queue if there is only one numeration in the stage
                    if (currentStageNum.Count == 1)
                    {
                        numerationQueue.Enqueue(currentStageNum.First());
                    }

                    for (int i = 0; i < tempBFSCode.Count - 1; i++)
                    {
                        if (tempBFSCode.ElementAt(i).CompareTo(tempBFSCode.ElementAt(i + 1)) == 0)
                        {
                            int tag = tempBFSCode.ElementAt(i).BFSEdges.Last()[0];
                            numerationQueue.Enqueue(currentStageNum[tag]);
                            //adds last numeration to Queue if all BFSCodes have the same size
                            if (i == tempBFSCode.Count - 2)
                            {
                                tag = tempBFSCode.ElementAt(i + 1).BFSEdges.Last()[0];
                                numerationQueue.Enqueue(currentStageNum[tag]);
                            }
                        }
                        else if (tempBFSCode.ElementAt(i).CompareTo(tempBFSCode.ElementAt(i + 1)) < 0)
                        {
                            int tag = tempBFSCode.ElementAt(i).BFSEdges.Last()[0];
                            numerationQueue.Enqueue(currentStageNum[tag]);
                            break;
                        }
                    }
                }
            }
            BFSCode BFSCode = codes[0];
            BFSCode.BFSEdges.Remove(BFSCode.BFSEdges.Last());

            return BFSCode;
        }

        /// <summary>
        /// Puts permutations of different lists together, to form all valid combinations.
        /// </summary>
        /// <param name="permutations">List of permutationlists</param>
        /// <param name="listCounter">A counter, which tells on which list you next work on</param>
        /// <param name="combinationList">the list in which all valid combinations are saved</param>
        /// <param name="previousPermutations">A combination of permutations from previous lists</param>
        private void combineList(List<List<int[]>> permutations, int listCounter, List<int[]> combinationList, int[] previousPermutations)
        {
            //adds permutation to the permutationsList if the last list is reached
            if (listCounter == permutations.Count - 1)
            {
                foreach (int[] permutation in permutations[listCounter])
                {
                    int[] current = previousPermutations.Concat(permutation).ToArray();
                    combinationList.Add(current);
                }
            }
            //creates all the valid permutations for the current list and moves the listCounter n
            else
            {
                foreach (int[] permutation in permutations[listCounter])
                {
                    int[] currentPer = previousPermutations.Concat(permutation).ToArray();
                    combineList(permutations, listCounter + 1, combinationList, currentPer);
                }
            }
        }

        /// <summary>
        /// Creates the BFSCode Edgelist of a specific numeration
        /// </summary>
        /// <param name="graph">The current graph</param>
        /// <param name="numeration">The numeration of which the BFSCode is supposed to be created</param>
        /// <returns>Returns a list with the ordered edges of the BFSCode</returns>
        private List<int[]> createBFSCode(Graph graph, int[] numeration)
        {
            setBFSID(graph, numeration);
            List<int[]> EdgeList = new List<int[]>();

            foreach (Edge edge in graph.Edges)
            {
                EdgeList.Add(createBFSEdge(edge));
            }

            List<int[]> BFSCode = EdgeList.OrderBy(x => x[2]).ThenBy(x => x[1]).ToList();

            //create forward edges
            BFSCode[0][0] = 1;
            for (int i = 1; i < BFSCode.Count; i++)
            {
                if (BFSCode[i][2] != BFSCode[i - 1][2])
                {
                    BFSCode[i][0] = 1;
                }
            }

            resetBFSID(graph.Vertices);

            return BFSCode;
        }

        /// <summary>
        /// Creates backward BFSEdges in an array
        /// </summary>
        /// <param name="edge">Edge to be transformed into a BFSEdge</param>
        /// <returns>Returns the created BFSEdge</returns>
        private int[] createBFSEdge(Edge edge)
        {
            int[] BFSEdge = new int[3];

            BFSEdge[0] = -1;

            if (edge.Vertex1.BFSID < edge.Vertex2.BFSID)
            {
                BFSEdge[1] = edge.Vertex1.BFSID;
                BFSEdge[2] = edge.Vertex2.BFSID;
            }
            else
            {
                BFSEdge[1] = edge.Vertex2.BFSID;
                BFSEdge[2] = edge.Vertex1.BFSID;
            }

            return BFSEdge;
        }

        /// <summary>
        /// Creates the profile string of the graph
        /// </summary>
        /// <param name="BFSCodes">The local BFSCodes of which the profile is created</param>
        /// <returns>Returns the profile string</returns>
        private string createProfile(List<BFSCode> BFSCodes)
        {
            string profile = "";
            foreach (BFSCode code in BFSCodes)
            {
                profile = profile + code.BFSEdges[0][0].ToString() + "," + code.BFSEdges[0][1].ToString() + "," + code.BFSEdges[0][2].ToString();
                for (int i = 1; i < code.BFSEdges.Count; i++)
                {
                    //adds an edge to the string
                    //Decides whether a new fragment starts and adds a ? if that is the case.
                    if (code.BFSEdges[i][2] == code.BFSEdges[i - 1][2])
                    {
                        profile = profile + ";" + code.BFSEdges[i][0].ToString() + "," + code.BFSEdges[i][1].ToString() + "," + code.BFSEdges[i][2].ToString();
                    }
                    else
                    {
                        profile = profile + "?" + code.BFSEdges[i][0].ToString() + "," + code.BFSEdges[i][1].ToString() + "," + code.BFSEdges[i][2].ToString();
                    }
                }
                profile = profile + ",0?";
            }
            profile = profile.Remove(profile.Length - 1);
            return profile;
        }

        /// <summary>
        /// Creates vertexsets in a stage, in which a set is a set of vertices with the same parentvertex in the previous stage.
        /// </summary>
        /// <param name="stage">The current stage, of which sets are built</param>
        /// <param name="graph">The current graph</param>
        /// <param name="previousStage">The stage before the current stage</param>
        /// <returns>Returns a list with the sets in.</returns>
        private List<List<Vertex>> createSets(List<Vertex> stage, Graph graph, List<Vertex> previousStage)
        {
            List<List<Vertex>> sets = new List<List<Vertex>>();
            List<Vertex> tempStage = new List<Vertex>(stage);

            previousStage = previousStage.OrderBy(x => x.BFSID).ToList();

            foreach (Vertex vertex in previousStage)
            {
                if (tempStage.Count != 0)
                {
                    List<Vertex> newSet = new List<Vertex>();
                    List<Edge> neighbours = graph.FindEdges(vertex);
                    foreach (Edge edge in neighbours)
                    {
                        //adds vertices to the set if they are in the current stage
                        if (edge.Vertex1.ID == vertex.ID && tempStage.Contains(edge.Vertex2))
                        {
                            newSet.Add(edge.Vertex2);
                            tempStage.Remove(edge.Vertex2);
                        }
                        else if (edge.Vertex2.ID == vertex.ID && tempStage.Contains(edge.Vertex1))
                        {
                            newSet.Add(edge.Vertex1);
                            tempStage.Remove(edge.Vertex1);
                        }
                    }
                    if (newSet.Count != 0)
                    {
                        sets.Add(newSet);
                    }
                }
            }
            return sets;
        }

        /// <summary>
        /// Creates the stages for the breadthfirstsearch
        /// </summary>
        /// <param name="start">The vertex from which the breadthfirstsearch is started</param>
        /// <param name="graph">The current graph</param>
        /// <returns>Returns a list with all the stages in it </returns>
        private List<List<Vertex>> createStages(Vertex start, Graph graph)
        {
            //sets the visited property of all vertices to false
            foreach (Vertex vertex in graph.Vertices)
            {
                vertex.Visited = false;
            }

            Queue<Vertex> vertexQueue = new Queue<Vertex>();
            List<List<Vertex>> stages = new List<List<Vertex>>();

            //creates the 0 stage, in which the start vertex is
            vertexQueue.Enqueue(start);
            vertexQueue.Enqueue(null);
            start.Visited = true;
            List<Vertex> firstStage = new List<Vertex>();
            firstStage.Add(start);
            stages.Add(firstStage);

            while (vertexQueue.Count != 1)
            {
                List<Vertex> currentStage = new List<Vertex>();
                Vertex current;

                int currentQueueLength = vertexQueue.Count();

                //creates the next stage by taking all the vertices of the previous stage,
                //getting their unvisited neighbours and adding them to the current stage
                //also adds the next stage to the queue
                for (int i = 0; i < currentQueueLength; i++)
                {
                    current = vertexQueue.Dequeue();

                    if (current != null)
                    {
                        List<Edge> edgeList = graph.FindEdges(current);

                        foreach (Edge edge in edgeList)
                        {
                            if (edge.Vertex1.ID == current.ID && edge.Vertex2.Visited == false)
                            {
                                currentStage.Add(edge.Vertex2);
                                edge.Vertex2.Visited = true;
                                vertexQueue.Enqueue(edge.Vertex2);
                            }
                            else if (edge.Vertex2.ID == current.ID && edge.Vertex1.Visited == false)
                            {
                                currentStage.Add(edge.Vertex1);
                                edge.Vertex1.Visited = true;
                                vertexQueue.Enqueue(edge.Vertex1);
                            }
                        }
                    }
                }
                if (currentStage.Count != 0)
                {
                    stages.Add(currentStage);
                }
                vertexQueue.Enqueue(null);
            }

            return stages;
        }

        /// <summary>
        /// Creates all valid permutations of a stage
        /// </summary>
        /// <param name="sets">All the sets in a stage</param>
        /// <returns>Returns a list with all valid permutations</returns>
        private List<int[]> permute(List<List<Vertex>> sets)
        {
            List<int[]> permutations = new List<int[]>();
            List<int[]> setArrays = new List<int[]>();
            List<List<int[]>> setPermutations = new List<List<int[]>>();

            //create the arrays of the sets
            foreach (List<Vertex> set in sets)
            {
                int[] temp = new int[set.Count];
                for (int i = 0; i < set.Count; i++)
                {
                    temp[i] = set[i].ID;
                }
                setArrays.Add(temp);
            }

            //find all permutations of a set
            foreach (int[] set in setArrays)
            {
                List<int[]> tempPermute = new List<int[]>();
                permuteSet(set, set.Length, tempPermute);
                setPermutations.Add(tempPermute);
            }

            //combine all permutations of the sets to a valid stage permutation
            if (sets.Count <= 1)
            {
                foreach (int[] permutation in setPermutations[0])
                {
                    permutations.Add(permutation);
                }
            }
            else
            {
                int[] firstPermutation = new int[0];
                combineList(setPermutations, 0, permutations, firstPermutation);
            }

            return permutations;
        }

        /// <summary>
        /// Permutes and array. This method implements the permutation algorithm by heap.
        /// </summary>
        /// <param name="set">The array to be permuted</param>
        /// <param name="n">Counter on which element you work on. Starts at the lenght of the array</param>
        /// <param name="permutations">The list, in which all resulting permutations are saved</param>
        private void permuteSet(int[] set, int n, List<int[]> permutations)
        {
            int[] count = new int[set.Length];
            for (int j = 0; j < count.Length; j++)
            {
                count[j] = 0;
            }

            int[] first = new int[set.Length];
            System.Array.Copy(set, first, set.Length);
            permutations.Add(first);

            int i = 0;

            while (i < set.Length)
            {
                if (count[i] < i)
                {
                    if (i % 2 == 0)
                    {
                        swap(set, 0, i);
                    }
                    else
                    {
                        swap(set, count[i], i);
                    }
                    int[] temp = new int[set.Length];
                    System.Array.Copy(set, temp, set.Length);
                    permutations.Add(temp);
                    count[i]++;
                    i = 0;
                }
                else
                {
                    count[i] = 0;
                    i++;
                }
            }
        }

        /// <summary>
        /// Sets all BFSIDs of the Vertices in the list to 0.
        /// </summary>
        /// <param name="vertices">Vertices to have their BFSID resetted</param>
        private void resetBFSID(List<Vertex> vertices)
        {
            foreach (Vertex vertex in vertices)
            {
                vertex.BFSID = 0;
            }
        }

        /// <summary>
        /// Takes a numeration and sets the BFSID accordingly
        /// </summary>
        /// <param name="graph">The current graph</param>
        /// <param name="numeration">The numeration that is to be used</param>
        private void setBFSID(Graph graph, int[] numeration)
        {
            foreach (Vertex vertex in graph.Vertices)
            {
                vertex.BFSID = numeration[vertex.ID - 1];
            }
        }

        /// <summary>
        /// Swaps two elements in an array
        /// </summary>
        /// <param name="set">The array in which elements are to be swapped</param>
        /// <param name="firstIndex">The index of the first element</param>
        /// <param name="secondIndex">The index of the second element</param>
        private void swap(int[] set, int firstIndex, int secondIndex)
        {
            int temp = set[firstIndex];
            set[firstIndex] = set[secondIndex];
            set[secondIndex] = temp;
        }

        #endregion Private Methods
    }
}