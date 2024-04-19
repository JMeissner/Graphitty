using System;
using System.Windows.Media;

namespace Graphitty.Model.Graphs
{
    public class Vertex : IEquatable<Vertex>
    {
        #region Private Fields

        private int bfsID;
        private int color;
        private int degree;
        private int id;
        private Brush vColor;
        private bool visited;

        #endregion Private Fields

        #region Public Properties

        public int BFSID { get => bfsID; set => bfsID = value; }
        public int Color { get => color; set => color = value; }
        public int Degree { get => degree; set => degree = value; }
        public int ID { get => id; set => id = value; }
        public Brush VColor { get => vColor; set => vColor = value; }
        public bool Visited { get => visited; set => visited = value; }

        #endregion Public Properties

        #region Public Constructors

        public Vertex(int id)
        {
            ID = id;
            VColor = Brushes.Gray;
            Degree = 0;
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// Compares two vertices.
        /// </summary>
        /// <param name="vertex">The vertex to compare with.</param>
        /// <returns>Returns true if the vertices are equal, else false.</returns>
        public bool Equals(Vertex other)
        {
            return ID == other.ID;
        }

        #endregion Public Methods
    }
}