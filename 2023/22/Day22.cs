
namespace Aoc2022.Aoc2023;

internal class Day22 : BaseDay
{
    public Day22(bool shouldPrint) : base(2023, nameof(Day22), shouldPrint)
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