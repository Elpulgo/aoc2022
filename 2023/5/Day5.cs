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

        var ranges = new List<(long From, long To)>();
        var mapInfos = Parse(input);

        for (int i = 0; i < seedRanges.Length; i += 2)
        {
            var startSeed = long.Parse(seedRanges[i]);
            var steps = long.Parse(seedRanges[i + 1]);

            ranges.Add((startSeed, startSeed + steps));
        }

        foreach (var mapInfo in mapInfos)
        {
            var orderedParts = mapInfo.Parts.OrderBy(x => x.SourceRangeStart).ToList();
            var mapRanges = new List<(long from, long to)>();

            foreach (var range in ranges)
            {
                var currentRange = range;
                foreach (var part in orderedParts)
                {
                    if (!part.HandleFrom(ref currentRange, ref mapRanges))
                        break;

                    if (!part.HandleTo(ref currentRange, ref mapRanges))
                        break;
                }

                // If range is valid, add to map
                if (currentRange.From <= currentRange.To)
                    mapRanges.Add(currentRange);
            }

            ranges = mapRanges;
        }

        var result = ranges.Min(r => r.From);
        SecondSolution(result);
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
    public MapInfo(IReadOnlyList<string> lines)
    {
        Parts = lines.Skip(1).Select(s => new MapParts(s)).ToList();
    }

    public List<MapParts> Parts { get; }

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

    private long From => SourceRangeStart;
    private long To => SourceRangeStart + RangeLength - 1;
    private long Offset => DestRangeStart - SourceRangeStart;

    private long DestRangeStart => long.Parse(_lineParts[0]);
    public long SourceRangeStart => long.Parse(_lineParts[1]);
    private long RangeLength => long.Parse(_lineParts[2]);

    public (bool, long) FindValue(long source)
    {
        var isInRange = source >= SourceRangeStart && source <= SourceRangeStart + RangeLength;

        if (!isInRange)
            return (false, source);

        var steps = SourceRangeStart + RangeLength - source;

        return (true, DestRangeStart + RangeLength - steps);
    }

    public bool HandleFrom(
        ref (long From, long To) currentRange,
        ref List<(long From, long To)> rangesForPart)
    {
        if (currentRange.From < From)
        {
            // Set start of range
            // Take min of last in range || start of part
            rangesForPart.Add((
                currentRange.From,
                Math.Min(currentRange.To, From - 1)));
            currentRange.From = From;

            // Outside bounce
            if (currentRange.From > currentRange.To)
                return false;
        }

        return true;
    }

    public bool HandleTo(
        ref (long From, long To) currentRange,
        ref List<(long From, long To)> rangesForPart)
    {
        if (currentRange.From <= To)
        {
            // Set end of range
            // Take min of last in range || destination of part
            rangesForPart.Add((
                currentRange.From + Offset,
                Math.Min(currentRange.To, To) + Offset));

            currentRange.From = To + 1;

            // Outside bounce
            if (currentRange.From > currentRange.To)
                return false;
        }

        return true;
    }
}