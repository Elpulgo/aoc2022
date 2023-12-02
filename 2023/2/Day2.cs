using System;
using System.IO;
using System.Linq;
using Aoc2022;

namespace Aoc2022.Aoc2023;

internal class Day2 : BaseDay
{
    private readonly Func<string, (int, string[], Dictionary<string, List<int>>)> _setFunc = (string line) =>
    {
        var lineParts = line.Split(":", StringSplitOptions.RemoveEmptyEntries);
        var gameNr = lineParts[0].ParseSingleInt();
        var sets = lineParts[1].Split(";");

        return (gameNr, sets, new Dictionary<string, List<int>>
        {
            { "green", new List<int>() },
            { "blue", new List<int>() },
            { "red", new List<int>() }
        });
    };

    private readonly Func<string, (int, string)> _variablesFunc = (string setPart) =>
    {
        var cubeInfo = setPart.Split(" ", StringSplitOptions.RemoveEmptyEntries);
        var point = cubeInfo[0].ParseSingleInt();
        var color = cubeInfo[1].Trim();

        return (point, color);
    };

    public Day2(bool shouldPrint) : base(2023, nameof(Day2), shouldPrint)
    {
    }

    public override void Execute()
    {
        PartOne();
        PartTwo();
    }

    private void PartOne()
    {
        var colorPoints = new Dictionary<string, int>
        {
            { "red", 12 },
            { "green", 13 },
            { "blue", 14 }
        };

        var result = ReadInput(true)
            .Select(line =>
            {
                var (gameNr, sets, colorMap) = _setFunc(line);

                foreach (var setPart in sets.SelectMany(s => s.Split(",").Select(ss => ss)))
                {
                    var (point, color) = _variablesFunc(setPart);
                    colorMap[color].Add(point);
                }

                return (gameNr, colorMap.All(a => colorPoints[a.Key] >= a.Value.Max()));
            }).ToList();

        var sum = result.Where(w => w.Item2).Sum(s => s.gameNr);

        FirstSolution(sum);
    }

    private void PartTwo()
    {
        var result = ReadInput(false, partTwo: true)
            .Select(line =>
            {
                var (_, sets, colorMap) = _setFunc(line);

                foreach (var setPart in sets.SelectMany(s => s.Split(",").Select(ss => ss)))
                {
                    var (point, color) = _variablesFunc(setPart);
                    colorMap[color].Add(point);
                }

                return colorMap.Aggregate(1, (a, b) => a * b.Value.Max());
            }).ToList();

        var sum = result.Sum();

        SecondSolution(sum);
    }
}