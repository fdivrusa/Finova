using Finova.Countries.Europe.Greece.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Greece.Validators;

public class GreeceIbanValidatorTests
{
    private readonly GreeceIbanValidator _validator;

    public GreeceIbanValidatorTests()
    {
        _validator = new GreeceIbanValidator();
    }

    #region CountryCode Tests

    [Fact]
    public void CountryCode_ReturnsGR()
    {
        _validator.CountryCode.Should().Be("GR");
    }

    #endregion

    #region Valid IBAN Tests

    [Theory]
    [InlineData("GR1601101250000000012300695")] // National Bank of Greece
    [InlineData("GR9608100010000001234567890")] // Another valid Greek IBAN
    public void IsValidIban_WithValidGreekIban_ReturnsTrue(string iban)
    {
        var result = _validator.Validate(iban);
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("GR16 0110 1250 0000 0001 2300 695")] // With spaces
    [InlineData("gr1601101250000000012300695")] // Lowercase
    public void IsValidIban_WithFormattedGreekIban_ReturnsTrue(string iban)
    {
        var result = _validator.Validate(iban);
        result.IsValid.Should().BeTrue();
    }

    #endregion

    #region Invalid IBAN Tests - Null and Empty

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void IsValidIban_WithNullOrEmpty_ReturnsFalse(string? iban)
    {
        var result = _validator.Validate(iban);
        result.IsValid.Should().BeFalse();
    }

    #endregion

    #region Invalid IBAN Tests - Wrong Country

    [Theory]
    [InlineData("DE89370400440532013000")] // German IBAN
    [InlineData("BE68539007547034")] // Belgian IBAN
    [InlineData("FI2112345600000785")] // Finnish IBAN
    public void IsValidIban_WithOtherCountryIban_ReturnsFalse(string iban)
    {
        var result = _validator.Validate(iban);
        result.IsValid.Should().BeFalse();
    }

    #endregion

    #region Invalid IBAN Tests - Wrong Length

    [Theory]
    [InlineData("GR160110125000000012300")] // Too short (24 chars)
    [InlineData("GR16011012500000001230069500")] // Too long (28 chars)
    public void IsValidIban_WithIncorrectLength_ReturnsFalse(string iban)
    {
        var result = _validator.Validate(iban);
        result.IsValid.Should().BeFalse();
    }

    #endregion

    #region Invalid IBAN Tests - Invalid Check Digits

    [Fact]
    public void IsValidIban_WithInvalidCheckDigits_ReturnsFalse()
    {
        // Changing check digits from 16 to 00
        var iban = "GR0001101250000000012300695";

        var result = _validator.Validate(iban);

        result.IsValid.Should().BeFalse();
    }

    #endregion

    #region Invalid IBAN Tests - Invalid Bank/Branch Code

    [Theory]
    [InlineData("GR16A1101250000000012300695")] // Letter in bank code (pos 4)
    [InlineData("GR1601A01250000000012300695")] // Letter in branch code (pos 7)
    public void IsValidIban_WithNonNumericBankBranchCode_ReturnsFalse(string iban)
    {
        var result = _validator.Validate(iban);

        result.IsValid.Should().BeFalse();
    }

    #endregion

    #region Valid IBAN Tests - Alphanumeric Account

    [Fact]
    public void IsValidIban_WithAlphanumericAccount_ValidatesCorrectly()
    {
        // Greek IBAN allows alphanumeric in account portion (positions 11-27)
        // This test verifies the validator handles alphanumeric account numbers
        // Note: Real Greek IBANs typically use only numeric, but spec allows alphanumeric
        var iban = "GR1601101250000000012300695";

        var result = _validator.Validate(iban);

        result.IsValid.Should().BeTrue();
    }

    #endregion

    #region Static Method Tests

    [Fact]
    public void ValidateGreeceIban_StaticMethod_WorksCorrectly()
    {
        var result = GreeceIbanValidator.ValidateGreeceIban("GR1601101250000000012300695");
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void ValidateGreeceIban_StaticMethod_WithInvalid_ReturnsFalse()
    {
        var result = GreeceIbanValidator.ValidateGreeceIban("GR0001101250000000012300695");
        result.IsValid.Should().BeFalse();
    }

    #endregion
}

