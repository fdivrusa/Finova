using System;

namespace Finova.Examples.ConsoleApp.Helpers;

public static class ConsoleHelper
{
    public static void WriteHeader(string text)
    {
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("╔" + new string('═', 74) + "╗");
        Console.WriteLine("║" + text.PadLeft(37 + text.Length / 2).PadRight(74) + "║");
        Console.WriteLine("╚" + new string('═', 74) + "╝");
        Console.ResetColor();
        Console.WriteLine();
    }

    public static void WriteSectionHeader(string text)
    {
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("┌" + new string('─', 74) + "┐");
        Console.WriteLine("│" + text.PadLeft(37 + text.Length / 2).PadRight(74) + "│");
        Console.WriteLine("└" + new string('─', 74) + "┘");
        Console.ResetColor();
        Console.WriteLine();
    }

    public static void WriteSubHeader(string number, string text)
    {
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.Write($"  ■ {number}. ");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine(text);
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("  " + new string('─', 50));
        Console.ResetColor();
    }

    public static void WriteCountryHeader(string flag, string country)
    {
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"    {flag} {country}");
        Console.ResetColor();
    }

    public static void WriteResult(string label, string value, bool isValid, string? extra = null)
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write($"      {label}: ");
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write($"{value,-32} ");

        if (isValid)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("✓ Valid");
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("✗ Invalid");
        }

        if (!string.IsNullOrEmpty(extra))
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write($" [{extra}]");
        }

        Console.ResetColor();
        Console.WriteLine();
    }

    public static void WriteCode(string code)
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write("      Code: ");
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.WriteLine(code);
        Console.ResetColor();
    }

    public static void WriteSimpleResult(string value, bool isValid, string? extra = null)
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write($"      {value,-35} ");

        if (isValid)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("✓ Valid");
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("✗ Invalid");
        }

        if (!string.IsNullOrEmpty(extra))
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write($" [{extra}]");
        }

        Console.ResetColor();
        Console.WriteLine();
    }

    public static void WriteInfo(string label, string value)
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write($"      {label}: ");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine(value);
        Console.ResetColor();
    }

    public static void WriteSuccess(string message)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"      ✓ {message}");
        Console.ResetColor();
    }

    public static void WriteError(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"      ✗ {message}");
        Console.ResetColor();
    }

    public static void WriteBullet(string text)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write("      • ");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine(text);
        Console.ResetColor();
    }
}
