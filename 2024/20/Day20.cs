
namespace Aoc2022.Aoc2024;

internal class Day20 : BaseDay
{
    public Day20(bool shouldPrint) : base(2024, nameof(Day20), shouldPrint)
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