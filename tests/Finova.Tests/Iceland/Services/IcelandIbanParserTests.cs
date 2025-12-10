using Finova.Countries.Europe.Iceland.Models;
using Finova.Countries.Europe.Iceland.Services;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Iceland.Services;

public class IcelandIbanParserTests
{
    private readonly IcelandIbanParser _parser;

    public IcelandIbanParserTests()
    {
        _parser = IcelandIbanParser.Create();
    }

    [Fact]
    public void ParseIban_WithValidIban_ReturnsIbanDetails()
    {
        var iban = "IS750001121234563108962099";
        var details = _parser.ParseIban(iban);

        details.Should().NotBeNull();
        details.Should().BeOfType<IcelandIbanDetails>();

        var icelandDetails = (IcelandIbanDetails)details!;
        icelandDetails.CountryCode.Should().Be("IS");
        icelandDetails.Banki.Should().Be("0001");
        icelandDetails.Hb.Should().Be("12");
        icelandDetails.Reikningsnumer.Should().Be("123456");
        icelandDetails.Kennitala.Should().Be("3108962099");
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
