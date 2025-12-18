using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.SanMarino.Validators;

/// <summary>
/// Validator for San Marino BBAN (Basic Bank Account Number).
/// </summary>
public class SanMarinoBbanValidator : IBbanValidator
{
    public string CountryCode => "SM";
    ValidationResult IValidator<string>.Validate(string? input) => Validate(input ?? "");

    // Same OddValues table as Italy
    private static readonly int[] OddValues =
    [
        1, 0, 5, 7, 9, 13, 15, 17, 19, 21, 2, 4, 18, 20, 11, 3, 6, 8, 12, 14, 16, 10, 22, 25, 24, 23
    ];

    /// <summary>
    /// Validates the San Marino BBAN structure and checksum (CIN).
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
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidSanMarinoCin);
        }

        // ABI (Pos 1-5) and CAB (Pos 6-10) must be digits
        for (int i = 1; i < 11; i++)
        {
            if (!char.IsDigit(bban[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidSanMarinoAbiCab);
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
    /// </summary>
    /// <param name="bban">The BBAN to validate.</param>
    /// <returns>True if valid, false otherwise.</returns>
    private static bool ValidateCin(string bban)
    {
        char expectedCin = char.ToUpperInvariant(bban[0]);
        string bbanPart = bban.Substring(1, 22); // ABI + CAB + Account
        int sum = 0;

        for (int i = 0; i < bbanPart.Length; i++)
        {
            char c = char.ToUpperInvariant(bbanPart[i]);
            bool isOddPosition = (i % 2) == 0; // 0-based index means 1st char is Odd

            int charValue;
            if (isOddPosition)
            {
                if (char.IsDigit(c))
                {
                    charValue = OddValues[c - '0'];
                }
                else
                {
                    charValue = OddValues[c - 'A'];
                }
            }
            else
            {
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
        return (char)('A' + (sum % 26)) == expectedCin;
    }

    /// <inheritdoc/>
    public string? Parse(string? input)
    {
        return Validate(input).IsValid ? input : null;
    }
}
