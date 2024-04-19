using System.Collections.Generic;
using System.Linq;
using Graphitty.Model.Graphs;

namespace Graphitty.Model.Algorithms
{
    public class CliqueSearch : Algorithm
    {
        #region Public Constructors

        /// <summary>
        /// Creates a CliqueSearch object. Intentionally left empty.
        /// </summary>
        public CliqueSearch() { }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// Runs a graph through the private methods
        /// and saves the maximum clique size and the number of cliques of a certain size into a graph.
        /// This is a template method.
        /// </summary>
        /// <param name="graph">The current graph</param>
        public override void Run(Graph graph)
        {
            graph.LargestCliqueSize = findMaxCliqueSize(graph.BFSCodeBitvector);
            int[] cliquesOfSizeK = new int[graph.LargestCliqueSize - 2];

            for (int k = 3; k <= graph.LargestCliqueSize; k++)
            {
                cliquesOfSizeK[k - 3] = cliqueSearch(k, graph);
            }

            string numCliques = "";
            for (int i = 0; i < cliquesOfSizeK.Length; i++)
            {
                int size = i + 3;
                numCliques = numCliques + "\'" + size.ToString() + "\': " + cliquesOfSizeK[i].ToString();
                if (i != cliquesOfSizeK.Length - 1)
                {
                    numCliques = numCliques + ", ";
                }
            }
            if (graph.LargestCliqueSize == 2)
            {
                numCliques = "# Edges";
            }
            graph.NumCliquesOfSizeK = numCliques;
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Starts the search for cliques of a certain size.
        /// </summary>
        /// <param name="size">The size of the cliques, which are supposed to be found.</param>
        /// <param name="graph">The current graph</param>
        /// <returns></returns>
        private int cliqueSearch(int size, Graph graph)
        {
            List<List<Vertex>> cliques = new List<List<Vertex>>();
            foreach (Vertex vertex in graph.Vertices)
            {
                List<Vertex> start = new List<Vertex>();
                start.Add(vertex);
                findClique(size, 1, start, cliques, graph);
            }

            //since this algorithm finds all permutations of all cliques,
            //you need to divide the number of all found cliques by the number of possible permutations,
            //which is the faculty of the size
            int numberOfCliques = cliques.Count / faculty(size);
            return numberOfCliques;
        }

        /// <summary>
        /// Calculates the faculty of a number.
        /// </summary>
        /// <param name="n">The number to run through the calculation</param>
        /// <returns>Returns the faculty</returns>
        private int faculty(int n)
        {
            int fac = n;
            for (int i = 1; i < n; i++)
            {
                fac = fac * i;
            }
            return fac;
        }

        /// <summary>
        /// Looks for cliques of a certain size in a graph.
        /// </summary>
        /// <param name="size">The size of the clieques supposed to be found</param>
        /// <param name="iteration">Counter of the depth of the recursion</param>
        /// <param name="previous">List of vertices visited before this recursion</param>
        /// <param name="cliques">A result list in which all cliques are saved</param>
        /// <param name="graph">The current graph</param>
        private void findClique(int size, int iteration, List<Vertex> previous, List<List<Vertex>> cliques, Graph graph)
        {
            List<Vertex> connected = findConnectedVertices(previous, graph);
            iteration++;
            //stops the recursion if wished for clique size (recursion depth) is reached
            //and adds all resulting cliques to the cliques list
            if (iteration == size)
            {
                foreach (Vertex vertex in connected)
                {
                    List<Vertex> current = new List<Vertex>(previous);
                    current.Add(vertex);
                    current = current.OrderBy(x => x.ID).ToList();
                    cliques.Add(current);
                }
            }
            //continues the recursion
            else
            {
                foreach (Vertex vertex in connected)
                {
                    List<Vertex> current = new List<Vertex>(previous);
                    current.Add(vertex);
                    findClique(size, iteration, current, cliques, graph);
                }
            }
        }

        /// <summary>
        /// Finds all vertices connected to a clique.
        /// </summary>
        /// <param name="clique">The current clique</param>
        /// <param name="graph">The current graph</param>
        /// <returns>Returns a list of all connected vertices.</returns>
        private List<Vertex> findConnectedVertices(List<Vertex> clique, Graph graph)
        {
            List<Edge> edges = graph.FindEdges(clique[0]);
            List<Vertex> temp = new List<Vertex>();

            //adds all vertices connected to the first vertex of the clique,
            //which aren't in the clique yet, to the connected list
            foreach (Edge edge in edges)
            {
                if (edge.Vertex1 == clique[0] && clique.Contains(edge.Vertex2) == false)
                {
                    temp.Add(edge.Vertex2);
                }
                else if (edge.Vertex2 == clique[0] && clique.Contains(edge.Vertex1) == false)
                {
                    temp.Add(edge.Vertex1);
                }
            }

            List<Vertex> connected = new List<Vertex>(temp);
            //checks whether all vertices in temp are connected to the clique
            //removes vertices for which that is not true
            foreach (Vertex vertex in temp)
            {
                for (int i = 1; i < clique.Count; i++)
                {
                    Edge edge = new Edge(vertex, clique[i]);
                    if (graph.ContainsEdge(edge) == false)
                    {
                        connected.Remove(vertex);
                        break;
                    }
                }
            }
            return connected;
        }

        /// <summary>
        /// Finds the maximum clique size of a graph, by looking which fragments of the bit vector of a graph are full.
        /// </summary>
        /// <param name="bitVector">The bit vector of the current graph.</param>
        /// <returns></returns>
        private int findMaxCliqueSize(string bitVector)
        {
            //shows the index the current fragment ends on
            int fragmentEnd = 0;
            int size = 1;
            //shows the size of the current fragment
            int fragSize = 1;
            for (int i = 0; i < bitVector.Length; i++)
            {
                if (bitVector.ElementAt(i).Equals('1'))
                {
                    if (i == fragmentEnd)
                    {
                        fragSize++;
                        fragmentEnd = fragmentEnd + fragSize;
                        size++;
                    }
                }
                else
                {
                    break;
                }
            }
            return size;
        }

        #endregion Private Methods
    }
}