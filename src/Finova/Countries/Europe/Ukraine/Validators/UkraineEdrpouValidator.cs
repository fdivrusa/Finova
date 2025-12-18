using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Ukraine.Validators;

public partial class UkraineEdrpouValidator : ITaxIdValidator
{
    [GeneratedRegex(@"^\d{8}$")]
    private static partial Regex EdrpouRegex();

    private static readonly int[] Weights1 = [1, 2, 3, 4, 5, 6, 7];
    private static readonly int[] Weights2 = [3, 4, 5, 6, 7, 8, 9];

    public string CountryCode => "UA";
    public ValidationResult Validate(string? number) => ValidateEdrpou(number);

    public string? Parse(string? number) => Normalize(number);

    public static ValidationResult ValidateEdrpou(string? number)
    {
        if (string.IsNullOrWhiteSpace(number))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var cleaned = number.ToUpperInvariant().Replace("UA", "").Replace(" ", "");

        if (!EdrpouRegex().IsMatch(cleaned))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidUkraineEdrpouFormat);
        }

        // Pass 1
        int sum = ChecksumHelper.CalculateWeightedSum(cleaned[..7], Weights1);
        int remainder = sum % 11;
        int checkDigit;
        if (remainder < 10)
        {
            checkDigit = remainder;
        }
        else
        {
            // Pass 2
            sum = ChecksumHelper.CalculateWeightedSum(cleaned[..7], Weights2);
            remainder = sum % 11;

            if (remainder < 10)
            {
                checkDigit = remainder;
            }
            else
            {
                checkDigit = 0;
            }
        }

        if (checkDigit != (cleaned[7] - '0'))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidUkraineEdrpouChecksum);
        }

        return ValidationResult.Success();
    }

    public static string? Normalize(string? number)
    {
        if (string.IsNullOrWhiteSpace(number)) return null;
        var cleaned = number.ToUpperInvariant().Replace("UA", "").Replace(" ", "");
        return EdrpouRegex().IsMatch(cleaned) ? cleaned : null;
    }
}
