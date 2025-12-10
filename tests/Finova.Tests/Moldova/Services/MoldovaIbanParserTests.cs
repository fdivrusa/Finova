using Finova.Countries.Europe.Moldova.Models;
using Finova.Countries.Europe.Moldova.Services;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Moldova.Services;

public class MoldovaIbanParserTests
{
    private readonly MoldovaIbanParser _parser;

    public MoldovaIbanParserTests()
    {
        _parser = MoldovaIbanParser.Create();
    }

    [Fact]
    public void ParseIban_WithValidIban_ReturnsIbanDetails()
    {
        var iban = "MD76AA100000000000000001";
        var details = _parser.ParseIban(iban);

        details.Should().NotBeNull();
        details.Should().BeOfType<MoldovaIbanDetails>();

        var moldovaDetails = (MoldovaIbanDetails)details!;
        moldovaDetails.CountryCode.Should().Be("MD");
        moldovaDetails.CodBanca.Should().Be("AA");
        moldovaDetails.NumarCont.Should().Be("100000000000000001");
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
