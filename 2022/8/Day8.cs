using System;
using System.IO;
using System.Linq;
using Aoc2022;

namespace Aoc2022.Aoc2022
{
    internal class Day8 : BaseDay
    {
        public Day8(bool shouldPrint) : base(2022, nameof(Day8), shouldPrint)
        {
        }

        public override void Execute()
        {
            var input = ReadInput().ToList();

            StartTimerOne();
            StartTimerTwo();

            Solve(input);
        }


        private void Solve(IReadOnlyCollection<string> input)
        {
            var max = input.Count;

            var cellFactory = new CellFactory(
                maxRow: input.First().Length - 1,
                maxColumn: input.Count - 1);

            var cells = new Dictionary<string, Cell>();

            InnerExecute(false);
            InnerExecute(true);
            
            FirstSolution(cells.Count(w => w.Value.IsVisible).ToString());
            SecondSolution(cells.Select(w => w.Value.GetSum()).Max().ToString());

            void InnerExecute(bool reverse)
            {
                var maxValueColumnMap = Enumerable.Range(0, max).ToDictionary(d => d, d => 0);
                var maxValueColumnStack = Enumerable.Range(0, max).ToDictionary(d => d, d => new Stack<int>());


                var outerOperation = BuildFunc(reverse);
                var innerOperation = BuildInnerFunc(reverse);

                foreach (var (r, indexRow) in outerOperation(input))
                {
                    var maxValueRow = 0;
                    var rowStack = new Stack<int>();

                    foreach (var (value, indexColumn) in innerOperation(r))
                    {
                        Cell cell;

                        var idxRow = reverse
                            ? max - 1 - indexRow
                            : indexRow;

                        var idxColumn = reverse
                            ? max - 1 - indexColumn
                            : indexColumn;

                        if (!cells.TryGetValue(Cell.BuildKey(idxRow, idxColumn), out cell))
                        {
                            cell = cellFactory.Create(value, idxRow, idxColumn);
                            cells.Add(cell.Key, cell);
                        }

                        // Part 1
                        PartOne(
                            cell,
                            value,
                            idxColumn,
                            maxValueColumnMap,
                            maxValueRow);

                        // Part 2
                        PartTwo(
                            cell,
                            value,
                            idxColumn,
                            rowStack,
                            maxValueColumnStack);
                    }
                }
            }

            void PartOne(
                Cell cell,
                int value,
                int idxColumn,
                Dictionary<int, int> maxValueColumnMap,
                int maxValueRow)
            {
                if (value > maxValueColumnMap[idxColumn])
                {
                    maxValueColumnMap[idxColumn] = value;
                    cells[cell.Key].MarkAsVisible();
                }

                if (value > maxValueRow)
                {
                    maxValueRow = value;
                    cells[cell.Key].MarkAsVisible();
                }
            }

            void PartTwo(
                Cell cell,
                int value,
                int idxColumn,
                Stack<int> rowStack,
                Dictionary<int, Stack<int>> maxValueColumnStack)
            {
                cell.AddPoint(RetracePath(rowStack, value));
                cell.AddPoint(RetracePath(maxValueColumnStack[idxColumn], value));

                maxValueColumnStack[idxColumn].Push(value);
                rowStack.Push(value);
            }
        }

        private static int RetracePath(Stack<int> path, int value)
        {
            var result = 0;

            foreach (var treeHeight in path)
            {
                if (value > treeHeight)
                {
                    result++;
                    continue;
                }

                result++;
                break;
            }

            return result;
        }

        private static Func<IEnumerable<string>, IEnumerable<(char[], int)>> BuildFunc(bool reversed)
        {
            return !reversed
                ? input => input.Select((s, i) => (s.ToCharArray(), i))
                : input => input.Reverse().Select((s, i) => (s.ToCharArray(), i));
        }

        private static Func<IEnumerable<char>, IEnumerable<(int, int)>> BuildInnerFunc(bool reversed)
        {
            return !reversed
                ? input => input.Select((s, i) => (s.CharToInt(), i))
                : input => input.Reverse().Select((s, i) => (s.CharToInt(), i));
        }
    }
}