
namespace Aoc2022.Aoc2024;

internal class Day7 : BaseDay
{
    public Day7(bool shouldPrint) : base(2024, nameof(Day7), shouldPrint)
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