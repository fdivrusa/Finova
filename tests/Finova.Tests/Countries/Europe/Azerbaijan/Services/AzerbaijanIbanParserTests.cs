using Finova.Countries.Europe.Azerbaijan.Models;
using Finova.Countries.Europe.Azerbaijan.Services;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Azerbaijan.Services;

public class AzerbaijanIbanParserTests
{
    private readonly AzerbaijanIbanParser _parser;

    public AzerbaijanIbanParserTests()
    {
        _parser = AzerbaijanIbanParser.Create();
    }

    [Fact]
    public void ParseIban_WithValidIban_ReturnsIbanDetails()
    {
        var iban = "AZ21NABZ00000000137010001944";
        var details = _parser.ParseIban(iban);

        details.Should().NotBeNull();
        details.Should().BeOfType<AzerbaijanIbanDetails>();

        var azerbaijanDetails = (AzerbaijanIbanDetails)details!;
        azerbaijanDetails.CountryCode.Should().Be("AZ");
        azerbaijanDetails.BankKodu.Should().Be("NABZ");
        azerbaijanDetails.HesabNomresi.Should().Be("00000000137010001944");
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

