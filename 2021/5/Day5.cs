using System;
using System.IO;
using System.Linq;
using Aoc2022;

namespace Aoc2022.Aoc2021
{
    internal class Day5 : BaseDay
    {
        public Day5(bool shouldPrint) : base(2021, nameof(Day5), shouldPrint)
        {
        }

        public override void Execute()
        {
            Console.WriteLine("Day 5:");

            FirstSolution(Execute(isPartTwo: false).ToString());
            SecondSolution(Execute(isPartTwo: true).ToString());
        }

        private int Execute(bool isPartTwo)
        {
            var coords = new Dictionary<(int, int), int>();

            ReadInput()
                .ToList()
                .Select(line =>
                    {
                        var coordinates = line.Split("->", StringSplitOptions.RemoveEmptyEntries);

                        var firstCoords = coordinates.First().Split(",", StringSplitOptions.RemoveEmptyEntries);
                        var lastCoords = coordinates.Last().Split(",", StringSplitOptions.RemoveEmptyEntries);

                        var first = (
                            x: Convert.ToInt32(firstCoords.First().Trim()),
                            y: Convert.ToInt32(firstCoords.Last().Trim())
                        );

                        var last = (
                            x: Convert.ToInt32(lastCoords.First().Trim()),
                            y: Convert.ToInt32(lastCoords.Last().Trim())
                        );

                        return (first, last);
                    }
                )
                .Select(s =>
                    s.first.x == s.last.x
                        ? s.first.y == new List<int>()
                        {
                            s.first.y,
                            s.last.y
                        }.Min()
                            ? (s.first, s.last)
                            : (s.last, s.first)
                        : s.first.x == new List<int>()
                        {
                            s.first.x,
                            s.last.x
                        }.Min()
                            ? (s.first, s.last)
                            : (s.last, s.first)
                )
                .Select(s => (first: s.Item1, last: s.Item2))
                .ToList()
                .ForEach(line =>
                {
                    if (line.first.x == line.last.x || line.first.y == line.last.y)
                    {
                        var (first, last, opposite, xOrY) = line.first.x == line.last.x
                            ? (line.first.y, line.last.y, line.first.x, 0)
                            : (line.first.x, line.last.x, line.first.y, 1);

                        Enumerable.Range(first, last - first + 1)
                            .ToList()
                            .ForEach(c =>
                            {
                                (int x, int y) = xOrY == 0 ? (opposite, c) : (c, opposite);
                                coords.AddOrUpdate(x, y);
                            });

                        return;
                    }

                    if (!isPartTwo)
                        return;

                    var (start, end) = line.first.x < line.last.x
                        ? (line.first, line.last)
                        : (line.last, line.first);

                    var positiveSlope = start.y > end.y;

                    int xd = start.x, yd = start.y;

                    while ((xd, yd) != (end.x, end.y))
                    {
                        coords.AddOrUpdate(xd, yd);
                        xd += 1;
                        yd = positiveSlope
                            ? yd -= 1
                            : yd += 1;
                    }

                    coords.AddOrUpdate(xd, yd);
                });

            return coords.Count(c => c.Value > 1);
        }
    }

    public static class DictionaryExtension
    {
        public static void AddOrUpdate(this Dictionary<(int, int), int> dictionary, int x, int y)
        {
            if (dictionary.ContainsKey((x, y)))
            {
                dictionary[(x, y)]++;
                return;
            }

            dictionary[(x, y)] = 1;
        }
    }
}