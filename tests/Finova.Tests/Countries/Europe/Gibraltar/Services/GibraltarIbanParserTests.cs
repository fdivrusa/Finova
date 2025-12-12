using Finova.Countries.Europe.Gibraltar.Models;
using Finova.Countries.Europe.Gibraltar.Services;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Gibraltar.Services;

public class GibraltarIbanParserTests
{
    private readonly GibraltarIbanParser _parser;

    public GibraltarIbanParserTests()
    {
        _parser = GibraltarIbanParser.Create();
    }

    [Fact]
    public void ParseIban_WithValidIban_ReturnsIbanDetails()
    {
        var iban = "GI56XAPO000001234567890";
        var details = _parser.ParseIban(iban);

        details.Should().NotBeNull();
        details.Should().BeOfType<GibraltarIbanDetails>();

        var gibraltarDetails = (GibraltarIbanDetails)details!;
        gibraltarDetails.CountryCode.Should().Be("GI");
        gibraltarDetails.BankCodeGi.Should().Be("XAPO");
        gibraltarDetails.AccountNumberGi.Should().Be("000001234567890");
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

