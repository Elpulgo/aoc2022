using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using Aoc2022;
using Microsoft.VisualBasic.FileIO;

namespace Aoc2022.Aoc2022
{
    internal class Day5 : BaseDay
    {
        private readonly Regex _regex = new(".?(\\d+)", RegexOptions.Compiled);
        private Dictionary<int, Stack<char>> _stacks;

        public Day5(bool shouldPrint) : base(2022, nameof(Day5), shouldPrint)
        {
        }

        public override void Execute()
        {
            StartTimerOne();
            InnerExecute(MoveInStackFashion);
            var resultPartOne = new string(_stacks.Select(s => s.Value.Peek()).ToArray());
            FirstSolution(resultPartOne);

            StartTimerTwo();
            InnerExecute(MoveInBulkFashion);
            var resultPartTwo = new string(_stacks.Select(s => s.Value.Peek()).ToArray());
            SecondSolution(resultPartTwo);

            void InnerExecute(Action<int, int, int> strategy)
            {
                Setup();

                Input.ToList().ForEach(s =>
                {
                    if (!s.StartsWith("move"))
                        return;

                    var regexMatch = _regex.Matches(s);
                    var numberToMove = int.Parse(regexMatch[0].Value);
                    var idxFrom = int.Parse(regexMatch[1].Value);
                    var idxTo = int.Parse(regexMatch[2].Value);

                    strategy(numberToMove, idxFrom, idxTo);
                });
            }
        }

        private void MoveInStackFashion(
            int numberToMove,
            int idxFrom,
            int idxTo)
        {
            for (var i = 0; i < numberToMove; i++)
            {
                var crate = _stacks[idxFrom].Pop();
                _stacks[idxTo].Push(crate);
            }
        }

        private void MoveInBulkFashion(
            int numberToMove,
            int idxFrom,
            int idxTo)
        {
            var moveAll = new List<char>();
            for (var i = 0; i < numberToMove; i++)
            {
                var crate = _stacks[idxFrom].Pop();
                moveAll.Add(crate);
            }

            moveAll.Reverse();
            foreach (var crate in moveAll)
            {
                _stacks[idxTo].Push(crate);
            }
        }

        private void Setup()
        {
            _stacks = new Dictionary<int, Stack<char>>();
            // Index for the chars, e.g DCM has index 5
            var cratePositions = new Dictionary<int, List<char>>();
            // Index for the crates, e.g. 1 -> 1, 2 -> 5 etc.
            var crateIndexPositions = new Dictionary<int, int>();

            // Ugly solution where we only can populate after looping through everything
            // since it comes at last

            foreach (var line in Input)
            {
                if (line.StartsWith("move"))
                    break;

                foreach (var (c, index) in line.Select((s, i) => (s, i)))
                {
                    if (char.IsNumber(c))
                    {
                        var stackNumber = int.Parse(c.ToString());
                        crateIndexPositions.Add(stackNumber, index);
                        _stacks.Add(stackNumber, new Stack<char>());
                        continue;
                    }

                    if (char.IsLetter(c))
                    {
                        if (cratePositions.ContainsKey(index))
                            cratePositions[index].Add(c);
                        else
                            cratePositions.Add(index, new List<char> { c });
                    }
                }
            }

            foreach (var stack in _stacks)
            {
                // Find corresponding char index for chars (D,C,M) and which crate index they belong to
                // Then add them to the stack.
                if (cratePositions.ContainsKey(crateIndexPositions[stack.Key]))
                {
                    var crateIndex = crateIndexPositions[stack.Key];
                    cratePositions[crateIndex].Reverse();
                    _stacks[stack.Key] = new Stack<char>(cratePositions[crateIndex]);
                }
            }
        }
    }
}