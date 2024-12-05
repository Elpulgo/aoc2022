namespace Aoc2022.Aoc2024;

internal class Day5 : BaseDay
{
    public Day5(bool shouldPrint) : base(2024, nameof(Day5), shouldPrint)
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

        var (orderingRules, pageSequences) = Setup(input);

        var sum = pageSequences
            .Where(w => !IsSeqBroken(w, orderingRules))
            .Sum(cor => cor[cor.Count / 2]);

        FirstSolution(sum);
    }

    private void PartTwo()
    {
        var input = ReadInput(false, partTwo: true).ToList();

        var (orderingRulesSet, pageSequences) = Setup(input);

        var brokenSequences = pageSequences
            .Where(w => IsSeqBroken(w, orderingRulesSet))
            .ToList();

        foreach (var corruptSequence in brokenSequences)
        {
            do
            {
                for (int i = 0; i < corruptSequence.Count; i++)
                {
                    // Out of bounds
                    if (i >= corruptSequence.Count - 1)
                        continue;

                    var a = corruptSequence[i];
                    var b = corruptSequence[i + 1];

                    if (orderingRulesSet.Contains(b + "|" + a))
                    {
                        (corruptSequence[i], corruptSequence[i + 1]) = (corruptSequence[i + 1], corruptSequence[i]);
                    }
                }
            } while (IsSeqBroken(corruptSequence, orderingRulesSet));
        }

        var sum = brokenSequences.Sum(cor => cor[cor.Count / 2]);

        SecondSolution(sum);
    }

    private static (HashSet<string>, List<List<int>>) Setup(List<string> input)
    {
        var pageSequences = input
            .Where(w => !w.Contains('|') && !string.IsNullOrWhiteSpace(w))
            .Select(s => s.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(s1 => s1.ParseSingleInt()).ToList())
            .ToList();

        var orderingRules = input
            .Where(w => w.Contains('|'))
            .ToHashSet();

        return (orderingRules, pageSequences);
    }

    private static bool IsSeqBroken(List<int> seq, HashSet<string> map)
    {
        for (int i = 0; i < seq.Count; i++)
        {
            // Out of bounds
            if (i >= seq.Count - 1)
                continue;

            var a = seq[i];
            var b = seq[i + 1];

            if (map.Contains(b + "|" + a))
            {
                return true;
            }
        }

        return false;
    }
}