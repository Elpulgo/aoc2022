namespace Aoc2022.Algorithms;

public static class Dijkstra
{
    public static (Dictionary<TState, int>, TState) RunDijkstra<TState>(
        TState start,
        Func<TState, IEnumerable<(TState state, int cost)>> getNextStates,
        Func<Dictionary<TState, int>, TState, bool> endingCondition) where TState : DijkstraNode, IComparable<TState>
    {
        var totalCost = new Dictionary<TState, int>();
        var seen = new HashSet<(int, int)>();
        var pq = new PriorityQueue<TState>();

        pq.Enqueue(start);

        TState p = default;
        while (pq.HasValue)
        {
            p = pq.Dequeue();
            var cost = p.Cost;

            while (pq.Count != 0 && totalCost.ContainsKey(p))
            {
                p = pq.Dequeue();
                cost = p.Cost;
            }

            totalCost[p] = cost;
            if (endingCondition(totalCost, p))
                break;

            var states = getNextStates(p)
                .Select(s => (State: s.state, Cost: cost + s.state.Cost));

            foreach (var st in states.Where(w => !seen.Contains((w.State.X, w.State.Y))))
                pq.Enqueue(st.State with { Cost = st.Cost });

            seen.Add((p.X, p.Y));
        }

        return (totalCost, p)!;
    }

    public record DijkstraNode(int X, int Y, int Cost);
}