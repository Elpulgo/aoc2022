using System;
using System.IO;
using System.Linq;
using Aoc2022;

namespace Aoc2022.Aoc2022
{
    internal class Day10 : BaseDay
    {
        public Day10(bool shouldPrint) : base(2022, nameof(Day10), shouldPrint)
        {
        }

        public override void Execute()
        {
            var input = ReadInput();

            StartTimerOne();
            FirstSolution(SolveOne(input).ToString());

            StartTimerTwo();
            SolveTwo(input); // Answer is RBPARAGF
        }

        private void SolveTwo(IEnumerable<string> input)
        {
            var rowIndex = 0;
            var spritePositions = new List<int>() { 0, 1, 2 };

            var lines = new List<string>();

            var registerXValue = 1;
            var line = string.Empty;

            foreach (var operation in input)
            {
                var operationValue = operation.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                var cycles = Cycles(operationValue[0]);

                // Start of cycle 1
                Cycle();

                if (cycles == 1)
                    continue;

                // Start of cycle 2
                Cycle();

                registerXValue += Operation(operationValue[1]);

                spritePositions = new List<int> { registerXValue - 1, registerXValue, registerXValue + 1 };
                // End of cycle 2
            }

            foreach (var l in lines)
            {
                Console.WriteLine(l);
            }

            void Cycle()
            {
                line += spritePositions.Contains(rowIndex)
                    ? "#"
                    : ".";

                rowIndex++;
                if (rowIndex == 40)
                {
                    lines.Add(line);
                    line = string.Empty;
                    rowIndex = 0;
                }
            }
        }

        private int SolveOne(IEnumerable<string> input)
        {
            var cyclesIndex = new List<int>
            {
                20,
                60,
                100,
                140,
                180,
                220
            };

            var result = new List<int>();

            var registerXValue = 1;
            var cyclesCount = 0;

            foreach (var operation in input)
            {
                var operationValue = operation.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                var cycles = Cycles(operationValue[0]);

                // Start of cycle 1
                cyclesCount++;
                AddRegisterValueIfCorrectCycle(cyclesCount);

                if (cycles == 1)
                    continue;

                // Start of cycle 2
                cyclesCount++;
                AddRegisterValueIfCorrectCycle(cyclesCount);

                registerXValue += Operation(operationValue[1]);
                // End of cycle 2
            }

            void AddRegisterValueIfCorrectCycle(int cycle)
            {
                if (cyclesIndex.Contains(cycle))
                {
                    result.Add(cyclesCount * registerXValue);
                }
            }

            return result.Sum();
        }

        private int Cycles(string input) => input == "noop" ? 1 : 2;
        private int Operation(string input) => int.Parse(input);
    }
}