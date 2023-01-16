using System;
using System.IO;
using System.Linq;
using Aoc2022;
using Aoc2022.Algorithms;
using Aoc2022.Algorithms.Models;

namespace Aoc2022.Aoc2022
{
    internal class Day12 : BaseDay
    {
        public Day12(bool shouldPrint) : base(2022, nameof(Day12), shouldPrint)
        {
        }

        public override void Execute()
        {
            var input = ReadInput();

            StartTimerOne();
            FirstSolution(Solve(input, Condition, 'S', 'E').ToString());

            StartTimerTwo();
            SecondSolution(Solve(input, ConditionTwo, 'E', 'a').ToString());
        }

        private static int Solve(
            IEnumerable<string> input,
            Func<GraphPoint, GraphPoint, bool> condition,
            char start,
            char end)
        {
            var graph = GraphBuilder.BuildFrom<GraphPoint>(input);

            var endPoints = graph.AdjacencyList
                .Where(w => w.Key.value == end)
                .ToList();

            var startPoint = graph.AdjacencyList
                .SingleOrDefault(s => s.Key.value == start)
                .Key;

            var shortestPath = BFS.ShortestPathFunction(
                graph,
                startPoint,
                condition);

            // Offset by one cause of start is included
            return endPoints
                .Where(w => shortestPath.WasVisited(w.Key))
                .Select(s => shortestPath.GetShortestPathOrNull(s.Key))
                .Select(s => s.Count())
                .Min() - 1;
        }

        private static bool Condition(GraphPoint current, GraphPoint neighbour)
        {
            // If at most one higher we are okay
            return GetNumericRepresentation(neighbour.value) - GetNumericRepresentation(current.value) <= 1;
        }

        private static bool ConditionTwo(GraphPoint current, GraphPoint neighbour)
        {
            // If at most one higher we are okay
            var value = GetNumericRepresentation(neighbour.value) - GetNumericRepresentation(current.value);
            return value is >= 1 or -1 or 0;
        }

        private static int GetNumericRepresentation(char v) => v switch
        {
            'S' => 1,
            'E' => 26,
            _ => v.GetAlphabeticIndexFromChar()
        };
    }
}