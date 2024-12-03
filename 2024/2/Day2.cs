namespace Aoc2022.Aoc2024;

internal class Day2 : BaseDay
{
    private const int ThresholdMin = 1;
    private const int ThresholdMax = 3;

    public Day2(bool shouldPrint) : base(2024, nameof(Day2), shouldPrint)
    {
    }

    public override void Execute()
    {
        PartOne();
        PartTwo();
    }

    private void PartOne()
    {
        var input = ReadInput(true)
            .Select(s => s
                .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                .Select(s1 => s1.ParseSingleInt())
                .ToList())
            .ToList();

        var score = input.Sum(row => IsSafe(row) ? 1 : 0);

        FirstSolution(score);
    }


    private void PartTwo()
    {
        var input = ReadInput(false, partTwo: true)
            .Select(s => s
                .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                .Select(s1 => s1.ParseSingleInt())
                .ToList())
            .ToList();

        var score = input
            .Sum(row =>
            {
                var isSafe = IsSafe(row);

                if (isSafe)
                    return 1;

                // If it's only a single issue in the series, then it can be mitigated to safe by removing
                // otherwise we are busted, and no points given
                for (var i = 0; i < row.Count; i++)
                {
                    var candidate = row.ToList();
                    candidate.RemoveAt(i);

                    if (IsSafe(candidate))
                    {
                        return 1;
                    }
                }

                return 0;
            });

        SecondSolution(score);
    }

    private static bool IsSafe(List<int> row)
    {
        var isDecreasing = row[0] - row[^1] > 0;

        return row
            .Select((s, index) => (s, index))
            .Skip(1)
            .Aggregate(true, (current, valueAndIndex) =>
            {
                // Short circuit
                if (!current)
                    return current;

                var diff = row[valueAndIndex.index - 1] - row[valueAndIndex.index];
                if (Math.Abs(diff) < ThresholdMin || Math.Abs(diff) > ThresholdMax)
                    return false;

                return (!isDecreasing || diff >= 0) && (isDecreasing || diff <= 0);
            });
    }
}