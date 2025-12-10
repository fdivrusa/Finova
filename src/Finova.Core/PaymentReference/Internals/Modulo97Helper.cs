namespace Finova.Core.PaymentReference.Internals;

public static class Modulo97Helper
{
    /// <summary>
    /// Calculates the modulo 97 of a numeric string.
    /// </summary>
    public static int Calculate(string numericString)
    {
        if (string.IsNullOrWhiteSpace(numericString))
        {
            throw new ArgumentException("Input cannot be null or empty.", nameof(numericString));
        }

        int remainder = 0;
        foreach (char c in numericString)
        {
            if (!char.IsDigit(c))
            {
                throw new ArgumentException("Input must contain only digits.", nameof(numericString));
            }

            int digit = c - '0';
            remainder = (remainder * 10 + digit) % 97;
        }

        return remainder;
    }
}
