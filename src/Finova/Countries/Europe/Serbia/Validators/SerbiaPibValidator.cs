using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Enterprise;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Serbia.Validators;

public partial class SerbiaPibValidator : ITaxIdValidator
{
    [GeneratedRegex(@"^\d{9}$")]
    private static partial Regex PibRegex();

    public string CountryCode => "RS";
    public EnterpriseNumberType Type => EnterpriseNumberType.SerbiaPib;

    public ValidationResult Validate(string? number) => ValidatePib(number);

    public string? Parse(string? number) => Normalize(number);

    public static ValidationResult ValidatePib(string? number)
    {
        if (string.IsNullOrWhiteSpace(number))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var cleaned = number.ToUpperInvariant().Replace("RS", "").Replace(" ", "");

        if (!PibRegex().IsMatch(cleaned))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidSerbiaPibFormat);
        }

        // ISO 7064 Modulo 11, 10 - Recursive
        int m = 10;
        for (int i = 0; i < 8; i++)
        {
            int digit = cleaned[i] - '0';
            m = (m + digit) % 10;
            if (m == 0) m = 10;
            m = (m * 2) % 11;
        }

        int checkDigit = (11 - m) % 10;
        int lastDigit = cleaned[8] - '0';

        if (checkDigit != lastDigit)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidSerbiaPibChecksum);
        }

        return ValidationResult.Success();
    }

    public string? Normalize(string? number)
    {
        if (string.IsNullOrWhiteSpace(number)) return null;
        var cleaned = number.ToUpperInvariant().Replace("RS", "").Replace(" ", "");
        return PibRegex().IsMatch(cleaned) ? cleaned : null;
    }
}
