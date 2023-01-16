using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Aoc2022;

namespace Aoc2022.Aoc2022
{
    internal class Day11 : BaseDay
    {
        public Day11(bool shouldPrint) : base(2022, nameof(Day11), shouldPrint)
        {
        }

        public override void Execute()
        {
            var input = ReadInput();
            
            StartTimerOne();
            FirstSolution(Solve(input, 20).ToString());
            
            StartTimerTwo();
            SecondSolution(Solve(input, 10000).ToString());
        }

        private long Solve(
            IEnumerable<string> input, 
            int rounds)
        {
            var monkeys = input.PartitionBasedOnEmptyLine()
                .Select(s => MonkeyBuilder.Build(s.ToList()))
                .ToDictionary(d => d.MonkeyNr, d => d);

            
            // Couldn't solve part 2, had to get help.. Something with prime numbers modulo to 
            // avoid overflow..
            Func<long, long> boredFunc = rounds == 20
                ? i => (int) Math.Floor((decimal) i / 3)
                : i => i % monkeys.Aggregate(1, (monkeyNr, monkey) => monkeyNr * monkey.Value.TestValue);

            var roundCount = 0;

            while (roundCount < rounds)
            {
                roundCount++;

                foreach (var change in monkeys.SelectMany(monkey => monkey.Value.TakeTurn(boredFunc)))
                {
                    monkeys[change.MonkeyNr].ItemThrownTo(change.Item);
                }
            }

            return monkeys
                .Select(s => s.Value.InspectCount)
                .OrderByDescending(o => o)
                .Take(2)
                .Aggregate((long)1, (a, b) => a * b);
        }
    }

    internal class Monkey
    {
        public int MonkeyNr { get; }
        public int TestValue { get; }
        public Queue<long> Items { get; } = new();
        private readonly Func<long, long> _operation;
        private readonly Func<long, bool> _testFunc;
        private readonly int _testTrueReceiver;
        private readonly int _testFalseReceiver;

        public long InspectCount { get; private set; } = 0;

        public Monkey(
            int monkeyNr,
            List<int> items,
            Func<long, long> operation,
            Func<long, bool> testFunc,
            int throwWhenTrue,
            int throwWhenFalse,
            int testValue)
        {
            MonkeyNr = monkeyNr;
            TestValue = testValue;
            foreach (var item in items)
            {
                Items.Enqueue(item);
            }

            _operation = operation;
            _testFunc = testFunc;
            _testTrueReceiver = throwWhenTrue;
            _testFalseReceiver = throwWhenFalse;
        }

        public List<(int MonkeyNr, long Item)> TakeTurn(
            Func<long, long> isBoredFunc)
        {
            var turnResult = new List<(int MonkeyNr, long Item)>();

            while (Items.TryDequeue(out long i))
            {
                var result = Inspect(i, isBoredFunc);
                turnResult.Add((result.MonkeyNr, result.Item));
            }

            return turnResult;
        }

        public void ItemThrownTo(long item) => Items.Enqueue(item);

        private (int MonkeyNr, long Item) Inspect(
            long item, 
            Func<long, long> isBoredFunc)
        {
            InspectCount++;
            var newWorryLevel = _operation(item);
            newWorryLevel = isBoredFunc(newWorryLevel);
            return _testFunc(newWorryLevel)
                ? (_testTrueReceiver, newWorryLevel)
                : (_testFalseReceiver, newWorryLevel);
        }

        private int IsBored(int value) =>  (int) Math.Floor((decimal) value / 3);
    }

    internal static class MonkeyBuilder
    {
        private static readonly Regex _operationRegex = new("Operation: new = old ([+|*]) (\\d+|[old]+)");
        private static readonly Regex _testRegex = new("Test: divisible by (\\d+)");

        private static Func<string, List<int>> _itemsFunc = s =>
            s.TrimStart()
                .Replace("Starting items: ", "")
                .Replace(" ", "")
                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToList();

        private static Func<string, int> _falseStatement = s =>
            int.Parse(s.TrimStart()
                .Replace("If false: throw to monkey ", ""));

        private static Func<string, int> _trueStatement = s =>
            int.Parse(s.TrimStart()
                .Replace("If true: throw to monkey ", ""));

        private static Func<string, int> _monkeyNr = s =>
            int.Parse(s.TrimStart().Replace("Monkey ", "").Replace(":", "").Trim());

        public static Monkey Build(List<string> input)
        {
            var monkeyNr = _monkeyNr(input[0]);
            var items = _itemsFunc(input[1]);
            var operation = OperationExpression(input[2]);
            var testValue = int.Parse(_testRegex.Match(input[3].TrimStart()).Groups[1].Value);
            Func<long, bool> test = i => i % testValue == 0;
            var monkeyThrowWhenTrue = _trueStatement(input[4]);
            var monkeyThrowWhenFalse = _falseStatement(input[5]);

            return new Monkey(
                monkeyNr,
                items,
                operation,
                test,
                monkeyThrowWhenTrue,
                monkeyThrowWhenFalse,
                testValue);
        }

        public static Func<long, long> OperationExpression(string input)
        {
            var matches = _operationRegex.Match(input.TrimStart()).Groups.Values.Select(s => s.Value).Skip(1).ToList();

            return (matches[0], matches[1]) switch
            {
                ("*", var value) when int.TryParse(value, out var _) => o => o * int.Parse(value),
                ("*", "old") => o => o * o,
                ("+", var value) when int.TryParse(value, out var _) => o => o + int.Parse(value),
                ("+", "old") => o => o + o,
                _ => throw new ArgumentException("Combination not supported")
            };
        }
    }
}