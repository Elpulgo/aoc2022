using System.Text.RegularExpressions;

namespace Aoc2022.Aoc2024;

internal class Day4 : BaseDay
{
    private Regex XmasRegex = new(@"X.*M.*A.*S", RegexOptions.Compiled);
    private Regex XmasRegexReverse = new(@"S.*A.*M.*X", RegexOptions.Compiled);

    private HashSet<char> WhiteList = new()
    {
        'X',
        'M',
        'A',
        'S'
    };

    public Day4(bool shouldPrint) : base(2024, nameof(Day4), shouldPrint)
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

        var max = input.Count - 1;

        var columns = new Dictionary<int, List<char>>();
        var diagonalLeftToRight = LeftToRight(max, input);
        var diagonalRightToLeft = RightToLeft(max, input);

        foreach (var t in input)
        {
            var row = t.ToCharArray();
            for (int x = 0; x < row.Length; x++)
            {
                if (WhiteList.Contains(row[x]))
                {
                    if (columns.ContainsKey(x))
                    {
                        columns[x].Add(row[x]);
                    }
                    else
                    {
                        columns[x] = new List<char>
                        {
                            row[x]
                        };
                    }
                }
            }
        }

        var sum = 0;

        foreach (var row in input)
        {
            var target = new string(row.Where(w => WhiteList.Contains(w)).ToArray());
            sum += SumRegexes(target);
        }

        foreach (var col in columns)
        {
            var target = new string(col.Value.ToArray());
            sum += SumRegexes(target);
        }

        foreach (var diaLeftToRight in diagonalLeftToRight)
        {
            sum += SumRegexes(diaLeftToRight);
        }

        foreach (var diaRightToLeft in diagonalRightToLeft)
        {
            sum += SumRegexes(diaRightToLeft);
        }

        FirstSolution(sum);
    }

    private int SumRegexes(string input)
    {
        var sum = 0;
        for (Match match = XmasRegex.Match(input); match.Success; match = XmasRegex.Match(input, match.Index + 1))
        {
            sum++;
        }
        
        for (Match match = XmasRegexReverse.Match(input); match.Success; match = XmasRegexReverse.Match(input, match.Index + 1))
        {
            sum++;
        }

        return sum;
    }

    private List<string> RightToLeft(int max, List<string> input)
    {
        var diagonalRightToLeft = new List<string>();
        
        // RIGHT TO LEFT RIGHT LEFT
        for (int i = max; i >= 0; i--)
        {
            var diagonalRightToLeftX = i;
            var diagonalRightToLeftY = 0;
            
            var rightToLeft = new List<char>();
            while (diagonalRightToLeftX >= 0 && diagonalRightToLeftY <= max)
            {
                var target = input[diagonalRightToLeftY].ToCharArray()[diagonalRightToLeftX];
                if (WhiteList.Contains(target))
                {
                    rightToLeft.Add(target);
                }

                diagonalRightToLeftY++;
                diagonalRightToLeftX--;
            }

            if (i == max)
            {
                diagonalRightToLeft.Add(new string(rightToLeft.ToArray()));
                continue;
            }
            
            diagonalRightToLeftX = 0;
            diagonalRightToLeftY = i;
            
            diagonalRightToLeft.Add(new string(rightToLeft.ToArray()));

            rightToLeft = new List<char>();
            while (diagonalRightToLeftX >= 0 && diagonalRightToLeftY <= max)
            {
                var target = input[diagonalRightToLeftY].ToCharArray()[diagonalRightToLeftX];
                if (WhiteList.Contains(target))
                {
                    rightToLeft.Add(target);
                }

                diagonalRightToLeftY++;
                diagonalRightToLeftX--;
            }

            diagonalRightToLeft.Add(new string(rightToLeft.ToArray()));
        }

        return diagonalRightToLeft;
    }

    private List<string> LeftToRight(int max, List<string> input)
    {
        var diagonalLeftToRight = new List<string>();

        // LEFT TO RIGHT LEFT TO RIGHT
        for (int i = 0; i <= max; i++)
        {
            var diagonalLeftToRightX = i;
            var diagonalLeftToRightY = 0;

            var leftToRight = new List<char>();
            while (diagonalLeftToRightX <= max && diagonalLeftToRightY <= max)
            {
                var target = input[diagonalLeftToRightY].ToCharArray()[diagonalLeftToRightX];
                if (WhiteList.Contains(target))
                {
                    leftToRight.Add(target);
                }

                diagonalLeftToRightY++;
                diagonalLeftToRightX++;
            }

            if (i == 0)
            {
                diagonalLeftToRight.Add(new string(leftToRight.ToArray()));
                continue;
            }
            
            diagonalLeftToRightX = 0;
            diagonalLeftToRightY = i;
            
            diagonalLeftToRight.Add(new string(leftToRight.ToArray()));

            leftToRight = new List<char>();
            while (diagonalLeftToRightX <= max && diagonalLeftToRightY <= max)
            {
                var target = input[diagonalLeftToRightY].ToCharArray()[diagonalLeftToRightX];
                if (WhiteList.Contains(target))
                {
                    leftToRight.Add(target);
                }

                diagonalLeftToRightY++;
                diagonalLeftToRightX++;
            }

            diagonalLeftToRight.Add(new string(leftToRight.ToArray()));
        }

        return diagonalLeftToRight;
    }


    private void PartTwo()
    {
        var input = ReadInput(false, partTwo: true).ToList();
    }
}