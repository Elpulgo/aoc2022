using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Aoc2022;

namespace Aoc2022.Aoc2021
{
    internal class Day11 : BaseDay
    {
        public Day11(bool shouldPrint) : base(2021, nameof(Day11), shouldPrint)
        {
        }

        private Dictionary<(int X, int Y), int> _matrix = new();

        public override void Execute()
        {
            Console.WriteLine(nameof(Day11));

            var lines = ReadInput().Select(s => s.ToCharArray().Select(s => Convert.ToInt32(s - '0')).ToList())
                .ToList();
            var sw = Stopwatch.StartNew();

            for (var y = 0; y < lines.Count; y++)
            {
                for (var x = 0; x < lines[y].Count; x++)
                {
                    _matrix.Add((x, y), lines[y][x]);
                }
            }

            var totalFlash = 0;
            var synchronizedFlashes = 0;
            var steps = 0;

            while (steps < 1000)
            {
                var stepVisited = new HashSet<(int X, int Y)>();

                // First increment all
                foreach (var coord in _matrix)
                {
                    var value = _matrix[coord.Key];
                    _matrix[coord.Key] = value == 9 ? 0 : value + 1;
                }

                do
                {
                    // Check if anyone flash
                    foreach (var coord in _matrix.Where(w => !stepVisited.Contains(w.Key) && w.Value == 0))
                    {
                        if (_matrix[coord.Key] != 0)
                            continue;

                        stepVisited.Add(coord.Key);
                        totalFlash++;

                        // Increment neighbours
                        foreach (var neighbour in GetNeighbours(coord.Key))
                        {
                            var value = _matrix[neighbour];
                            if (value == 0)
                                continue;

                            _matrix[neighbour] = value == 9 ? 0 : value + 1;
                        }
                    }
                } while (_matrix.Any(w => !stepVisited.Contains(w.Key) && w.Value == 0));

                steps++;

                if (steps == 100)
                {
                    Console.WriteLine($"Part 1 took {sw.ElapsedMilliseconds} ms");
                    FirstSolution(totalFlash.ToString());
                }

                if (_matrix.All(a => a.Value == 0))
                {
                    synchronizedFlashes = steps;
                    break;
                }
            }

            Console.WriteLine($"Part 2 took {sw.ElapsedMilliseconds} ms");
            SecondSolution(synchronizedFlashes.ToString());
            sw.Stop();
        }

        private IEnumerable<(int X, int Y)> GetNeighbours((int X, int Y) coord)
        {
            // Top
            if (_matrix.ContainsKey((coord.X, coord.Y - 1)))
                yield return (coord.X, coord.Y - 1);

            // Bottom
            if (_matrix.ContainsKey((coord.X, coord.Y + 1)))
                yield return (coord.X, coord.Y + 1);

            // Left
            if (_matrix.ContainsKey((coord.X - 1, coord.Y)))
                yield return (coord.X - 1, coord.Y);

            // Right
            if (_matrix.ContainsKey((coord.X + 1, coord.Y)))
                yield return (coord.X + 1, coord.Y);

            // Diagonal Left Top
            if (_matrix.ContainsKey((coord.X - 1, coord.Y - 1)))
                yield return (coord.X - 1, coord.Y - 1);

            // Diagonal Right Top
            if (_matrix.ContainsKey((coord.X + 1, coord.Y - 1)))
                yield return (coord.X + 1, coord.Y - 1);

            // Diagonal Left Bottom
            if (_matrix.ContainsKey((coord.X - 1, coord.Y + 1)))
                yield return (coord.X - 1, coord.Y + 1);

            // Diagonal Right Bottom
            if (_matrix.ContainsKey((coord.X + 1, coord.Y + 1)))
                yield return (coord.X + 1, coord.Y + 1);
        }
    }
}