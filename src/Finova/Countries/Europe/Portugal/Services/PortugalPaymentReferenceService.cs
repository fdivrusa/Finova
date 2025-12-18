using Finova.Core.Common;
using Finova.Core.PaymentReference;
using Finova.Core.PaymentReference.Internals;
using System.Text.RegularExpressions;

namespace Finova.Countries.Europe.Portugal.Services;

/// <summary>
/// Service for generating and validating Portuguese payment references (Multibanco).
/// </summary>
public partial class PortugalPaymentReferenceService : IPaymentReferenceGenerator
{
    public string CountryCode => "PT";

    [GeneratedRegex(@"[^\d]")]
    private static partial Regex DigitsOnlyRegex();

    public string Generate(string rawReference, PaymentReferenceFormat format = PaymentReferenceFormat.LocalPortugal) => format switch
    {
        PaymentReferenceFormat.LocalPortugal => GenerateMultibancoRef(rawReference),
        PaymentReferenceFormat.IsoRf => IsoReferenceHelper.Generate(rawReference),
        _ => throw new NotSupportedException($"Format {format} is not supported by {CountryCode}")
    };

    #region Static Methods

    public static string GenerateStatic(string rawReference) => GenerateMultibancoRef(rawReference);

    public static ValidationResult ValidateStatic(string communication)
    {
        if (string.IsNullOrWhiteSpace(communication))
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);

        if (communication.Trim().StartsWith("RF", StringComparison.OrdinalIgnoreCase))
            return IsoReferenceValidator.Validate(communication);

        return ValidateMultibancoRef(communication);
    }

    #endregion

    private static string GenerateMultibancoRef(string rawReference)
    {
        var cleanRef = DigitsOnlyRegex().Replace(rawReference, "");
        if (cleanRef.Length > 9) throw new ArgumentException("Multibanco reference cannot exceed 9 digits.");

        // Pad to 9 digits
        return cleanRef.PadLeft(9, '0');
    }

    private static ValidationResult ValidateMultibancoRef(string communication)
    {
        var clean = DigitsOnlyRegex().Replace(communication, "");

        // Multibanco references are strictly 9 digits
        if (clean.Length != 9)
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, string.Format(ValidationMessages.InvalidLengthExpectedX, 9));

        return ValidationResult.Success();
    }

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

        // Multibanco
        var clean = DigitsOnlyRegex().Replace(reference, "");

        return new PaymentReferenceDetails
        {
            Reference = reference,
            Content = clean,
            Format = PaymentReferenceFormat.LocalPortugal,
            IsValid = true
        };
    }
}
