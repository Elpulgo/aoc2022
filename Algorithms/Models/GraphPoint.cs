namespace Aoc2022.Algorithms.Models;

public record GraphPoint(int X, int Y, char value = default);

internal class NumericCellContainer
{
    public string Key => string.Join("-", Cells.Select(s => s.Y).Concat(Cells.Select(s => s.X)));
    public HashSet<GraphPoint> Cells { get; set; } = new();

    public int NumericValue => int.Parse(string.Join("", Cells.Select(s => s.value.CharToInt())));
}

internal static class GraphPointExtensions
{
    private static readonly Func<List<NumericCellContainer>, int, int, NumericCellContainer?> FindNeighboursFunc
        = (c, x, y) => c.SingleOrDefault(a => a.Cells.Any(a => a.X == x && a.Y == y));

    internal static List<NumericCellContainer> GetAllTouchingNumericContainers(
        this GraphPoint source,
        List<NumericCellContainer> c)
    {
        var containers = GetInternal(source.X, source.Y, c)
            .Where(w => w is not null)
            .Select(s => s!)
            .ToList();

        var distinctByKey =
            containers
                .DistinctBy(d => d.Key)
                .ToList();

        return distinctByKey;
    }

    private static IEnumerable<NumericCellContainer?> GetInternal(int x, int y, List<NumericCellContainer> c)
    {
        var left = FindNeighboursFunc(c, x - 1, y);
        var right = FindNeighboursFunc(c, x + 1, y);
        var top = FindNeighboursFunc(c, x, y - 1);
        var down = FindNeighboursFunc(c, x, y + 1);
        var topLeft = FindNeighboursFunc(c, x - 1, y - 1);
        var topRight = FindNeighboursFunc(c, x + 1, y - 1);
        var downLeft = FindNeighboursFunc(c, x - 1, y + 1);
        var downRight = FindNeighboursFunc(c, x + 1, y + 1);

        yield return left;
        yield return right;
        yield return top;
        yield return down;
        yield return topLeft;
        yield return topRight;
        yield return downLeft;
        yield return downRight;
    }
}