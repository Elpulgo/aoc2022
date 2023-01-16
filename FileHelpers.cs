namespace Aoc2022;

public static class FileHelpers
{
    public static string GetFileRootDirectory()
    {
        var release = Directory.GetParent(Directory.GetCurrentDirectory());
        var bin = Directory.GetParent(release.FullName);
        var projectRoot = Directory.GetParent(bin.FullName);

        return projectRoot.FullName;
    }

    public static string SanitizeDay(this string day) => day.Replace("Day", "");
}