using System;
using System.IO;
using System.Linq;
using Aoc2022;

namespace Aoc2022.Aoc2022
{
    internal class Day9 : BaseDay
    {
        public Day9(bool shouldPrint) : base(2022, nameof(Day9), shouldPrint)
        {
        }

        public override void Execute()
        {
            var input = ParseInput(ReadInput());

            StartTimerOne();
            FirstSolution(Solve(input, 1).ToString());

            StartTimerTwo();
            SecondSolution(Solve(input, 9).ToString());
        }

        private static int Solve(
            IEnumerable<(Direction Direction, int Steps)> moves,
            int ropeLength)
        {
            var headCoordinates = new Coord(x: 0, y: 0, 0);

            var tails = new Dictionary<int, Coord>();
            for (int i = 1; i <= ropeLength; i++)
            {
                tails.Add(i, new Coord(x: 0, y: 0, i));
            }

            foreach (var move in moves)
            {
                for (int step = 0; step < move.Steps; step++)
                {
                    headCoordinates.Move(move.Direction, 1);

                    foreach (var (key, tail) in tails)
                    {
                        var coordToFollow = key == 1
                            ? headCoordinates
                            : tails[key - 1];

                        if (tail.IsTouching(coordToFollow))
                            continue;

                        tail.Follow(coordToFollow);
                        tail.MarkAsVisited();
                    }
                }
            }

            return tails[ropeLength].Visited.Count;
        }

        private static IEnumerable<(Direction Direction, int Steps)> ParseInput(IEnumerable<string> input) =>
            input.Select(s =>
            {
                var split = s.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                var direction = split[0] switch
                {
                    "U" => Direction.Up,
                    "D" => Direction.Down,
                    "L" => Direction.Left,
                    "R" => Direction.Right
                };

                return (direction, int.Parse(split[1]));
            }).ToList();
    }

    public enum Direction
    {
        Up,
        Down,
        Left,
        Right,
        Diagonal
    }

    public class Coord
    {
        public Coord(int x, int y, int number)
        {
            X = x;
            Y = y;
            Number = number;
        }

        public int X { get; set; }
        public int Y { get; set; }
        public int Number { get; }

        public void MarkAsVisited() => Visited.Add((X, Y));

        public HashSet<(int X, int Y)> Visited { get; } = new() { (0, 0) };

        public void Move(Direction direction, int steps)
        {
            switch (direction)
            {
                case Direction.Up:
                    MoveUp(steps);
                    break;
                case Direction.Down:
                    MoveDown(steps);
                    break;
                case Direction.Left:
                    MoveLeft(steps);
                    break;
                case Direction.Right:
                    MoveRight(steps);
                    break;
                case Direction.Diagonal:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
        }

        public void MoveLeft(int steps) => X -= steps;
        public void MoveRight(int steps) => X += steps;
        public void MoveUp(int steps) => Y += steps;
        public void MoveDown(int steps) => Y -= steps;

        public bool IsTouching(Coord other)
        {
            // Overlaps
            if (X == other.X && Y == other.Y)
                return true;

            // Vertical touching
            if (X == other.X && Math.Abs(Y - other.Y) == 1)
                return true;

            // Horizontal touching
            if (Y == other.Y && Math.Abs(X - other.X) == 1)
                return true;

            // Diagonal up right
            if (X + 1 == other.X &&
                Y + 1 == other.Y)
                return true;

            // Diagonal up left
            if (X - 1 == other.X &&
                Y + 1 == other.Y)
                return true;

            // Diagonal down right
            if (X + 1 == other.X &&
                Y - 1 == other.Y)
                return true;

            // Diagonal down left
            if (X - 1 == other.X &&
                Y - 1 == other.Y)
                return true;

            return false;
        }

        public void Follow(Coord other)
        {
            Action follow = (this, other) switch
            {
                var (tail, head) when tail.X == head.X && tail.Y > head.Y => () => MoveDown(1),
                var (tail, head) when tail.X == head.X && tail.Y < head.Y => () => MoveUp(1),
                var (tail, head) when tail.Y == head.Y && tail.X > head.X => () => MoveLeft(1),
                var (tail, head) when tail.Y == head.Y && tail.X < head.X => () => MoveRight(1),
                // Diagonal up right
                var (tail, head) when
                    head.Y != tail.Y && head.X != tail.X &&
                    head.Y > tail.Y &&
                    head.X > tail.X => () =>
                    {
                        MoveRight(1);
                        MoveUp(1);
                    },
                // Diagonal up left
                var (tail, head) when
                    head.Y != tail.Y && head.X != tail.X &&
                    head.Y > tail.Y &&
                    head.X < tail.X => () =>
                    {
                        MoveLeft(1);
                        MoveUp(1);
                    },
                // Diagonal down right
                var (tail, head) when
                    head.Y != tail.Y && head.X != tail.X &&
                    head.Y < tail.Y &&
                    head.X > tail.X => () =>
                    {
                        MoveRight(1);
                        MoveDown(1);
                    },
                // Diagonal down left
                var (tail, head) when
                    head.Y != tail.Y && head.X != tail.X &&
                    head.Y < tail.Y &&
                    head.X < tail.X => () =>
                    {
                        MoveLeft(1);
                        MoveDown(1);
                    },
                _ => throw new ArgumentException("Not a valid action..")
            };

            follow();
        }
    }
}