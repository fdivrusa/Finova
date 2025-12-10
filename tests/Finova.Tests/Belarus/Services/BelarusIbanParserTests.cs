using Finova.Countries.Europe.Belarus.Models;
using Finova.Countries.Europe.Belarus.Services;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Belarus.Services;

public class BelarusIbanParserTests
{
    private readonly BelarusIbanParser _parser;

    public BelarusIbanParserTests()
    {
        _parser = BelarusIbanParser.Create();
    }

    [Fact]
    public void ParseIban_WithValidIban_ReturnsIbanDetails()
    {
        var iban = "BY29BAPB30000000000000000001";
        var details = _parser.ParseIban(iban);

        details.Should().NotBeNull();
        details.Should().BeOfType<BelarusIbanDetails>();

        var belarusDetails = (BelarusIbanDetails)details!;
        belarusDetails.CountryCode.Should().Be("BY");
        belarusDetails.KodBanku.Should().Be("BAPB");
        belarusDetails.BalansovyRahunak.Should().Be("3000");
        belarusDetails.NumarRahunku.Should().Be("0000000000000001");
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
