using Finova.Core.Common;

namespace Finova.Core.Vat;

public interface IVatValidator : IValidator<VatDetails>
{
    /// <summary>
    /// ISO country code (ex: "BE", "FR")
    /// </summary>
    string CountryCode { get; }
}
