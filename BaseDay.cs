using System.Diagnostics;
using System.Globalization;

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

    protected void FirstSolution(int result)
    {
        StopTimerOne();

        if (_shouldPrint)
        {
            Console.WriteLine($"{ReadDescription(_day)} - {_day} - Part 1: {result} | ({TimePartOne} s)");
            return;
        }

        _resultPartOne = result.ToString(CultureInfo.InvariantCulture);
    }

    protected void FirstSolution(long result)
    {
        StopTimerOne();

        if (_shouldPrint)
        {
            Console.WriteLine($"{ReadDescription(_day)} - {_day} - Part 1: {result} | ({TimePartOne} s)");
            return;
        }

        _resultPartOne = result.ToString(CultureInfo.InvariantCulture);
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

    protected void SecondSolution(int result)
    {
        StopTimerTwo();

        if (_shouldPrint)
        {
            Console.WriteLine($"{ReadDescription(_day)} - {_day} - Part 2: {result} | ({TimePartTwo} s)");
            return;
        }

        _resultPartTwo = result.ToString(CultureInfo.InvariantCulture);
    }

    protected void SecondSolution(long result)
    {
        StopTimerTwo();

        if (_shouldPrint)
        {
            Console.WriteLine($"{ReadDescription(_day)} - {_day} - Part 2: {result} | ({TimePartTwo} s)");
            return;
        }

        _resultPartTwo = result.ToString(CultureInfo.InvariantCulture);
    }

    protected IEnumerable<string> ReadInput(bool partOne, bool partTwo = false)
    {
        var lines = File
            .ReadAllLines(Path.Combine(
                FileHelpers.GetFileRootDirectory(),
                _year.ToString(),
                _day.SanitizeDay(),
                $"{_day}.txt"));

        if (partOne)
        {
            StartTimerOne();
        }

        if (partTwo)
        {
            StartTimerTwo();
        }

        return lines;
    }

    protected IEnumerable<string> ReadInput()
        => File
            .ReadAllLines(Path.Combine(
                FileHelpers.GetFileRootDirectory(),
                _year.ToString(),
                _day.SanitizeDay(),
                $"{_day}.txt"));

    protected string ReadInputRaw(bool partOne, bool partTwo = false)
    {
        var raw = File
            .ReadAllText(Path.Combine(
                FileHelpers.GetFileRootDirectory(),
                _year.ToString(),
                _day.SanitizeDay(),
                $"{_day}.txt"));
        
        
        if (partOne)
        {
            StartTimerOne();
        }

        if (partTwo)
        {
            StartTimerTwo();
        }

        return raw;
    }

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
        Console.WriteLine(
            $"{ReadDescription(_day)}\t\t | {_resultPartOne} ({TimePartOne} s) | {_resultPartTwo} ({TimePartTwo} s)");
    }
}