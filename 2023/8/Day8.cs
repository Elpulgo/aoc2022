using System.Text;
using Aoc2022.Algorithms;
using Aoc2022.Algorithms.Models;

namespace Aoc2022.Aoc2023;

internal class Day8 : BaseDay
{
    private int StepCount = 0;

    public Day8(bool shouldPrint) : base(2023, nameof(Day8), shouldPrint)
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
        var (map, instructions) = Parse(input);

        var stackInstructions = new Stack<int>(instructions);
        var steps = 0;
        var currentKey = "AAA";

        while (true)
        {
            // At end of instructions, start over
            if (!stackInstructions.TryPop(out var instruction))
            {
                stackInstructions = new Stack<int>(instructions);
                continue;
            }

            steps++;

            var choiceLeft = map[currentKey][0];
            var choiceRight = map[currentKey][1];

            currentKey = instruction == 0
                ? choiceLeft
                : choiceRight;

            if (currentKey == "ZZZ")
                break;
        }

        FirstSolution(steps);
    }

    private void PartTwo()
    {
        var input = ReadInput(false, partTwo: true).ToList();

        var (map, instructions) = Parse(input);

        var stackInstructions = new Stack<int>(instructions);
        var steps = 0;

        var currentKeys = map.Where(w => w.Key.EndsWith('A')).Select(s => s.Key);

        var allSteps = new List<long>();

        while (true)
        {
            // At end of instructions, start over
            if (!stackInstructions.TryPop(out var instruction))
            {
                stackInstructions = new Stack<int>(instructions);
                continue;
            }

            steps++;

            var tempKeys = new List<string>();
            foreach (var c in currentKeys)
            {
                tempKeys.Add(map[c][instruction]);
            }

            currentKeys = tempKeys;

            if (currentKeys.Any(a => a.EndsWith('Z')))
            {
                allSteps.Add(steps);
                currentKeys = currentKeys.Where(w => !w.EndsWith('Z')).ToList();
            }

            if (!currentKeys.Any())
            {
                break;
            }
        }

        SecondSolution(LeastCommonMulitple(allSteps.ToArray()));
    }

    private static long GreatestCommonMulitiple(long n1, long n2) => n2 == 0
        ? n1
        : GreatestCommonMulitiple(n2, n1 % n2);

    private static long LeastCommonMulitple(long[] numbers) =>
        numbers.Aggregate((S, val) => S * val / GreatestCommonMulitiple(S, val));


    private static (Dictionary<string, string[]>, List<int>) Parse(
        IReadOnlyCollection<string> input)
    {
        var map = new Dictionary<string, string[]>();
        var instructions = input
            .First()
            .Reverse()
            .Select(instruction => instruction == 'L' ? 0 : 1)
            .ToList();

        foreach (var line in input.Skip(2))
        {
            var parts = line.Split("=", StringSplitOptions.RemoveEmptyEntries);
            var key = parts[0].Trim();
            var values = parts[1].ReplaceWithEmpty("(", ")").Split(",", StringSplitOptions.RemoveEmptyEntries);

            map.Add(key, new[] { values[0].Trim(), values[1].Trim() });
        }

        return (map, instructions);
    }
}