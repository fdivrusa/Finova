using Finova.Countries.Europe.SanMarino.Models;
using Finova.Countries.Europe.SanMarino.Services;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.SanMarino.Services;

public class SanMarinoIbanParserTests
{
    private readonly SanMarinoIbanParser _parser;

    public SanMarinoIbanParserTests()
    {
        _parser = SanMarinoIbanParser.Create();
    }

    [Fact]
    public void ParseIban_WithValidIban_ReturnsIbanDetails()
    {
        var iban = "SM76P0854009812123456789123";
        var details = _parser.ParseIban(iban);

        details.Should().NotBeNull();
        details.Should().BeOfType<SanMarinoIbanDetails>();

        var sanMarinoDetails = (SanMarinoIbanDetails)details!;
        sanMarinoDetails.CountryCode.Should().Be("SM");
        sanMarinoDetails.Cin.Should().Be("P");
        sanMarinoDetails.Abi.Should().Be("08540");
        sanMarinoDetails.Cab.Should().Be("09812");
        sanMarinoDetails.NumeroConto.Should().Be("123456789123");
    }

    [Fact]
    public void ParseIban_WithNullIban_ReturnsNull()
    {
        _parser.ParseIban(null).Should().BeNull();
    }

    [Fact]
    public void ParseIban_WithInvalidIban_ReturnsNull()
    {
        _parser.ParseIban("invalid").Should().BeNull();
    }
}

