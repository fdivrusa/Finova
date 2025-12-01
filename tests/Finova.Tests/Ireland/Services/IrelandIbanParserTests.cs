using Finova.Countries.Europe.Ireland.Models;
using Finova.Countries.Europe.Ireland.Services;
using Finova.Countries.Europe.Ireland.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Ireland.Services;

public class IrelandIbanParserTests
{
    private readonly IrelandIbanParser _parser;

    public IrelandIbanParserTests()
    {
        _parser = new IrelandIbanParser(new IrelandIbanValidator());
    }

    #region ParseIban Tests

    [Fact]
    public void ParseIban_WithValidIrishIban_ReturnsCorrectDetails()
    {
        // Arrange - AIB Bank example
        var iban = "IE29AIBK93115212345678";

        // Act
        var result = _parser.ParseIban(iban) as IrelandIbanDetails;

        // Assert
        result.Should().NotBeNull();
        result!.Iban.Should().Be("IE29AIBK93115212345678");
        result.CountryCode.Should().Be("IE");
        result.CheckDigits.Should().Be("29");
        result.BankIdentifier.Should().Be("AIBK");
        result.SortCode.Should().Be("931152");
        result.DomesticAccountNumber.Should().Be("12345678");
        result.BankCode.Should().Be("AIBK");
        result.BranchCode.Should().Be("931152");
        result.AccountNumber.Should().Be("12345678");
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("IE29 AIBK 9311 5212 3456 78")] // With spaces
    [InlineData("ie29aibk93115212345678")] // Lowercase
    public void ParseIban_WithFormattedIban_ReturnsNormalizedDetails(string iban)
    {
        // Act
        var result = _parser.ParseIban(iban) as IrelandIbanDetails;

        // Assert
        result.Should().NotBeNull();
        result!.Iban.Should().Be("IE29AIBK93115212345678");
    }

    [Theory]
    [InlineData("IE00AIBK93115212345678")] // Wrong check digits
    [InlineData("DE89370400440532013000")] // Wrong country
    [InlineData("IE29AIBK931152123456")] // Too short
    [InlineData("")] // Empty
    [InlineData(null)] // Null
    public void ParseIban_WithInvalidIban_ReturnsNull(string? iban)
    {
        // Act
        var result = _parser.ParseIban(iban);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void ParseIban_WithDifferentBank_ParsesCorrectly()
    {
        // Arrange - Irish Permanent example
        var iban = "IE64IRCE92050112345678";

        // Act
        var result = _parser.ParseIban(iban) as IrelandIbanDetails;

        // Assert
        result.Should().NotBeNull();
        result!.BankIdentifier.Should().Be("IRCE");
        result.SortCode.Should().Be("920501");
        result.DomesticAccountNumber.Should().Be("12345678");
        result.CheckDigits.Should().Be("64");
    }

    #endregion

    #region Factory Method Tests

    [Fact]
    public void Create_ReturnsValidParser()
    {
        // Act
        var parser = IrelandIbanParser.Create();

        // Assert
        parser.Should().NotBeNull();
        parser.CountryCode.Should().Be("IE");
    }

    [Fact]
    public void Create_ParserCanParseValidIban()
    {
        // Arrange
        var parser = IrelandIbanParser.Create();
        var iban = "IE29AIBK93115212345678";

        // Act
        var result = parser.ParseIban(iban) as IrelandIbanDetails;

        // Assert
        result.Should().NotBeNull();
        result!.BankIdentifier.Should().Be("AIBK");
    }

    #endregion

    #region CountryCode Tests

    [Fact]
    public void CountryCode_ReturnsIE()
    {
        // Act
        var countryCode = _parser.CountryCode;

        // Assert
        countryCode.Should().Be("IE");
    }

    #endregion

    #region Edge Cases

    [Fact]
    public void ParseIban_ExtractsAllComponents()
    {
        // Arrange
        var iban = "IE29AIBK93115212345678";

        // Act
        var result = _parser.ParseIban(iban) as IrelandIbanDetails;

        // Assert - verify all components are extracted
        result.Should().NotBeNull();
        result!.CountryCode.Should().Be("IE");
        result.CheckDigits.Should().HaveLength(2);
        result.BankIdentifier.Should().HaveLength(4);
        result.SortCode.Should().HaveLength(6);
        result.DomesticAccountNumber.Should().HaveLength(8);
    }

    [Fact]
    public void ParseIban_MapsGenericProperties()
    {
        // Arrange
        var iban = "IE29AIBK93115212345678";

        // Act
        var result = _parser.ParseIban(iban) as IrelandIbanDetails;

        // Assert - verify generic properties map to Irish-specific ones
        result.Should().NotBeNull();
        result!.BankCode.Should().Be(result.BankIdentifier);
        result.BranchCode.Should().Be(result.SortCode);
        result.AccountNumber.Should().Be(result.DomesticAccountNumber);
    }

    #endregion
}
