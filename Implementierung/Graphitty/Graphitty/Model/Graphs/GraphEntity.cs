using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Graphitty.Model.Graphs
{
    [Table("Graph")]
    public class GraphEntity
    {
        #region Public Properties

        public double AverageVertexDegree { get => averageVertexDegree; set => averageVertexDegree = value; }

        [Column(TypeName = "longtext"), Required]
        public String BFSCode { get => bFSCode; set => bFSCode = value; }

        [Column(TypeName = "longtext")]
        public string BFSCodeBitvector { get => bFSCodeBitvecor; set => bFSCodeBitvecor = value; }

        [Key]
        public int Id { get => id; set => id = value; }

        public bool IsTCCFulfilled { get => isTCCFulfilled; set => isTCCFulfilled = value; }
        public int LargestCliqueSize { get => largestCliqueSize; set => largestCliqueSize = value; }
        public int MaxVertexDegree { get => maxVertexDegree; set => maxVertexDegree = value; }
        public int MinimalColouringComplexityInMiliSeconds { get => minimalColouringComplexityInSeconds; set => minimalColouringComplexityInSeconds = value; }
        public string NumCliquesOfSizeK { get => numCliquesOfSizeK; set => numCliquesOfSizeK = value; }
        public int NumDenserGraphsBFS { get => numDenserGraphsBFS; set => numDenserGraphsBFS = value; }
        public int NumDenserGraphsProfile { get => numDenserGraphsProfile; set => numDenserGraphsProfile = value; }
        public int NumEdges { get => numEdges; set => numEdges = value; }
        public int NumGraphsWithSmallerEqualChromaticNumber { get => numGraphsWithSmallerEqualChromaticNumber; set => numGraphsWithSmallerEqualChromaticNumber = value; }
        public int NumVertices { get => numVertices; set => numVertices = value; }
        public string Profile { get => profile; set => profile = value; }
        public int TotalChromaticNumber { get => totalChromaticNumber; set => totalChromaticNumber = value; }

        #endregion Public Properties

        #region Protected Fields

        protected double averageVertexDegree;
        protected string bFSCode;
        protected string bFSCodeBitvecor;
        protected int id;
        protected bool isTCCFulfilled;
        protected int largestCliqueSize;
        protected int maxVertexDegree;
        protected int minimalColouringComplexityInSeconds;
        protected string numCliquesOfSizeK;
        protected int numDenserGraphsBFS;
        protected int numDenserGraphsProfile;
        protected int numDifferentTotalColourings;
        protected int numEdges;
        protected int numGraphsWithSmallerEqualChromaticNumber;
        protected int numVertices;
        protected string profile;
        protected int totalChromaticNumber;

        #endregion Protected Fields
    }
}