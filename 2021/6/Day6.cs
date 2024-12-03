using System;
using System.IO;
using System.Linq;
using Aoc2022;

namespace Aoc2022.Aoc2021
{
    internal class Day6 : BaseDay
    {
        public Day6(bool shouldPrint) : base(2021, nameof(Day6), shouldPrint)
        {
        }

        public override void Execute()
        {
            Console.WriteLine("Day 6:");

            FirstSolution(Execute(80).ToString());
            SecondSolution(Execute(256).ToString());
        }

        private ulong Execute(int days)
        {
            var initialLanternFish = ReadInputRaw(true)
                .Split(",")
                .Select(s => Convert.ToInt32(s))
                .ToList();

            var ages = new ulong[9];

            foreach (var fish in initialLanternFish)
            {
                ages[fish]++;
            }

            foreach (var day in Enumerable.Range(0, days))
            {
                var spawnFish = ages[0];
                var resetFish = ages[0];

                for (var index = 0; index < ages.Length - 1; index++)
                {
                    ages[index] = ages[index + 1];
                }

                ages[8] = spawnFish;
                ages[6] += resetFish;
            }

            ulong spawned = 0;

            for (var i = 0; i <= 8; i++)
            {
                spawned += ages[i];
            }

            return spawned;
        }
    }
}