using System;
using System.IO;
using System.Linq;
using Aoc2022;
using Aoc2022.Algorithms.Models;

namespace Aoc2022.Aoc2023;

internal class Day4 : BaseDay
{
    public Day4(bool shouldPrint) : base(2023, nameof(Day4), shouldPrint)
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
        var cards = Parse(input);

        FirstSolution(cards.ToList().Sum(s => s.Points));
    }

    private void PartTwo()
    {
        var input = ReadInput(false, partTwo: true).ToList();
        var cards = Parse(input).ToDictionary(d => d.Nr, d => d);

        var instances = new Dictionary<int, int>();
        var all = new Dictionary<int, int>();

        foreach (var c in cards)
        {
            instances[c.Key] = 1;

            if (all.TryGetValue(c.Key, out var value))
            {
                instances[c.Key] += value;
            }

            c.Value.FindNextWins(ref all, cards);
        }

        var result = instances.Sum(s => s.Value);

        SecondSolution(result);
    }

    private static List<Card> Parse(List<string> input)
        => input.Select(line =>
            {
                var splitted = line.SplitByAndThen(':', '|');
                var cardNr = splitted[0][0].ParseSingleInt();

                var winningNr = splitted[1][0].Split(" ", StringSplitOptions.RemoveEmptyEntries)
                    .Select(int.Parse)
                    .ToArray();

                var ourNr = splitted[1][1].Split(" ", StringSplitOptions.RemoveEmptyEntries)
                    .Select(int.Parse)
                    .ToArray();

                return new Card
                {
                    Nr = cardNr,
                    Wins = winningNr.Intersect(ourNr).ToList()
                };
            })
            .ToList();
}

internal class Card
{
    public int Nr { get; set; }
    public int WinCount => Wins.Count;

    public List<int> Wins { get; set; }

    public int Points => Wins.Aggregate(0, (a, b) => a == 0
        ? 1
        : a * 2);

    public void FindNextWins(
        ref Dictionary<int, int> all,
        Dictionary<int, Card> cards)
    {
        for (var i = Nr + 1; i <= Nr + WinCount; i++)
        {
            if (!cards.TryGetValue(i, out var card))
                continue;

            if (all.ContainsKey(card.Nr))
                all[card.Nr] += 1;
            else
                all[card.Nr] = 1;

            card.FindNextWins(ref all, cards);
        }
    }
}