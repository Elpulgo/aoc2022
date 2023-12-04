namespace Aoc2022.Algorithms.Models;

public interface INode
{
    List<INode> Children { get; set; }
}