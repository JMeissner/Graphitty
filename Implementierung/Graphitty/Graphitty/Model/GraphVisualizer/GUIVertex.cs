using GraphX.PCL.Common.Models;
using System.Windows.Media;

namespace Graphitty.Model.GraphVisualizer
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class GUIVertex : VertexBase
    {
        #region Public Constructors

        public GUIVertex()
        {
            Color = Brushes.Gray;
            ToolTip = "Vertex: " + ID.ToString();
        }

        public GUIVertex(Brush pColor)
        {
            Color = pColor;
            ToolTip = "Vertex: " + ID.ToString();
        }

        public GUIVertex(Brush pColor, long pID)
        {
            Color = pColor;
            ID = pID;
            ToolTip = "Vertex: " + ID.ToString();
        }

        #endregion Public Constructors

        #region Public Properties

        public Brush Color { get; set; }
        public string ToolTip { get; set; }

        #endregion Public Properties
    }
}