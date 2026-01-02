using Finova.Core.Common;
using Finova.Core.Identifiers;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Identifiers;

public class CurrencyValidatorTests
{
    private readonly CurrencyValidator _validator = new();

    #region Validate Tests - Major Currencies

    [Theory]
    [InlineData("EUR")] // Euro
    [InlineData("USD")] // US Dollar
    [InlineData("GBP")] // British Pound
    [InlineData("JPY")] // Japanese Yen
    [InlineData("CHF")] // Swiss Franc
    [InlineData("AUD")] // Australian Dollar
    [InlineData("CAD")] // Canadian Dollar
    [InlineData("CNY")] // Chinese Yuan
    public void Validate_MajorCurrencies_ReturnsSuccess(string currency)
    {
        // Act
        var result = CurrencyValidator.Validate(currency);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    #endregion

    #region Validate Tests - European Currencies

    [Theory]
    [InlineData("SEK")] // Swedish Krona
    [InlineData("NOK")] // Norwegian Krone
    [InlineData("DKK")] // Danish Krone
    [InlineData("PLN")] // Polish Zloty
    [InlineData("CZK")] // Czech Koruna
    [InlineData("HUF")] // Hungarian Forint
    [InlineData("RON")] // Romanian Leu
    [InlineData("BGN")] // Bulgarian Lev
    public void Validate_EuropeanCurrencies_ReturnsSuccess(string currency)
    {
        // Act
        var result = CurrencyValidator.Validate(currency);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    #endregion

    #region Validate Tests - Special Codes

    [Theory]
    [InlineData("XAU")] // Gold
    [InlineData("XAG")] // Silver
    [InlineData("XPT")] // Platinum
    [InlineData("XDR")] // SDR
    [InlineData("XXX")] // No currency
    public void Validate_SpecialCodes_ReturnsSuccess(string currency)
    {
        // Act
        var result = CurrencyValidator.Validate(currency);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    #endregion

    #region Validate Tests - Invalid Input

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Validate_EmptyInput_ReturnsInvalidInput(string? input)
    {
        // Act
        var result = CurrencyValidator.Validate(input);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors[0].Code.Should().Be(ValidationErrorCode.InvalidInput);
    }

    [Theory]
    [InlineData("EU")] // Too short (2)
    [InlineData("EURO")] // Too long (4)
    [InlineData("E")] // Way too short
    public void Validate_InvalidLength_ReturnsInvalidLength(string currency)
    {
        // Act
        var result = CurrencyValidator.Validate(currency);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors[0].Code.Should().Be(ValidationErrorCode.InvalidLength);
    }

    [Theory]
    [InlineData("XYZ")] // Unknown currency
    [InlineData("AAA")] // Unknown currency
    [InlineData("ZZZ")] // Unknown currency
    public void Validate_UnknownCurrency_ReturnsInvalidFormat(string currency)
    {
        // Act
        var result = CurrencyValidator.Validate(currency);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors[0].Code.Should().Be(ValidationErrorCode.InvalidFormat);
    }

    [Fact]
    public void Validate_WithLowercaseCurrency_ReturnsSuccess()
    {
        // Arrange
        var currency = "eur";

        // Act
        var result = CurrencyValidator.Validate(currency);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_WithWhitespace_ReturnsSuccess()
    {
        // Arrange
        var currency = "  EUR  ";

        // Act
        var result = CurrencyValidator.Validate(currency);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    #endregion

    #region Parse Tests

    [Fact]
    public void Parse_Eur_ReturnsCorrectDetails()
    {
        // Act
        var details = CurrencyValidator.Parse("EUR");

        // Assert
        details.Should().NotBeNull();
        details!.Code.Should().Be("EUR");
        details.Name.Should().Be("Euro");
        details.NumericCode.Should().Be("978");
        details.MinorUnits.Should().Be(2);
        details.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Parse_Jpy_ReturnsZeroMinorUnits()
    {
        // Act
        var details = CurrencyValidator.Parse("JPY");

        // Assert
        details.Should().NotBeNull();
        details!.Code.Should().Be("JPY");
        details.Name.Should().Be("Yen");
        details.MinorUnits.Should().Be(0); // No decimal places
        details.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Parse_Kwd_ReturnsThreeMinorUnits()
    {
        // Act
        var details = CurrencyValidator.Parse("KWD");

        // Assert
        details.Should().NotBeNull();
        details!.Code.Should().Be("KWD");
        details.Name.Should().Be("Kuwaiti Dinar");
        details.MinorUnits.Should().Be(3); // 3 decimal places
        details.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Parse_Gold_ReturnsSpecialMinorUnits()
    {
        // Act
        var details = CurrencyValidator.Parse("XAU");

        // Assert
        details.Should().NotBeNull();
        details!.Code.Should().Be("XAU");
        details.Name.Should().Be("Gold");
        details.MinorUnits.Should().Be(-1); // Precious metals have no minor units
        details.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Parse_NullInput_ReturnsNull()
    {
        // Act
        var details = CurrencyValidator.Parse(null);

        // Assert
        details.Should().BeNull();
    }

    [Fact]
    public void Parse_UnknownCurrency_ReturnsNull()
    {
        // Act
        var details = CurrencyValidator.Parse("XYZ");

        // Assert
        details.Should().BeNull();
    }

    [Fact]
    public void Parse_TooShortInput_ReturnsNull()
    {
        // Act
        var details = CurrencyValidator.Parse("EU");

        // Assert
        details.Should().BeNull();
    }

    #endregion

    #region IsValid Tests

    [Theory]
    [InlineData("EUR", true)]
    [InlineData("USD", true)]
    [InlineData("XYZ", false)]
    [InlineData(null, false)]
    [InlineData("", false)]
    [InlineData("EU", false)]
    public void IsValid_VariousInputs_ReturnsExpectedResult(string? currency, bool expected)
    {
        // Act
        var result = CurrencyValidator.IsValid(currency);

        // Assert
        result.Should().Be(expected);
    }

    #endregion

    #region GetAllCurrencyCodes Tests

    [Fact]
    public void GetAllCurrencyCodes_ReturnsNonEmptyCollection()
    {
        // Act
        var codes = CurrencyValidator.GetAllCurrencyCodes().ToList();

        // Assert
        codes.Should().NotBeEmpty();
        codes.Should().Contain("EUR");
        codes.Should().Contain("USD");
        codes.Should().Contain("GBP");
    }

    [Fact]
    public void GetAllCurrencyCodes_ContainsExpectedCount()
    {
        // Act
        var count = CurrencyValidator.GetAllCurrencyCodes().Count();

        // Assert
        count.Should().BeGreaterThan(100); // We have ~170+ currencies defined
    }

    #endregion

    #region Interface Tests

    [Fact]
    public void Interface_Validate_WorksCorrectly()
    {
        // Arrange
        ICurrencyValidator validator = _validator;

        // Act
        var result = validator.Validate("EUR");

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Interface_Parse_WorksCorrectly()
    {
        // Arrange
        ICurrencyValidator validator = _validator;

        // Act
        var details = validator.Parse("EUR");

        // Assert
        details.Should().NotBeNull();
        details!.IsValid.Should().BeTrue();
    }

    #endregion
}
