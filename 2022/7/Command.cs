namespace Aoc2022.Aoc2022;

public static class Command
{
    public static bool IsCommand(this string input) => input.StartsWith("$");

    public static CommandOutcome Parse(this string input)
    {
        var commandArgs = input.Split(" ", StringSplitOptions.RemoveEmptyEntries);
        if (commandArgs[0] != "$")
        {
            throw new ArgumentException("Is not a command..");
        }

        return commandArgs[1] switch
        {
            "ls" => new CommandOutcome() { Operation = Operation.Ls },
            "cd" => new CommandOutcome() { Operation = Operation.Cd, Arguments = commandArgs[2] },
            _ => throw new ArgumentException("Invalid command")
        };
    }
}

public class CommandOutcome
{
    public Operation Operation { get; set; }
    public string Arguments { get; set; }
}

public enum Operation
{
    Ls,
    Cd
}