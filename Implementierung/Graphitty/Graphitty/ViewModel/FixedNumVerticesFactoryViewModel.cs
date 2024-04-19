using Graphitty.Model.GraphGeneration;
using Graphitty.Model.GraphGeneration.Factories;
using Prism.Mvvm;

namespace Graphitty.ViewModel
{
    /// <summary>
    /// ViewModel for a vertex factory which uses a fixed number of vertices.
    /// Implements the IVerteXFactoryViewModel to offer basic information which is used by the GeneratorViewModel and the template.
    /// To configure the appearance of this ViewModel change its template.
    /// </summary>
    /// <see cref="ViewModel.IVertexFactoryViewModel"/>
    /// <see cref="View.FactoryViewTemplates"/>
    public class FixedNumVerticesFactoryViewModel : BindableBase, IVertexFactoryViewModel
    {
        #region Private Fields

        private FixedNumVerticesFactory fixedNumVerticesFactory;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// Initializes the ViewModel with a FixedNumVerticesFactory and sets the default number of vertices to 20.
        /// </summary>
        public FixedNumVerticesFactoryViewModel()
        {
            fixedNumVerticesFactory = new FixedNumVerticesFactory();
            NumVertices = 20;
        }

        #endregion Public Constructors

        #region Public Properties

        /// <see cref="ViewModel.IVertexFactoryViewModel.DisplayName"/>
        public string DisplayName => "Fixed number of Vertices Factory";

        /// <see cref="ViewModel.IVertexFactoryViewModel.NumVertices"/>
        public int NumVertices
        {
            get
            {
                return fixedNumVerticesFactory.NumVertices;
            }
            set
            {
                fixedNumVerticesFactory.NumVertices = value;
                RaisePropertyChanged("NumVertices");
            }
        }

        /// <see cref="ViewModel.IVertexFactoryViewModel.VertexFactory"/>
        public IVertexFactory VertexFactory { get => fixedNumVerticesFactory; }

        #endregion Public Properties
    }
}