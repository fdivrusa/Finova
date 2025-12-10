using Finova.Countries.Europe.NorthMacedonia.Models;
using Finova.Countries.Europe.NorthMacedonia.Services;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.NorthMacedonia.Services;

public class NorthMacedoniaIbanParserTests
{
    private readonly NorthMacedoniaIbanParser _parser;

    public NorthMacedoniaIbanParserTests()
    {
        _parser = NorthMacedoniaIbanParser.Create();
    }

    [Fact]
    public void ParseIban_WithValidIban_ReturnsIbanDetails()
    {
        var iban = "MK31100000000000001";
        var details = _parser.ParseIban(iban);

        details.Should().NotBeNull();
        details.Should().BeOfType<NorthMacedoniaIbanDetails>();

        var northMacedoniaDetails = (NorthMacedoniaIbanDetails)details!;
        northMacedoniaDetails.CountryCode.Should().Be("MK");
        northMacedoniaDetails.SifraBanka.Should().Be("100");
        northMacedoniaDetails.BrojSmetka.Should().Be("0000000000");
        northMacedoniaDetails.KontrolenBroj.Should().Be("01");
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
