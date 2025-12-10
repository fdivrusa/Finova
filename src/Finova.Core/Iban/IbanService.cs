using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;

namespace Finova.Core.Iban;

/// <summary>
/// Service for handling IBAN operations including validation, formatting, and parsing.
/// This service can be injected with a specific <see cref="IIbanValidator"/> to provide
/// enhanced validation logic (e.g., country-specific rules).
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="IbanService"/> class.
/// </remarks>
/// <param name="validator">Optional specific validator to use for validation logic.</param>
public class IbanService(IIbanValidator? validator = null) : IIbanService
{
    private readonly IIbanValidator? _validator = validator;

    /// <summary>
    /// Gets the country code this service is associated with.
    /// Returns null as this is a generic service, unless a specific validator is injected.
    /// </summary>
    public string? CountryCode => _validator?.CountryCode;

    /// <summary>
    /// Validates the specified IBAN.
    /// If a specific validator was injected, it delegates validation to it.
    /// Otherwise, it performs basic structure and checksum validation.
    /// </summary>
    /// <param name="iban">The IBAN to validate.</param>
    /// <returns>A <see cref="ValidationResult"/> indicating success or failure.</returns>
    public ValidationResult Validate(string? iban)
    {
        if (_validator != null)
        {
            return _validator.Validate(iban);
        }

        if (IbanHelper.IsValidIban(iban))
        {
            return ValidationResult.Success();
        }
        return ValidationResult.Failure(ValidationErrorCode.InvalidIban, "Invalid IBAN.");
    }

    /// <summary>
    /// Formats the IBAN by adding spaces every 4 characters.
    /// </summary>
    /// <param name="iban">The IBAN to format.</param>
    /// <returns>The formatted IBAN string.</returns>
    public string FormatIban(string? iban)
    {
        return IbanHelper.FormatIban(iban);
    }

    /// <summary>
    /// Normalizes the IBAN by removing spaces and converting to uppercase.
    /// </summary>
    /// <param name="iban">The IBAN to normalize.</param>
    /// <returns>The normalized IBAN string.</returns>
    public string NormalizeIban(string? iban)
    {
        return IbanHelper.NormalizeIban(iban);
    }

    /// <summary>
    /// Extracts the country code from the IBAN.
    /// </summary>
    /// <param name="iban">The IBAN.</param>
    /// <returns>The two-letter country code.</returns>
    public string GetCountryCode(string? iban)
    {
        return IbanHelper.GetCountryCode(iban);
    }

    /// <summary>
    /// Extracts the check digits from the IBAN.
    /// </summary>
    /// <param name="iban">The IBAN.</param>
    /// <returns>The check digits as an integer.</returns>
    public int GetCheckDigits(string? iban)
    {
        return IbanHelper.GetCheckDigits(iban);
    }

    /// <summary>
    /// Validates the checksum of the IBAN using the Modulo 97 algorithm.
    /// </summary>
    /// <param name="iban">The IBAN to validate.</param>
    /// <returns>True if the checksum is valid; otherwise, false.</returns>
    public bool ValidateChecksum([NotNullWhen(true)] string? iban)
    {
        return IbanHelper.ValidateChecksum(iban);
    }
}
