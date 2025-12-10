using Finova.Core.Iban;

namespace Finova.Countries.Europe.Portugal.Models;

/// <summary>
/// Portugal-specific IBAN details.
/// PT IBAN format: PT + 2 check + 4 Banco + 4 Balcão + 11 Conta + 2 Controlo.
/// </summary>
public record PortugalIbanDetails : IbanDetails
{
    /// <summary>
    /// Código de Banco (4 digits).
    /// Bank code.
    /// Position: 5-8
    /// </summary>
    public required string CodigoBanco { get; init; }

    /// <summary>
    /// Código de Balcão (4 digits).
    /// Branch code.
    /// Position: 9-12
    /// </summary>
    public required string CodigoBalcao { get; init; }

    /// <summary>
    /// Número de Conta (11 digits).
    /// Account number.
    /// Position: 13-23
    /// </summary>
    public required string NumeroConta { get; init; }

    /// <summary>
    /// Algarismos de Controlo (2 digits).
    /// National control key specific to NIB.
    /// Position: 24-25
    /// </summary>
    public required string AlgarismosControlo { get; init; }
}
