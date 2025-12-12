using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Enterprise;

namespace Finova.Countries.Europe.Austria.Validators;

/// <summary>
/// Validator for Austrian Firmenbuchnummer (Commercial Register Number).
/// Format: 1-6 digits followed by a check letter (e.g., 123456x).
/// Often prefixed with FN (e.g., FN 123456x).
/// </summary>
public partial class AustriaFirmenbuchValidator : IEnterpriseValidator
{
    private const string CountryCodePrefix = "AT";

    [GeneratedRegex(@"^(?:FN\s?)?(\d{1,6})([a-zA-Z])$")]
    private static partial Regex FirmenbuchRegex();

    public string CountryCode => CountryCodePrefix;

    public ValidationResult Validate(string? number) => ValidateFirmenbuch(number);

    public string? Parse(string? number) => number?.Trim();

    public static ValidationResult ValidateFirmenbuch(string? number)
    {
        if (string.IsNullOrWhiteSpace(number))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, "Firmenbuchnummer cannot be empty.");
        }

        var normalized = number.Trim();

        var match = FirmenbuchRegex().Match(normalized);
        if (!match.Success)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Invalid format. Expected 1-6 digits followed by a letter (optional FN prefix).");
        }

        // TODO: Implement checksum algorithm if available.
        // The last character is a check letter, but the algorithm is not publicly documented in detail.

        return ValidationResult.Success();
    }
}
