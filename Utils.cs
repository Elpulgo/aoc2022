﻿using Aoc2022.Algorithms.Models;

namespace Aoc2022;

public static class Utils
{
    public static IEnumerable<T[]> SplitInTwo<T>(this IEnumerable<T> input)
    {
        var first = input.Take((input.Count() + 1) / 2).ToArray();
        var second = input.Skip((input.Count() + 1) / 2).ToArray();

        yield return first;
        yield return second;
    }

    public static IEnumerable<IEnumerable<string>> PartitionBasedOnEmptyLine(this IEnumerable<string> input)
    {
        return input.Aggregate(new List<List<string>>(), (list, s) =>
        {
            if (list.Count == 0 || string.IsNullOrEmpty(s))
            {
                list.Add(new List<string>());
            }

            if (string.IsNullOrEmpty(s))
                return list;

            list.Last().Add(s);
            return list;
        });
    }

    public static IEnumerable<List<T>> Partition<T>(this IEnumerable<T> values, int partitionSize)
    {
        return values
            .Select((s, i) => new { Index = i, Value = s })
            .GroupBy(g => g.Index / partitionSize)
            .Select(s => s.Select(v => v.Value).ToList())
            .ToList();
    }

    public static List<T> IntersectAoC<T>(this IEnumerable<IEnumerable<T>> input)
    {
        return input
            .Skip(1)
            .Aggregate(
                new HashSet<T>(input.First()),
                (h, e) =>
                {
                    h.IntersectWith(e);
                    return h;
                })
            .ToList();
    }

    public static int GetAlphabeticIndexFromChar(this char c) => char.ToUpper(c) - 64;

    public static void Swap<T>(this T[] data, int first, int last) => (data[first], data[last]) = (data[last], data[first]);

    public static int CharToInt(this char c) => c - '0';
    
    
    /// <summary>
    /// Example input:
    /// 
    /// Sabqponm
    /// abcryxxl
    /// accszExk
    /// acctuvwj
    /// abdefghi
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="input"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static IEnumerable<GraphPoint> BuildNeighboursWithValue<T>(
        int x,
        int y,
        int width,
        int height,
        string[] input) where T : GraphPoint
    {
        if (x - 1 > -1)
            yield return new GraphPoint(x - 1, y, input[y][x -1]);
        if (x + 1 < width)
            yield return new GraphPoint(x + 1, y, input[y][x + 1]);
        if (y - 1 > -1)
            yield return new GraphPoint(x, y - 1, input[y - 1][x]);
        if (y + 1 < height)
            yield return new GraphPoint(x, y + 1, input[y + 1][x]);
    }
    
    public static IEnumerable<GraphPoint> BuildNeighbours<T>(
        int x,
        int y,
        int width,
        int height) where T : GraphPoint
    {
        if (x - 1 > -1)
            yield return new GraphPoint(x - 1, y);
        if (x + 1 < width)
            yield return new GraphPoint(x + 1, y);
        if (y - 1 > -1)
            yield return new GraphPoint(x, y - 1);
        if (y + 1 < height)
            yield return new GraphPoint(x, y + 1);
    }
}