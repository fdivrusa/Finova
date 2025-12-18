using Finova.Core.Common;
using Finova.Core.Identifiers;
using System.Diagnostics.CodeAnalysis;

namespace Finova.Countries.Europe.Italy.Validators;

/// <summary>
/// Validator for Italian BBAN (Basic Bank Account Number).
/// </summary>
public class ItalyBbanValidator : IBbanValidator
{
    public string CountryCode => "IT";

    /// <summary>
    /// Table for Odd positions values in CIN calculation.
    /// Mapping: A=1, B=0, C=5, D=7 ... Z=23.
    /// Digits 0-9 map to the same values as A-J.
    /// </summary>
    private static readonly int[] OddValues =
    [
        1, 0, 5, 7, 9, 13, 15, 17, 19, 21, 2, 4, 18, 20, 11, 3, 6, 8, 12, 14, 16, 10, 22, 25, 24, 23
    ];

    ValidationResult IValidator<string>.Validate(string? input) => Validate(input);

    /// <summary>
    /// Validates the Italian BBAN structure and checksum (CIN).
    /// </summary>
    /// <param name="bban">The BBAN string (23 characters: 1 CIN + 5 ABI + 5 CAB + 12 Account).</param>
    /// <returns>A ValidationResult indicating success or failure.</returns>
    public static ValidationResult Validate(string? bban)
    {
        bban = InputSanitizer.Sanitize(bban);

        if (string.IsNullOrWhiteSpace(bban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        // Length check: CIN (1) + ABI (5) + CAB (5) + Account (12) = 23
        if (bban.Length != 23)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, string.Format(ValidationMessages.InvalidLengthExpectedXGotY, 23, bban.Length));
        }

        // Structure Validation:
        // CIN (Pos 0) must be a letter
        if (!char.IsLetter(bban[0]))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.ItalyCinMustBeLetter);
        }

        // ABI (Pos 1-5) and CAB (Pos 6-10) must be digits
        for (int i = 1; i < 11; i++)
        {
            if (!char.IsDigit(bban[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.ItalyAbiCabMustBeDigits);
            }
        }

        // Account Number (Pos 11-22) is usually numeric but legally alphanumeric in Italy
        for (int i = 11; i < 23; i++)
        {
            if (!char.IsLetterOrDigit(bban[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.ItalyAccountNumberMustBeAlphanumeric);
            }
        }

        // Validate CIN
        if (!ValidateCin(bban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidCin);
        }

        return ValidationResult.Success();
    }

    /// <summary>
    /// Validates the CIN (Control Internal Number).
    /// The CIN is the character at index 0 of the BBAN.
    /// It is calculated based on the rest of the BBAN (ABI + CAB + Account).
    /// </summary>
    private static bool ValidateCin(string bban)
    {
        // CIN is at index 0
        char expectedCin = char.ToUpperInvariant(bban[0]);

        // BBAN payload starts at index 1 (ABI + CAB + Account)
        // Length of BBAN logic inputs: 5 (ABI) + 5 (CAB) + 12 (Account) = 22 characters
        ReadOnlySpan<char> bbanPart = bban.AsSpan(1, 22);

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

    /// <inheritdoc/>
    public string? Parse(string? input)
    {
        if (string.IsNullOrWhiteSpace(input)) return null;
        string sanitized = input.Replace(" ", "").Replace("-", "").Trim();
        return Validate(sanitized).IsValid ? sanitized : null;
    }
}
