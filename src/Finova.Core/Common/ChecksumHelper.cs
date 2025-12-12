using System.Numerics;

namespace Finova.Core.Common;

/// <summary>
/// Provides common checksum validation algorithms.
/// </summary>
public static class ChecksumHelper
{
    /// <summary>
    /// Validates a number string using the Luhn algorithm (Modulo 10).
    /// </summary>
    /// <param name="input">The numeric string to validate.</param>
    /// <returns>True if the input is valid according to the Luhn algorithm; otherwise, false.</returns>
    public static bool ValidateLuhn(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return false;
        }

        int sum = 0;
        bool doubleDigit = false;

        for (int i = input.Length - 1; i >= 0; i--)
        {
            if (!char.IsDigit(input[i]))
            {
                continue;
            }

            int digit = input[i] - '0';

            if (doubleDigit)
            {
                digit *= 2;
                if (digit > 9)
                {
                    digit -= 9;
                }
            }

            sum += digit;
            doubleDigit = !doubleDigit;
        }

        return (sum % 10) == 0;
    }

    /// <summary>
    /// Validates a number string using the Modulo 11 algorithm with weights 1, 2, 3... 10.
    /// </summary>
    /// <param name="input">The numeric string to validate.</param>
    /// <returns>True if the input is valid according to Modulo 11; otherwise, false.</returns>
    public static bool ValidateModulo11(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return false;
        }

        long sum = 0;
        int weight = 1;

        for (int i = input.Length - 1; i >= 0; i--)
        {
            if (!char.IsDigit(input[i]))
            {
                continue;
            }

            int digit = input[i] - '0';
            weight++;

            if (weight > 10)
            {
                weight = 1;
            }

            sum += digit * weight;
        }

        return sum % 11 == 0;
    }

    /// <summary>
    /// Calculates the Modulo 97 remainder of a number string.
    /// </summary>
    /// <param name="input">The numeric string.</param>
    /// <returns>The remainder of the division by 97.</returns>
    public static int CalculateModulo97(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return -1;
        }

        long remainder = 0;
        foreach (char c in input)
        {
            if (!char.IsDigit(c))
            {
                return -1;
            }

            int digit = c - '0';
            remainder = (remainder * 10 + digit) % 97;
        }

        return (int)remainder;
    }

    /// <summary>
    /// Validates a number string using the ISO 7064 Modulo 97 algorithm (used for IBANs).
    /// </summary>
    /// <param name="input">The numeric string to validate.</param>
    /// <returns>True if the input is valid according to Modulo 97; otherwise, false.</returns>
    public static bool ValidateModulo97(string input)
    {
        int remainder = CalculateModulo97(input);
        return remainder == 1;
    }

    /// <summary>
    /// Validates a number string using the ISO 7064 Modulo 11, 10 algorithm.
    /// </summary>
    /// <param name="input">The numeric string to validate.</param>
    /// <returns>True if the input is valid; otherwise, false.</returns>
    public static bool ValidateISO7064Mod11_10(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return false;
        }

        int product = 10;
        for (int i = 0; i < input.Length - 1; i++)
        {
            if (!char.IsDigit(input[i]))
            {
                return false;
            }

            int digit = input[i] - '0';
            int sum = (digit + product) % 10;
            if (sum == 0) sum = 10;
            product = (2 * sum) % 11;
        }

        int checkDigit = 11 - product;
        if (checkDigit == 10) checkDigit = 0;

        int lastDigit = input[^1] - '0';
        return checkDigit == lastDigit;
    }

    /// <summary>
    /// Calculates the weighted Modulo 11 sum of a number string.
    /// </summary>
    /// <param name="input">The numeric string.</param>
    /// <param name="weights">The weights to apply.</param>
    /// <returns>The sum of products.</returns>
    public static int CalculateWeightedSum(string input, int[] weights)
    {
        if (string.IsNullOrWhiteSpace(input) || weights == null || input.Length != weights.Length)
        {
            return -1;
        }

        int sum = 0;
        for (int i = 0; i < input.Length; i++)
        {
            if (!char.IsDigit(input[i]))
            {
                return -1;
            }
            sum += (input[i] - '0') * weights[i];
        }
        return sum;
    }

    /// <summary>
    /// Calculates the weighted Modulo 11 remainder.
    /// </summary>
    /// <param name="input">The numeric string.</param>
    /// <param name="weights">The weights to apply.</param>
    /// <returns>The remainder of the weighted sum divided by 11.</returns>
    public static int CalculateWeightedModulo11(string input, int[] weights)
    {
        int sum = CalculateWeightedSum(input, weights);
        if (sum == -1) return -1;
        return sum % 11;
    }

    /// <summary>
    /// Validates a number string using a weighted Modulo 11 algorithm with a custom remainder validator.
    /// </summary>
    /// <param name="input">The numeric string.</param>
    /// <param name="weights">The weights to apply.</param>
    /// <param name="remainderValidator">A function that takes the remainder and returns true if valid.</param>
    /// <returns>True if valid, false otherwise.</returns>
    public static bool ValidateWeightedModulo11(string input, int[] weights, Func<int, bool> remainderValidator)
    {
        int remainder = CalculateWeightedModulo11(input, weights);
        if (remainder == -1) return false;
        return remainderValidator(remainder);
    }

    /// <summary>
    /// Calculates a weighted sum where products > 9 have their digits summed (Luhn-style).
    /// Used for Austria VAT.
    /// </summary>
    /// <param name="input">The numeric string.</param>
    /// <param name="weights">The weights to apply.</param>
    /// <returns>The calculated sum.</returns>
    public static int CalculateLuhnStyleWeightedSum(string input, int[] weights)
    {
        if (string.IsNullOrWhiteSpace(input) || weights == null || input.Length != weights.Length)
        {
            return -1;
        }

        int sum = 0;
        for (int i = 0; i < input.Length; i++)
        {
            if (!char.IsDigit(input[i]))
            {
                return -1;
            }
            int digit = input[i] - '0';
            int product = digit * weights[i];

            if (product > 9)
            {
                sum += (product / 10) + (product % 10);
            }
            else
            {
                sum += product;
            }
        }
        return sum;
    }

    /// <summary>
    /// Converts a string containing letters to a string of digits using a specified mapping.
    /// Default mapping: A=1, B=2...
    /// </summary>
    /// <param name="input">The input string.</param>
    /// <returns>The converted string.</returns>
    public static string ConvertLettersToDigits(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return string.Empty;

        var sb = new System.Text.StringBuilder();
        foreach (char c in input)
        {
            if (char.IsDigit(c))
            {
                sb.Append(c);
            }
            else if (char.IsLetter(c))
            {
                sb.Append(char.ToUpperInvariant(c) - 'A' + 1);
            }
        }
        return sb.ToString();
    }

    /// <summary>
    /// Checks if the numeric string is divisible by the divisor.
    /// </summary>
    /// <param name="input">The numeric string.</param>
    /// <param name="divisor">The divisor.</param>
    /// <returns>True if divisible, false otherwise.</returns>
    public static bool IsDivisibleBy(string input, int divisor)
    {
        if (string.IsNullOrWhiteSpace(input) || divisor == 0)
        {
            return false;
        }

        long remainder = 0;
        foreach (char c in input)
        {
            if (!char.IsDigit(c))
            {
                return false;
            }
            int digit = c - '0';
            remainder = (remainder * 10 + digit) % divisor;
        }
        return remainder == 0;
    }
}
