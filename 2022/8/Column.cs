namespace Aoc2022.Aoc2022;

public class Column
{
    public int Index { get; }

    public List<Cell> Cells { get; } = new();

    public Column(int index)
    {
        Index = index;
    }
}

public class Row
{
    public int Index { get; }
    public List<Cell> Cells { get; } = new();

    public Row(int index)
    {
        Index = index;
    }
}