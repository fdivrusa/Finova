using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.MiddleEast.Israel.Validators;

/// <summary>
/// Validates Israeli Teudat Zehut (National ID) numbers.
/// </summary>
public class IsraelTeudatZehutValidator : INationalIdValidator
{
    /// <inheritdoc/>
    public string CountryCode => "IL";

    /// <inheritdoc/>
    public ValidationResult Validate(string? input)
    {
        return ValidateStatic(input);
    }

    /// <inheritdoc/>
    public string? Parse(string? input)
    {
        if (Validate(input).IsValid)
        {
            return input?.Trim().PadLeft(9, '0');
        }
        return null;
    }

    /// <summary>
    /// Validates an Israeli Teudat Zehut number.
    /// </summary>
    /// <param name="idNumber">The ID number (up to 9 digits).</param>
    /// <returns>A ValidationResult indicating success or failure.</returns>
    public static ValidationResult ValidateStatic(string? idNumber)
    {
        if (string.IsNullOrWhiteSpace(idNumber))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        if (!long.TryParse(idNumber, out _))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidFormat);
        }

        // Pad with zeros to 9 digits
        var paddedId = idNumber.Trim().PadLeft(9, '0');

        if (paddedId.Length != 9)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidLength);
        }

        if (!ChecksumHelper.ValidateLuhn(paddedId))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
        }

        return ValidationResult.Success();
    }
}
