using Finova.Core.Common;
using Finova.Core.Iban;
using Finova.Core.Bic;
using System.Collections.Concurrent;

namespace Finova.Services.Global;

public record BatchValidationResult(string Input, bool IsValid, ValidationResult Result, string? SanitizedValue);

/// <summary>
/// Service for high-performance batch validation of financial identifiers.
/// Designed for SaaS and bulk processing scenarios.
/// </summary>
public class BatchValidationService
{
    // We use static helpers for global validation to avoid complex DI for now.
    // In a real SaaS, you might inject specific factories.

    /// <summary>
    /// Validates a batch of IBANs in parallel.
    /// </summary>
    public IEnumerable<BatchValidationResult> ValidateIbans(IEnumerable<string> ibans)
    {
        var results = new ConcurrentBag<BatchValidationResult>();

        Parallel.ForEach(ibans, (iban) =>
        {
            // 1. Sanitize
            var sanitized = IbanHelper.NormalizeIban(iban);
            
            // 2. Validate
            // We use IbanHelper.IsValidIban for a quick check, but we want detailed errors.
            // We don't have a global "GetErrors" static method easily accessible without instantiating specific validators.
            // However, for the SaaS MVP, IsValid + Sanitized is often enough.
            // If we want detailed errors, we need to resolve the specific country validator.
            
            bool isValid = IbanHelper.IsValidIban(sanitized);
            var result = isValid ? ValidationResult.Success() : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, "Invalid IBAN");

            results.Add(new BatchValidationResult(iban, isValid, result, isValid ? sanitized : null));
        });

        return results;
    }

    /// <summary>
    /// Validates a batch of BICs in parallel.
    /// </summary>
    public IEnumerable<BatchValidationResult> ValidateBics(IEnumerable<string> bics)
    {
        var results = new ConcurrentBag<BatchValidationResult>();

        Parallel.ForEach(bics, (bic) =>
        {
            var result = BicValidator.Validate(bic);
            string? sanitized = null;
            
            if (result.IsValid)
            {
                var details = BicValidator.Parse(bic);
                sanitized = details?.Bic;
            }

            results.Add(new BatchValidationResult(bic, result.IsValid, result, sanitized));
        });

        return results;
    }
}
