using Finova.Countries.Europe.Monaco.Models;
using Finova.Countries.Europe.Monaco.Services;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Monaco.Services;

public class MonacoIbanParserTests
{
    private readonly MonacoIbanParser _parser;

    public MonacoIbanParserTests()
    {
        _parser = MonacoIbanParser.Create();
    }

    [Fact]
    public void ParseIban_WithValidIban_ReturnsIbanDetails()
    {
        var iban = "MC5810096180790123456789085";
        var details = _parser.ParseIban(iban);

        details.Should().NotBeNull();
        details.Should().BeOfType<MonacoIbanDetails>();

        var monacoDetails = (MonacoIbanDetails)details!;
        monacoDetails.CountryCode.Should().Be("MC");
        monacoDetails.CodeBanque.Should().Be("10096");
        monacoDetails.CodeGuichet.Should().Be("18079");
        monacoDetails.NumeroCompte.Should().Be("01234567890");
        monacoDetails.CleRib.Should().Be("85");
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

