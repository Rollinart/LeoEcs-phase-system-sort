using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Rollin.LeoEcs.Extensions
{
    public readonly struct Graph<T>
    {
        public Dictionary<T, (int, T[])> AdjacencyList { get; }

        public Graph(int vertexAmount)
        {
            AdjacencyList = new Dictionary<T, (int, T[])>(vertexAmount);
        }

        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public void AddVertex(T vertex)
        {
            if (AdjacencyList.ContainsKey(vertex))
            {
                return;
            }

            AdjacencyList.Add(vertex, (0, new T[3]));
        }

        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public void AddEdge(T vertex, T edge)
        {
            if (!AdjacencyList.TryGetValue(vertex, out var vertexEdges))
                return;

            if (vertexEdges.Item2.Length <= vertexEdges.Item1)
                Array.Resize(ref vertexEdges.Item2, vertexEdges.Item1 + 1);

            vertexEdges.Item2[vertexEdges.Item1++] = edge;

            AdjacencyList[vertex] = vertexEdges;
        }

        public T FindVertex(Func<T, bool> compareFunc)
        {
            foreach (var keyValuePair in AdjacencyList)
            {
                if (compareFunc(keyValuePair.Key))
                    return keyValuePair.Key;
            }

            throw new Exception("Can't find the vertex");
        }
    }

    public static class GraphExtensions
    {
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static HashSet<T> DeepFirstSort<T>(this in Graph<T> graph)
        {
            var result = new HashSet<T>();

            foreach (var keyValuePair in graph.AdjacencyList)
            {
                Sort(graph, keyValuePair.Key, result);
            }

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void Sort<T>(in Graph<T> graph, T vertex, ISet<T> visited)
        {
            if (visited.Contains(vertex))
                return;

            for (var index = 0; index < graph.AdjacencyList[vertex].Item1; index++)
            {
                var neighborVertex = graph.AdjacencyList[vertex].Item2[index];
                Sort(graph, neighborVertex, visited);
            }

            visited.Add(vertex);
        }
    }
}