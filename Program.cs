using System.Reflection;
using Aoc2022;
using Aoc2022.Aoc2024;

Console.WriteLine("Hello, Advent!");

var day = new Day1(true);
day.Execute();

// Runner.RunAll("Aoc2022.Aoc2021");
// Initializer.Run(2024);

public static class Initializer
{
    public static void Run(int year)
    {
        var yearDirectory = Path.Join(
            FileHelpers.GetFileRootDirectory(),
            year.ToString());

        if (Directory.Exists(yearDirectory))
            return;

        var createdYear = Directory.CreateDirectory(yearDirectory);

        for (var i = 0; i <= 24; i++)
        {
            var day = Path.Join(createdYear.FullName, i.ToString());
            
            if (Directory.Exists(day))
                continue;

            var dayDirectory = Directory.CreateDirectory(day);
            File.Create(Path.Join(dayDirectory.FullName, $"Day{i}.txt"));
            File.Create(Path.Join(dayDirectory.FullName, $"Day{i}.md"));

            File.WriteAllText(Path.Join(dayDirectory.FullName, $"Day{i}.cs"), TemplateV2($"Day{i}", year));
        }
    }

    private static string Template(string day, int year) => $@"
using System;
using System.IO;
using System.Linq;
using Aoc2022;

namespace Aoc2022.Aoc{year}
{{
    internal class {day} : BaseDay
    {{
        public {day}(bool shouldPrint): base({year}, nameof({day}), shouldPrint) {{
        }}

        public override void Execute()
        {{
           
        }}
    }}
}}";

    private static string TemplateV2(string day, int year) => $@"
namespace Aoc2022.Aoc{year};

internal class {day} : BaseDay
{{
    public {day}(bool shouldPrint) : base({year}, nameof({day}), shouldPrint)
    {{
    }}

    public override void Execute()
    {{
        PartOne();
        PartTwo();
    }}

    private void PartOne()
    {{
        var input = ReadInput(true).ToList();
    }}


    private void PartTwo()
    {{
        var input = ReadInput(false, partTwo: true).ToList();
    }}
}}";
}

public static class Runner
{
    public static void RunAll(string ns)
    {
        
        var allDays = Assembly.GetExecutingAssembly().GetTypes()
            .Where(w => string.Equals(w.Namespace, ns, StringComparison.Ordinal)
                        && !w.IsAbstract
                        && w.BaseType == typeof(BaseDay))
            .Select(s => (BaseDay)Activator.CreateInstance(s, new object[] { true })!) // Add more to match constructor of days if needed..
            .OrderBy(o => o.Day)
            .ToList();

        foreach (var d in allDays)
        {
            Console.WriteLine();
            d.Execute();
        }
    }
}