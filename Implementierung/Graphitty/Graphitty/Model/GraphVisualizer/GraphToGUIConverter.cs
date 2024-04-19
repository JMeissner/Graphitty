using Graphitty.Model.Graphs;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Graphitty.Model.GraphVisualizer
{
    public class GraphToGUIConverter
    {
        #region Public Constructors

        //Constructor
        public GraphToGUIConverter() { }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// Converts a guigraph to a graph. Only returns a graph, if the graph is connected
        /// </summary>
        /// <param name="GraphToConvert">guigraph to convert</param>
        /// <returns>graph if connected, null otherwise</returns>
        public Graph Convert(GUIGraph GraphToConvert)
        {
            //allocate Temp-Lists
            List<Vertex> _vList = new List<Vertex>();
            List<Edge> _eList = new List<Edge>();

            //Add Edges and Degrees
            foreach (GUIEdge e in GraphToConvert.Edges)
            {
                Vertex _s = new Vertex((int)e.Source.ID);
                Vertex _t = new Vertex((int)e.Target.ID);
                //vertex _s
                if (!_vList.Contains(_s))
                {
                    _vList.Add(_s);
                }

                //vertex _t
                if (!_vList.Contains(_t))
                {
                    _vList.Add(_t);
                }
                Edge edge = new Edge(_vList.ElementAt(_vList.IndexOf(_s)), _vList.ElementAt(_vList.IndexOf(_t)));
                edge.IncrementDegrees();
                _eList.Add(edge);
            }
            Graph _g = new Graph(_vList, _eList);
            for (int i = 1; i < _g.Vertices.Count + 1; i++)
            {
                _g.Vertices.ElementAt(i - 1).ID = i;
            }
            if (IsConnected(_g)) { return _g; }
            return null;
        }

        /// <summary>
        /// Converts a graph to a guigraph
        /// </summary>
        /// <param name="GraphToConvert">graph to convert</param>
        /// <returns>guigraph</returns>
        public GUIGraph Convert(Graph GraphToConvert)
        {
            GUIGraph convertedGraph = new GUIGraph();
            //Converts the vertices of the graph to the GUIGraph format
            foreach (Vertex v in GraphToConvert.Vertices)
            {
                GUIVertex v2 = new GUIVertex(v.VColor, (long)v.ID);
                convertedGraph.AddVertex(v2);
            }

            GUIVertex[] verts = convertedGraph.Vertices.ToArray();
            //Converts the edges of the graph to the GUIGraph format
            foreach (Edge e in GraphToConvert.Edges)
            {
                GUIVertex v1 = null;
                GUIVertex v2 = null;
                for (int i = 0; i < verts.Length; i++)
                {
                    if ((int)verts[i].ID == e.Vertex1.ID && v1 == null) { v1 = verts[i]; }
                    if ((int)verts[i].ID == e.Vertex2.ID && v2 == null) { v2 = verts[i]; }
                }
                GUIEdge e2 = new GUIEdge(v1, v2, e.EColor);
                convertedGraph.AddEdge(e2);
            }
            return convertedGraph;
        }

        #endregion Public Methods

        #region Connectivity

        private Graph gDFS;

        //DFS Algorithm for ConnectedComponents
        private void Dfs(Vertex v)
        {
            if (v.Visited) { return; } //return if vertex is visited
            v.Visited = true; //set vertex to visited
            //get all neighbors
            List<Edge> _neighbors = gDFS.FindEdges(v);
            foreach (Edge e in _neighbors)
            {
                if (!e.Vertex1.Visited) { Dfs(e.Vertex1); }
                if (!e.Vertex2.Visited) { Dfs(e.Vertex2); }
            }
        }

        //Runs DFS to determine if a graph is connected
        private bool IsConnected(Graph graph)
        {
            //Setup and run DFS Search
            gDFS = graph;
            if (!graph.Vertices.Any()) { return false; }
            Vertex vertex = gDFS.Vertices.First();
            Dfs(vertex);
            //Check if graph is connected
            foreach (Vertex v in gDFS.Vertices)
            {
                if (!v.Visited)
                {
                    //TODO: Logeintrag?
                    Debug.WriteLine("Graph not connected! Not connected at Vertex:" + v.ID);
                    return false;
                }
            }
            return true;
        }

        #endregion Connectivity
    }
}