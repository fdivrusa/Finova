using Finova.Countries.Europe.Croatia.Models;
using Finova.Countries.Europe.Croatia.Services;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Croatia.Services;

public class CroatiaIbanParserTests
{
    private readonly CroatiaIbanParser _parser;

    public CroatiaIbanParserTests()
    {
        _parser = CroatiaIbanParser.Create();
    }

    [Fact]
    public void ParseIban_WithValidIban_ReturnsIbanDetails()
    {
        var iban = "HR1723600001101234565";
        var details = _parser.ParseIban(iban);

        details.Should().NotBeNull();
        details.Should().BeOfType<CroatiaIbanDetails>();

        var croatiaDetails = (CroatiaIbanDetails)details!;
        croatiaDetails.CountryCode.Should().Be("HR");
        croatiaDetails.VodeciBrojBanke.Should().Be("2360000");
        croatiaDetails.BrojRacuna.Should().Be("1101234565");
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

