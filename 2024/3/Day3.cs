using System.Text.RegularExpressions;

namespace Aoc2022.Aoc2024;

internal class Day3 : BaseDay
{
    private static readonly Regex MultiplyRegex = new(@"mul\(\d+,\d+\)", RegexOptions.Compiled);

    public Day3(bool shouldPrint) : base(2024, nameof(Day3), shouldPrint)
    {
    }

    public override void Execute()
    {
        PartOne();
        PartTwo();
    }

    private void PartOne()
    {
        var result = MultiplyRegex
            .Matches(ReadInputRaw(true))
            .Sum(s => PerformMultiply(s.Value));

        FirstSolution(result);
    }

    private void PartTwo()
    {
        var input = ReadInputRaw(false, partTwo: true);

        var regexDo = new Regex(@"do()", RegexOptions.Compiled);
        var regexDont = new Regex(@"don't()", RegexOptions.Compiled);

        var capturedMatches = new List<TypeOfMatch>();

        capturedMatches.AddRange(MultiplyRegex
            .Matches(input)
            .Select(s => new TypeOfMatch(s.Index, s.Value, null)));

        capturedMatches.AddRange(regexDo
            .Matches(input)
            .Select(s => new TypeOfMatch(s.Index, string.Empty, true)));

        capturedMatches.AddRange(regexDont
            .Matches(input)
            .Select(s => new TypeOfMatch(s.Index, string.Empty, false)));

        var sum = capturedMatches
            .OrderBy(o => o.StartIndex)
            .Aggregate((0, true), (c, match) =>
            {
                if (c.Item2 && match.ShouldDo is null)
                {
                    c.Item1 += PerformMultiply(match.Value);
                    return c;
                }

                c.Item2 = match.ShouldPerformOperation;
                return c;
            }).Item1;
        
        SecondSolution(sum);
    }

    private static int PerformMultiply(string value)
    {
        var values = value.Split(",", StringSplitOptions.RemoveEmptyEntries);
        var first = values[0].ParseSingleInt();
        var second = values[1].ParseSingleInt();

        return first * second;
    }

    private record TypeOfMatch(int StartIndex, string Value, bool? ShouldDo)
    {
        public bool ShouldPerformOperation => ShouldDo.HasValue && ShouldDo.Value;
    }
}