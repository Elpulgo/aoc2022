namespace Aoc2022.Aoc2022;

public class Cell
{
    private readonly int _maxRow;
    private readonly int _maxColumn;

    public Cell(int value, int row, int column, int maxRow, int maxColumn)
    {
        _maxRow = maxRow;
        _maxColumn = maxColumn;
        Value = value;
        Row = row;
        Column = column;
        _points = new List<int>();
    }

    private bool _isVisible;
    private List<int> _points;

    public bool IsVisible => IsEdge || _isVisible;
    public int Value { get; }
    public int Row { get; }
    public int Column { get; }

    public bool IsEdge =>
        Row == 0 ||
        Row == _maxRow ||
        Column == 0 ||
        Column == _maxColumn;

    public string Key => $"R{Row}C{Column}";

    public void MarkAsVisible()
    {
        if (IsEdge)
            return;

        _isVisible = true;
    }

    public void AddPoint(int point) => _points.Add(point);
    public int GetSum() => _points.Aggregate(1, (a, b) => a * b);

    public static string BuildKey(int row, int column) => $"R{row}C{column}";
}

public class CellFactory
{
    private readonly int _maxRow;
    private readonly int _maxColumn;

    public CellFactory(int maxRow, int maxColumn)
    {
        _maxRow = maxRow;
        _maxColumn = maxColumn;
    }

    public Cell Create(int value, int row, int column)
    {
        return new Cell(value, row, column, _maxRow, _maxColumn);
    }
}