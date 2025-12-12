using Finova.Countries.Europe.Liechtenstein.Models;
using Finova.Countries.Europe.Liechtenstein.Services;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Liechtenstein.Services;

public class LiechtensteinIbanParserTests
{
    private readonly LiechtensteinIbanParser _parser;

    public LiechtensteinIbanParserTests()
    {
        _parser = LiechtensteinIbanParser.Create();
    }

    [Fact]
    public void ParseIban_WithValidIban_ReturnsIbanDetails()
    {
        var iban = "LI2200000000000000001";
        var details = _parser.ParseIban(iban);

        details.Should().NotBeNull();
        details.Should().BeOfType<LiechtensteinIbanDetails>();

        var liechtensteinDetails = (LiechtensteinIbanDetails)details!;
        liechtensteinDetails.CountryCode.Should().Be("LI");
        liechtensteinDetails.Bankleitzahl.Should().Be("00000");
        liechtensteinDetails.Kontonummer.Should().Be("000000000001");
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

