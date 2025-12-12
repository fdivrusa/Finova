using Finova.Countries.Europe.Bulgaria.Models;
using Finova.Countries.Europe.Bulgaria.Services;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Bulgaria.Services;

public class BulgariaIbanParserTests
{
    private readonly BulgariaIbanParser _parser;

    public BulgariaIbanParserTests()
    {
        _parser = BulgariaIbanParser.Create();
    }

    [Fact]
    public void ParseIban_WithValidIban_ReturnsIbanDetails()
    {
        var iban = "BG19STSA93000123456789";
        var details = _parser.ParseIban(iban);

        details.Should().NotBeNull();
        details.Should().BeOfType<BulgariaIbanDetails>();

        var bulgariaDetails = (BulgariaIbanDetails)details!;
        bulgariaDetails.CountryCode.Should().Be("BG");
        bulgariaDetails.BankovKod.Should().Be("STSA");
        bulgariaDetails.Klon.Should().Be("9300");
        bulgariaDetails.VidSmetka.Should().Be("01");
        bulgariaDetails.NomerSmetka.Should().Be("23456789");
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

