using System.Diagnostics.CodeAnalysis;
using Finova.Core.Accounts;
using Finova.Core.Interfaces;

namespace Finova.Countries.Europe.SanMarino.Validators;

public class SanMarinoIbanValidator : IIbanValidator
{
    public string CountryCode => "SM";
    private const int SmIbanLength = 27;
    private const string SmCountryCode = "SM";

    // Same OddValues table as Italy
    private static readonly int[] OddValues =
    [
        1, 0, 5, 7, 9, 13, 15, 17, 19, 21, 2, 4, 18, 20, 11, 3, 6, 8, 12, 14, 16, 10, 22, 25, 24, 23
    ];

    public bool IsValidIban(string? iban)
    {
        return ValidateSanMarinoIban(iban);
    }

    public static bool ValidateSanMarinoIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban)) return false;
        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != SmIbanLength) return false;
        if (!normalized.StartsWith(SmCountryCode, StringComparison.OrdinalIgnoreCase)) return false;

        // CIN (Pos 4) check
        if (!char.IsLetter(normalized[4])) return false;

        // ABI & CAB (Pos 5-15) check
        for (int i = 5; i < 15; i++)
        {
            if (!char.IsDigit(normalized[i])) return false;
        }

        // CIN Validation (Same algorithm as Italy)
        if (!ValidateCin(normalized)) return false;

        return IbanHelper.IsValidIban(normalized);
    }

    private static bool ValidateCin(string iban)
    {
        char expectedCin = char.ToUpperInvariant(iban[4]);
        string bbanPart = iban.Substring(5, 22); // ABI + CAB + Account
        int sum = 0;

        for (int i = 0; i < bbanPart.Length; i++)
        {
            char c = char.ToUpperInvariant(bbanPart[i]);
            bool isOddPosition = (i % 2) == 0; // 0-based index means 1st char is Odd

            int charValue;
            if (isOddPosition)
            {
                if (char.IsDigit(c)) charValue = OddValues[c - '0'];
                else charValue = OddValues[c - 'A'];
            }
            else
            {
                if (char.IsDigit(c)) charValue = c - '0';
                else charValue = c - 'A';
            }
            sum += charValue;
        }
        return (char)('A' + (sum % 26)) == expectedCin;
    }
}
