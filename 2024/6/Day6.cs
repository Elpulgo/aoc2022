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

        var visited = new HashSet<Coord>();

        var (currentCoord, map) = BuildMap(input);

        visited.Add(currentCoord);

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

        FirstSolution(visited.Count);
    }


    private void PartTwo()
    {
        var input = ReadInput(false, partTwo: true).ToList();
        var (currentCoord, map) = BuildMap(input);

        var starting = new Coord(currentCoord.X, currentCoord.Y);
        var sum = 0;

        var v = map.Where(m => !m.Value && m.Key != currentCoord);
        var countOuter = 0;
        var okayLoop = new List<Coord>();
        var notOkayLoop = new List<Coord>();


        foreach (var m in v)
        {
            countOuter++;
            currentCoord = starting;
            map[m.Key] = true;
            var visited = new HashSet<Coord>();

            var obstacleHitWithSameDirection = new HashSet<Direction>();

            visited.Add(starting);

            var vis = new Dictionary<Coord, int>();

            var exitedMap = false;
            var looped = false;
            var direction = Direction.Up;

            var count = 0;
            do
            {

                if (vis.TryGetValue(currentCoord, out var c1) && c1 > 4)
                {
                    looped = true;
                    break;
                }
                
                count++;
                if (count > (input.Count * input[0].Length) * 10)
                {
                    looped = true;
                    notOkayLoop.Add(m.Key);
                    break;
                }

                if (IsOutOfBounds(currentCoord, map))
                {
                    exitedMap = true;
                    continue;
                }

                var nextCoord = GetCoordInNewDirection(currentCoord, direction);

                if (vis.TryGetValue(currentCoord, out var c))
                {
                    vis[currentCoord]++;
                }
                else
                {
                    vis[currentCoord] = 1;
                }

                var isObstacle = IsObstacle(nextCoord, map);

                if (isObstacle)
                {
                    if (nextCoord == m.Key)
                    {
                        if (obstacleHitWithSameDirection.Contains(direction))
                        {
                            looped = true;
                            okayLoop.Add(m.Key);
                            break;
                        }
                        else
                        {
                            obstacleHitWithSameDirection.Add(direction);
                        }
                    }
                    
                    direction = GetNewDirection(direction);
                    continue;
                }

                currentCoord = nextCoord;
            } while (!exitedMap);

            /* x:3 y: 6
             * x:6 y: 7
             * x:7 y: 7
             * x:1 y: 8
             * x:3 y: 8
             * x:7 y: 9 ???
             */


            map[m.Key] = false;
            sum += looped ? 1 : 0;
        }

        SecondSolution(sum);
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
        Direction.Up => coord with { Y = coord.Y - 1 },
        Direction.Right => coord with { X = coord.X + 1 },
        Direction.Down => coord with { Y = coord.Y + 1 },
        Direction.Left => coord with { X = coord.X - 1 },
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