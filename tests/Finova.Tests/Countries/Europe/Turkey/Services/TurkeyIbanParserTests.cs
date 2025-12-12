using Finova.Countries.Europe.Turkey.Models;
using Finova.Countries.Europe.Turkey.Services;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Turkey.Services;

public class TurkeyIbanParserTests
{
    private readonly TurkeyIbanParser _parser;

    public TurkeyIbanParserTests()
    {
        _parser = TurkeyIbanParser.Create();
    }

    [Fact]
    public void ParseIban_WithValidIban_ReturnsIbanDetails()
    {
        var iban = "TR960006100000000000000001";
        var details = _parser.ParseIban(iban);

        details.Should().NotBeNull();
        details.Should().BeOfType<TurkeyIbanDetails>();

        var turkeyDetails = (TurkeyIbanDetails)details!;
        turkeyDetails.CountryCode.Should().Be("TR");
        turkeyDetails.BankaKodu.Should().Be("00061");
        turkeyDetails.RezervAlan.Should().Be("0");
        turkeyDetails.HesapNumarasi.Should().Be("0000000000000001");
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

