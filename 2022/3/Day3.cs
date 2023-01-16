using System;
using System.IO;
using System.Linq;
using Aoc2022;

namespace Aoc2022.Aoc2022
{
    internal class Day3 : BaseDay
    {
        public Day3(bool shouldPrint) : base(2022, nameof(Day3), shouldPrint)
        {
        }

        public override void Execute()
        {
            var input = ReadInput();
            First(input);
            Second(input);
        }

        private void First(IEnumerable<string> input)
        {
            StartTimerOne();

            var result = input.Select(s =>
            {
                var containers = s.ToCharArray().SplitInTwo();
                var commonChar = containers.IntersectAoC().Single();
                return GetCharIndex(commonChar);
            }).Sum();

            FirstSolution(result.ToString());
        }

        private void Second(IEnumerable<string> input)
        {
            StartTimerTwo();

            var result = input.Partition(3).Select(s =>
            {
                var commonChar = s.IntersectAoC().Single();
                return GetCharIndex(commonChar);
            }).Sum();

            SecondSolution(result.ToString());
        }

        private static int GetCharIndex(char c)
        {
            var index = c.GetAlphabeticIndexFromChar();
            return char.IsLower(c)
                ? index
                : index + 26;
        }
    }
}