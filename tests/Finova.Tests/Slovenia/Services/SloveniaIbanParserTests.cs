using Finova.Countries.Europe.Slovenia.Models;
using Finova.Countries.Europe.Slovenia.Services;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Slovenia.Services;

public class SloveniaIbanParserTests
{
    private readonly SloveniaIbanParser _parser;

    public SloveniaIbanParserTests()
    {
        _parser = SloveniaIbanParser.Create();
    }

    [Fact]
    public void ParseIban_WithValidIban_ReturnsIbanDetails()
    {
        var iban = "SI56192001234567892";
        var details = _parser.ParseIban(iban);

        details.Should().NotBeNull();
        details.Should().BeOfType<SloveniaIbanDetails>();

        var sloveniaDetails = (SloveniaIbanDetails)details!;
        sloveniaDetails.CountryCode.Should().Be("SI");
        sloveniaDetails.StevilkaBanke.Should().Be("19200");
        sloveniaDetails.StevilkaRacuna.Should().Be("12345678");
        sloveniaDetails.KontrolnaStevilka.Should().Be("92");
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
