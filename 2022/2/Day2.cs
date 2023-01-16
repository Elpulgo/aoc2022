using System;
using System.IO;
using System.Linq;
using Aoc2022;

namespace Aoc2022.Aoc2022
{
    internal class Day2 : BaseDay
    {
        public Day2(bool shouldPrint) : base(2022, nameof(Day2), shouldPrint)
        {
        }

        public override void Execute()
        {
            StartTimerOne();
            StartTimerTwo();

            var input = ReadInput();

            var outcome = input.Select(s =>
            {
                var left = _map[s[0]];
                var right = _map[s[2]];

                var pointsSolutionOne = GetPoints(left, right) + right;

                var (lose, win) = _mapMoveWinOrLose[s[0]];

                // A bit backwards.. but we always think from our opponents eyes..
                var pointsSolutionTwo = s[2] switch
                {
                    'Y' => left + 3,
                    'X' => _map[win], // Need to lose, so chose which opponent will win on
                    'Z' => _map[lose] + 6 // Need to win, so chose which will win over opponent
                };

                return (pointsSolutionOne, pointsSolutionTwo);
            }).ToList();

            FirstSolution(outcome.Select(s => s.pointsSolutionOne).Sum().ToString());
            SecondSolution(outcome.Select(s => s.pointsSolutionTwo).Sum().ToString());
        }

        private readonly Dictionary<char, int> _map = new()
        {
            // Rock
            { 'A', 1 },
            { 'X', 1 },
            // Paper
            { 'B', 2 },
            { 'Y', 2 },
            // Scissors
            { 'C', 3 },
            { 'Z', 3 }
        };

        private readonly Dictionary<char, (char Lose, char Win)> _mapMoveWinOrLose = new()
        {
            { 'A', ('B', 'C') },
            { 'B', ('C', 'A') },
            { 'C', ('A', 'B') }
        };

        private static int GetPoints(int left, int right)
        {
            // Draw
            if (left == right)
                return 3;

            return left switch
            {
                // Lost, special case where rock beat scissors
                1 when right == 3 => 0,
                // Won, special case where rock beat scissors
                3 when right == 1 => 6,
                _ => left > right ? 0 : 6
            };
        }
    }
}