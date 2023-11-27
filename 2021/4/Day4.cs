using System;
using System.IO;
using System.Linq;
using Aoc2022;

namespace Aoc2022.Aoc2021
{
    internal class Day4 : BaseDay
    {
        public Day4(bool shouldPrint) : base(2021, nameof(Day4), shouldPrint)
        {
        }

        public override void Execute()
        {
            Console.WriteLine("Day 4:");

            var input = ReadInput().ToList();

            var bingoSequence = input
                .First()
                .Split(",")
                .Select(s => Convert.ToInt32(s))
                .ToList();

            var bricks = new List<Brick>();

            Brick brick = new();

            foreach (var line in input.Skip(1))
            {
                if (string.IsNullOrEmpty(line))
                    continue;

                brick.AddRow(line.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(s => Convert.ToInt32(s))
                    .ToList());

                if (brick.IsAllRowsSet)
                {
                    brick.ComputeBingo(bingoSequence);
                    bricks.Add(brick);
                    brick = new();
                }
            }

            var winningBingoBrick = bricks
                .Where(w => w.BingoData.Bingo)
                .OrderBy(o => o.BingoData.Count)
                .FirstOrDefault();

            if (winningBingoBrick == null)
                throw new Exception("No brick won the bingo!");

            FirstSolution(winningBingoBrick.ComputeWinningData().ToString());

            var lastBingoBrick = bricks
                .Where(w => w.BingoData.Bingo)
                .OrderBy(o => o.BingoData.Count)
                .LastOrDefault();

            SecondSolution(lastBingoBrick.ComputeWinningData().ToString());
        }
    }

    internal class Brick
    {
        public Dictionary<int, Dictionary<int, Cell>> Rows { get; set; } = new();

        public Dictionary<int, Dictionary<int, Cell>> Columns
        {
            get
            {
                var columns = new Dictionary<int, Dictionary<int, Cell>>();
                foreach (var item in Rows)
                {
                    columns.Add(
                        item.Key,
                        Rows
                            .SelectMany(s => s.Value)
                            .Where(w => w.Key == item.Key)
                            .Select((value, index) => (index, value))
                            .ToDictionary(d => d.index, d => d.value.Value)
                    );
                }

                return columns;
            }
        }

        public bool IsAllRowsSet => Rows.Count == 5;

        public (bool Bingo, int Count, int WinningNumber) BingoData { get; private set; } = (false, 0, 0);

        public void ComputeBingo(List<int> bingoNumbers)
        {
            var count = 0;
            foreach (var bingoNumber in bingoNumbers)
            {
                var match = Rows
                    .SelectMany(s => s.Value)
                    .Where(w => w.Value.Value == bingoNumber)
                    .SingleOrDefault();

                if (match.Value != null)
                {
                    match.Value.Marked = true;
                }

                if (IsBingo())
                {
                    BingoData = (true, count, bingoNumber);
                    return;
                }

                count++;
            }
        }

        public int ComputeWinningData()
        {
            var sum = Rows
                .SelectMany(s => s.Value)
                .Where(w => !w.Value.Marked)
                .Select(s => s.Value.Value)
                .Sum();

            return sum * BingoData.WinningNumber;
        }

        private bool IsBingo()
        {
            var anyRowsBingo = Rows.Any(a => a.Value.Values.All(a => a.Marked));
            if (anyRowsBingo)
                return true;

            var anyColumnsBingo = Columns.Any(a => a.Value.Values.All(a => a.Marked));
            if (anyColumnsBingo)
                return true;

            return false;
        }

        public void AddRow(List<int> row)
        {
            var index = Rows.Count;
            var innerLookup = new Dictionary<int, Cell>();

            var count = 0;
            foreach (var value in row)
            {
                innerLookup.Add(count, new Cell { Value = value, Marked = false });
                count++;
            }

            Rows.Add(index, innerLookup);
        }
    }

    internal class Cell
    {
        public int Value { get; set; }
        public bool Marked { get; set; }
    }
}