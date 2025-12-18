using System.Text;
using Finova.Core.Common;

namespace Finova.Core.PaymentReference.Internals;

public static class IsoReferenceValidator
{
    private const string IsoPrefix = "RF";

    /// <summary>
    /// Validates an ISO 11649 reference by checking the RF prefix and the Modulo 97 checksum.
    /// </summary>
    public static ValidationResult Validate(string reference)
    {
        if (string.IsNullOrWhiteSpace(reference))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var cleanReference = reference.Replace(" ", "").ToUpperInvariant();

        if (!cleanReference.StartsWith(IsoPrefix))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidIsoReferencePrefix);
        }

        if (cleanReference.Length < 6)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidIsoReferenceLength);
        }

        var bodyAndPrefix = string.Concat(cleanReference.AsSpan(4), cleanReference.AsSpan(0, 4));

        var numericReference = ConvertLettersToDigits(bodyAndPrefix);

        var modResult = Modulo97Helper.Calculate(numericReference);

        return modResult == 1
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidCheckDigit, ValidationMessages.InvalidCheckDigit);
    }

    private static string ConvertLettersToDigits(string input)
    {
        var sb = new StringBuilder(input.Length * 2);
        foreach (var c in input)
        {
            if (char.IsDigit(c))
            {
                sb.Append(c);
            }
            else if (c >= 'A' && c <= 'Z')
            {
                sb.Append(c - 55);
            }
        }
        return sb.ToString();
    }
}
