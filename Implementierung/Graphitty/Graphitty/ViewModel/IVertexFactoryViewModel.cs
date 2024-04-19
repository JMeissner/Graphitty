using Graphitty.Model.GraphGeneration;

namespace Graphitty.ViewModel
{
    /// <summary>
    ///  Interface which has to be implemented by all VertexFactoryViewModels.
    /// </summary>
    public interface IVertexFactoryViewModel
    {
        #region Public Properties

        /// <summary>
        /// Display name of the EdgeFactory.
        /// </summary>
        string DisplayName { get; }

        /// <summary>
        /// Number of vertices which should be generated.
        /// This property gets set and read by a GeneratorViewModel, which uses this number to
        /// set the maximal number of possible edges.
        /// </summary>
        /// <see cref="ViewModel.GeneratorViewModel"/>
        /// <see cref="ViewModel.IVertexFactoryViewModel"/>
        int NumVertices { get; set; }

        /// <summary>
        /// The vertex factory of the ViewModel
        /// </summary>
        IVertexFactory VertexFactory { get; }

        #endregion Public Properties
    }
}