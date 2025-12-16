using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Enterprise;

namespace Finova.Countries.Europe.Italy.Validators;

/// <summary>
/// Validator for Italian Codice Fiscale (Tax Code) and Partita IVA (VAT Number).
/// </summary>
public partial class ItalyCodiceFiscaleValidator : IEnterpriseValidator
{
    [GeneratedRegex(@"^[A-Z]{6}\d{2}[A-Z]\d{2}[A-Z]\d{3}[A-Z]$")]
    private static partial Regex CodiceFiscaleRegex();

    [GeneratedRegex(@"^\d{11}$")]
    private static partial Regex PartitaIvaRegex();

    [GeneratedRegex(@"[^\w]")]
    private static partial Regex AlphanumericOnlyRegex();

    public string CountryCode => "IT";

    public ValidationResult Validate(string? instance) => ValidateItalianIdentifier(instance);

    public string? Parse(string? instance) => Normalize(instance);

    /// <summary>
    /// Validates an Italian Codice Fiscale or Partita IVA.
    /// </summary>
    public static ValidationResult ValidateItalianIdentifier(string? number)
    {
        if (string.IsNullOrWhiteSpace(number))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var normalized = Normalize(number);

        // Check length to decide validation strategy
        if (normalized.Length == 16)
        {
            return ValidateCodiceFiscale(normalized);
        }
        else if (normalized.Length == 11)
        {
            return ValidatePartitaIva(normalized);
        }
        else
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidItalianIdentifierLength);
        }
    }

    private static ValidationResult ValidateCodiceFiscale(string cf)
    {
        if (!CodiceFiscaleRegex().IsMatch(cf))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidCodiceFiscaleFormat);
        }

        // Checksum calculation
        // Odd positions (1, 3, 5...) -> Index 0, 2, 4...
        // Even positions (2, 4, 6...) -> Index 1, 3, 5...
        // Note: Official docs say "Odd" and "Even" based on 1-based index.
        // So index 0 is "Odd", index 1 is "Even".

        int sum = 0;
        for (int i = 0; i < 15; i++)
        {
            char c = cf[i];
            if ((i + 1) % 2 != 0) // Odd position
            {
                sum += GetOddValue(c);
            }
            else // Even position
            {
                sum += GetEvenValue(c);
            }
        }

        int remainder = sum % 26;
        char checkChar = (char)('A' + remainder);

        return cf[15] == checkChar
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidCheckDigit, ValidationMessages.InvalidCodiceFiscaleCheckDigit);
    }

    private static ValidationResult ValidatePartitaIva(string piva)
    {
        if (!PartitaIvaRegex().IsMatch(piva))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidPartitaIvaFormat);
        }

        // Luhn Algorithm (standard for Partita IVA)
        return ChecksumHelper.ValidateLuhn(piva)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidCheckDigit, ValidationMessages.InvalidPartitaIvaCheckDigit);
    }

    private static int GetOddValue(char c)
    {
        // Odd position values table
        return c switch
        {
            '0' => 1,
            '1' => 0,
            '2' => 5,
            '3' => 7,
            '4' => 9,
            '5' => 13,
            '6' => 15,
            '7' => 17,
            '8' => 19,
            '9' => 21,
            'A' => 1,
            'B' => 0,
            'C' => 5,
            'D' => 7,
            'E' => 9,
            'F' => 13,
            'G' => 15,
            'H' => 17,
            'I' => 19,
            'J' => 21,
            'K' => 2,
            'L' => 4,
            'M' => 18,
            'N' => 20,
            'O' => 11,
            'P' => 3,
            'Q' => 6,
            'R' => 8,
            'S' => 12,
            'T' => 14,
            'U' => 16,
            'V' => 10,
            'W' => 22,
            'X' => 25,
            'Y' => 24,
            'Z' => 23,
            _ => 0
        };
    }

    private static int GetEvenValue(char c)
    {
        // Even position values: 0-9 -> 0-9, A-Z -> 0-25
        if (char.IsDigit(c))
        {
            return c - '0';
        }

        return c - 'A';
    }

    public static string Format(string? number)
    {
        if (!ValidateItalianIdentifier(number).IsValid)
        {
            throw new ArgumentException("Invalid Input", nameof(number));
        }
        return Normalize(number);
    }

    public static string Normalize(string? number)
    {
        if (string.IsNullOrWhiteSpace(number))
        {
            return string.Empty;
        }

        return AlphanumericOnlyRegex().Replace(number, "").ToUpperInvariant();
    }
}
