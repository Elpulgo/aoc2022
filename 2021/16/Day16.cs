using System;
using System.IO;
using System.Linq;
using Aoc2022;

namespace Aoc2022.Aoc2021
{
    internal class Day16 : BaseDay
    {
        public Day16(bool shouldPrint) : base(2021, nameof(Day16), shouldPrint)
        {
        }

        public override void Execute()
        {
            var binaryInput = String.Join(
                "",
                string.Join("", ReadInput()).Select(
                    c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')));

            PartOne(binaryInput);
            PartTwo(binaryInput);
        }

        private void PartTwo(string binaryInput)
        {
            base.StartTimerTwo();
            var (packet, _) = DecodePacket(binaryInput);
            base.StopTimerTwo();

            SecondSolution(packet.Calculate().ToString());
        }

        private (Packet, string) DecodePacket(string binaryInput)
        {
            string remaining = "";
            var packet = new Packet
            {
                Version = ConvertBin64(binaryInput, 0, 3),
                PacketType = ConvertBin64(binaryInput, 3, 3)
            };

            binaryInput = binaryInput[6..];

            if (packet.IsLiteral)
            {
                var result = DecodeLiteralPacket(binaryInput);
                packet.LiteralValue = Convert.ToInt64(result.Result, 2);
                remaining = result.Remaining;

                return (packet, remaining);
            }

            packet.LengthTypeId = ConvertBin64(binaryInput, 0, 1);
            binaryInput = binaryInput[1..];

            if (packet.LengthTypeId == 0)
                remaining = DecodeGivenLengthOfSubpackages(packet, binaryInput);
            else if (packet.LengthTypeId == 1)
                remaining = DecodeNumberOfContainedSubPackages(packet, binaryInput);

            return (packet, remaining);
        }

        private string DecodeGivenLengthOfSubpackages(Packet packet, string binaryInput)
        {
            var lengthOfSubPackets = ConvertBin32(binaryInput, 0, 15);
            var subBits = binaryInput.Substring(15, lengthOfSubPackets);
            var remaining = binaryInput[(15 + lengthOfSubPackets)..];

            while (subBits.Length > 0)
            {
                Packet subPacket;
                (subPacket, subBits) = DecodePacket(subBits);
                packet.SubPackets.Add(subPacket);
            }

            return remaining;
        }

        private string DecodeNumberOfContainedSubPackages(Packet packet, string binaryInput)
        {
            var numberOfContainedPackets = ConvertBin32(binaryInput, 0, 11);
            var remaining = binaryInput[11..];

            foreach (var i in Enumerable.Range(0, numberOfContainedPackets))
            {
                Packet current;
                (current, remaining) = DecodePacket(remaining);
                packet.SubPackets.Add(current);
            }

            return remaining;
        }

        private (string Result, string Remaining) DecodeLiteralPacket(string input)
        {
            var position = 0;
            var result = "";

            while (input[position] != '0')
            {
                result += input.Substring(position + 1, 4);
                position += 5;
            }

            // Last package since we found 0
            result += input.Substring(position + 1, 4);

            return (result, input[(position + 5)..]);
        }

        private void PartOne(string binaryInput)
        {
            base.StartTimerOne();
            var packetVersions = new List<long>();

            while (!string.IsNullOrEmpty(binaryInput))
            {
                if (binaryInput.Length < 6)
                    break;

                var version = binaryInput.Substring(0, 3);
                var type = binaryInput.Substring(3, 6);

                packetVersions.Add(Convert.ToInt64(version, 2));

                var packetBits = new List<string>();

                binaryInput = binaryInput[6..];

                while (true)
                {
                    var newBits = binaryInput.Substring(0, 5);
                    packetBits.Add(newBits.Substring(1, 4));

                    binaryInput = binaryInput[5..];

                    if (newBits.StartsWith("0"))
                    {
                        // Remove trailing zeros
                        binaryInput = binaryInput[3..];
                        break;
                    }
                }
            }

            base.StopTimerOne();
            FirstSolution(packetVersions.Sum().ToString());
        }

        private static Int32 ConvertBin32(string input, int start, int length)
            => Convert.ToInt32(input.Substring(start, length), 2);

        private static Int64 ConvertBin64(string input, int start, int length)
            => Convert.ToInt64(input.Substring(start, length), 2);

        public class Packet
        {
            public long LiteralValue = 0;
            public List<Packet> SubPackets = new();
            public List<long> Bits => SubPackets.Select(s => s.Calculate()).ToList();
            public long PacketType { get; set; }
            public long Version { get; set; }
            public bool IsOperator => PacketType != 4;
            public bool IsLiteral => PacketType == 4;
            public long LengthTypeId { get; set; }

            public long Calculate()
                => PacketType switch
                {
                    0 => Bits.Sum(),
                    1 when Bits.Count > 1 => Bits.Aggregate((long)1, (a, b) => a * b),
                    1 when Bits.Count < 2 => Bits.Any() ? Bits.Single() : 0,
                    2 => Bits.Min(),
                    3 => Bits.Max(),
                    4 => LiteralValue,
                    5 => Bits.First() > Bits.Last() ? 1 : 0,
                    6 => Bits.First() < Bits.Last() ? 1 : 0,
                    7 => Bits.First() == Bits.Last() ? 1 : 0,
                    _ => throw new NotSupportedException()
                };
        }
    }
}