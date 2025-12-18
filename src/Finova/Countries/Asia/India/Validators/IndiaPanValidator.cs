using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Asia.India.Validators;

/// <summary>
/// Validates Indian Permanent Account Number (PAN).
/// </summary>
public partial class IndiaPanValidator : ITaxIdValidator
{
    [GeneratedRegex(@"^[A-Z]{5}\d{4}[A-Z]$")]
    private static partial Regex PanRegex();

    /// <inheritdoc/>
    public string CountryCode => "IN";

    /// <inheritdoc/>
    public ValidationResult Validate(string? input)
    {
        return ValidatePan(input);
    }

    /// <inheritdoc/>
    public string? Parse(string? input)
    {
        if (Validate(input).IsValid)
        {
            return input?.Trim().ToUpperInvariant();
        }
        return null;
    }

    /// <summary>
    /// Validates an Indian PAN.
    /// </summary>
    /// <param name="pan">The PAN string (10 characters).</param>
    /// <returns>A ValidationResult indicating success or failure.</returns>
    public static ValidationResult ValidatePan(string? pan)
    {
        if (string.IsNullOrWhiteSpace(pan))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var clean = pan.Trim().ToUpperInvariant();

        if (clean.Length != 10)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidIndiaPanLength);
        }

        if (!PanRegex().IsMatch(clean))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidIndiaPanFormat);
        }

        // 4th character validation (Status)
        // P: Individual, C: Company, H: HUF, A: AOP, B: BOI, G: Govt, J: Artificial Juridical Person, L: Local Authority, F: Firm, T: Trust
        char status = clean[3];
        string validStatuses = "PCHABGJLFT";
        if (!validStatuses.Contains(status))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidIndiaPanStatus);
        }

        return ValidationResult.Success();
    }
}
