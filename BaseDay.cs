using System.Diagnostics;

namespace Aoc2022;

public abstract class BaseDay
{
    protected BaseDay(int year, string day = "", bool shouldPrint = true)
    {
        _shouldPrint = shouldPrint;
        _day = day;
        _year = year;

        Input = ReadInput();
    }

    public int Day => int.Parse(_day.Replace("Day", ""));

    private readonly string _day;

    private readonly int _year;

    private string _resultPartOne;
    private string _resultPartTwo;

    private readonly bool _shouldPrint = true;

    private readonly Stopwatch _stopwatchPartOne = new();
    private readonly Stopwatch _stopwatchPartTwo = new();

    private double TimePartOne => _stopwatchPartOne.Elapsed.TotalSeconds;
    private double TimePartTwo => _stopwatchPartTwo.Elapsed.TotalSeconds;

    protected IEnumerable<string> Input { get; }

    protected void FirstSolution(string result)
    {
        StopTimerOne();

        if (_shouldPrint)
        {
            Console.WriteLine($"{ReadDescription(_day)} - {_day} - Part 1: {result} | ({TimePartOne} s)");
            return;
        }

        _resultPartOne = result;
    }

    protected void SecondSolution(string result)
    {
        StopTimerTwo();

        if (_shouldPrint)
        {
            Console.WriteLine($"{ReadDescription(_day)} - {_day} - Part 2: {result} | ({TimePartTwo} s)");
            return;
        }

        _resultPartTwo = result;
    }

    protected IEnumerable<string> ReadInput()
        => File
            .ReadAllLines(Path.Combine(
                FileHelpers.GetFileRootDirectory(),
                _year.ToString(),
                _day.SanitizeDay(),
                $"{_day}.txt"));
    
    protected string ReadInputRaw()
        => File
            .ReadAllText(Path.Combine(
                FileHelpers.GetFileRootDirectory(),
                _year.ToString(),
                _day.SanitizeDay(),
                $"{_day}.txt"));

    private string ReadDescription(string day)
        => File.ReadAllLines(Path.Combine(
            FileHelpers.GetFileRootDirectory(),
            _year.ToString(),
            _day.SanitizeDay(),
            $"{_day}.md")).First().Replace("-", string.Empty).Trim();


    protected void StartTimerOne() => _stopwatchPartOne.Start();
    protected void StopTimerOne() => _stopwatchPartOne.Stop();
    protected void StartTimerTwo() => _stopwatchPartTwo.Start();
    protected void StopTimerTwo() => _stopwatchPartTwo.Stop();

    public abstract void Execute();

    public void Print()
    {
        Console.WriteLine($"{ReadDescription(_day)}\t\t | {_resultPartOne} ({TimePartOne} s) | {_resultPartTwo} ({TimePartTwo} s)");
    }
}