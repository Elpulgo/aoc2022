namespace Aoc2022.Aoc2022
{
    internal class Day1 : BaseDay
    {
        public Day1(bool shouldPrint) : base(2022, nameof(Day1), shouldPrint)
        {
        }


        public override void Execute()
        {
            StartTimerOne();
            StartTimerTwo();

            var input = ReadInput();

            var totals = new List<int> { 0 };
            var total = 0;
            foreach (var data in input)
            {
                if (string.IsNullOrEmpty(data))
                {
                    totals.Add(total);
                    total = 0;
                }
                else
                {
                    total += int.Parse(data);
                }
            }

            FirstSolution(totals.OrderByDescending(o => o).First().ToString());
            SecondSolution(totals.OrderByDescending(o => o).Take(3).Sum().ToString());
        }
    }
}