using System;
using System.IO;
using System.Linq;
using Aoc2022;

namespace Aoc2022.Aoc2021
{
    internal class Day9 : BaseDay
    {
        private int _rowLength = 0;
        private int _columnLength = 0;

        public Day9(bool shouldPrint) : base(2021, nameof(Day9), shouldPrint)
        {
        }

        public override void Execute()
        {
            Console.WriteLine("Day 9:");

            var lines = ReadInput().ToArray();
            var input = lines
                .Select(s => s.ToCharArray())
                .SelectMany((s, i) => s.Select((p, columnIndex) =>
                    new KeyValuePair<(int X, int Y), int>((columnIndex, i), Convert.ToInt32(p - '0'))))
                .ToDictionary(d => d.Key, d => d.Value);

            _rowLength = lines.First().Length;
            _columnLength = lines.Length;

            var riskLevel = 0;
            var basins = new List<int>();
            var visited = new HashSet<(int X, int Y)>();

            for (int x = 0; x < lines.First().Length; x++)
            {
                for (int y = 0; y < lines.Length; y++)
                {
                    var value = input[(x, y)];

                    if (visited.Contains((x, y)) || value == 9)
                        continue;

                    if (
                        IsLower(FindValue(x, y - 1), value) &&
                        IsLower(FindValue(x, y + 1), value) &&
                        IsLower(FindValue(x - 1, y), value) &&
                        IsLower(FindValue(x + 1, y), value))
                    {
                        riskLevel += (value + 1);
                        visited.Add((x, y));
                        basins.Add(CountBasin(x, y, input, visited) + 1);
                    }

                    bool IsLower(int? neighbour, int value) => !neighbour.HasValue
                        ? true
                        : neighbour.Value > value;

                    int? FindValue(int x, int y) => input.TryGetValue((x, y), out var downFound)
                        ? downFound
                        : null;
                }
            }

            FirstSolution(riskLevel.ToString());
            SecondSolution(basins.OrderByDescending(o => o).Take(3).Aggregate((a, b) => a * b).ToString());
        }

        private int Basins(int x,
            int y,
            Dictionary<(int X, int Y), int> input,
            HashSet<(int X, int Y)> visited,
            Func<int, int, bool> loopOperation,
            Func<int, int, (int, int)> incrementOperation)
        {
            var count = 0;
            while (loopOperation(x, y))
            {
                (x, y) = incrementOperation(x, y);
                if (visited.Contains((x, y)) || input[(x, y)] == 9)
                    break;

                visited.Add((x, y));
                count += CountBasin(x, y, input, visited) + 1;
            }

            return count;
        }

        private int CountBasin(int x,
            int y,
            Dictionary<(int X, int Y), int> input,
            HashSet<(int X, int Y)> visited)
        {
            var basinCount = 0;
            basinCount += Basins(x, y, input, visited, (x, y) => y < _columnLength - 1, (x, y) => (x, y + 1));
            basinCount += Basins(x, y, input, visited, (x, y) => x < _rowLength - 1, (x, y) => (x + 1, y));
            basinCount += Basins(x, y, input, visited, (x, y) => y > 0, (x, y) => (x, y - 1));
            basinCount += Basins(x, y, input, visited, (x, y) => x > 0, (x, y) => (x - 1, y));
            return basinCount;
        }
    }
}