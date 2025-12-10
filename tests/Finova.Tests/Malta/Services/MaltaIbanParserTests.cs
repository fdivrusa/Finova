using Finova.Countries.Europe.Malta.Models;
using Finova.Countries.Europe.Malta.Services;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Malta.Services;

public class MaltaIbanParserTests
{
    private readonly MaltaIbanParser _parser;

    public MaltaIbanParserTests()
    {
        _parser = MaltaIbanParser.Create();
    }

    [Fact]
    public void ParseIban_WithValidIban_ReturnsIbanDetails()
    {
        var iban = "MT31MALT01100000000000000000123";
        var details = _parser.ParseIban(iban);

        details.Should().NotBeNull();
        details.Should().BeOfType<MaltaIbanDetails>();

        var maltaDetails = (MaltaIbanDetails)details!;
        maltaDetails.CountryCode.Should().Be("MT");
        maltaDetails.BankBic.Should().Be("MALT");
        maltaDetails.SortCode.Should().Be("01100");
        maltaDetails.AccountNumberMt.Should().Be("000000000000000123");
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
