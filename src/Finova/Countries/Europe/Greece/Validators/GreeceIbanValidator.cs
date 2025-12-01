using System.Diagnostics.CodeAnalysis;
using Finova.Core.Accounts;
using Finova.Core.Interfaces;

namespace Finova.Countries.Europe.Greece.Validators;

public class GreeceIbanValidator : IIbanValidator
{
    public string CountryCode => "GR";
    private const int GreeceIbanLength = 27;

    public bool IsValidIban(string? iban) => ValidateGreeceIban(iban);

    public static bool ValidateGreeceIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return false;
        }
        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != GreeceIbanLength)
        {
            return false;
        }

        if (!normalized.StartsWith("GR", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }


        // Bank Code (Pos 4-6) and Branch Code (Pos 7-10) must be digits
        // Range: 4 to 11
        for (int i = 4; i < 11; i++)
        {
            if (!char.IsDigit(normalized[i]))
            {
                return false;

            }
        }

        // Account Number (Pos 11-26) can be Alphanumeric
        // Range: 11 to 27
        for (int i = 11; i < GreeceIbanLength; i++)
        {
            if (!char.IsLetterOrDigit(normalized[i]))
            {
                return false;
            }
        }

        // 4. Checksum Validation (Modulo 97)
        return IbanHelper.IsValidIban(normalized);
    }
}
