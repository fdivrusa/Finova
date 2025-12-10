using Finova.Countries.Europe.Ukraine.Models;
using Finova.Countries.Europe.Ukraine.Services;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Ukraine.Services;

public class UkraineIbanParserTests
{
    private readonly UkraineIbanParser _parser;

    public UkraineIbanParserTests()
    {
        _parser = UkraineIbanParser.Create();
    }

    [Fact]
    public void ParseIban_WithValidIban_ReturnsIbanDetails()
    {
        var iban = "UA443000230000000000000000000";
        var details = _parser.ParseIban(iban);

        details.Should().NotBeNull();
        details.Should().BeOfType<UkraineIbanDetails>();

        var ukraineDetails = (UkraineIbanDetails)details!;
        ukraineDetails.CountryCode.Should().Be("UA");
        ukraineDetails.KodBanku.Should().Be("300023");
        ukraineDetails.NomeraRahunku.Should().Be("0000000000000000000");
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
