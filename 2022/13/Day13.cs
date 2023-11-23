using System.Collections;
using System.Text.Json.Nodes;

namespace Aoc2022.Aoc2022
{
    internal class Day13 : BaseDay
    {
        public Day13(bool shouldPrint) : base(2022, nameof(Day13), shouldPrint)
        {
        }

        public override void Execute()
        {
            var input = ReadInputRaw()
                .Split("\r\n\r\n", StringSplitOptions.RemoveEmptyEntries)
                .Select((s, i) => (s.Split("\r\n", StringSplitOptions.RemoveEmptyEntries), i))
                .ToArray();

            StartTimerOne();
            FirstSolution(SolveOne(input).ToString());

            StartTimerTwo();
            SecondSolution(SolveTwo(input).ToString());
        }

        private static int SolveOne((string[], int i)[] input)
        {
            var correct = new HashSet<int>();

            foreach (var (pairs, index) in input)
            {
                var left = new DataRow(pairs[0]);
                var right = new DataRow(pairs[1]);

                var outcome = left.CompareTo(right);

                switch (outcome)
                {
                    case 1:
                        correct.Add(index + 1);
                        continue;
                    case -1:
                        continue;
                    case 0:
                        throw new Exception("Should not be same?");
                }
            }

            return correct.Sum();
        }

        private int SolveTwo((string[], int i)[] input)
        {
            var allRows = new List<DataRow>
            {
                new("[[2]]"),
                new("[[6]]")
            };

            foreach (var (pairs, index) in input)
            {
                allRows.Add(new DataRow(pairs[0]));
                allRows.Add(new DataRow(pairs[1]));
            }
            
            // var sorted = allRows.Sort(DataRow.SortByRules());

            throw new NotImplementedException();
        }
    }

    public enum Outcome
    {
        Inconclusive,
        Correct,
        Incorrect,
        Same
    }

    public class DataRow : IComparable
    {
        public string Input { get; }
        public JsonNode Node { get; }

        public DataRow(string input)
        {
            Input = input;
            Node = JsonArray.Parse(input);
        }

        private class SortByRulesHelper : IComparer
        {
            public int Compare(object? a, object? b)
            {
                var left = (JsonNode) a;
                var right = (JsonNode) b;
                var max = new List<int>
                {
                    left.AsArray().Count,
                    right.AsArray().Count
                }.Max();

                for (var j = 0; j < max; j++)
                {
                    if (IsOutsideBounds(left.AsArray(), j))
                        return 1;

                    if (IsOutsideBounds(right.AsArray(), j))
                        return -1;

                    var outcome = Evaluate(left[j], right[j]);
                    switch (outcome)
                    {
                        case Outcome.Same:
                            continue;
                        case Outcome.Correct:
                            return 1;
                        case Outcome.Incorrect:
                            return -1;
                        case Outcome.Inconclusive:
                            throw new Exception("Inconclusive");
                        default:
                            throw new Exception("Should not happen..");
                    }
                }

                return 0;
            }
        }

        public int CompareTo(object? obj)
        {
            var other = obj as DataRow;
            var sortHelper = new SortByRulesHelper();
            return sortHelper.Compare(Node, other.Node);
        }

        public static IComparer SortByRules() => new SortByRulesHelper();

        private static Outcome Evaluate(JsonNode left, JsonNode right)
        {
            var leftIsArray = left is JsonArray;
            var rightIsArray = right is JsonArray;

            var leftIsValue = left is JsonValue;
            var rightIsValue = right is JsonValue;

            if (leftIsValue && rightIsValue)
            {
                // Check the diff and return outcome
                var leftValue = left.AsValue().GetValue<int>();
                var rightValue = right.AsValue().GetValue<int>();

                if (leftValue.Equals(rightValue))
                    return Outcome.Same;

                return leftValue < rightValue
                    ? Outcome.Correct
                    : Outcome.Incorrect;
            }

            if (leftIsArray && rightIsArray)
            {
                var leftArr = left.AsArray();
                var rightArr = right.AsArray();
                var max = GetMax(leftArr, rightArr);

                for (var i = 0; i < max; i++)
                {
                    if (IsOutsideBounds(leftArr, i))
                    {
                        return Outcome.Correct;
                    }

                    if (IsOutsideBounds(rightArr, i))
                    {
                        return Outcome.Incorrect;
                    }

                    var outcome = Evaluate(leftArr[i], rightArr[i]);

                    if (outcome == Outcome.Same)
                        continue;

                    return outcome;
                }

                // All elements were correct, since we looped through everything
                return Outcome.Correct;
            }

            if (leftIsArray && rightIsValue)
            {
                return Evaluate(left, new JsonArray
                {
                    right.AsValue().GetValue<int>()
                });
            }

            if (rightIsArray && leftIsValue)
            {
                return Evaluate(new JsonArray
                {
                    left.AsValue().GetValue<int>()
                }, right);
            }

            return Outcome.Inconclusive;
        }

        private static bool IsOutsideBounds(JsonArray arr, int i) => arr.Count <= i;

        private static int GetMax(JsonArray left, JsonArray right)
        {
            return new List<int>
            {
                left.Count,
                right.Count
            }.Max();
        }
    }
}