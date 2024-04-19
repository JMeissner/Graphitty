using Graphitty.Model.GraphGeneration;
using Graphitty.Model.GraphGeneration.Factories;
using Prism.Mvvm;

namespace Graphitty.ViewModel
{
    /// <summary>
    /// ViewModel for a vertex factory which uses a maximal number of vertices.
    /// Implements the IVerteXFactoryViewModel to offer basic information which is used by the GeneratorViewModel and the template.
    /// To configure the appearance of this ViewModel change its template.
    /// </summary>
    /// <see cref="ViewModel.IVertexFactoryViewModel"/>
    /// <see cref="View.FactoryViewTemplates"/>
    public class MaxVerticesFactoryViewModel : BindableBase, IVertexFactoryViewModel
    {
        #region Private Fields

        private MaxVerticesFactory maxVertexFactory;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// Initializes the ViewModel with a FixedNumVerticesFactory and sets the default number of vertices to 20.
        /// </summary>
        public MaxVerticesFactoryViewModel()
        {
            maxVertexFactory = new MaxVerticesFactory();
            NumVertices = 20;
        }

        #endregion Public Constructors

        #region Public Properties

        /// <see cref="ViewModel.IVertexFactoryViewModel.DisplayName"/>
        public string DisplayName => "Max Vertices Factory";

        /// <see cref="ViewModel.IVertexFactoryViewModel.NumVertices"/>
        public int NumVertices
        {
            get { return maxVertexFactory.MaxVertices; }
            set
            {
                maxVertexFactory.MaxVertices = value;
                RaisePropertyChanged("NumVertices");
            }
        }

        /// <see cref="ViewModel.IVertexFactoryViewModel.VertexFactory"/>
        public IVertexFactory VertexFactory
        {
            get
            {
                return maxVertexFactory;
            }
        }

        #endregion Public Properties
    }
}