namespace Aoc2022.Aoc2023;

internal class Day7 : BaseDay
{
    public Day7(bool shouldPrint) : base(2023, nameof(Day7), shouldPrint)
    {
    }

    public override void Execute()
    {
        PartOne();
        PartTwo();
    }

    private void PartOne()
    {
        long result = ReadInput(true)
            .Select(inp => new ElfHand(inp))
            .OrderByDescending(o => o)
            .SelectWithIndex(offset: 1)
            .Sum(v => v.Value.Rank * v.Index);

        FirstSolution(result);
    }


    private void PartTwo()
    {
        long result = ReadInput(false, partTwo: true)
            .Select(inp => new ElfHand(inp, partTwo: true))
            .OrderByDescending(o => o)
            .SelectWithIndex(offset: 1)
            .Sum(v => v.Value.Rank * v.Index);

        SecondSolution(result);
    }
}

internal class ElfHand : IComparable<ElfHand>
{
    private static readonly Dictionary<char, int> CardMap = new()
    {
        { 'A', 13 },
        { 'K', 12 },
        { 'Q', 11 },
        { 'J', 10 },
        { 'T', 9 },
        { '9', 8 },
        { '8', 7 },
        { '7', 6 },
        { '6', 5 },
        { '5', 4 },
        { '4', 3 },
        { '3', 2 },
        { '2', 1 }
    };

    private char[] OrderedHand { get; }
    public int Rank { get; }
    private CardVariant Variant { get; }

    public ElfHand(string value, bool partTwo = false)
    {
        Rank = value.Split(" ")[1].ParseSingleInt();
        OrderedHand = value.Split(" ")[0].ToCharArray();
        Variant = Compare(partTwo);

        if (!partTwo)
            return;

        CardMap['J'] = 0;
    }

    public int CompareTo(ElfHand? other)
    {
        if (Variant != other?.Variant)
        {
            return Variant > other?.Variant
                ? -1
                : 1;
        }

        foreach (var hand in OrderedHand.SelectWithIndex())
        {
            var otherHand = other.OrderedHand[hand.Index];

            if (hand.Value == otherHand)
                continue;

            var leftScore = CardMap[hand.Value];
            var rightScore = CardMap[otherHand];

            return leftScore > rightScore
                ? -1
                : 1;
        }

        return 0;
    }

    private CardVariant Compare(bool partTwo = false)
    {
        var res = OrderedHand.GroupBy(g => g).ToDictionary(d => d.Key, d => d.ToList());
        var hasJs = res.TryGetValue('J', out var js);

        if (!partTwo || !hasJs)
            return InitialCardVariantMap(res);

        var nrOfJs = js!.Count;
        var nonJs = res.Where(w => w.Key != 'J').ToList();
        var withoutJs = InitialCardVariantMap(nonJs);

        return withoutJs switch
        {
            CardVariant.HighCard when nrOfJs == 1 => CardVariant.OnePair,
            CardVariant.HighCard when nrOfJs == 2 => CardVariant.ThreeKind,
            CardVariant.HighCard when nrOfJs == 3 => CardVariant.FourKind,
            CardVariant.HighCard when nrOfJs == 4 => CardVariant.FiveKind,
            CardVariant.HighCard when nrOfJs == 5 => CardVariant.FiveKind,

            CardVariant.OnePair when nrOfJs == 1 => CardVariant.ThreeKind,
            CardVariant.OnePair when nrOfJs == 2 => CardVariant.FourKind,
            CardVariant.OnePair when nrOfJs == 3 => CardVariant.FiveKind,

            CardVariant.TwoPair when nrOfJs == 1 => CardVariant.FullHouse,

            CardVariant.ThreeKind when nrOfJs == 1 => CardVariant.FourKind,
            CardVariant.ThreeKind when nrOfJs == 2 => CardVariant.FiveKind,

            CardVariant.FullHouse when nrOfJs == 1 => CardVariant.FourKind,
            CardVariant.FullHouse when nrOfJs == 2 => CardVariant.FiveKind,

            CardVariant.FourKind when nrOfJs == 1 => CardVariant.FiveKind,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private static CardVariant InitialCardVariantMap(
        IReadOnlyCollection<KeyValuePair<char, List<char>>> values) => values switch
    {
        _ when values.Any(a => a.Value.Count == 5) => CardVariant.FiveKind,
        _ when values.Any(a => a.Value.Count == 4) => CardVariant.FourKind,
        _ when values.Any(a => a.Value.Count == 3) && values.Any(a => a.Value.Count == 2) => CardVariant.FullHouse,
        _ when values.Any(a => a.Value.Count == 3) => CardVariant.ThreeKind,
        _ when values.Count(a => a.Value.Count == 2) == 2 => CardVariant.TwoPair,
        _ when values.Count(a => a.Value.Count == 2) == 1 => CardVariant.OnePair,
        _ => CardVariant.HighCard
    };
}

internal enum CardVariant
{
    FiveKind = 7,
    FourKind = 6,
    FullHouse = 5,
    ThreeKind = 4,
    TwoPair = 3,
    OnePair = 2,
    HighCard = 1
}