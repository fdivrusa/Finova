using System.Numerics;

namespace Finova.Core.Internals;

public static class Modulo97Helper
{
    /// <summary>
    /// Calculates the modulo 97 of a numeric string.
    /// </summary>
    public static int Calculate(string numericString)
    {
        if (string.IsNullOrWhiteSpace(numericString) || !numericString.All(char.IsDigit))
        {
            throw new ArgumentException("Input must contain only digits.", nameof(numericString));
        }

        // Use BigInteger to handle very large numbers
        BigInteger number = BigInteger.Parse(numericString);
        return (int)(number % 97);
    }
}
