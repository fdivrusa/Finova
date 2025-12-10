using Finova.Countries.Europe.Georgia.Models;
using Finova.Countries.Europe.Georgia.Services;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Georgia.Services;

public class GeorgiaIbanParserTests
{
    private readonly GeorgiaIbanParser _parser;

    public GeorgiaIbanParserTests()
    {
        _parser = GeorgiaIbanParser.Create();
    }

    [Fact]
    public void ParseIban_WithValidIban_ReturnsIbanDetails()
    {
        var iban = "GE6329NB00000001019049";
        var details = _parser.ParseIban(iban);

        details.Should().NotBeNull();
        details.Should().BeOfType<GeorgiaIbanDetails>();

        var georgiaDetails = (GeorgiaIbanDetails)details!;
        georgiaDetails.CountryCode.Should().Be("GE");
        georgiaDetails.BankisKodi.Should().Be("29"); // Wait, GE63 29NB ... Bank Code is 2 chars.
        // Let's recheck the parser.
        // var bankisKodi = validIban.Substring(4, 2);
        // IBAN: GE63 29NB ...
        // Indices: 0123 4567
        // 4,2 is "29". Correct.
        georgiaDetails.AngarishisNomeri.Should().Be("NB00000001019049");
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
