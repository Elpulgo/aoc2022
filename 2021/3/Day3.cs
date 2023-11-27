using System;
using System.IO;
using System.Linq;
using Aoc2022;

namespace Aoc2022.Aoc2021
{
    internal class Day3 : BaseDay
    {
        public Day3(bool shouldPrint) : base(2021, nameof(Day3), shouldPrint)
        {
        }

        public override void Execute()
        {
            var input = ReadInput()
                .Select(s => Array
                    .ConvertAll(s.Trim().ToCharArray(), c => c - '0')
                    .ToList()
                )
                .ToList();

            var digitCount = input.First().Count;

            var (first, second) = BothParts(input, digitCount);
            
            FirstSolution(first.ToString());
            SecondSolution(second.ToString());
        }

        private static (int Part1, int Part2) BothParts(List<List<int>> input, int digitCount)
        {
            // Part 1
            var lookUp = new Dictionary<int, BitLookup>();

            // Part 2
            var lookUpOxy = new Dictionary<int, BitLookup>();
            var lookUpCo2 = new Dictionary<int, BitLookup>();

            var oxyHit = new List<int>();
            var co2Hit = new List<int>();

            var shouldOxyBreak = false;
            var shouldCo2Break = false;


            var remainingOxyRows = Enumerable.Range(0, input.Count).ToHashSet();
            var remainingCo2Rows = Enumerable.Range(0, input.Count).ToHashSet();

            foreach (var index in Enumerable.Range(0, digitCount))
            {
                // Part 1
                lookUp[index] = new BitLookup();

                if (remainingOxyRows.Count == 1)
                {
                    oxyHit = input[remainingOxyRows.Single()];
                    shouldOxyBreak = true;
                }

                if (remainingCo2Rows.Count == 1)
                {
                    co2Hit = input[remainingCo2Rows.Single()];
                    shouldCo2Break = true;
                }

                // This guard won't work with part 1, but solves the puzzle.. since it is never hit
                if (shouldCo2Break && shouldOxyBreak)
                    break;

                lookUpOxy[index] = new BitLookup();
                lookUpCo2[index] = new BitLookup();

                var row = 0;
                foreach (var line in input)
                {
                    // Increment Part 1
                    lookUp[index].Increment(line[index]);

                    // Increment Part 2
                    if (remainingOxyRows.Contains(row) && !shouldOxyBreak)
                    {
                        lookUpOxy[index].IncrementPart2(line[index], row);
                    }

                    if (remainingCo2Rows.Contains(row) && !shouldCo2Break)
                    {
                        lookUpCo2[index].IncrementPart2(line[index], row);
                    }

                    row++;
                }

                if (!shouldOxyBreak)
                {
                    remainingOxyRows = lookUpOxy[index].MostCommonRows().ToHashSet();
                }

                if (!shouldCo2Break)
                {
                    remainingCo2Rows = lookUpCo2[index].LeastCommonRows().ToHashSet();
                }
            }

            // Print result Part 1

            var gam = string.Join("", lookUp.Select(s => s.Value.MostCommon));
            var eps = string.Join("", lookUp.Select(s => s.Value.LeastCommon));

            var gamma = Convert.ToInt32(string.Join("", lookUp.Select(s => s.Value.MostCommon)), 2);
            var epsilon = Convert.ToInt32(string.Join("", lookUp.Select(s => s.Value.LeastCommon)), 2);

            var result2 = gamma * epsilon;
            Console.WriteLine($"Part 1: {result2}");


            // Print result Part 2

            if (remainingOxyRows.Count != 1 || remainingCo2Rows.Count != 1)
                throw new Exception("Both need to have hit a single row in order to complete assignment");

            var oxyDecimal = Convert.ToInt32(string.Join("", input[remainingOxyRows.Single()].Select(s => s)), 2);
            var co2Decimal = Convert.ToInt32(string.Join("", input[remainingCo2Rows.Single()].Select(s => s)), 2);

            var result = oxyDecimal * co2Decimal;
            Console.WriteLine($"Part 2: {result}");

            return (result2, result);
        }
    }

    internal class BitLookup
    {
        internal int IsTrue { get; set; } = 0;
        internal int IsFalse { get; set; } = 0;

        internal int MostCommon => IsTrue > IsFalse
            ? 1
            : 0;

        internal int LeastCommon => Convert.ToInt32(!Convert.ToBoolean(MostCommon));

        internal List<int> TrueIndex = new();
        internal List<int> FalseIndex = new();

        internal List<int> MostCommonRows()
        {
            if (IsTrue == IsFalse)
                return TrueIndex;

            return IsTrue > IsFalse
                ? TrueIndex
                : FalseIndex;
        }

        internal List<int> LeastCommonRows()
        {
            if (IsTrue == IsFalse)
                return FalseIndex;

            return IsTrue < IsFalse
                ? TrueIndex
                : FalseIndex;
        }


        internal void Increment(int value)
        {
            if (value == 1)
            {
                IsTrue++;
                return;
            }

            IsFalse++;
        }

        internal void IncrementPart2(int value, int row)
        {
            if (value == 1)
            {
                IsTrue++;
                TrueIndex.Add(row);
                return;
            }

            IsFalse++;
            FalseIndex.Add(row);
        }
    }
}