using Aoc2022.Algorithms.Models;

public class Graph<T> where T : GraphPoint
{
    public Graph()
    {
    }

    public Graph(
        IEnumerable<T> vertices,
        IEnumerable<Tuple<T, T>> edges)
    {
        foreach (var vertex in vertices)
            AddVertex(vertex);

        foreach (var edge in edges)
            AddEdge(edge);
    }

    public Dictionary<T, HashSet<T>> AdjacencyList { get; } = new();

    private void AddEdge(Tuple<T, T> edge)
    {
        if (AdjacencyList.ContainsKey(edge.Item1) && AdjacencyList.ContainsKey(edge.Item2))
        {
            AdjacencyList[edge.Item1].Add(edge.Item2);
            AdjacencyList[edge.Item2].Add(edge.Item1);
        }
    }

    private void AddVertex(T vertex)
    {
        AdjacencyList[vertex] = new HashSet<T>();
    }
}