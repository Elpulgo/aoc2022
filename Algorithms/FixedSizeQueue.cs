namespace Aoc2022.Algorithms;

public class FixedSizeQueue<T> : Queue<T>
{
    public int Size { get; private set; }

    public FixedSizeQueue(int size)
    {
        Size = size;
    }

    public new void Enqueue(T item)
    {
        base.Enqueue(item);

        while (base.Count > Size)
        {
            T dequeuedItem;
            TryDequeue(out dequeuedItem);
        }
    }
}