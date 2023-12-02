using System;
using System.IO;
using System.Linq;
using Aoc2022;

namespace Aoc2022.Aoc2023;

internal class Day2 : BaseDay
{
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
                var lineParts = line.Split(":", StringSplitOptions.RemoveEmptyEntries);
                var gameNr = lineParts[0].ParseSingleInt();
                var sets = lineParts[1].Split(";");

                var pointsInSetsPossible = sets
                    .SelectMany(ss => ss.Split(",")
                        .Select(sss =>
                        {
                            var cubeInfo = sss.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                            var point = cubeInfo[0].ParseSingleInt();
                            var color = cubeInfo[1].Trim();
                            return colorPoints.TryGetValue(color, out var maxPoint) && point <= maxPoint;
                        }));

                return (gameNr, pointsInSetsPossible.All(a => a));
            }).ToList();

        var sum = result.Where(w => w.Item2).Sum(s => s.gameNr);

        FirstSolution(sum);
    }

    private void PartTwo()
    {
        var result = ReadInput(false, partTwo: true)
            .Select(line =>
            {
                var lineParts = line.Split(":", StringSplitOptions.RemoveEmptyEntries);
                var sets = lineParts[1].Split(";");

                var colorMap = new Dictionary<string, int>();

                foreach (var setPart in sets.SelectMany(s => s.Split(",").Select(ss => ss)))
                {
                    var cubeInfo = setPart.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                    var point = cubeInfo[0].ParseSingleInt();
                    var color = cubeInfo[1].Trim();

                    if (colorMap.TryGetValue(color, out var maxPoint))
                    {
                        if (point > maxPoint)
                        {
                            colorMap[color] = point;
                        }
                    }
                    else
                    {
                        colorMap[color] = point;
                    }
                }

                return colorMap.Aggregate(1, (a, b) => a * b.Value);
            }).Sum();

        SecondSolution(result);
    }
}