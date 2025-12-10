using Finova.Countries.Europe.FaroeIslands.Models;
using Finova.Countries.Europe.FaroeIslands.Services;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.FaroeIslands.Services;

public class FaroeIslandsIbanParserTests
{
    private readonly FaroeIslandsIbanParser _parser;

    public FaroeIslandsIbanParserTests()
    {
        _parser = FaroeIslandsIbanParser.Create();
    }

    [Fact]
    public void ParseIban_WithValidIban_ReturnsIbanDetails()
    {
        var iban = "FO7460000000000011";
        var details = _parser.ParseIban(iban);

        details.Should().NotBeNull();
        details.Should().BeOfType<FaroeIslandsIbanDetails>();

        var faroeIslandsDetails = (FaroeIslandsIbanDetails)details!;
        faroeIslandsDetails.CountryCode.Should().Be("FO");
        faroeIslandsDetails.SkrasetingarNummar.Should().Be("6000");
        faroeIslandsDetails.KontoNummar.Should().Be("000000001");
        faroeIslandsDetails.EftirlitsTal.Should().Be("1");
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
