using Finova.Countries.Europe.BosniaAndHerzegovina.Models;
using Finova.Countries.Europe.BosniaAndHerzegovina.Services;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.BosniaAndHerzegovina.Services;

public class BosniaAndHerzegovinaIbanParserTests
{
    private readonly BosniaAndHerzegovinaIbanParser _parser;

    public BosniaAndHerzegovinaIbanParserTests()
    {
        _parser = BosniaAndHerzegovinaIbanParser.Create();
    }

    [Fact]
    public void ParseIban_WithValidIban_ReturnsIbanDetails()
    {
        var iban = "BA783930000000000000";
        var details = _parser.ParseIban(iban);

        details.Should().NotBeNull();
        details.Should().BeOfType<BosniaAndHerzegovinaIbanDetails>();

        var bosniaDetails = (BosniaAndHerzegovinaIbanDetails)details!;
        bosniaDetails.CountryCode.Should().Be("BA");
        bosniaDetails.BrojBanke.Should().Be("393");
        bosniaDetails.BrojFilijale.Should().Be("000");
        bosniaDetails.BrojRacuna.Should().Be("00000000");
        bosniaDetails.KontrolniBroj.Should().Be("00");
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
