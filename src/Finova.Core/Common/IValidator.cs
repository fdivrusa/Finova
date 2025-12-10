namespace Finova.Core.Common;

/// <summary>
/// Generic interface for validating and parsing financial identifiers.
/// </summary>
/// <typeparam name="T">The type of details object returned by Parse.</typeparam>
public interface IValidator<out T>
{
    /// <summary>
    /// Validates the input string and returns detailed results.
    /// </summary>
    ValidationResult Validate(string? input);

    /// <summary>
    /// Parses the input string into a detailed object.
    /// Returns null if the input is invalid.
    /// </summary>
    T? Parse(string? input);
}
