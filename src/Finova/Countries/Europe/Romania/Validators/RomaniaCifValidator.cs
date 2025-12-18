using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Enterprise;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Romania.Validators;

public partial class RomaniaCifValidator : ITaxIdValidator
{
    [GeneratedRegex(@"^\d{2,10}$")]
    private static partial Regex CifRegex();

    private static readonly int[] Weights = { 7, 5, 3, 2, 1, 7, 5, 3, 2 };

    public string CountryCode => "RO";
    public EnterpriseNumberType Type => EnterpriseNumberType.RomaniaCif;

    public ValidationResult Validate(string? number) => ValidateCif(number);

    public string? Parse(string? number) => Normalize(number);

    public static ValidationResult ValidateCif(string? number)
    {
        if (string.IsNullOrWhiteSpace(number))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var cleaned = number.ToUpperInvariant().Replace("RO", "").Replace(" ", "");

        if (!CifRegex().IsMatch(cleaned))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidRomaniaCifFormat);
        }

        // Logic:
        // The last digit is the Control Digit.
        // Take all preceding digits (Data).
        // Reverse the Data digits.
        // Apply weights [7, 5, 3, 2, 1, 7, 5, 3, 2] to the reversed data.

        string dataPart = cleaned[..^1];
        int checkDigit = cleaned[^1] - '0';

        // Reverse dataPart
        char[] charArray = dataPart.ToCharArray();
        System.Array.Reverse(charArray);

        int sum = 0;
        for (int i = 0; i < charArray.Length; i++)
        {
            int digit = charArray[i] - '0';
            // Weights are applied cyclically or just up to length? 
            // The prompt implies the weights are fixed for the max length (9 data digits).
            // Since max length is 10 (9 data + 1 check), we won't exceed weights array.
            sum += digit * Weights[i];
        }

        int calculated = (sum * 10) % 11;
        if (calculated == 10)
        {
            calculated = 0;
        }

        if (calculated != checkDigit)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidRomaniaCifChecksum);
        }

        return ValidationResult.Success();
    }

    public string? Normalize(string? number)
    {
        if (string.IsNullOrWhiteSpace(number)) return null;
        var cleaned = number.ToUpperInvariant().Replace("RO", "").Replace(" ", "");
        return CifRegex().IsMatch(cleaned) ? cleaned : null;
    }
}
