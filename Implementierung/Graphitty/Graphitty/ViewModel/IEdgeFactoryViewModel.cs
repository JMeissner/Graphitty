using Graphitty.Model.GraphGeneration;

namespace Graphitty.ViewModel
{
    /// <summary>
    /// Interface which has to be implemented by all EdgeFactoryViewModels.
    /// </summary>
    public interface IEdgeFactoryViewModel
    {
        #region Public Properties

        /// <summary>
        /// Display name of the EdgeFactory.
        /// </summary>
        string DisplayName { get; }

        /// <summary>
        /// The edge factory of the ViewModel.
        /// </summary>
        IEdgeFactory EdgeFactory { get; }

        /// <summary>
        /// Defines the maximal possible number of edges that can be generated, it depends on a number of vertices.
        /// This property should be set by the GeneratorViewModel, which has knowledge about the
        /// currently set number of vertices in a VertexFactoryViewModel.
        /// </summary>
        /// <see cref="ViewModel.GeneratorViewModel"/>
        /// <see cref="ViewModel.IVertexFactoryViewModel"/>
        int MaxPossibleNumEdges { get; set; }

        #endregion Public Properties
    }
}