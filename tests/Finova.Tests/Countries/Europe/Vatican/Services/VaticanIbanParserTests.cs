using Finova.Countries.Europe.Vatican.Models;
using Finova.Countries.Europe.Vatican.Services;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Vatican.Services;

public class VaticanIbanParserTests
{
    private readonly VaticanIbanParser _parser;

    public VaticanIbanParserTests()
    {
        _parser = VaticanIbanParser.Create();
    }

    [Fact]
    public void ParseIban_WithValidIban_ReturnsIbanDetails()
    {
        var iban = "VA59001123000012345678";
        var details = _parser.ParseIban(iban);

        details.Should().NotBeNull();
        details.Should().BeOfType<VaticanIbanDetails>();

        var vaticanDetails = (VaticanIbanDetails)details!;
        vaticanDetails.CountryCode.Should().Be("VA");
        vaticanDetails.CodiceBanca.Should().Be("001");
        vaticanDetails.NumeroConto.Should().Be("123000012345678");
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

