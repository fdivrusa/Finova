using Finova.Countries.Europe.Andorra.Models;
using Finova.Countries.Europe.Andorra.Services;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Andorra.Services;

public class AndorraIbanParserTests
{
    private readonly AndorraIbanParser _parser;

    public AndorraIbanParserTests()
    {
        _parser = AndorraIbanParser.Create();
    }

    [Fact]
    public void ParseIban_WithValidIban_ReturnsIbanDetails()
    {
        var iban = "AD1400080001001234567890";
        var details = _parser.ParseIban(iban);

        details.Should().NotBeNull();
        details.Should().BeOfType<AndorraIbanDetails>();

        var andorraDetails = (AndorraIbanDetails)details!;
        andorraDetails.CountryCode.Should().Be("AD");
        andorraDetails.CodiEntitat.Should().Be("0008");
        andorraDetails.CodiOficina.Should().Be("0001");
        andorraDetails.NumeroCompte.Should().Be("001234567890");
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

