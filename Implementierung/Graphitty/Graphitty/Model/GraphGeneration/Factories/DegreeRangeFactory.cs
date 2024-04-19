using System;
using System.Collections.Generic;
using System.Linq;
using Graphitty.Model.Graphs;

namespace Graphitty.Model.GraphGeneration.Factories
{
    public class DegreeRangeFactory : IEdgeFactory
    {
        #region Private Fields

        private static Random random = new Random();

        #endregion Private Fields

        #region Public Constructors

        // TODO: Exception, wenn max < min
        public DegreeRangeFactory()
        {
            MinDegree = 1;
            MaxDegree = 2;
        }

        #endregion Public Constructors

        #region Public Properties

        public int MaxDegree { get; set; }
        public int MinDegree { get; set; }

        #endregion Public Properties

        #region Public Methods

        // TODO: Exception im viewmodel, falls (vertices.Count*(vertices.Count - 1))/2 = MaxPossibleEdges < MaxDegree
        ///<summary>
        /// Edges are created by connecting the given vertices while respecting the set range.
        /// </summary>
        public Graph AddEdges(ICollection<Vertex> vertices)
        {
            Graph newGraph = new Graph
            {
                Edges = new List<Edge>()
            };

            //These lists represent the 'state' a vertex can be in.
            //A vertex changes it's state, when it's degree reaches a threshold (MinDegree or MaxDegree)
            List<Vertex> _minNotReached = new List<Vertex>();
            List<Vertex> _inRange = new List<Vertex>();
            List<Vertex> _maxReached = new List<Vertex>();

            #region creating a connected graphList

            List<Vertex> _connectedVertices = new List<Vertex>();
            foreach (Vertex source in vertices)
            {
                if (_connectedVertices.Count > 0)
                {
                    int _targetIndex = random.Next(0, _connectedVertices.Count);
                    Vertex target = _connectedVertices[_targetIndex];
                    Edge newEdge = new Edge(source, target);
                    newEdge.IncrementDegrees();
                    newGraph.Edges.Add(newEdge);
                    if (target.Degree == MaxDegree)
                    {
                        _connectedVertices.Remove(target);
                        _maxReached.Add(target);
                    }
                }
                _connectedVertices.Add(source);
            }

            #endregion creating a connected graphList

            foreach (Vertex vertex in _connectedVertices)
            {
                if (vertex.Degree < MinDegree)
                {
                    _minNotReached.Add(vertex);
                }
                else if (vertex.Degree == MaxDegree)
                {
                    _maxReached.Add(vertex);
                }
                else if (vertex.Degree > MaxDegree)
                {
                    return null;
                }
                else
                {
                    _inRange.Add(vertex);
                }
            }

            #region minimum edge creation

            while (_minNotReached.Count > 0)
            {
                if (_minNotReached.Count == 1 && _maxReached.Count == (vertices.Count - 1))
                {
                    return null; //limitations on degrees are excluding each other and a valid graph can not be generated
                }
                Vertex _source, _target;
                addNewEdge(_minNotReached, _minNotReached.Concat(_inRange).ToList(), out _source, out _target);
                List<Vertex> tempList = new List<Vertex>() { _source, _target };
                foreach (Vertex vertex in tempList)
                {
                    if (vertex.Degree >= MinDegree && !(_inRange.Contains(vertex)))
                    {
                        _minNotReached.Remove(vertex);
                        _inRange.Add(vertex);
                    }
                    if (vertex.Degree == MaxDegree)
                    {
                        _minNotReached.Remove(vertex);
                        _inRange.Remove(vertex);
                        _maxReached.Add(vertex);
                    }
                }
            }

            #endregion minimum edge creation

            #region additional edge creation

            int maxEdges = (vertices.Count * (MaxDegree)) / 2;
            List<Edge> existingEdges = newGraph.Edges;
            int extraEdges = random.Next(0, (maxEdges - existingEdges.Count));
            for (int i = 0; i < extraEdges; i++)
            {
                Vertex _source, _target;
                addNewEdge(_inRange, _inRange, out _source, out _target);
                List<Vertex> tempList = new List<Vertex>() { _source, _target };
                foreach (Vertex vertex in tempList)
                {
                    if (vertex.Degree == MaxDegree)
                    {
                        _inRange.Remove(vertex);
                        _maxReached.Add(vertex);
                    }
                }
            }

            #endregion additional edge creation

            newGraph.Vertices = vertices.ToList();
            return newGraph;

            //Helping method to create a new and unique edge between two vertices
            //which are taken from a collection of vertices.
            void addNewEdge(List<Vertex> sourceList, List<Vertex> targetList, out Vertex source, out Vertex target)
            {
                bool exists = true;
                do
                {
                    int[] indices = GetIndices(sourceList.Count, targetList.Count);
                    source = sourceList[indices[0]];
                    target = targetList[indices[1]];
                    Edge newEdge = new Edge(source, target);
                    if (!(newGraph.Edges.Contains(newEdge)))
                    {
                        newEdge.IncrementDegrees();
                        newGraph.Edges.Add(newEdge);
                        exists = false;
                    }
                } while (exists);
            }
        }

        #endregion Public Methods

        #region Private Methods

        private int[] GetIndices(int sourceUpperBound, int targetUpperBound)
        {
            int _index1 = random.Next(0, sourceUpperBound);
            int _index2 = random.Next(0, targetUpperBound);
            while (_index1 == _index2)
            {
                _index2 = random.Next(0, targetUpperBound);
            }
            return new int[2] { _index1, _index2 };
        }

        #endregion Private Methods
    }
}