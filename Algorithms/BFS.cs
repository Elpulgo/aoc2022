using Aoc2022.Algorithms.Models;

namespace Aoc2022.Algorithms;

public static class BFS
{
    public static (
        Func<T, bool> WasVisited, 
        Func<T, IEnumerable<T>> GetShortestPathOrNull) ShortestPathFunction<T>(
        Graph<T> graph,
        T start,
        Func<T, T, bool> conditionalFunc = null) where T : GraphPoint
    {
        var visited = new Dictionary<T, T>();

        var queue = new Queue<T>();
        queue.Enqueue(start);

        while (queue.Any())
        {
            var vertex = queue.Dequeue();
            foreach (var neighbour in graph.AdjacencyList[vertex])
            {
                if (visited.ContainsKey(neighbour))
                    continue;

                if (!conditionalFunc(vertex, neighbour))
                    continue;

                visited[neighbour] = vertex;
                queue.Enqueue(neighbour);
            }
        }

        Func<T, bool> wasVisited = v => visited.ContainsKey(v);

        Func<T, IEnumerable<T>> shortestPathOrNull = v =>
        {
            var path = new List<T>();
            var current = v;
            while (!current.Equals(start))
            {
                path.Add(current);

                // We can never get to our point due to conditional specified above, so it was never visited..
                if (!visited.ContainsKey(current))
                    return null;

                current = visited[current];
            }

            path.Add(start);
            path.Reverse();
            return path;
        };

        return (wasVisited, shortestPathOrNull);
    }
}