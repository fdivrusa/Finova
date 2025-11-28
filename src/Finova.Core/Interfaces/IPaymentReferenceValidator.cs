using Finova.Core.Models;

namespace Finova.Core.Interfaces
{
    public interface IPaymentReferenceValidator
    {
        /// <summary>
        /// Checks if an existing RF reference is valid.
        /// </summary>
        bool IsValid(string? reference);

        /// <summary>
        /// Parses an RF reference into its components.
        /// </summary>
        PaymentReferenceDetails? Parse(string? reference);

        /// <summary>
        /// Generates a valid RF reference from a raw string.
        /// </summary>
        string Generate(string rawContent);
    }
}