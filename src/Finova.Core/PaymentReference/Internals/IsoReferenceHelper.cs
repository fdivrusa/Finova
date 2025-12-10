using System.Text;

namespace Finova.Core.PaymentReference.Internals;

public static class IsoReferenceHelper
{
    private const string IsoPrefix = "RF";
    private const string PlaceholderCheckDigits = "00";

    public static string Generate(string rawReference)
    {
        if (string.IsNullOrWhiteSpace(rawReference))
        {
            throw new ArgumentException("Raw reference cannot be empty for ISO generation.", nameof(rawReference));
        }

        var referenceBody = rawReference.Trim().ToUpperInvariant();

        var checkString = new StringBuilder()
            .Append(referenceBody)
            .Append(IsoPrefix)
            .Append(PlaceholderCheckDigits)
            .ToString();

        var numericString = ConvertLettersToDigits(checkString);

        var mod = Modulo97Helper.Calculate(numericString);

        var checkValue = 98 - mod;

        var finalCheckDigits = checkValue.ToString("00");

        return $"{IsoPrefix}{finalCheckDigits}{referenceBody}";
    }

    private static string ConvertLettersToDigits(string input)
    {
        var sb = new StringBuilder(input.Length * 2);
        foreach (var c in input)
        {
            if (char.IsDigit(c))
            {
                sb.Append(c);
            }
            else if (c >= 'A' && c <= 'Z')
            {
                sb.Append(c - 55);
            }
        }
        return sb.ToString();
    }
}
