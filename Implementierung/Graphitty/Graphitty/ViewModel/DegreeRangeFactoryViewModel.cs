using Graphitty.Model.GraphGeneration;
using Graphitty.Model.GraphGeneration.Factories;
using Prism.Mvvm;

namespace Graphitty.ViewModel
{
    /// <summary>
    /// ViewModel for a edge factory which uses a range of edge count.
    /// Implements the IEdgeFactoryViewModel to offer basic information which is used by the GeneratorViewModel and the template.
    /// To configure the appearance of this ViewModel change its template.
    /// </summary>
    /// <see cref="ViewModel.IEdgeFactoryViewModel"/>
    /// <see cref="View.FactoryViewTemplates"/>
    public class DegreeRangeFactoryViewModel : BindableBase, IEdgeFactoryViewModel
    {
        #region Private Fields

        private DegreeRangeFactory degreeRangeFactory;
        private int maxPossibleNumEdges;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// Initializes the factory with a DegreeRangeFactory.
        /// </summary>
        public DegreeRangeFactoryViewModel()
        {
            degreeRangeFactory = new DegreeRangeFactory();
        }

        #endregion Public Constructors

        #region Public Properties

        /// <see cref="ViewModel.IEdgeFactoryViewModel.DisplayName"/>
        public string DisplayName => "Degree Range Factory";

        /// <see cref="ViewModel.IEdgeFactoryViewModel.EdgeFactory"/>
        public IEdgeFactory EdgeFactory
        {
            get
            {
                return degreeRangeFactory;
            }
        }

        public int MaxDegree
        {
            get { return degreeRangeFactory.MaxDegree; }
            set
            {
                if (value == 1 && MinDegree == 1)
                {
                    value++;
                }
                degreeRangeFactory.MaxDegree = value;
                RaisePropertyChanged("MaxDegree");
            }
        }

        /// <see cref="ViewModel.IEdgeFactoryViewModel.MaxPossibleNumEdges"/>
        public int MaxPossibleNumEdges
        {
            get
            {
                return maxPossibleNumEdges;
            }
            set
            {
                SetProperty(ref maxPossibleNumEdges, value);
                RaisePropertyChanged("MaxPossibleNumEdges");
            }
        }

        public int MinDegree
        {
            get { return degreeRangeFactory.MinDegree; }
            set
            {
                degreeRangeFactory.MinDegree = value;
                RaisePropertyChanged("MinDegree");
            }
        }

        #endregion Public Properties
    }
}