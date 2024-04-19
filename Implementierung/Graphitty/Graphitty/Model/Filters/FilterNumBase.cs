using System;
using System.Diagnostics;
using Graphitty.Model.Graphs;

namespace Graphitty.Model.Filters
{
    class FilterNumBase : IFilter
    {
        #region Protected Fields

        protected string[] splitArgs;

        #endregion Protected Fields

        #region Public Properties

        public string Args { get; set; }
        public string Displayname { get; protected set; }
        public string Id { get; protected set; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// checks Args and selects the right predicte
        /// </summary>
        /// <returns>Returns a Function (GraphEntity -> bool) to filter the UoW</returns>
        public Func<GraphEntity, bool> Predicate()
        {
            //Check Args consistency
            Debug.WriteLine("Running Filter " + Id + ". Args:" + Args);
            if (!CheckArgs())
            {
                Debug.WriteLine("CheckArgs() failed.");
                return null;
            }
            splitArgs = Args.Split(' ');

            //initialize Predicate to null
            Func<GraphEntity, bool> pred = null;

            //return the correct function based on the Args
            switch (splitArgs[0])
            {
                case "=": pred = PredicateEqual; break;
                case "<": pred = PredicateLess; break;
                case ">": pred = PredicateGreater; break;
                case "!=": pred = PredicateNotEqual; break;
                default: Debug.WriteLine("Predicate selection failes. Op:" + splitArgs[0]); return null;
            }

            return pred;
        }

        #endregion Public Methods

        #region Protected Methods

        /// <summary>
        /// Checks whether the Args are correct and can be parsed
        /// </summary>
        /// <returns>true if Args are correct</returns>
        protected bool CheckArgs()
        {
            string[] splitArgs = Args.Split(' ');
            if (splitArgs.Length != 2)
            {
                Debug.WriteLine("Filter failed to parse: " + splitArgs[0] + "," + splitArgs[1]);
                return false;
            }
            Debug.WriteLine("Checking Ops:" + splitArgs[0]);
            if (splitArgs[0].Equals("=") || splitArgs[0].Equals("<") || splitArgs[0].Equals(">") || splitArgs[0].Equals("!=") /*|| splitArgs[0].Equals("==")*/)
            {
                try
                {
                    Debug.WriteLine("Filter trys to parse:" + splitArgs[1]);
                    float.Parse(splitArgs[1]);
                    return true;
                }
                catch
                {
                    //prevent crashes
                    //throw new ArgumentException("Your entered Number could not be parsed.(Parse Error: String -> Float)");
                    return false;
                }
            }
            return false;
        }

        #endregion Protected Methods

        #region Predicates

        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        virtual protected bool PredicateEqual(GraphEntity g)
        {
            throw new NotImplementedException();
        }

        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        virtual protected bool PredicateGreater(GraphEntity g)
        {
            throw new NotImplementedException();
        }

        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        virtual protected bool PredicateLess(GraphEntity g)
        {
            throw new NotImplementedException();
        }

        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        virtual protected bool PredicateNotEqual(GraphEntity g)
        {
            throw new NotImplementedException();
        }

        #endregion Predicates
    }
}