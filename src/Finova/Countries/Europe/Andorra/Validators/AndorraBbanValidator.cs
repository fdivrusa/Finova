using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Andorra.Validators;

public class AndorraBbanValidator : IBbanValidator
{
    public string CountryCode => "AD";
    ValidationResult IValidator<string>.Validate(string? input) => Validate(input);

    public static ValidationResult Validate(string? bban)
    {
        bban = InputSanitizer.Sanitize(bban);

        if (string.IsNullOrWhiteSpace(bban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }
        // BBAN format: 8 alphanumeric (Bank + Branch) + 12 alphanumeric (Account)
        // Total length: 20 characters

        if (bban.Length != 20)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidBbanLength);
        }

        // Structure check: Alphanumeric
        for (int i = 0; i < 20; i++)
        {
            if (!char.IsLetterOrDigit(bban[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, string.Format(ValidationMessages.InvalidIbanFormatAlphanumeric, "Andorra"));
            }
        }

        return ValidationResult.Success();
    }

    /// <inheritdoc/>
    public string? Parse(string? input)
    {
        return Validate(input).IsValid ? input : null;
    }

    /// <summary>
    /// Parses the Andorran BBAN and returns the structured details.
    /// </summary>
    /// <param name="bban">The BBAN string to parse.</param>
    /// <returns>A BbanDetails object if valid; otherwise, null.</returns>
    public BbanDetails? ParseDetails(string? bban)
    {
        bban = InputSanitizer.Sanitize(bban);

        if (!Validate(bban).IsValid)
        {
            return null;
        }

        // BBAN format: 4 alphanumeric (Bank) + 4 alphanumeric (Branch) + 12 alphanumeric (Account)
        return new BbanDetails
        {
            Bban = bban!,
            CountryCode = CountryCode,
            BankCode = bban!.Substring(0, 4),
            BranchCode = bban.Substring(4, 4),
            AccountNumber = bban.Substring(8, 12)
        };
    }
}
