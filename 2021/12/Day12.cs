using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using Aoc2022;

namespace Aoc2022.Aoc2021
{
    internal class Day12 : BaseDay
    {
        public Day12(bool shouldPrint) : base(2021, nameof(Day12), shouldPrint)
        {
        }

        public override void Execute()
        {
            FirstSolution(Run(false).ToString());
            SecondSolution(Run(true).ToString());

            int Run(bool isPartTwo)
            {
                var graph = BuildGraph(ReadInput().Select(s => s.Split("-")).ToList());
                var lookup = ReadInput().Select(s => s.Split("-")).SelectMany(s => s).Distinct()
                    .ToDictionary(d => d, d => 0);

                if (!isPartTwo)
                    base.StartTimerOne();
                else
                    base.StartTimerTwo();

                var paths = FindPaths(
                    current: graph.Single(s => s.Value == "start"),
                    endingNode: "end",
                    isPartTwo,
                    lookup);

                if (!isPartTwo)
                    base.StopTimerOne();
                else
                    base.StopTimerTwo();

                return paths;
            }
        }

        private List<Node> BuildGraph(List<string[]> lines)
        {
            var graph = new List<Node>();

            lines
                .SelectMany(s => s)
                .Distinct()
                .ToList()
                .ForEach(f => graph.Add(new Node(f)));

            lines.ForEach(line =>
            {
                var firstNode = graph
                    .Single(s => s.Value == line.First());
                var lastNode = graph
                    .Single(s => s.Value == line.Last());

                firstNode.AddAjecent(lastNode);
                lastNode.AddAjecent(firstNode);
            });

            return graph;
        }

        private int FindPaths(Node current, string endingNode, bool isPartTwo, Dictionary<string, int> lookup)
        {
            if (current.IsStartingOrEnd && lookup[current.Value] > 0)
                return 0;

            // Small cave has already been visited, can't continue with small cave
            if (isPartTwo &&
                current.CanOnlyBeVisitedTwice &&
                GetAllSmallCaves(lookup).Any(a => a.Value > 1) &&
                lookup[current.Value] == 1)
                return 0;

            // If current node occurs in our visited collection and it can't be visited, end this path without finding the end node.
            var canBeVisited = !(lookup[current.Value] > 0 && current.CanOnlyBeVistedOnce);
            if (!isPartTwo && !canBeVisited)
                return 0;

            current.MarkAsVisited();
            lookup[current.Value] += 1;

            // End node is found, return increment of paths found.
            if (current.Value == endingNode)
                return 1;

            var adjCanBeVisited = current.Adjecents
                .Where(w => !(lookup[w.Value] > 0 && w.CanOnlyBeVistedOnce))
                .ToList();

            if (isPartTwo)
            {
                adjCanBeVisited.Clear();

                foreach (var curAdj in current.Adjecents)
                {
                    if (curAdj.IsStartingOrEnd && lookup[curAdj.Value] < 1)
                        adjCanBeVisited.Add(curAdj);

                    if (curAdj.IsStartingOrEnd && lookup[curAdj.Value] == 1)
                        continue;

                    if (GetAllSmallCaves(lookup).Any(a => a.Value > 1) && curAdj.CanOnlyBeVisitedTwice &&
                        lookup[curAdj.Value] > 0)
                        continue;

                    // Big cave, can always be visited, or small cave only visited once
                    adjCanBeVisited.Add(curAdj);
                }
            }

            return adjCanBeVisited
                .Distinct()
                .Select(adj => FindPaths(
                    adj,
                    endingNode,
                    isPartTwo,
                    lookup.ToDictionary(d => d.Key, d => d.Value)))
                .Sum();

            List<KeyValuePair<string, int>> GetAllSmallCaves(Dictionary<string, int> lookup)
                => lookup.Where(w => w.Key.All(a => char.IsLower(a)) && w.Key != "start" && w.Key != "end").ToList();
        }
    }

    internal class Node
    {
        private string _value;
        private List<Node> _adjecentNodes = new();
        private bool _visited;
        public string Value => _value;
        public ImmutableList<Node> Adjecents => _adjecentNodes.ToImmutableList();
        public bool CanOnlyBeVistedOnce => _value.All(a => char.IsLower(a));
        public bool CanOnlyBeVisitedTwice => _value != "start" && Value != "end" && _value.All(a => char.IsLower(a));
        public bool IsStartingOrEnd => _value == "start" || _value == "end";

        public bool CanBeVisited
            => CanOnlyBeVistedOnce ? !_visited : true;

        public Node(string value)
            => _value = value;

        public void AddAjecent(Node node)
            => _adjecentNodes.Add(node);

        public void MarkAsVisited()
            => _visited = true;
    }
}