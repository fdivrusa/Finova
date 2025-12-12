using Finova.Countries.Europe.Latvia.Models;
using Finova.Countries.Europe.Latvia.Services;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Latvia.Services;

public class LatviaIbanParserTests
{
    private readonly LatviaIbanParser _parser;

    public LatviaIbanParserTests()
    {
        _parser = LatviaIbanParser.Create();
    }

    [Fact]
    public void ParseIban_WithValidIban_ReturnsIbanDetails()
    {
        var iban = "LV97HABA0012345678910";
        var details = _parser.ParseIban(iban);

        details.Should().NotBeNull();
        details.Should().BeOfType<LatviaIbanDetails>();

        var latviaDetails = (LatviaIbanDetails)details!;
        latviaDetails.CountryCode.Should().Be("LV");
        latviaDetails.BankasKods.Should().Be("HABA");
        latviaDetails.KlientaKontaNumurs.Should().Be("0012345678910");
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

