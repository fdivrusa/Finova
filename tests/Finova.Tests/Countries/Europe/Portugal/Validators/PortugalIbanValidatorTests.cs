using Finova.Countries.Europe.Portugal.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Portugal.Validators;

public class PortugalIbanValidatorTests
{
    private readonly PortugalIbanValidator _validator;

    public PortugalIbanValidatorTests()
    {
        _validator = new PortugalIbanValidator();
    }

    #region CountryCode Tests

    [Fact]
    public void CountryCode_ReturnsPT()
    {
        _validator.CountryCode.Should().Be("PT");
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
    [InlineData("GR1601101250000000012300695")] // Greek IBAN
    public void IsValidIban_WithOtherCountryIban_ReturnsFalse(string iban)
    {
        var result = _validator.Validate(iban);
        result.IsValid.Should().BeFalse();
    }

    #endregion

    #region Invalid IBAN Tests - Wrong Length

    [Theory]
    [InlineData("PT5000020123123456789015")] // Too short (24 chars)
    [InlineData("PT500002012312345678901540")] // Too long (26 chars)
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
        // Any PT IBAN with check digits 00 should fail
        var iban = "PT00000201231234567890154";

        var result = _validator.Validate(iban);

        result.IsValid.Should().BeFalse();
    }

    #endregion

    #region Invalid IBAN Tests - Non-Numeric Content

    [Theory]
    [InlineData("PT50A00201231234567890154")] // Letter in bank code
    [InlineData("PT500002A1231234567890154")] // Letter in branch code
    public void IsValidIban_WithNonNumericContent_ReturnsFalse(string iban)
    {
        var result = _validator.Validate(iban);

        result.IsValid.Should().BeFalse();
    }

    #endregion

    #region Structure Validation Tests

    [Fact]
    public void IsValidIban_ValidatesCountryPrefix()
    {
        // Wrong country prefix
        var result = _validator.Validate("ES9121000418450200051332");
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void IsValidIban_ValidatesLength()
    {
        // Portuguese IBAN must be exactly 25 characters
        var shortIban = "PT5000020123123456789";
        var longIban = "PT500002012312345678901234";

        _validator.Validate(shortIban).IsValid.Should().BeFalse();
        _validator.Validate(longIban).IsValid.Should().BeFalse();
    }

    #endregion
}



