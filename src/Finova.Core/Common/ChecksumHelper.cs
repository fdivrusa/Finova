using System.Numerics;

namespace Finova.Core.Common;

/// <summary>
/// Provides common checksum validation algorithms.
/// </summary>
/// <remarks>
/// ⚠️ **Advanced Usage Only**
/// This class is intended for developers who need to implement custom validators or use specific algorithms directly.
/// For standard validation (IBAN, VAT, Enterprise Numbers), use the high-level wrappers (e.g., <see cref="Finova.Services.EuropeEnterpriseValidator"/>).
/// </remarks>
public static class ChecksumHelper
{
    // Verhoeff Algorithm Tables
    private static readonly int[,] VerhoeffD = {
        {0, 1, 2, 3, 4, 5, 6, 7, 8, 9},
        {1, 2, 3, 4, 0, 6, 7, 8, 9, 5},
        {2, 3, 4, 0, 1, 7, 8, 9, 5, 6},
        {3, 4, 0, 1, 2, 8, 9, 5, 6, 7},
        {4, 0, 1, 2, 3, 9, 5, 6, 7, 8},
        {5, 9, 8, 7, 6, 0, 4, 3, 2, 1},
        {6, 5, 9, 8, 7, 1, 0, 4, 3, 2},
        {7, 6, 5, 9, 8, 2, 1, 0, 4, 3},
        {8, 7, 6, 5, 9, 3, 2, 1, 0, 4},
        {9, 8, 7, 6, 5, 4, 3, 2, 1, 0}
    };

    private static readonly int[,] VerhoeffP = {
        {0, 1, 2, 3, 4, 5, 6, 7, 8, 9},
        {1, 5, 7, 6, 2, 8, 3, 0, 9, 4},
        {5, 8, 0, 3, 7, 9, 6, 1, 4, 2},
        {8, 9, 1, 6, 0, 4, 3, 5, 2, 7},
        {9, 4, 5, 3, 1, 2, 6, 8, 7, 0},
        {4, 2, 8, 6, 5, 7, 3, 9, 0, 1},
        {2, 7, 9, 3, 8, 0, 6, 4, 1, 5},
        {7, 0, 4, 6, 9, 1, 3, 2, 5, 8}
    };

    /// <summary>
    /// Validates a number string using the Luhn algorithm (Modulo 10).
    /// </summary>
    /// <param name="input">The numeric string to validate.</param>
    /// <returns>True if the input is valid according to the Luhn algorithm; otherwise, false.</returns>
    public static bool ValidateLuhn(ReadOnlySpan<char> input)
    {
        if (input.IsEmpty || input.IsWhiteSpace())
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
    /// Validates a number string using the Luhn algorithm (Modulo 10).
    /// </summary>
    /// <param name="input">The numeric string to validate.</param>
    /// <returns>True if the input is valid according to the Luhn algorithm; otherwise, false.</returns>
    public static bool ValidateLuhn(string input) => ValidateLuhn(input.AsSpan());

    /// <summary>
    /// Calculates the Luhn check digit for a given numeric string.
    /// </summary>
    /// <param name="input">The numeric string (without check digit).</param>
    /// <returns>The calculated check digit (0-9).</returns>
    public static int CalculateLuhnCheckDigit(ReadOnlySpan<char> input)
    {
        if (input.IsEmpty || input.IsWhiteSpace())
        {
            throw new ArgumentException(ValidationMessages.InputCannotBeEmpty, nameof(input));
        }

        int sum = 0;
        bool doubleDigit = true;

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

        return (10 - (sum % 10)) % 10;
    }

    /// <summary>
    /// Calculates the Luhn check digit for a given numeric string.
    /// </summary>
    /// <param name="input">The numeric string (without check digit).</param>
    /// <returns>The calculated check digit (0-9).</returns>
    public static int CalculateLuhnCheckDigit(string input) => CalculateLuhnCheckDigit(input.AsSpan());

    /// <summary>
    /// Validates a number string using the Modulo 11 algorithm with weights 1, 2, 3... 10.
    /// </summary>
    /// <param name="input">The numeric string to validate.</param>
    /// <returns>True if the input is valid according to Modulo 11; otherwise, false.</returns>
    public static bool ValidateModulo11(ReadOnlySpan<char> input)
    {
        if (input.IsEmpty || input.IsWhiteSpace())
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
    /// Validates a number string using the Modulo 11 algorithm with weights 1, 2, 3... 10.
    /// </summary>
    /// <param name="input">The numeric string to validate.</param>
    /// <returns>True if the input is valid according to Modulo 11; otherwise, false.</returns>
    public static bool ValidateModulo11(string input) => ValidateModulo11(input.AsSpan());

    /// <summary>
    /// Calculates the Modulo 97 remainder of a number string.
    /// </summary>
    /// <param name="input">The numeric string.</param>
    /// <returns>The remainder of the division by 97.</returns>
    public static int CalculateModulo97(ReadOnlySpan<char> input)
    {
        if (input.IsEmpty || input.IsWhiteSpace())
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
    /// Calculates the Modulo 97 remainder of a number string.
    /// </summary>
    /// <param name="input">The numeric string.</param>
    /// <returns>The remainder of the division by 97.</returns>
    public static int CalculateModulo97(string input) => CalculateModulo97(input.AsSpan());

    /// <summary>
    /// Validates a number string using the ISO 7064 Modulo 97 algorithm (used for IBANs).
    /// </summary>
    /// <param name="input">The numeric string to validate.</param>
    /// <returns>True if the input is valid according to Modulo 97; otherwise, false.</returns>
    public static bool ValidateModulo97(ReadOnlySpan<char> input)
    {
        int remainder = CalculateModulo97(input);
        return remainder == 1;
    }

    /// <summary>
    /// Validates a number string using the ISO 7064 Modulo 97 algorithm (used for IBANs).
    /// </summary>
    /// <param name="input">The numeric string to validate.</param>
    /// <returns>True if the input is valid according to Modulo 97; otherwise, false.</returns>
    public static bool ValidateModulo97(string input) => ValidateModulo97(input.AsSpan());

    /// <summary>
    /// Validates a number string using the ISO 7064 Modulo 11, 10 algorithm.
    /// </summary>
    /// <param name="input">The numeric string to validate.</param>
    /// <returns>True if the input is valid; otherwise, false.</returns>
    public static bool ValidateISO7064Mod11_10(ReadOnlySpan<char> input)
    {
        if (input.IsEmpty || input.IsWhiteSpace())
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
    /// Validates a number string using the ISO 7064 Modulo 11, 10 algorithm.
    /// </summary>
    /// <param name="input">The numeric string to validate.</param>
    /// <returns>True if the input is valid; otherwise, false.</returns>
    public static bool ValidateISO7064Mod11_10(string input) => ValidateISO7064Mod11_10(input.AsSpan());

    /// <summary>
    /// Calculates the weighted Modulo 11 sum of a number string.
    /// </summary>
    /// <param name="input">The numeric string.</param>
    /// <param name="weights">The weights to apply.</param>
    /// <returns>The sum of products.</returns>
    public static int CalculateWeightedSum(ReadOnlySpan<char> input, int[] weights)
    {
        if (input.IsEmpty || input.IsWhiteSpace() || weights == null || input.Length != weights.Length)
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
    /// Calculates the weighted Modulo 11 sum of a number string.
    /// </summary>
    /// <param name="input">The numeric string.</param>
    /// <param name="weights">The weights to apply.</param>
    /// <returns>The sum of products.</returns>
    public static int CalculateWeightedSum(string input, int[] weights) => CalculateWeightedSum(input.AsSpan(), weights);

    /// <summary>
    /// Calculates the weighted Modulo 11 remainder.
    /// </summary>
    /// <param name="input">The numeric string.</param>
    /// <param name="weights">The weights to apply.</param>
    /// <returns>The remainder of the weighted sum divided by 11.</returns>
    public static int CalculateWeightedModulo11(ReadOnlySpan<char> input, int[] weights)
    {
        int sum = CalculateWeightedSum(input, weights);
        if (sum == -1) return -1;
        return sum % 11;
    }

    /// <summary>
    /// Calculates the weighted Modulo 11 remainder.
    /// </summary>
    /// <param name="input">The numeric string.</param>
    /// <param name="weights">The weights to apply.</param>
    /// <returns>The remainder of the weighted sum divided by 11.</returns>
    public static int CalculateWeightedModulo11(string input, int[] weights) => CalculateWeightedModulo11(input.AsSpan(), weights);

    /// <summary>
    /// Validates a number string using a weighted Modulo 11 algorithm with a custom remainder validator.
    /// </summary>
    /// <param name="input">The numeric string.</param>
    /// <param name="weights">The weights to apply.</param>
    /// <param name="remainderValidator">A function that takes the remainder and returns true if valid.</param>
    /// <returns>True if valid, false otherwise.</returns>
    public static bool ValidateWeightedModulo11(ReadOnlySpan<char> input, int[] weights, Func<int, bool> remainderValidator)
    {
        int remainder = CalculateWeightedModulo11(input, weights);
        if (remainder == -1) return false;
        return remainderValidator(remainder);
    }

    /// <summary>
    /// Validates a number string using a weighted Modulo 11 algorithm with a custom remainder validator.
    /// </summary>
    /// <param name="input">The numeric string.</param>
    /// <param name="weights">The weights to apply.</param>
    /// <param name="remainderValidator">A function that takes the remainder and returns true if valid.</param>
    /// <returns>True if valid, false otherwise.</returns>
    public static bool ValidateWeightedModulo11(string input, int[] weights, Func<int, bool> remainderValidator) => ValidateWeightedModulo11(input.AsSpan(), weights, remainderValidator);

    /// <summary>
    /// Calculates a weighted sum where products > 9 have their digits summed (Luhn-style).
    /// Used for Austria VAT.
    /// </summary>
    /// <param name="input">The numeric string.</param>
    /// <param name="weights">The weights to apply.</param>
    /// <returns>The calculated sum.</returns>
    public static int CalculateLuhnStyleWeightedSum(ReadOnlySpan<char> input, int[] weights)
    {
        if (input.IsEmpty || input.IsWhiteSpace() || weights == null || input.Length != weights.Length)
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

    /// <summary>
    /// Validates a number string using the Verhoeff algorithm.
    /// </summary>
    /// <param name="input">The numeric string to validate.</param>
    /// <returns>True if the input is valid according to the Verhoeff algorithm; otherwise, false.</returns>
    public static bool ValidateVerhoeff(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return false;
        }

        if (!input.All(char.IsDigit))
        {
            return false;
        }

        int c = 0;
        int[] myArray = input.Select(ch => ch - '0').Reverse().ToArray();

        for (int i = 0; i < myArray.Length; i++)
        {
            c = VerhoeffD[c, VerhoeffP[i % 8, myArray[i]]];
        }

        return c == 0;
    }
}
