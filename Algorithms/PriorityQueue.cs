namespace Aoc2022.Algorithms;

public class PriorityQueue<T> where T : IComparable<T>
{
    private readonly List<T> _internal = new();

    private int LastIndex => _internal.Count - 1;

    public bool HasValue => _internal.Any();

    public int Count => _internal.Count;

    public void Enqueue(T item)
    {
        _internal.Add(item);
        BubbleUp();
    }

    public T Dequeue()
    {
        var highestPrioritizedItem = _internal.First();

        MoveLastItemToTop();
        SinkDown();

        return highestPrioritizedItem;
    }

    // Min Heap Bubble
    // https://algorithms.tutorialhorizon.com/binary-min-max-heap/
    private void BubbleUp()
    {
        var childIndex = LastIndex;

        while (childIndex > 0)
        {
            var parentIndex = (childIndex - 1) / 2;

            if (_internal[childIndex].CompareTo(_internal[parentIndex]) >= 0)
                break;

            Swap(childIndex, parentIndex);
            childIndex = parentIndex;
        }
    }

    private void MoveLastItemToTop()
    {
        _internal[0] = _internal[LastIndex];
        _internal.RemoveAt(LastIndex);
    }

    // Min Heap Sink
    // https://algorithms.tutorialhorizon.com/binary-min-max-heap/
    private void SinkDown()
    {
        var parentIndex = 0;

        while (true)
        {
            var firstChildIndex = parentIndex * 2 + 1;
            if (firstChildIndex > LastIndex)
                break;

            var secondChildIndex = firstChildIndex + 1;

            if (secondChildIndex <= LastIndex && _internal[secondChildIndex].CompareTo(_internal[firstChildIndex]) < 0)
            {
                firstChildIndex = secondChildIndex;
            }

            if (_internal[parentIndex].CompareTo(_internal[firstChildIndex]) < 0)
                break;

            Swap(parentIndex, firstChildIndex);
            parentIndex = firstChildIndex;
        }
    }

    private void Swap(int first, int last) => (_internal[first], _internal[last]) = (_internal[last], _internal[first]);
}