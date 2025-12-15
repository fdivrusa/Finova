using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Enterprise;

namespace Finova.Countries.Europe.Norway.Validators;

public partial class NorwayOrgNumberValidator : IEnterpriseValidator
{
    [GeneratedRegex(@"^\d{9}$")]
    private static partial Regex OrgRegex();

    private static readonly int[] Weights = [3, 2, 7, 6, 5, 4, 3, 2];

    public string CountryCode => "NO";
    public static EnterpriseNumberType Type => EnterpriseNumberType.NorwayOrgNumber;

    public ValidationResult Validate(string? number) => ValidateOrgNumber(number);

    public string? Parse(string? number) => Normalize(number);

    public static ValidationResult ValidateOrgNumber(string? number)
    {
        if (string.IsNullOrWhiteSpace(number))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, "Empty.");
        }

        var cleaned = number.ToUpperInvariant().Replace("NO", "").Replace(" ", "");

        if (!OrgRegex().IsMatch(cleaned))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Invalid Norway Org Number format.");
        }

        int sum = ChecksumHelper.CalculateWeightedSum(cleaned[..8], Weights);
        int remainder = sum % 11;
        int checkDigit = remainder switch
        {
            0 => 0,
            1 => -1, // Invalid
            _ => 11 - remainder
        };

        if (checkDigit == -1)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, "Invalid Norway Org Number checksum (forbidden remainder).");
        }

        if (checkDigit != (cleaned[8] - '0'))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, "Invalid Norway Org Number checksum.");
        }

        return ValidationResult.Success();
    }

    public static string? Normalize(string? number)
    {
        if (string.IsNullOrWhiteSpace(number)) return null;
        var cleaned = number.ToUpperInvariant().Replace("NO", "").Replace(" ", "");
        return OrgRegex().IsMatch(cleaned) ? cleaned : null;
    }
}
