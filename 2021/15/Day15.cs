using System;
using System.IO;
using System.Linq;
using Aoc2022;
using Aoc2022.Algorithms;

namespace Aoc2022.Aoc2021
{
    internal class Day15 : BaseDay
    {
        private int[,] _riskLevels;
        private int _height;
        private int _width;

        public Day15(bool shouldPrint) : base(2021, nameof(Day15), shouldPrint)
        {
        }

        public override void Execute()
        {
            //PartOne();
            PartTwo();

            void PartTwo()
            {
                var input = ReadInput().ToArray();
                _width = input.First().Length;
                _height = input.Length;
                var scaledWidth = _width * 5;
                var scaledHeight = _height * 5;
                _riskLevels = new int[scaledWidth, scaledHeight];

                for (int y = 0; y < _height; y++)
                {
                    string item = input[y];
                    for (int x = 0; x < _width; x++)
                    {
                        _riskLevels[x, y] = item[x] - '0';

                        var value = item[x] - '0';
                        for (int scale = 1; scale < 5; scale++)
                        {
                            if (value == 9)
                                value = 1;
                            else
                                value++;

                            _riskLevels[x + (scale * _width), y] = value;
                            _riskLevels[x, y + (scale * _height)] = value;
                        }
                    }
                }

                var height = _height;
                while (height < _height * 5)
                {
                    PrintRest(height, 10, 10);
                    height += 10;
                }


                void PrintRest(int height, int heightFactor, int widthFactor)
                {
                    for (int y = height; y < height + heightFactor; y++)
                    {
                        for (int x = widthFactor; x < scaledWidth; x++)
                        {
                            var value = _riskLevels[x, y - heightFactor];

                            if (value == 9)
                                value = 1;
                            else
                                value++;

                            _riskLevels[x, y] = value;
                        }
                    }
                }

                void PrintMatrix()
                {
                    for (var y = 0; y < 500; y++)
                    {
                        var line = "";
                        for (var x = 0; x < 500; x++)
                        {
                            line += $"{_riskLevels[x, y]}";
                        }

                        Console.WriteLine(line);
                    }
                }


                _width = scaledWidth;
                _height = scaledHeight;

                base.StartTimerTwo();
                var result = FindPathTest();
                base.StopTimerTwo();

                SecondSolution(result.ToString());
            }

            void PartOne()
            {
                var input = ReadInput().ToArray();
                _width = input.First().Length;
                _height = input.Length;

                _riskLevels = new int[_width, _height];
                for (int i = 0; i < _height; i++)
                {
                    string item = input[i];
                    for (int j = 0; j < _width; j++)
                    {
                        _riskLevels[j, i] = item[j] - '0';
                    }
                }

                base.StartTimerOne();
                var result = FindPath();
                base.StopTimerOne();

                FirstSolution(result.ToString());
            }
        }

        private int FindPathTest()
        {
            var end = new Point(_width - 1, _height - 1, 0);
            var start = new Point(0, 0, 0);
            var sideLength = _width / 5;

            var (totCost, endingPoint) = Dijkstra.RunDijkstra(
                start,
                p => BuildNeighbours(p.X, p.Y).Select(s => (s, s.Cost))
                    .Where(q => q.s.Y >= 0 && q.s.Y <= end.Y
                                           && q.s.X >= 0 && q.s.X <= end.X)
                    .Select(q => (q.s, getRisk(q.s.X, q.s.Y))),
                (_, p) => p.X == end.X && p.Y == end.Y);

            return endingPoint.Cost;

            int getRisk(int x, int y)
            {
                var increase = y / sideLength + x / sideLength;
                (x, y) = (x % sideLength, y % sideLength);
                return (((_riskLevels[x, y] - 1) + increase) % 9) + 1;
            }
        }

        private int FindPath()
        {
            var end = new Point(_width - 1, _height - 1, 0);
            var start = new Point(0, 0, 0);

            var (totCost, endingPoint) = Dijkstra.RunDijkstra(
                start,
                p => BuildNeighbours(p.X, p.Y).Select(s => (s, s.Cost)),
                (_, p) => p.X == end.X && p.Y == end.Y);

            return endingPoint.Cost;
        }

        private IEnumerable<Point> BuildNeighbours(int x, int y)
        {
            if (x - 1 > -1)
                yield return new Point(x - 1, y, _riskLevels[x - 1, y]);
            if (x + 1 < _width)
                yield return new Point(x + 1, y, _riskLevels[x + 1, y]);
            if (y - 1 > -1)
                yield return new Point(x, y - 1, _riskLevels[x, y - 1]);
            if (y + 1 < _height)
                yield return new Point(x, y + 1, _riskLevels[x, y + 1]);
        }
    }

    internal record Point(int X, int Y, int Cost) : Dijkstra.DijkstraNode(X, Y, Cost), IComparable<Point>
    {
        public int CompareTo(Point other)
            => base.Cost.CompareTo(other.Cost);
    }
}