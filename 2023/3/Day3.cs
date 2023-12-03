using System;
using System.IO;
using System.Linq;
using Aoc2022;
using Aoc2022.Algorithms.Models;

namespace Aoc2022.Aoc2023;

internal class Day3 : BaseDay
{
    public Day3(bool shouldPrint) : base(2023, nameof(Day3), shouldPrint)
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
        var (symbols, cellContainers) = Parse(input, (_) => true);

        var result = symbols
            .SelectMany(s => s.GetAllTouchingNumericContainers(cellContainers))
            .Sum(s => s.NumericValue);

        FirstSolution(result);
    }

    private void PartTwo()
    {
        var input = ReadInput(false, partTwo: true).ToList();
        var (symbols, cellContainers) = Parse(input, cell => cell == '*');

        var result = symbols
            .Select(s => s.GetAllTouchingNumericContainers(cellContainers))
            .Where(w => w.Count == 2)
            .Sum(s => s
                .Select(ss => ss.NumericValue)
                .Aggregate((a, b) => a * b));

        SecondSolution(result);
    }


    private static (
        HashSet<GraphPoint>,
        List<NumericCellContainer>) Parse(
            List<string> input,
            Func<char, bool> addSymbolFunc)
    {
        var cellContainers = new List<NumericCellContainer>();
        var symbols = new HashSet<GraphPoint>();

        foreach (var (line, y) in input.SelectWithIndex())
        {
            var cellContainer = new NumericCellContainer();

            foreach (var (cell, x) in line.SelectWithIndex())
            {
                if (char.IsNumber(cell))
                {
                    cellContainer.Cells.Add(new GraphPoint(x, y, cell));

                    if (x == line.Length - 1) // Last index, make sure to add
                    {
                        cellContainers.Add(cellContainer);
                    }

                    continue;
                }

                if (cellContainer.Cells.Any())
                {
                    cellContainers.Add(cellContainer);
                }

                cellContainer = new NumericCellContainer();

                if (cell == '.')
                {
                    continue;
                }

                if (addSymbolFunc(cell))
                {
                    symbols.Add(new GraphPoint(x, y, cell));
                }
            }
        }

        return (
            symbols,
            cellContainers);
    }
}