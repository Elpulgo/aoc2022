using Aoc2022.Aoc2022;

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

        var aData = new List<int>();
        var bData = new List<int>();
        
        foreach (var rowData in input.Select(row => row
                     .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                     .Select(s => s.ParseSingleInt())
                     .ToArray()))
        {
            aData.Add(rowData[0]);
            bData.Add(rowData[1]);
        }

        var aDataOrdered = aData.OrderBy(o => o).ToArray();
        var bDataOrdered = bData.OrderBy(o => o).ToArray();

        var sum = 0;
        foreach (var (a, index) in aDataOrdered.Select((s,i) => (s,i)))
        {
            var b = bDataOrdered[index];
            
            sum += Math.Abs(a - b);
        }
        
        FirstSolution(sum);
    }


    private void PartTwo()
    {
        var input = ReadInput(false, partTwo: true).ToList();
        
        var aData = new List<int>();
        var bData = new List<int>();
        
        foreach (var rowData in input.Select(row => row
                     .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                     .Select(s => s.ParseSingleInt())
                     .ToArray()))
        {
            aData.Add(rowData[0]);
            bData.Add(rowData[1]);
        }

        var bDataGrouped = bData
            .GroupBy(g => g)
            .ToDictionary(d => d.Key, v => v.Count());
        
        var sum = aData.Sum(a =>
        {
            bDataGrouped.TryGetValue(a, out var bSimilarity);
            return Math.Abs(a * bSimilarity);
        });
        
        SecondSolution(sum);
    }
}