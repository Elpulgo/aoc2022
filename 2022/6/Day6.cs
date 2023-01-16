using System;
using System.IO;
using System.Linq;
using Aoc2022;
using Aoc2022.Algorithms;

namespace Aoc2022.Aoc2022
{
    internal class Day6 : BaseDay
    {
        public Day6(bool shouldPrint) : base(2022, nameof(Day6), shouldPrint)
        {
        }

        public override void Execute()
        {
            var input = ReadInput().Single().ToCharArray();

            StartTimerOne();
            FirstSolution(SolvePartThree(input, 4));

            StartTimerTwo();
            SecondSolution(SolvePartThree(input, 14));
        }

        private static string SolvePartThree(char[] input, int size)
        {
            var queue = new FixedSizeQueue<char>(size);

            for (var i = 0; i < input.Length; i++)
            {
                queue.Enqueue(input[i]);

                if (queue.Count < queue.Size)
                    continue;

                if (queue.Distinct().Count() != size)
                    continue;

                // Offset cause of array index
                i++;
                return i.ToString();
            }

            return input.Length.ToString();
        }
    }
}