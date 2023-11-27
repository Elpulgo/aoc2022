using System;
using System.IO;
using System.Linq;
using Aoc2022;

namespace Aoc2022.Aoc2021
{
    internal class Day8 : BaseDay
    {
        public Day8(bool shouldPrint) : base(2021, nameof(Day8), shouldPrint)
        {
        }

        public override void Execute()
        {
            Console.WriteLine("Day 8:");
            var input = ReadInput();

            var inputPart1 = input
                .Select(s => s.Split("|", StringSplitOptions.RemoveEmptyEntries).Last())
                .SelectMany(s => s.Split(" ", StringSplitOptions.RemoveEmptyEntries));

            FirstSolution(inputPart1.Count(c => new int[4] { 2, 3, 4, 7 }.Contains(c.Count())).ToString());

            var inputPart2 = input.Select(s => new Segment(s).Compute()).Sum();

            SecondSolution(inputPart2.ToString());
        }
    }

    public class Segment
    {
        public Segment(string line)
        {
            Patterns = line.Split("|")[0].Split(" ", StringSplitOptions.RemoveEmptyEntries);
            Digits = line.Split("|")[1].Split(" ", StringSplitOptions.RemoveEmptyEntries);
        }

        public string[] Digits { get; set; }
        public string[] Patterns { get; set; }

        public List<string> FiveTwoThree => Patterns.Where(w => w.Length == 5).ToList();
        public List<string> SixNineZero => Patterns.Where(w => w.Length == 6).ToList();


        public int Compute()
        {
            Dictionary<int, (char[] Chars, int Bit)> segmentDecoder = new();

            var one = Patterns.SingleOrDefault(s => s.Length == 2).ToCharArray();
            var four = Patterns.SingleOrDefault(s => s.Length == 4).ToCharArray();
            var seven = Patterns.SingleOrDefault(s => s.Length == 3).ToCharArray();
            var eight = Patterns.SingleOrDefault(s => s.Length == 7).ToCharArray();

            segmentDecoder.Add(1, (one, one.MapToBitSum()));
            segmentDecoder.Add(4, (four, four.MapToBitSum()));
            segmentDecoder.Add(7, (seven, seven.MapToBitSum()));
            segmentDecoder.Add(8, (eight, eight.MapToBitSum()));

            foreach (var item in FiveTwoThree)
            {
                if (!segmentDecoder[1].Chars.Except(item).Any())
                {
                    segmentDecoder.Add(3, (item.ToCharArray(), item.ToCharArray().MapToBitSum()));
                    continue;
                }

                if (segmentDecoder[4].Chars.Except(item).Count() > 1)
                {
                    segmentDecoder.Add(2, (item.ToCharArray(), item.ToCharArray().MapToBitSum()));
                    continue;
                }

                segmentDecoder.Add(5, (item.ToCharArray(), item.ToCharArray().MapToBitSum()));
            }

            foreach (var item in SixNineZero)
            {
                if (segmentDecoder[7].Chars.Except(item).Count() == 1)
                {
                    segmentDecoder.Add(6, (item.ToCharArray(), item.ToCharArray().MapToBitSum()));
                    continue;
                }

                if (!segmentDecoder[4].Chars.Except(item).Any())
                {
                    segmentDecoder.Add(9, (item.ToCharArray(), item.ToCharArray().MapToBitSum()));
                    continue;
                }

                segmentDecoder.Add(0, (item.ToCharArray(), item.ToCharArray().MapToBitSum()));
            }

            var decodedDigits = new List<int>();

            foreach (var digitSum in Digits.Select(s => s.ToCharArray().MapToBitSum()))
            {
                decodedDigits.Add(segmentDecoder.Single(s => s.Value.Bit == digitSum).Key);
            }

            return Convert.ToInt32(string.Join("", decodedDigits.Select(s => s)));
        }
    }

    internal static class BitHelper
    {
        internal static int MapToBitSum(this char[] array)
        {
            return array.Select(s => Bits[s]).Sum();
        }

        internal static Dictionary<char, int> Bits = new Dictionary<char, int>()
        {
            { 'd', 1 },
            { 'e', 2 },
            { 'a', 4 },
            { 'f', 8 },
            { 'g', 16 },
            { 'b', 32 },
            { 'c', 64 }
        };
    }
}