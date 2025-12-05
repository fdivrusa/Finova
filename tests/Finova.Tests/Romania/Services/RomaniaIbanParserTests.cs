using Finova.Countries.Europe.Romania.Models;
using Finova.Countries.Europe.Romania.Services;
using Finova.Countries.Europe.Romania.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Romania.Services;

public class RomaniaIbanParserTests
{
    private readonly RomaniaIbanParser _parser;

    public RomaniaIbanParserTests()
    {
        _parser = new RomaniaIbanParser(new RomaniaIbanValidator());
    }

    #region ParseIban Tests

    [Fact]
    public void ParseIban_WithValidRomanianIban_ReturnsCorrectDetails()
    {
        // Arrange
        var iban = "RO49AAAA1B31007593840000";

        // Act
        var result = _parser.ParseIban(iban) as RomaniaIbanDetails;

        // Assert
        result.Should().NotBeNull();
        result!.Iban.Should().Be("RO49AAAA1B31007593840000");
        result.CountryCode.Should().Be("RO");
        result.CheckDigits.Should().Be("49");
        result.CodulBancii.Should().Be("AAAA");
        result.NumarCont.Should().Be("1B31007593840000");
        result.BankCode.Should().Be("AAAA");
        result.AccountNumber.Should().Be("1B31007593840000");
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("RO49 AAAA 1B31 0075 9384 0000")] // With spaces
    [InlineData("ro49aaaa1b31007593840000")] // Lowercase
    public void ParseIban_WithFormattedIban_ReturnsNormalizedDetails(string iban)
    {
        // Act
        var result = _parser.ParseIban(iban) as RomaniaIbanDetails;

        // Assert
        result.Should().NotBeNull();
        result!.Iban.Should().Be("RO49AAAA1B31007593840000");
    }

    [Theory]
    [InlineData("RO00AAAA1B31007593840000")] // Wrong check digits
    [InlineData("DE89370400440532013000")] // Wrong country
    [InlineData("RO49AAAA1B3100759384000")] // Too short
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
    public void ParseIban_WithUniCreditExample_ParsesCorrectly()
    {
        // Arrange - UniCredit Bank example
        var iban = "RO66BACX0000001234567890";

        // Act
        var result = _parser.ParseIban(iban) as RomaniaIbanDetails;

        // Assert
        result.Should().NotBeNull();
        result!.CodulBancii.Should().Be("BACX");
        result.NumarCont.Should().Be("0000001234567890");
        result.CheckDigits.Should().Be("66");
    }

    [Fact]
    public void ParseIban_WithOTPBankExample_ParsesCorrectly()
    {
        // Arrange - OTP Bank example
        var iban = "RO09BCYP0000001234567890";

        // Act
        var result = _parser.ParseIban(iban) as RomaniaIbanDetails;

        // Assert
        result.Should().NotBeNull();
        result!.CodulBancii.Should().Be("BCYP");
        result.NumarCont.Should().Be("0000001234567890");
        result.BankCode.Should().Be("BCYP");
        result.AccountNumber.Should().Be("0000001234567890");
    }

    #endregion

    #region Factory Method Tests

    [Fact]
    public void CreateDefault_ReturnsValidParser()
    {
        // Act
        var parser = RomaniaIbanParser.Create();

        // Assert
        parser.Should().NotBeNull();
        parser.CountryCode.Should().Be("RO");
    }

    #endregion

    #region Consistency Tests

    [Fact]
    public void ParseIban_CalledMultipleTimes_ReturnsConsistentResults()
    {
        // Arrange
        var iban = "RO49AAAA1B31007593840000";

        // Act
        var result1 = _parser.ParseIban(iban);
        var result2 = _parser.ParseIban(iban);

        // Assert
        result1.Should().BeEquivalentTo(result2);
    }

    [Theory]
    [InlineData("BE68539007547034")] // Belgian IBAN
    [InlineData("FR7630006000011234567890189")] // French IBAN
    [InlineData("IT60X0542811101000000123456")] // Italian IBAN
    public void ParseIban_WithOtherCountryIbans_ReturnsNull(string iban)
    {
        // Act
        var result = _parser.ParseIban(iban);

        // Assert
        result.Should().BeNull();
    }

    #endregion

    #region Alphanumeric Account Tests

    [Fact]
    public void ParseIban_WithAlphanumericAccount_ParsesCorrectly()
    {
        // Arrange - IBAN with letters in account number
        var iban = "RO49AAAA1B31007593840000";

        // Act
        var result = _parser.ParseIban(iban) as RomaniaIbanDetails;

        // Assert
        result.Should().NotBeNull();
        result!.NumarCont.Should().Be("1B31007593840000");
        // Account contains alphanumeric characters
        result.NumarCont.Any(char.IsLetter).Should().BeTrue();
    }

    #endregion
}
