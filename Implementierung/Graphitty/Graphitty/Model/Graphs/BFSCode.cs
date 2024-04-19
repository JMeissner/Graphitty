using System;
using System.Collections.Generic;
using System.Linq;

namespace Graphitty.Model.Graphs
{
    public class BFSCode : IComparable<BFSCode>
    {
        #region Private Methods

        // Creates a BFSCode-Object with a BFSCode-String.
        private void BFSCodeCreateFromBFSCode(String bfs)
        {
            bfsEdges = new List<int[]>();
            String[] splitBFS = bfs.Split(';');

            for (int i = 0; i < splitBFS.Length; i++)
            {
                String[] splitEdge = splitBFS[i].Split(',');
                int[] edgeArray = new int[3];
                for (int j = 0; j < 3; j++)
                {
                    edgeArray[j] = int.Parse(splitEdge[j]);
                }
                bfsEdges.Add(edgeArray);
            }
        }

        // Creates a BFSCode-Object with a BitVector.
        private void BFSCodeCreateFromBitvector(String bitVector)
        {
            bfsEdges = new List<int[]>();
            int[] edgeCode;

            int fragmentCounter = 1;
            int fragmentPlace = 1;
            int direction = 1;
            int backVertex = 2;

            for (int i = 0; i < bitVector.Length; i++)
            {
                if (i == fragmentCounter)
                {
                    fragmentCounter = fragmentCounter + backVertex;
                    fragmentPlace = 1;
                    direction = 1;
                    backVertex++;
                }
                if (bitVector.Substring(i, 1).Equals("1"))
                {
                    edgeCode = new int[3];
                    edgeCode[0] = direction;
                    edgeCode[1] = fragmentPlace;
                    edgeCode[2] = backVertex;

                    bfsEdges.Add(edgeCode);

                    direction = -1;
                    fragmentPlace++;
                }
                else
                {
                    fragmentPlace++;
                }
            }
        }

        //returns if a edge from vertex1 to vertex2 exists
        private bool containsEdge(int vertex1, int vertex2)
        {
            int[] bfsEdge;
            for (int i = 0; i < bfsEdges.Count; i++)
            {
                bfsEdge = bfsEdges.ElementAt(i);
                if (bfsEdge[1] == vertex1 && bfsEdge[2] == vertex2)
                {
                    return true;
                }
            }
            return false;
        }

        //Calculates the amount of vertices.
        private int getAmountOfVertices()
        {
            return bfsEdges.ElementAt(bfsEdges.Count - 1)[2];
        }

        #endregion Private Methods

        #region Private Fields

        private List<int[]> bfsEdges;

        #endregion Private Fields

        #region Public Fields

        public List<int[]> BFSEdges { get => bfsEdges; set => bfsEdges = value; }

        #endregion Public Fields

        #region Public Constructors

        /// <summary>
        /// Creates a BFSCode-Object from a list of edges.
        /// </summary>
        /// <param name="bfsCode">The list of edges to create the object.</param>
        public BFSCode(List<int[]> bfsCode)
        {
            BFSEdges = bfsCode;
        }

        /// <summary>
        /// Creates a BFSCode-Object with a Bitvector or BFSCode-String.
        /// </summary>
        /// <param name="bfsCode">The list of edges to create the object.</param>
        public BFSCode(String bfs)
        {
            if (bfs.Contains(','))
            {
                BFSCodeCreateFromBFSCode(bfs);
            }
            else
            {
                BFSCodeCreateFromBitvector(bfs);
            }
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// Returns the BFSCode of this Object as a String.
        /// </summary>
        /// <returns>Returns BFSCode as String.</returns>
        public String BFSToString()
        {
            String bfsCode = "";

            for (int i = 0; i < bfsEdges.Count; i++)
            {
                int[] edgeCode = bfsEdges.ElementAt(i);
                for (int j = 0; j < edgeCode.Length; j++)
                {
                    bfsCode += edgeCode[j].ToString();
                    if (j < edgeCode.Length - 1)
                    {
                        bfsCode += ",";
                    }
                }
                if (i < bfsEdges.Count - 1)
                {
                    bfsCode += ";";
                }
                else
                {
                    bfsCode += ",0";
                }
            }

            return bfsCode;
        }

        /// <summary>
        /// Implemented for IComparable interface.
        /// Compares two BFSCodes.
        /// </summary>
        /// <param name="code">The BFSCode to compare with.</param>
        /// <returns>Returns 1 if given BFSCode is denser, returns 0 if they have the same density,
        /// returns -1 if current BFSCode is denser. </returns>
        public int CompareTo(BFSCode code)
        {
            int min = Math.Min(bfsEdges.Count, code.bfsEdges.Count);

            for (int i = 0; i < min; i++)
            {
                if (bfsEdges[i].Length == 3 && code.bfsEdges[i].Length == 3)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (code.bfsEdges.ElementAt(i)[j] < bfsEdges.ElementAt(i)[j])
                        {
                            return 1;
                        }
                        else if (bfsEdges.ElementAt(i)[j] < code.bfsEdges.ElementAt(i)[j])
                        {
                            return -1;
                        }
                    }
                }
            }

            //checks whether one graph is a prefix of the other
            if (bfsEdges.Count > code.bfsEdges.Count)
            {
                return -1;
            }
            else if (code.bfsEdges.Count > bfsEdges.Count)
            {
                return 1;
            }
            return 0;
        }

        /// <summary>
        /// Returns the BFSCode as a BitVector.
        /// </summary>
        /// <returns>Returns BFSCode as BitVector.</returns>
        public string GetBitVector()
        {
            String bitVector = "";
            String[][] bitVectorArray = new String[getAmountOfVertices()][];
            String[] fragmentBitVector;
            for (int i = getAmountOfVertices(); i > 0; i--)
            {
                fragmentBitVector = new String[i];
                for (int j = 1; j <= i; j++)
                {
                    if (!(i == j))
                    {
                        if (containsEdge(j, i))
                        {
                            fragmentBitVector[j - 1] = "1";
                        }
                        else
                        {
                            fragmentBitVector[j - 1] = "0";
                        }
                    }
                }
                bitVectorArray[i - 1] = fragmentBitVector;
            }
            for (int k = 0; k < bitVectorArray.GetLength(0); k++)
            {
                for (int l = 0; l < bitVectorArray[k].Length; l++)
                {
                    bitVector += bitVectorArray[k][l];
                }
            }
            return bitVector;
        }

        #endregion Public Methods
    }
}