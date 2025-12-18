using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Enterprise;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Poland.Validators;

public partial class PolandNipValidator : ITaxIdValidator
{
    [GeneratedRegex(@"^\d{10}$")]
    private static partial Regex NipRegex();

    private static readonly int[] Weights = { 6, 5, 7, 2, 3, 4, 5, 6, 7 };

    public string CountryCode => "PL";
    public EnterpriseNumberType Type => EnterpriseNumberType.PolandNip;

    public ValidationResult Validate(string? number) => ValidateNip(number);

    public string? Parse(string? number) => Normalize(number);

    public static ValidationResult ValidateNip(string? number)
    {
        if (string.IsNullOrWhiteSpace(number))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var cleaned = number.ToUpperInvariant().Replace("PL", "").Replace("-", "").Replace(" ", "");

        if (!NipRegex().IsMatch(cleaned))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidPolandNipFormat);
        }

        int sum = ChecksumHelper.CalculateWeightedSum(cleaned[..9], Weights);
        int remainder = sum % 11;

        if (remainder == 10)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidPolandNipChecksumForbidden);
        }

        if (remainder != (cleaned[9] - '0'))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidPolandNipChecksum);
        }

        return ValidationResult.Success();
    }

    public string? Normalize(string? number)
    {
        if (string.IsNullOrWhiteSpace(number)) return null;
        var cleaned = number.ToUpperInvariant().Replace("PL", "").Replace("-", "").Replace(" ", "");
        return NipRegex().IsMatch(cleaned) ? cleaned : null;
    }
}
