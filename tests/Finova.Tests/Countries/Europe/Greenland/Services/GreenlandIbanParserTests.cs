using Finova.Countries.Europe.Greenland.Models;
using Finova.Countries.Europe.Greenland.Services;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Greenland.Services;

public class GreenlandIbanParserTests
{
    private readonly GreenlandIbanParser _parser;

    public GreenlandIbanParserTests()
    {
        _parser = GreenlandIbanParser.Create();
    }

    [Fact]
    public void ParseIban_WithValidIban_ReturnsIbanDetails()
    {
        var iban = "GL5360000000000001";
        var details = _parser.ParseIban(iban);

        details.Should().NotBeNull();
        details.Should().BeOfType<GreenlandIbanDetails>();

        var greenlandDetails = (GreenlandIbanDetails)details!;
        greenlandDetails.CountryCode.Should().Be("GL");
        greenlandDetails.BankKode.Should().Be("6000");
        greenlandDetails.KontoNummer.Should().Be("0000000001");
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

