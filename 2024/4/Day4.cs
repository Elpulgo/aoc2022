using System.Text;

namespace Aoc2022.Aoc2024;

internal class Day4 : BaseDay
{
    private Dictionary<(int X, int Y), char> _matrix = new();


    public Day4(bool shouldPrint) : base(2024, nameof(Day4), shouldPrint)
    {
    }

    public override void Execute()
    {
        PartOne();
        PartTwo();
    }


    private void PartOne()
    {
        var lines = ReadInput(true)
            .Select(s => s.ToCharArray())
            .ToList();

        for (var y = 0; y < lines.Count; y++)
        {
            for (var x = 0; x < lines[y].Length; x++)
            {
                _matrix.Add((x, y), lines[y][x]);
            }
        }

        const string expectedWord = "XMAS";
        const string expectedWordReverse = "SAMX";

        var sum = 0;
        var keys = new HashSet<string>();

        for (var y = 0; y < lines.Count; y++)
        {
            for (var x = 0; x < lines[y].Length; x++)
            {
                foreach (var d in Enum.GetValues(typeof(Direction)).Cast<Direction>())
                {
                    var neighbours = new[] {(X: x, Y: y)}.Concat(GetNeighbours((x, y), d, 3)).ToList();
                    var word = StringContainsMatch(neighbours);

                    if (word is expectedWord or expectedWordReverse)
                    {
                        var key = string.Join(" ",
                            neighbours.OrderBy(o => o.X).ThenBy(o => o.Y).Select(s => $"{s.X}/{s.Y}"));

                        if (!keys.Contains(key))
                        {
                            sum++;
                            keys.Add(key);
                        }
                    }
                }
            }
        }

        FirstSolution(sum);
    }

    private string StringContainsMatch(IEnumerable<(int X, int Y)> coords)
    {
        var sb = new StringBuilder();

        foreach (var (x, y) in coords)
        {
            sb.Append(_matrix[(x, y)]);
        }

        return sb.ToString();
    }

    private IEnumerable<(int X, int Y)> GetNeighbours(
        (int X, int Y) coord,
        Direction direction,
        int steps)
    {
        for (int i = 1; i <= steps; i++)
        {
            switch (direction)
            {
                // Top
                case Direction.N:
                {
                    if (_matrix.ContainsKey((coord.X, coord.Y - i)))
                        yield return (coord.X, coord.Y - i);
                    break;
                }
                // Bottom
                case Direction.S:
                {
                    if (_matrix.ContainsKey((coord.X, coord.Y + i)))
                        yield return (coord.X, coord.Y + i);
                    break;
                }
                // Left
                case Direction.W:
                {
                    if (_matrix.ContainsKey((coord.X - i, coord.Y)))
                        yield return (coord.X - i, coord.Y);
                    break;
                }
                // Right
                case Direction.E:
                {
                    if (_matrix.ContainsKey((coord.X + i, coord.Y)))
                        yield return (coord.X + i, coord.Y);
                    break;
                }
                // Diagonal Left Top
                case Direction.SW:
                {
                    if (_matrix.ContainsKey((coord.X - i, coord.Y - i)))
                        yield return (coord.X - i, coord.Y - i);
                    break;
                }
                // Diagonal Right Top
                case Direction.SE:
                {
                    if (_matrix.ContainsKey((coord.X + i, coord.Y - i)))
                        yield return (coord.X + i, coord.Y - i);
                    break;
                }
                // Diagonal Left Bottom
                case Direction.NW:
                {
                    if (_matrix.ContainsKey((coord.X - i, coord.Y + i)))
                        yield return (coord.X - i, coord.Y + i);
                    break;
                }
                // Diagonal Right Bottom
                case Direction.NE:
                {
                    if (_matrix.ContainsKey((coord.X + i, coord.Y + i)))
                        yield return (coord.X + i, coord.Y + i);
                    break;
                }
            }
        }
    }

    public enum Direction
    {
        N,
        S,
        W,
        E,
        NE,
        NW,
        SE,
        SW
    }



    private void PartTwo()
    {
        var input = ReadInput(false, partTwo: true).ToList();
    }
}