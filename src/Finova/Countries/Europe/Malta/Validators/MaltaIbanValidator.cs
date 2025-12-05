using System.Diagnostics.CodeAnalysis;
using Finova.Core.Accounts;
using Finova.Core.Interfaces;

namespace Finova.Countries.Europe.Malta.Validators;

public class MaltaIbanValidator : IIbanValidator
{
    public string CountryCode => "MT";
    private const int MaltaIbanLength = 31;
    private const string MaltaCountryCode = "MT";

    public bool IsValidIban(string? iban)
    {
        return ValidateMaltaIban(iban);
    }

    public static bool ValidateMaltaIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return false;
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != MaltaIbanLength)
        {
            return false;
        }

        if (!normalized.StartsWith(MaltaCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        // Structure Validation:

        // 1. Bank BIC (Pos 4-8): Must be letters
        for (int i = 4; i < 8; i++)
        {
            if (!char.IsLetter(normalized[i]))
            {
                return false;
            }
        }

        // 2. Sort Code (Pos 8-13): Must be digits
        for (int i = 8; i < 13; i++)
        {
            if (!char.IsDigit(normalized[i]))
            {
                return false;
            }
        }

        // 3. Account Number (Pos 13-31): Alphanumeric
        for (int i = 13; i < 31; i++)
        {
            if (!char.IsLetterOrDigit(normalized[i]))
            {
                return false;
            }
        }

        return IbanHelper.IsValidIban(normalized);
    }
}
