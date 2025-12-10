namespace Finova.Core.Common;

/// <summary>
/// Represents the error codes for validation failures.
/// </summary>
public enum ValidationErrorCode
{
    /// <summary>
    /// The input is null or empty.
    /// </summary>
    InvalidInput,

    /// <summary>
    /// The IBAN format is invalid (e.g. contains invalid characters).
    /// </summary>
    InvalidFormat,

    /// <summary>
    /// The IBAN checksum is invalid.
    /// </summary>
    InvalidChecksum,

    /// <summary>
    /// The country code is invalid or not supported.
    /// </summary>
    InvalidCountryCode,

    /// <summary>
    /// The IBAN length is invalid for the specific country.
    /// </summary>
    InvalidLength,

    /// <summary>
    /// The IBAN is invalid (general error).
    /// </summary>
    InvalidIban,

    /// <summary>
    /// The country code is not supported.
    /// </summary>
    UnsupportedCountry,

    /// <summary>
    /// The check digits are invalid.
    /// </summary>
    InvalidCheckDigit
}
