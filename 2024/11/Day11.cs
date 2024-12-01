
namespace Aoc2022.Aoc2024;

internal class Day11 : BaseDay
{
    public Day11(bool shouldPrint) : base(2024, nameof(Day11), shouldPrint)
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