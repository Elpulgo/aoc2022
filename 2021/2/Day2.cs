using System;
using System.IO;
using System.Linq;
using Aoc2022;

namespace Aoc2022.Aoc2021
{
    internal class Day2 : BaseDay
    {
        public Day2(bool shouldPrint) : base(2021, nameof(Day2), shouldPrint)
        {
        }

        public override void Execute()
        {
            var lines = ReadInput();

            var horizontal = 0;
            var depth = 0;

            foreach (var line in lines)
            {
                var output = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);

                var operation = output.First();
                var value = Convert.ToInt32(output.Last());

                switch (operation)
                {
                    case "forward":
                        horizontal += value;
                        break;
                    case "down":
                        depth += value;
                        break;
                    case "up":
                        depth -= value;
                        break;
                }
            }

            FirstSolution((horizontal * depth).ToString());

            horizontal = 0;
            depth = 0;
            var aim = 0;

            foreach (var line in lines)
            {
                var output = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);

                var operation = output.First();
                var value = Convert.ToInt32(output.Last());

                switch (operation)
                {
                    case "forward":
                        horizontal += value;
                        depth += aim * value;
                        break;
                    case "down":
                        aim += value;
                        break;
                    case "up":
                        aim -= value;
                        break;
                }
            }

            SecondSolution((horizontal * depth).ToString());
        }
    }
}