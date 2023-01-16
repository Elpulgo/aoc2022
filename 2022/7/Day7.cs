using System;
using System.IO;
using System.Linq;
using Aoc2022;

namespace Aoc2022.Aoc2022
{
    internal class Day7 : BaseDay
    {
        public Day7(bool shouldPrint) : base(2022, nameof(Day7), shouldPrint)
        {
        }

        public override void Execute()
        {
            StartTimerOne();
            var input = ReadInput();
            
            SolveFirst(input);
            SolveSecond(input);
        }

        private void SolveFirst(IEnumerable<string> input)
        {
            StartTimerOne();

            FirstSolution(GetDirectories(input)
                .Where(w => w.Size < 100000)
                .Select(s => s.Size)
                .Sum()
                .ToString());
        }

        private void SolveSecond(IEnumerable<string> input)
        {
            StartTimerTwo();

            const int totalSizeOfDisk = 70000000;
            const int totalNeedOfDisk = 30000000;
            
            var directories = GetDirectories(input)
                .OrderBy(o => o.Size)
                .ToList();

            var unsuedSpaceToFree = totalSizeOfDisk - directories.Single(w => w.Parent is null).Size;
            var diskNeededToAchieveGoal = totalNeedOfDisk - unsuedSpaceToFree;

            var smallest = directories.First(f => f.Size >= diskNeededToAchieveGoal);
            
            SecondSolution(smallest.Size.ToString());
        }

        private static IEnumerable<Node> GetDirectories(IEnumerable<string> input)
        {
            var rootNode = new Node("/", false, null);
            var currentNode = rootNode;

            foreach (var line in input.Skip(1))
            {
                if (line.IsCommand())
                {
                    var command = line.Parse();
                    if (command.Operation == Operation.Cd)
                    {
                        if (command.Arguments == "..")
                        {
                            currentNode = currentNode.Parent;
                            continue;
                        }

                        var newNode = new Node(command.Arguments, false, currentNode);
                        var addedNode = currentNode.AddChildIfNotExists(newNode);
                        currentNode = addedNode;
                    }

                    continue;
                }

                // If not a command, we are listing the files/directories
                var (name, size) = ParseFileInfo(line);

                var foundNode = new Node(
                    name,
                    size.HasValue,
                    currentNode,
                    size);

                currentNode.AddChildIfNotExists(foundNode);
            }

            return FindAllDirectories(rootNode);
        }

        private static (string Name, int? Size) ParseFileInfo(string input)
        {
            var data = input.Split(" ", StringSplitOptions.RemoveEmptyEntries);

            return data[0] == "dir"
                ? (data[1], null)
                : (data[1], int.Parse(data[0]));
        }

        private static IEnumerable<Node> FindAllDirectories(Node node)
        {
            var nodes = new List<Node>();

            if (node.IsLeaf)
                return nodes;

            nodes.Add(node);
            foreach (var child in node.Children)
            {
                nodes.AddRange(FindAllDirectories(child.Value));
            }

            return nodes;
        }
    }
}