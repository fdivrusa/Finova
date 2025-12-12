using Finova.Countries.Europe.Kosovo.Models;
using Finova.Countries.Europe.Kosovo.Services;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Kosovo.Services;

public class KosovoIbanParserTests
{
    private readonly KosovoIbanParser _parser;

    public KosovoIbanParserTests()
    {
        _parser = KosovoIbanParser.Create();
    }

    [Fact]
    public void ParseIban_WithValidIban_ReturnsIbanDetails()
    {
        var iban = "XK950505000000000000";
        var details = _parser.ParseIban(iban);

        details.Should().NotBeNull();
        details.Should().BeOfType<KosovoIbanDetails>();

        var kosovoDetails = (KosovoIbanDetails)details!;
        kosovoDetails.CountryCode.Should().Be("XK");
        kosovoDetails.KodiBankes.Should().Be("05");
        kosovoDetails.KodiDeges.Should().Be("05");
        kosovoDetails.NumriLlogarise.Should().Be("0000000000");
        kosovoDetails.ShifraKontrollit.Should().Be("00");
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

