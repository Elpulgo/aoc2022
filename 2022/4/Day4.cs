using System;
using System.IO;
using System.Linq;
using Aoc2022;

namespace Aoc2022.Aoc2022
{
    internal class Day4 : BaseDay
    {
        public Day4(bool shouldPrint) : base(2022, nameof(Day4), shouldPrint)
        {
        }

        public override void Execute()
        {
            StartTimerOne();
            StartTimerTwo();

            var result = Input.Aggregate((PartOne: 0, PartTwo: 0), (total, s) =>
            {
                var pairs = s.Split(",");
                var firstSection = GetSections(pairs[0]);
                var secondSection = GetSections(pairs[1]);

                if (firstSection.IsSubsetOf(secondSection) || secondSection.IsSubsetOf(firstSection))
                {
                    ++total.PartOne;
                }

                if (firstSection.Overlaps(secondSection) || secondSection.Overlaps(firstSection))
                {
                    ++total.PartTwo;
                }

                return total;
            });

            FirstSolution(result.PartOne.ToString());
            SecondSolution(result.PartTwo.ToString());
        }

        private static HashSet<int> GetSections(string pair)
        {
            var interval = pair
                .Split("-")
                .Select(int.Parse)
                .ToArray();

            return Enumerable.Range(
                interval[0],
                interval[1] - interval[0] + 1
            ).ToHashSet();
        }
    }
}