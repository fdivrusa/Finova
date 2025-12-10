using System.Diagnostics.CodeAnalysis;
using Finova.Core.Iban;
using Finova.Core.Common;

namespace Finova.Countries.Europe.Portugal.Validators;

public class PortugalIbanValidator : IIbanValidator
{
    public string CountryCode => "PT";
    private const int PortugalIbanLength = 25;

    public ValidationResult Validate(string? iban) => ValidatePortugalIban(iban);

    public static ValidationResult ValidatePortugalIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, "IBAN cannot be empty.");
        }
        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != PortugalIbanLength)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, $"Invalid length. Expected {PortugalIbanLength}, got {normalized.Length}.");
        }


        if (!normalized.StartsWith("PT", StringComparison.OrdinalIgnoreCase))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, "Invalid country code. Expected PT.");
        }


        for (int i = 2; i < PortugalIbanLength; i++)
        {
            if (!char.IsDigit(normalized[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Portugal IBAN must contain only digits after the country code.");
            }
        }

        // Extract the parts relevant to NIB (Positions 4 to 25)
        // Bank (4) + Branch (4) + Account (11) = 19 digits Body
        // Check (2) = last 2 digits
        var nibBody = normalized.Substring(4, 19);
        var nibKey = normalized.Substring(23, 2);

        if (!ValidateNibKey(nibBody, nibKey))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, "Invalid NIB Key.");
        }

        return IbanHelper.IsValidIban(normalized)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, "Invalid checksum.");
    }

    /// <summary>
    /// Validates the specific Portuguese NIB control digits (Algarismos de Controlo).
    /// Algorithm: 98 - (Body % 97)
    /// </summary>
    private static bool ValidateNibKey(string nibBody, string expectedKey)
    {
        if (!decimal.TryParse(nibBody, out decimal bodyValue))
        {
            return false;
        }

        // Calculate remainder
        var remainder = (int)(bodyValue % 97);

        // Calculate check digit
        var checkValue = 98 - remainder;

        // Format to 2 digits (e.g., 7 becomes "07")
        var calculatedKey = checkValue.ToString("00");

        return calculatedKey == expectedKey;
    }
}
