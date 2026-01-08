using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Netherlands.Validators;

/// <summary>
/// Validator for Netherlands KvK (Kamer van Koophandel) numbers.
/// KvK numbers are 8-digit registration numbers assigned to businesses by the Dutch Chamber of Commerce.
/// Format: 8 digits (e.g., 12345678)
/// </summary>
public partial class NetherlandsKvkValidator : ITaxIdValidator
{
    private const string CountryPrefix = "NL";

    public string CountryCode => CountryPrefix;

    public ValidationResult Validate(string? instance) => ValidateKvk(instance);

    public string? Parse(string? instance)
    {
        var normalized = Normalize(instance);
        return ValidateKvk(normalized).IsValid ? normalized : null;
    }

    /// <summary>
    /// Validates a Netherlands KvK number.
    /// Format: 8 digits
    /// Note: There is no checksum algorithm for KvK numbers - validation is format-based only.
    /// </summary>
    public static ValidationResult ValidateKvk(string? kvk)
    {
        if (string.IsNullOrWhiteSpace(kvk))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var cleaned = Normalize(kvk);

        if (string.IsNullOrEmpty(cleaned))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidNetherlandsKvkFormat);
        }

        // KvK number must be exactly 8 digits
        if (cleaned.Length != 8)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidNetherlandsKvkFormat);
        }

        // Validate all characters are digits
        if (!long.TryParse(cleaned, out _))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidNetherlandsKvkFormat);
        }

        // KvK numbers cannot start with 0
        if (cleaned[0] == '0')
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidNetherlandsKvkFormat);
        }

        return ValidationResult.Success();
    }

    /// <summary>
    /// Normalizes a KvK number by removing whitespace and non-numeric characters.
    /// </summary>
    public static string Normalize(string? number)
    {
        if (string.IsNullOrWhiteSpace(number))
        {
            return string.Empty;
        }

        // Remove all whitespace and separators
        var cleaned = number.Trim().ToUpperInvariant()
            .Replace(" ", "")
            .Replace("-", "")
            .Replace(".", "");

        // Remove NL prefix if present
        if (cleaned.StartsWith(CountryPrefix))
        {
            cleaned = cleaned[2..];
        }

        // Remove KVK prefix if present (sometimes written as "KVK 12345678")
        if (cleaned.StartsWith("KVK"))
        {
            cleaned = cleaned[3..].TrimStart();
        }

        return cleaned;
    }
}
