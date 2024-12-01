
namespace Aoc2022.Aoc2024;

internal class Day19 : BaseDay
{
    public Day19(bool shouldPrint) : base(2024, nameof(Day19), shouldPrint)
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