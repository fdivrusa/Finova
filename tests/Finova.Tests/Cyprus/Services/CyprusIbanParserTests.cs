using Finova.Countries.Europe.Cyprus.Models;
using Finova.Countries.Europe.Cyprus.Services;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Cyprus.Services;

public class CyprusIbanParserTests
{
    private readonly CyprusIbanParser _parser;

    public CyprusIbanParserTests()
    {
        _parser = CyprusIbanParser.Create();
    }

    [Fact]
    public void ParseIban_WithValidIban_ReturnsIbanDetails()
    {
        var iban = "CY21002001950000357001234567";
        var details = _parser.ParseIban(iban);

        details.Should().NotBeNull();
        details.Should().BeOfType<CyprusIbanDetails>();

        var cyprusDetails = (CyprusIbanDetails)details!;
        cyprusDetails.CountryCode.Should().Be("CY");
        cyprusDetails.BankCodeCy.Should().Be("002");
        cyprusDetails.BranchCodeCy.Should().Be("00195");
        cyprusDetails.AccountNumberCy.Should().Be("0000357001234567");
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
