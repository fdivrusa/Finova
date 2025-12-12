using Finova.Countries.Europe.Albania.Models;
using Finova.Countries.Europe.Albania.Services;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Albania.Services;

public class AlbaniaIbanParserTests
{
    private readonly AlbaniaIbanParser _parser;

    public AlbaniaIbanParserTests()
    {
        _parser = AlbaniaIbanParser.Create();
    }

    [Fact]
    public void ParseIban_WithValidIban_ReturnsIbanDetails()
    {
        var iban = "AL96100100010000000000000001";
        var details = _parser.ParseIban(iban);

        details.Should().NotBeNull();
        details.Should().BeOfType<AlbaniaIbanDetails>();

        var albaniaDetails = (AlbaniaIbanDetails)details!;
        albaniaDetails.CountryCode.Should().Be("AL");
        albaniaDetails.KodiBankes.Should().Be("100");
        albaniaDetails.KodiDeges.Should().Be("1000");
        albaniaDetails.ShifraKontrollit.Should().Be("1");
        albaniaDetails.NumriLlogarise.Should().Be("0000000000000001");
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

