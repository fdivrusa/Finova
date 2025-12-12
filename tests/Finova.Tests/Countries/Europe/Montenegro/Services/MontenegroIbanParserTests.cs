using Finova.Countries.Europe.Montenegro.Models;
using Finova.Countries.Europe.Montenegro.Services;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Montenegro.Services;

public class MontenegroIbanParserTests
{
    private readonly MontenegroIbanParser _parser;

    public MontenegroIbanParserTests()
    {
        _parser = MontenegroIbanParser.Create();
    }

    [Fact]
    public void ParseIban_WithValidIban_ReturnsIbanDetails()
    {
        var iban = "ME36500000000000000001";
        var details = _parser.ParseIban(iban);

        details.Should().NotBeNull();
        details.Should().BeOfType<MontenegroIbanDetails>();

        var montenegroDetails = (MontenegroIbanDetails)details!;
        montenegroDetails.CountryCode.Should().Be("ME");
        montenegroDetails.SifraBanke.Should().Be("500");
        montenegroDetails.BrojRacuna.Should().Be("0000000000000");
        montenegroDetails.KontrolniBroj.Should().Be("01");
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

