using System.Diagnostics.CodeAnalysis;
using Finova.Core.Iban;
using Finova.Core.Common;

namespace Finova.Countries.Europe.Italy.Validators;

/// <summary>
/// Validator for Italian bank accounts.
/// Italy IBAN format: IT + 2 check + 1 CIN + 5 ABI + 5 CAB + 12 Account (27 characters total).
/// </summary>
public class ItalyIbanValidator : IIbanValidator
{
    public string CountryCode => "IT";

    private const int ItalyIbanLength = 27;
    private const string ItalyCountryCode = "IT";

    /// <summary>
    /// Table for Odd positions values in CIN calculation.
    /// Mapping: A=1, B=0, C=5, D=7 ... Z=23.
    /// Digits 0-9 map to the same values as A-J.
    /// </summary>
    private static readonly int[] OddValues =
    [
        1, 0, 5, 7, 9, 13, 15, 17, 19, 21, 2, 4, 18, 20, 11, 3, 6, 8, 12, 14, 16, 10, 22, 25, 24, 23
    ];

    #region Instance Methods (for Dependency Injection)

    public ValidationResult Validate(string? iban) => ValidateItalyIban(iban);

    #endregion

    #region Static Methods (for Direct Usage)

    /// <summary>
    /// Validates an Italian IBAN.
    /// </summary>
    /// <param name="iban">The IBAN to validate.</param>
    public static ValidationResult ValidateItalyIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, "IBAN cannot be empty.");
        }
        var normalized = IbanHelper.NormalizeIban(iban);

        // 1. Check length (Italy IBAN is exactly 27 characters)
        if (normalized.Length != ItalyIbanLength)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, $"Invalid length. Expected {ItalyIbanLength}, got {normalized.Length}.");
        }

        // 2. Check country code
        if (!normalized.StartsWith(ItalyCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, "Invalid country code. Expected IT.");
        }

        // 3. Structure Validation:

        // CIN (Pos 4) must be a letter
        if (!char.IsLetter(normalized[4]))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Italy CIN must be a letter.");
        }

        // ABI (Pos 5-9) and CAB (Pos 10-14) must be digits
        // ABI maps to Bank Code, CAB maps to Branch Code
        for (int i = 5; i < 15; i++)
        {
            if (!char.IsDigit(normalized[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Italy ABI/CAB must be digits.");
            }
        }

        // Account Number (Pos 15-26) is usually numeric but legally alphanumeric in Italy
        for (int i = 15; i < 27; i++)
        {
            if (!char.IsLetterOrDigit(normalized[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Italy Account Number must be alphanumeric.");
            }
        }

        // 4. Validate CIN (Control Internal Number) - Specific to Italy
        // Checks the consistency of ABI + CAB + Account
        if (!ValidateCin(normalized))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, "Invalid CIN.");
        }

        // 5. Global Checksum (Modulo 97)
        return IbanHelper.IsValidIban(normalized)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, "Invalid checksum.");
    }

    /// <summary>
    /// Validates the CIN (Control Internal Number).
    /// The CIN is the character at index 4.
    /// It is calculated based on the BBAN (ABI + CAB + Account).
    /// </summary>
    private static bool ValidateCin(string iban)
    {
        // CIN is at index 4
        char expectedCin = char.ToUpperInvariant(iban[4]);

        // BBAN starts at index 5 (ABI + CAB + Account)
        // Length of BBAN logic inputs: 5 (ABI) + 5 (CAB) + 12 (Account) = 22 characters
        string bbanPart = iban.Substring(5, 22);

        int sum = 0;

        for (int i = 0; i < bbanPart.Length; i++)
        {
            char c = char.ToUpperInvariant(bbanPart[i]);

            // Determine if position is Odd or Even.
            // Note: The algorithm uses 1-based indexing for "Odd/Even".
            // So index 0 (1st char) is ODD. Index 1 (2nd char) is EVEN.
            bool isOddPosition = (i % 2) == 0;

            int charValue;

            if (isOddPosition)
            {
                // Odd position logic: Map using the OddValues table
                if (char.IsDigit(c))
                {
                    // '0' -> maps to index 0 ('A'), '1' -> maps to index 1 ('B')
                    charValue = OddValues[c - '0'];
                }
                else
                {
                    // 'A' -> index 0, 'B' -> index 1
                    charValue = OddValues[c - 'A'];
                }
            }
            else
            {
                // Even position logic:
                // Digits count as their value.
                // Letters map to: A=0, B=1 ... Z=25
                if (char.IsDigit(c))
                {
                    charValue = c - '0';
                }
                else
                {
                    charValue = c - 'A';
                }
            }

            sum += charValue;
        }

        // Calculate remainder
        int remainder = sum % 26;

        // Convert remainder to letter (0='A', 1='B'...)
        char calculatedCin = (char)('A' + remainder);

        return calculatedCin == expectedCin;
    }

    #endregion
}
