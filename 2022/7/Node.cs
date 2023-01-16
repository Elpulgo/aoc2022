namespace Aoc2022.Aoc2022;

public class Node
{
    public Node(
        string name,
        bool isLeaf,
        Node parent,
        int? size = null)
    {
        Name = name;
        IsLeaf = isLeaf;
        Parent = parent;
        _size = size;
    }

    private readonly int? _size;
    public Dictionary<string, Node> Children { get; } = new();
    public bool IsLeaf { get; }
    public string Name { get; }
    public Node Parent { get; }

    public int Size
    {
        get
        {
            return IsLeaf
                ? _size.GetValueOrDefault(0)
                : Children.Sum(s => s.Value.Size);
        }
    }

    // Only add if exists, no need to add if we already visited node. 
    // This assumes each directory/file within a directory are unique, but that's mostly the case within a file system..
    public Node AddChildIfNotExists(Node node)
    {
        if (!Children.ContainsKey(node.Name))
        {
            Children.Add(node.Name, node);
        }

        return Children[node.Name];
    }
}