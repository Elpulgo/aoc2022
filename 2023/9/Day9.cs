namespace Aoc2022.Aoc2023;

internal class Day9 : BaseDay
{
    public Day9(bool shouldPrint) : base(2023, nameof(Day9), shouldPrint)
    {
    }

    public override void Execute()
    {
        PartOne();
        PartTwo();
    }

    private void PartOne()
    {
        var result = ReadInput(true)
            .Select(s => ParseLine(s, partTwo: false))
            .Sum();

        FirstSolution(result);
    }


    private void PartTwo()
    {
        var result = ReadInput(false, partTwo: true)
            .Select(s => ParseLine(s, partTwo: true))
            .Sum();
        
        SecondSolution(result);
    }

    private static long ParseLine(string line, bool partTwo = false)
    {
        var history = line
            .Split(" ", StringSplitOptions.RemoveEmptyEntries)
            .Select(s => long.Parse(s.Trim()))
            .ToArray();

        var lastNumeric = new Stack<long>();

        while (true)
        {
            lastNumeric.Push(partTwo
                ? history.First()
                : history.Last());

            var newHistory = new long[history.Length - 1];

            for (var i = 1; i < history.Length; i++)
                newHistory[i - 1] = history[i] - history[i - 1];

            if (newHistory.Sum() == 0)
            {
                // All is zeroes
                break;
            }

            history = newHistory;
        }

        long lineResult = lastNumeric.Pop();
        
        while (lastNumeric.TryPop(out var lastNum))
        {
            lineResult = partTwo
                ? lastNum - lineResult
                : lineResult + lastNum;
        }

        return lineResult;
    }
}