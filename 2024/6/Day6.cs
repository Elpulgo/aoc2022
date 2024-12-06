using System.Collections.Concurrent;

namespace Aoc2022.Aoc2024;

internal class Day6 : BaseDay
{
    public Day6(bool shouldPrint) : base(2024, nameof(Day6), shouldPrint)
    {
    }

    public override void Execute()
    {
        PartOne();
        PartTwo();
    }

    private void PartOne()
    {
        var input = ReadInput(true).ToList();
        var (currentCoord, map) = BuildMap(input);
        var visited = GetGuardVisitCoords(currentCoord, map);
        FirstSolution(visited.Count);
    }

    private void PartTwo()
    {
        var input = ReadInput(false, partTwo: true).ToList();
        var (currentCoord, map) = BuildMap(input);
        var visited = GetGuardVisitCoords(currentCoord, map)
            .Where(w => w != currentCoord)
            .ToList();

        var starting = new Coord(currentCoord.X, currentCoord.Y);
        var sum = 0;

        Parallel.ForEach(
            visited,
            new ParallelOptions {MaxDegreeOfParallelism = Environment.ProcessorCount},
            coord =>
            {
                var currentCoordLocal = starting;
                var clonedMap = new Dictionary<Coord, bool>(map)
                {
                    [coord] = true
                };

                var vis = new ConcurrentDictionary<Coord, int>();

                var exitedMap = false;
                var looped = false;
                var direction = Direction.Up;

                do
                {
                    if (vis.TryGetValue(currentCoordLocal, out var c1) && c1 > 4)
                    {
                        looped = true;
                        break;
                    }

                    if (IsOutOfBounds(currentCoordLocal, clonedMap))
                    {
                        exitedMap = true;
                        continue;
                    }

                    var nextCoord = GetCoordInNewDirection(currentCoordLocal, direction);

                    if (!vis.TryAdd(currentCoordLocal, 1))
                    {
                        vis[currentCoordLocal]++;
                    }

                    if (IsObstacle(nextCoord, clonedMap))
                    {
                        direction = GetNewDirection(direction);
                        continue;
                    }

                    currentCoordLocal = nextCoord;
                } while (!exitedMap);

                clonedMap[coord] = false;

                if (looped)
                {
                    Interlocked.Increment(ref sum);
                }
            });

        SecondSolution(sum);
    }

    private static HashSet<Coord> GetGuardVisitCoords(Coord currentCoord, Dictionary<Coord, bool> map)
    {
        var visited = new HashSet<Coord> {currentCoord};

        var exitedMap = false;
        var direction = Direction.Up;

        do
        {
            var nextCoord = GetCoordInNewDirection(currentCoord, direction);

            if (IsOutOfBounds(nextCoord, map))
            {
                exitedMap = true;
                continue;
            }

            if (IsObstacle(nextCoord, map))
            {
                direction = GetNewDirection(direction);
                continue;
            }

            currentCoord = nextCoord;
            visited.Add(nextCoord);
        } while (!exitedMap);


        return visited;
    }

    private static bool IsOutOfBounds(Coord coord, Dictionary<Coord, bool> map) => !map.ContainsKey(coord);

    private static bool IsObstacle(Coord coord, Dictionary<Coord, bool> map) => map.GetValueOrDefault(coord, false);

    private static (Coord, Dictionary<Coord, bool>) BuildMap(List<string> input)
    {
        var map = new Dictionary<Coord, bool>();
        var startingPoint = new Coord(0, 0);

        foreach (var y in input.SelectWithIndex())
        {
            foreach (var x in y.Value.SelectWithIndex())
            {
                if (x.Value == '^')
                {
                    startingPoint = new Coord(x.Index, y.Index);
                }

                map.Add(new Coord(x.Index, y.Index), x.Value == '#');
            }
        }

        return (startingPoint, map);
    }

    private record Coord(int X, int Y);

    private static Coord GetCoordInNewDirection(Coord coord, Direction direction) => direction switch
    {
        Direction.Up => coord with {Y = coord.Y - 1},
        Direction.Right => coord with {X = coord.X + 1},
        Direction.Down => coord with {Y = coord.Y + 1},
        Direction.Left => coord with {X = coord.X - 1},
        _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
    };

    private static Direction GetNewDirection(Direction current) =>
        current switch
        {
            Direction.Up => Direction.Right,
            Direction.Right => Direction.Down,
            Direction.Down => Direction.Left,
            Direction.Left => Direction.Up,
            _ => throw new ArgumentOutOfRangeException(nameof(current), current, null)
        };

    private enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
}