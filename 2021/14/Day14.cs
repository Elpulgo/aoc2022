using System;
using System.IO;
using System.Linq;
using Aoc2022;

namespace Aoc2022.Aoc2021
{
    internal class Day14 : BaseDay
    {
        public Day14(bool shouldPrint) : base(2021, nameof(Day14), shouldPrint)
        {
        }

        private Dictionary<string, char> _operations = new();
        private Dictionary<char, int> _count = new();
        private Queue<(char First, char Second, int Increment)> _queue = new();

        public override void Execute()
        {
            var input = ReadInput();
            var template = input.First();
            base.StartTimerOne();

            _operations = input.Skip(1).Where(w => w != string.Empty)
                .Select(s => s.Split("->", StringSplitOptions.TrimEntries))
                .ToDictionary(k => k[0].Trim(), v => char.Parse(v[1]));

            var chars = template.ToCharArray();

            for (var i = 0; i < 10; i++)
                chars = Step(chars).ToArray();

            base.StopTimerOne();

            var group = chars
                .GroupBy(g => g)
                .OrderByDescending(o => o.Count());

            FirstSolution((group.First().Count() - group.Last().Count()).ToString());

            // Solution 2 - had to get hints at other solutions, couldn't compute xD
            base.StartTimerTwo();

            var charCount = template
                .ToCharArray()
                .GroupBy(g => g)
                .ToDictionary(k => k.Key, v => (long)v.Count());

            var pairs = Enumerable.Range(0, template.Length - 1)
                .Select((c => template[c..(c + 2)]))
                .GroupBy(g => g)
                .ToDictionary(k => k.Key, v => (long)v.Count());

            var steps = 0;
            while (steps != 40)
            {
                var newPairs = new Dictionary<string, long>();

                foreach (var pair in pairs)
                {
                    var newChar = _operations[pair.Key];
                    var newPair1 = $"{pair.Key[0]}{newChar}";
                    var newPair2 = $"{newChar}{pair.Key[1]}";

                    if (!charCount.ContainsKey(newChar))
                        charCount.Add(newChar, 1);
                    else
                        charCount[newChar] += pair.Value;


                    if (newPairs.ContainsKey(newPair1))
                        newPairs[newPair1] += pair.Value;
                    else
                        newPairs.Add(newPair1, pair.Value);


                    if (newPairs.ContainsKey(newPair2))
                        newPairs[newPair2] += pair.Value;
                    else
                        newPairs.Add(newPair2, pair.Value);
                }

                pairs = new Dictionary<string, long>(newPairs);
                steps++;
            }

            base.StopTimerTwo();
            SecondSolution((charCount.Max(m => m.Value) - charCount.Min(m => m.Value)).ToString());
        }

        private IEnumerable<char> Step(char[] chars)
        {
            for (int i = 0; i < chars.Length - 1; i++)
            {
                var first = chars[i];
                var last = chars[i + 1];

                yield return first;
                yield return _operations[$"{chars[i]}{chars[i + 1]}"];

                if (i == chars.Length - 2)
                    yield return last;
            }
        }
    }
}