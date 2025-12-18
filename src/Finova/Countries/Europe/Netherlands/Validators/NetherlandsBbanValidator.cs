using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Netherlands.Validators;

/// <summary>
/// Validator for Dutch BBAN (Basic Bank Account Number).
/// </summary>
public class NetherlandsBbanValidator : IBbanValidator
{
    public string CountryCode => "NL";
    ValidationResult IValidator<string>.Validate(string? input) => Validate(input ?? "");

    /// <summary>
    /// Validates the Dutch BBAN structure and checksum (Elfproef).
    /// Format: 4 Bank (Letters) + 10 Account (Digits) (Total 14 chars).
    /// Algorithm: Elfproef (11-test) on the 10-digit account number.
    /// </summary>
    /// <param name="bban">The BBAN string (14 characters).</param>
    /// <returns>A ValidationResult indicating success or failure.</returns>
    public static ValidationResult Validate(string? bban)
    {
        bban = InputSanitizer.Sanitize(bban);

        if (string.IsNullOrWhiteSpace(bban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        if (bban.Length != 14)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, string.Format(ValidationMessages.InvalidLengthExpectedXGotY, 14, bban.Length));
        }

        // Bank Code: Positions 0-3 (4 chars) must be letters
        for (int i = 0; i < 4; i++)
        {
            if (!char.IsLetter(bban[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.NetherlandsBankCodeMustBeLetters);
            }
        }

        // Account Number: Positions 4-13 (10 chars) must be digits
        for (int i = 4; i < 14; i++)
        {
            if (!char.IsDigit(bban[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.NetherlandsAccountNumberMustBeDigits);
            }
        }

        // Verify the consistency of the 10-digit account number
        ReadOnlySpan<char> accountNumber = bban.AsSpan(4, 10);
        if (!ValidateElfProef(accountNumber))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidElfproefCheck);
        }

        return ValidationResult.Success();
    }

    /// <summary>
    /// Validates the Dutch "Elfproef" (11-test) on the 10-digit account number.
    /// Algorithm: Sum of (Digit * Weight) must be divisible by 11.
    /// Weights are: 10, 9, 8, 7, 6, 5, 4, 3, 2, 1.
    /// </summary>
    private static bool ValidateElfProef(ReadOnlySpan<char> account)
    {
        int sum = 0;

        // Iterate over the 10 digits
        for (int i = 0; i < 10; i++)
        {
            int digit = account[i] - '0';

            // Weight decreases from 10 down to 1
            // Index 0 -> Weight 10
            // Index 1 -> Weight 9
            // ...
            // Index 9 -> Weight 1
            int weight = 10 - i;
            sum += digit * weight;
        }

        return sum % 11 == 0;
    }

    /// <inheritdoc/>
    public string? Parse(string? input)
    {
        return Validate(input).IsValid ? input : null;
    }
}
