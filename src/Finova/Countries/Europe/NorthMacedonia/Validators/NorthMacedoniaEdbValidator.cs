using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Enterprise;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.NorthMacedonia.Validators;

public partial class NorthMacedoniaEdbValidator : ITaxIdValidator
{
    [GeneratedRegex(@"^\d{13}$")]
    private static partial Regex EdbRegex();

    private static readonly int[] Weights = { 7, 6, 5, 4, 3, 2, 7, 6, 5, 4, 3, 2 };

    public string CountryCode => "MK";
    public EnterpriseNumberType Type => EnterpriseNumberType.NorthMacedoniaEdb;

    public ValidationResult Validate(string? number) => ValidateEdb(number);

    public string? Parse(string? number) => Normalize(number);

    public static ValidationResult ValidateEdb(string? number)
    {
        if (string.IsNullOrWhiteSpace(number))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var cleaned = number.Replace(" ", "");

        if (!EdbRegex().IsMatch(cleaned))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidNorthMacedoniaEdbFormat);
        }

        int sum = ChecksumHelper.CalculateWeightedSum(cleaned[..12], Weights);
        int remainder = sum % 11;
        int checkDigit = remainder switch
        {
            0 => 0,
            1 => -1, // Invalid
            _ => 11 - remainder
        };

        if (checkDigit == -1)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidNorthMacedoniaEdbChecksumForbiddenRemainder);
        }

        if (checkDigit != (cleaned[12] - '0'))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidNorthMacedoniaEdbChecksum);
        }

        return ValidationResult.Success();
    }

    public string? Normalize(string? number)
    {
        if (string.IsNullOrWhiteSpace(number)) return null;
        var cleaned = number.Replace(" ", "");
        return EdbRegex().IsMatch(cleaned) ? cleaned : null;
    }
}
