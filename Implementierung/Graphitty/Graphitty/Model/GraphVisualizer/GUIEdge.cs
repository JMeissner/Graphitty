using GraphX.PCL.Common.Models;
using System.Windows.Media;

namespace Graphitty.Model.GraphVisualizer
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class GUIEdge : EdgeBase<GUIVertex>
    {
        #region Public Constructors

        public GUIEdge(GUIVertex source, GUIVertex target, double weight = 1)
            : base(source, target, weight)
        {
            Color = Brushes.Gray;
            ToolTip = source.ID + " <-> " + target.ID;
        }

        public GUIEdge(GUIVertex source, GUIVertex target, Brush pColor, double weight = 1)
            : base(source, target, weight)
        {
            Color = pColor;
            ToolTip = source.ID + " <-> " + target.ID;
        }

        public GUIEdge()
            : base(null, null, 1)
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public Brush Color { get; set; }
        public string ToolTip { get; set; }

        #endregion Public Properties
    }
}