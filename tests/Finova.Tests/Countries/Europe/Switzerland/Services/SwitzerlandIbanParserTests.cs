using Finova.Countries.Europe.Switzerland.Models;
using Finova.Countries.Europe.Switzerland.Services;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Switzerland.Services;

public class SwitzerlandIbanParserTests
{
    private readonly SwitzerlandIbanParser _parser;

    public SwitzerlandIbanParserTests()
    {
        _parser = SwitzerlandIbanParser.Create();
    }

    [Fact]
    public void ParseIban_WithValidIban_ReturnsIbanDetails()
    {
        var iban = "CH5604835012345678009";
        var details = _parser.ParseIban(iban);

        details.Should().NotBeNull();
        details.Should().BeOfType<SwitzerlandIbanDetails>();

        var switzerlandDetails = (SwitzerlandIbanDetails)details!;
        switzerlandDetails.CountryCode.Should().Be("CH");
        switzerlandDetails.ClearingNummer.Should().Be("04835");
        switzerlandDetails.KontoNummer.Should().Be("012345678009");
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

