using System;
using System.IO;
using System.Linq;
using Aoc2022;

namespace Aoc2022.Aoc2023;

internal class Day5 : BaseDay
{
    public Day5(bool shouldPrint) : base(2023, nameof(Day5), shouldPrint)
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
        var seeds = input[0].SplitByAndThen(':', ' ')[1].Select(Int64.Parse).ToList();

        var mapInfos = Parse(input);

        var result = seeds
            .Select(s => MapTraverser(s, mapInfos))
            .Min();

        FirstSolution(result);
    }


    private void PartTwo()
    {
        var input = ReadInput(false, partTwo: true).ToList();
        var seedRanges = input[0].SplitByAndThen(':', ' ')[1];

        var ranges = new List<(long, long)>();

        for (int i = 0; i < seedRanges.Length; i += 2)
        {
            var startSeed = long.Parse(seedRanges[i]);
            var steps = long.Parse(seedRanges[i + 1]);

            ranges.Add((startSeed, steps));
        }

        var min = ranges.Select(s => s.Item1).Min();
        var max = ranges.Select(s => s.Item1 + s.Item2).Max();

        var between = max - min;

        var mapInfos = Parse(input);

        var range = new List<long>();
        SecondSolution(range.Min());
    }

    private static List<MapInfo> Parse(List<string> lines)
    {
        var stack = new Stack<string>(lines.Skip(1).AppendLine("").Reverse());
        var mapInfos = new List<MapInfo>();
        var mapInfoLines = new List<string>();

        while (stack.TryPop(out var line))
        {
            if (line.Trim() == string.Empty && mapInfoLines.Any())
            {
                mapInfos.Add(new MapInfo(mapInfoLines));
                mapInfoLines = new List<string>();
                continue;
            }

            if (line.Trim() != string.Empty)
            {
                mapInfoLines.Add(line);
            }
        }

        return mapInfos;
    }

    private static long MapTraverser(
        long value,
        IEnumerable<MapInfo> all)
        => all.Aggregate(value, (current, mapInfo) => mapInfo.FindValue(current));
}

internal class MapInfo
{
    private readonly string[] _headerLines;

    public MapInfo(IReadOnlyList<string> lines)
    {
        _headerLines = lines[0].Replace(" map:", "").Split("-", StringSplitOptions.RemoveEmptyEntries);
        Parts = lines.Skip(1).Select(s => new MapParts(s)).ToList();
    }

    public string Source => _headerLines[0];
    public string Destination => _headerLines[2];
    private List<MapParts> Parts { get; }

    public long FindValue(long source)
    {
        foreach (var p in Parts)
        {
            var (found, value) = p.FindValue(source);

            if (found)
            {
                return value;
            }
        }

        return source;
    }
}

internal class MapParts
{
    private readonly string[] _lineParts;

    public MapParts(string line)
    {
        _lineParts = line.Split(" ");
    }

    public long DestRangeStart => long.Parse(_lineParts[0]);
    public long SourceRangeStart => long.Parse(_lineParts[1]);
    public long RangeLength => long.Parse(_lineParts[2]);

    public (bool, long) FindValue(long source)
    {
        var isInRange = source >= SourceRangeStart && source <= SourceRangeStart + RangeLength;

        if (!isInRange)
            return (false, source);

        var steps = SourceRangeStart + RangeLength - source;

        return (true, DestRangeStart + RangeLength - steps);
    }
}