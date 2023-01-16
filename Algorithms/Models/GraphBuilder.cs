namespace Aoc2022.Algorithms.Models;

public static class GraphBuilder
{
    public static Graph<T> BuildFrom<T>(IEnumerable<string> input) where T : GraphPoint
    {
        var to = input.ToArray();

        var width = to.First().Length;
        var height = to.Length;

        var vertices = new List<T>();
        var edges = new List<Tuple<T, T>>();

        for (var y = 0; y < height; y++)
        for (var x = 0; x < width; x++)
        {
            var vertex = new GraphPoint(x, y, to[y][x]);
            vertices.Add((vertex as T)!);
            edges.AddRange(
                Utils.BuildNeighboursWithValue<GraphPoint>(x, y, width, height, to)
                    .Select(s => new Tuple<GraphPoint, GraphPoint>(vertex, s)) as IEnumerable<Tuple<T, T>>
                ?? Array.Empty<Tuple<T, T>>());
        }

        return new Graph<T>(vertices, edges);
    }
}