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
    /// <param name="input">The input string to validate.</param>
    /// <returns>A <see cref="ValidationResult"/>.</returns>
    /// <example>
    /// <code>
    /// var result = validator.Validate("BE0123456789");
    /// </code>
    /// </example>
    ValidationResult Validate(string? input);

    /// <summary>
    /// Parses the input string into a detailed object.
    /// Returns null if the input is invalid.
    /// </summary>
    /// <param name="input">The input string to parse.</param>
    /// <returns>The parsed details object or null.</returns>
    /// <example>
    /// <code>
    /// var details = validator.Parse("BE0123456789");
    /// if (details != null)
    /// {
    ///     Console.WriteLine(details.VatNumber);
    /// }
    /// </code>
    /// </example>
    T? Parse(string? input);
}
