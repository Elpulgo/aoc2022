
namespace Aoc2022.Aoc2023;

internal class Day19 : BaseDay
{
    public Day19(bool shouldPrint) : base(2023, nameof(Day19), shouldPrint)
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
    }


    private void PartTwo()
    {
        var input = ReadInput(false, partTwo: true).ToList();
    }
}