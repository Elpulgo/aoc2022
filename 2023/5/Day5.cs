using System;
using System.IO;
using System.Linq;
using Aoc2022;

namespace Aoc2022.Aoc2023;

internal class Day5 : BaseDay
{
    public Day5(bool shouldPrint) : base(2023, nameof(Day5), shouldPrint)
    {
    }

    public override void Execute()
    {
        PartOne();
        PartTwo();
    }

    private void PartOne()
    {
        var input = ReadInput(false, partTwo: true).ToList();
    }


    private void PartTwo()
    {
        var input = ReadInput(false, partTwo: true).ToList();
    }
}