using System.Diagnostics.CodeAnalysis;
using Finova.Core.Accounts;
using Finova.Core.Interfaces;

namespace Finova.Countries.Europe.Gibraltar.Validators;

public class GibraltarIbanValidator : IIbanValidator
{
    public string CountryCode => "GI";
    private const int GibraltarIbanLength = 23;
    private const string GibraltarCountryCode = "GI";

    public bool IsValidIban(string? iban)
    {
        return ValidateGibraltarIban(iban);
    }

    public static bool ValidateGibraltarIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return false;
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != GibraltarIbanLength)
        {
            return false;
        }

        if (!normalized.StartsWith(GibraltarCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        // Structure check:
        // Bank Code (4-8) is usually letters (BIC)
        for (int i = 4; i < 8; i++)
        {
            if (!char.IsLetter(normalized[i]))
            {
                return false;
            }
        }

        // Account (8-23) is alphanumeric
        for (int i = 8; i < GibraltarIbanLength; i++)
        {
            if (!char.IsLetterOrDigit(normalized[i]))
            {
                return false;
            }
        }

        return IbanHelper.IsValidIban(normalized);
    }
}
