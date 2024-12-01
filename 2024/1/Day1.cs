
namespace Aoc2022.Aoc2024;

internal class Day1 : BaseDay
{
    public Day1(bool shouldPrint) : base(2024, nameof(Day1), shouldPrint)
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