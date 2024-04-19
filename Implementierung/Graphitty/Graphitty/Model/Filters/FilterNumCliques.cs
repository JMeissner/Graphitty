using System;
using System.Diagnostics;
using Graphitty.Model.Graphs;

namespace Graphitty.Model.Filters
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    class FilterNumCliques : FilterNumBase
    {
        #region Public Constructors

        /// <summary>
        /// Sets the Id and the Displayname of the Filter
        /// </summary>
        public FilterNumCliques()
        {
            Id = "NumCliques";
            Displayname = "Number of accumulated Cliques with size > 2";
        }

        #endregion Public Constructors

        #region predicates

        override protected bool PredicateEqual(GraphEntity g)
        {
            int[] numCliquesOfSizeK = parseStringToArray(g.NumCliquesOfSizeK);
            int numCliques = numOfCliques(numCliquesOfSizeK);
            if (numCliques == float.Parse(splitArgs[1]))
            {
                return true;
            }
            return false;
        }

        override protected bool PredicateGreater(GraphEntity g)
        {
            int[] numCliquesOfSizeK = parseStringToArray(g.NumCliquesOfSizeK);
            int numCliques = numOfCliques(numCliquesOfSizeK);
            if (numCliques > float.Parse(splitArgs[1]))
            {
                return true;
            }
            return false;
        }

        override protected bool PredicateLess(GraphEntity g)
        {
            int[] numCliquesOfSizeK = parseStringToArray(g.NumCliquesOfSizeK);
            int numCliques = numOfCliques(numCliquesOfSizeK);
            if (numCliques < float.Parse(splitArgs[1]))
            {
                return true;
            }
            return false;
        }

        override protected bool PredicateNotEqual(GraphEntity g)
        {
            int[] numCliquesOfSizeK = parseStringToArray(g.NumCliquesOfSizeK);
            int numCliques = numOfCliques(numCliquesOfSizeK);

            if (numCliques != float.Parse(splitArgs[1]))
            {
                return true;
            }

            return false;
        }

        #endregion predicates

        #region Private Methods

        private int numOfCliques(int[] numOfCliquesOfSizeK)
        {
            int numOfCliques = 0;

            for (int i = 1; i < numOfCliquesOfSizeK.Length; i += 2)
            {
                numOfCliques = numOfCliques + numOfCliquesOfSizeK[i];
            }

            return numOfCliques;
        }

        private int[] parseStringToArray(string NumCliquesOfSizeK)
        {
            if (NumCliquesOfSizeK.Equals("# Edges"))
            {
                int[] zero = new int[1] { 0 };
                return zero;
            }
            string[] separators = new string[] { ",", ":", "\'", " " };
            string[] temp = NumCliquesOfSizeK.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            int[] ret = new int[temp.Length];
            Debug.WriteLine(NumCliquesOfSizeK);
            for (int i = 0; i < ret.Length; i++)
            {
                Debug.WriteLine(ret.ToString());
                ret[i] = Int32.Parse(temp[i]);
            }

            return ret;
        }

        #endregion Private Methods
    }
}