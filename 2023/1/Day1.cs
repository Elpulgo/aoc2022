using System;
using System.IO;
using System.Linq;
using Aoc2022;

namespace Aoc2022.Aoc2023;

internal class Day1 : BaseDay
{
    public Day1(bool shouldPrint) : base(2023, nameof(Day1), shouldPrint)
    {
    }

    public override void Execute()
    {
        PartOne();
        PartTwo();
    }

    private void PartOne()
    {
        var input = ReadInput(partOne: true).ToList();
        var sum = input.Aggregate(0,
            (sum, line) => sum + int.Parse($"{line.First(char.IsNumber)}{line.Last(char.IsNumber)}"));

        FirstSolution(sum);
    }

    private void PartTwo()
    {
        var whitelistedWords = new Dictionary<string, int>
        {
            { "one", 1 },
            { "two", 2 },
            { "three", 3 },
            { "four", 4 },
            { "five", 5 },
            { "six", 6 },
            { "seven", 7 },
            { "eight", 8 },
            { "nine", 9 }
        };

        var input = ReadInput(false, partTwo: true).ToList();

        var sum = input.Aggregate(0,
            (sum, line) =>
            {
                var first = FindOccurrence(line, whitelistedWords, leftToRight: true);
                var last = FindOccurrence(line, whitelistedWords, leftToRight: false);
                return sum + int.Parse($"{first}{last}");
            });

        SecondSolution(sum);
    }

    private static int FindOccurrence(string line, Dictionary<string, int> whitelistedWords, bool leftToRight)
    {
        string word = "";
        var charArray = line.ToCharArray();

        if (!leftToRight)
        {
            charArray = charArray.Reverse().ToArray();
        }

        foreach (var lineChar in charArray)
        {
            if (char.IsNumber(lineChar))
                return lineChar - '0';

            word = leftToRight
                ? word + lineChar
                : lineChar + word;

            if (whitelistedWords.TryGetValue(word, out var numericRepresentation))
                return numericRepresentation;

            foreach (var keyValue in whitelistedWords)
            {
                if (word.Contains(keyValue.Key))
                    return keyValue.Value;
            }
        }

        throw new ArgumentOutOfRangeException($"Didn't find a match on line '{line}'");
    }
}