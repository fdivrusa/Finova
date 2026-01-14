using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.PaymentReference;
using Finova.Core.PaymentReference.Internals;


namespace Finova.Countries.Europe.Slovenia.Services;

/// <summary>
/// Service for generating and validating Slovenian payment references (SI12).
/// Uses Modulo 97 algorithm.
/// </summary>
public partial class SloveniaPaymentReferenceService : IPaymentReferenceGenerator
{
    public string CountryCode => "SI";

    [GeneratedRegex(@"[^\d]")]
    private static partial Regex DigitsOnlyRegex();

    /// <summary>
    /// Generates a Slovenian SI12 reference.
    /// Format: SI12 + Reference + CheckDigits (Mod 97).
    /// </summary>
    public string Generate(string rawReference, PaymentReferenceFormat format = PaymentReferenceFormat.LocalSlovenia) => format switch
    {
        PaymentReferenceFormat.LocalSlovenia => GenerateSi12(rawReference),
        PaymentReferenceFormat.IsoRf => IsoReferenceHelper.Generate(rawReference),
        _ => throw new NotSupportedException(string.Format(ValidationMessages.UnsupportedFormat, format))
    };

    public PaymentReferenceDetails Parse(string reference)
    {
        var validation = ValidateStatic(reference);
        if (!validation.IsValid)
        {
            return new PaymentReferenceDetails
            {
                Reference = reference,
                Content = string.Empty,
                Format = PaymentReferenceFormat.Unknown,
                IsValid = false
            };
        }

        if (reference.Trim().StartsWith("RF", StringComparison.OrdinalIgnoreCase))
        {
            return new PaymentReferenceDetails
            {
                Reference = reference,
                Content = IsoReferenceHelper.Parse(reference),
                Format = PaymentReferenceFormat.IsoRf,
                IsValid = true
            };
        }

        // SI12
        // Format: SI12 + Reference + CheckDigits (2)
        var digits = DigitsOnlyRegex().Replace(reference.Substring(4), ""); // Remove SI12 prefix
        var data = digits[..^2];

        return new PaymentReferenceDetails
        {
            Reference = reference,
            Content = data,
            Format = PaymentReferenceFormat.LocalSlovenia,
            IsValid = true
        };
    }

    #region Static Methods (for Direct Usage)

    /// <summary>
    /// Generates a Slovenian SI12 reference.
    /// </summary>
    public static string GenerateStatic(string rawReference) => GenerateSi12(rawReference);

    /// <summary>
    /// Validates a Slovenian SI12 reference.
    /// </summary>
    public static Core.Common.ValidationResult ValidateStatic(string communication)
    {
        if (string.IsNullOrWhiteSpace(communication))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        if (communication.Trim().StartsWith("RF", StringComparison.OrdinalIgnoreCase))
        {
            return IsoReferenceValidator.Validate(communication);
        }

        return ValidateSi12(communication);
    }

    #endregion

    private static string GenerateSi12(string rawReference)
    {
        var cleanRef = DigitsOnlyRegex().Replace(rawReference, "");

        if (string.IsNullOrEmpty(cleanRef))
        {
            throw new ArgumentException(ValidationMessages.InputCannotBeEmpty);
        }

        // Calculate Mod 97 on the reference
        var mod = Modulo97Helper.Calculate(cleanRef);
        var checkDigitValue = mod == 0 ? 97 : mod;
        var checkDigits = checkDigitValue.ToString("00");

        return $"SI12{cleanRef}{checkDigits}";
    }

    private static ValidationResult ValidateSi12(string communication)
    {
        if (string.IsNullOrWhiteSpace(communication))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        // Must start with SI12
        if (!communication.StartsWith("SI12"))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidSloveniaReferencePrefix);
        }

        var digits = DigitsOnlyRegex().Replace(communication.Substring(4), ""); // Remove SI12 prefix

        if (digits.Length < 3) // At least 1 digit data + 2 check digits
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidSloveniaReferenceLength);
        }

        var data = digits[..^2];
        var checkDigitStr = digits.Substring(digits.Length - 2);

        var mod = Modulo97Helper.Calculate(data);
        var expectedCheckDigitValue = mod == 0 ? 97 : mod;

        return checkDigitStr == expectedCheckDigitValue.ToString("00")
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidCheckDigit, ValidationMessages.InvalidCheckDigit);
    }
}

