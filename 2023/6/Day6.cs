namespace Aoc2022.Aoc2023;

internal class Day6 : BaseDay
{
    private readonly Func<List<string>, int, IEnumerable<int>> _parser = (input, lineNr) =>
        input[lineNr].SplitByAndThen(':', ' ')[1].Select(int.Parse);

    public Day6(bool shouldPrint) : base(2023, nameof(Day6), shouldPrint)
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

        var times = _parser(input, 0).ToList();
        var distance = _parser(input, 1).ToList();

        var result = times.SelectWithIndex().Select(s =>
        {
            var d = distance[s.Item2];
            return Algorithm(s.Item1, d);
        }).Aggregate((a, b) => a * b);

        FirstSolution(result);
    }


    private void PartTwo()
    {
        var input = ReadInput(false, partTwo: true).ToList();

        var time = string.Join(
                "",
                _parser(input, 0))
            .ParseSingleLong();

        var distance = string.Join(
                "",
                _parser(input, 1))
            .ParseSingleLong();

        var result = Algorithm(time, distance);
        SecondSolution(result);
    }

    private static int Algorithm(long time, long distance)
    {
        var distancesTraveled = new List<long>();

        for (var speedPerMs = 1; speedPerMs < time; speedPerMs++)
        {
            var remainingTime = time - speedPerMs;
            distancesTraveled.Add(speedPerMs * remainingTime);
        }

        return distancesTraveled.Count(w => w > distance);
    }
}