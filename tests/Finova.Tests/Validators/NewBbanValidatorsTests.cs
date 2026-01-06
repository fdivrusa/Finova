using Finova.Countries.Africa.Egypt.Validators;
using Finova.Countries.Africa.Mauritania.Validators;
using Finova.Countries.MiddleEast.Kuwait.Validators;
using Finova.Countries.MiddleEast.Lebanon.Validators;
using Finova.Countries.MiddleEast.Qatar.Validators;
using Finova.Countries.MiddleEast.SaudiArabia.Validators;
using Finova.Countries.MiddleEast.UAE.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Validators;

public class NewBbanValidatorsTests
{
    [Theory]
    [InlineData("099900000001001901229114", true)]
    [InlineData("09990000000100190122911", false)] // Too short
    [InlineData("0999000000010019012291145", false)] // Too long
    [InlineData("09990000000100190122911A", true)] // Alphanumeric allowed
    [InlineData("", false)]
    public void LebanonBbanValidator_Tests(string bban, bool expectedValid)
    {
        var validator = new LebanonBbanValidator();
        var result = validator.Validate(bban);
        result.IsValid.Should().Be(expectedValid);
        if (expectedValid)
        {
            var details = validator.ParseDetails(bban);
            details.Should().NotBeNull();
            details!.CountryCode.Should().Be("LB");
            details.BankCode.Should().Be(bban[..4]);
        }
    }

    [Theory]
    [InlineData("13000200010100001234567", true)]
    [InlineData("1300020001010000123456", false)] // Too short
    [InlineData("130002000101000012345678", false)] // Too long
    [InlineData("1300020001010000123456A", false)] // Digits only
    public void MauritaniaBbanValidator_Tests(string bban, bool expectedValid)
    {
        var validator = new MauritaniaBbanValidator();
        var result = validator.Validate(bban);
        result.IsValid.Should().Be(expectedValid);
        if (expectedValid)
        {
            var details = validator.ParseDetails(bban);
            details.Should().NotBeNull();
            details!.CountryCode.Should().Be("MR");
            details.BankCode.Should().Be(bban[..5]);
        }
    }

    [Theory]
    [InlineData("0331234567890123456", true)]
    [InlineData("033123456789012345", false)] // Too short
    [InlineData("03312345678901234567", false)] // Too long
    [InlineData("033123456789012345A", false)] // Digits only
    public void UaeBbanValidator_Tests(string bban, bool expectedValid)
    {
        var validator = new UaeBbanValidator();
        var result = validator.Validate(bban);
        result.IsValid.Should().Be(expectedValid);
        if (expectedValid)
        {
            var details = validator.ParseDetails(bban);
            details.Should().NotBeNull();
            details!.CountryCode.Should().Be("AE");
            details.BankCode.Should().Be(bban[..3]);
        }
    }

    [Theory]
    [InlineData("80000000608010167519", true)]
    [InlineData("8000000060801016751", false)] // Too short
    [InlineData("800000006080101675199", false)] // Too long
    [InlineData("8000000060801016751A", true)] // Alphanumeric allowed
    public void SaudiArabiaBbanValidator_Tests(string bban, bool expectedValid)
    {
        var validator = new SaudiArabiaBbanValidator();
        var result = validator.Validate(bban);
        result.IsValid.Should().Be(expectedValid);
        if (expectedValid)
        {
            var details = validator.ParseDetails(bban);
            details.Should().NotBeNull();
            details!.CountryCode.Should().Be("SA");
            details.BankCode.Should().Be(bban[..2]);
        }
    }

    [Theory]
    [InlineData("CBKU0000000000001234560101", true)]
    [InlineData("CBKU000000000000123456010", false)] // Too short
    [InlineData("CBKU00000000000012345601011", false)] // Too long
    public void KuwaitBbanValidator_Tests(string bban, bool expectedValid)
    {
        var validator = new KuwaitBbanValidator();
        var result = validator.Validate(bban);
        result.IsValid.Should().Be(expectedValid);
        if (expectedValid)
        {
            var details = validator.ParseDetails(bban);
            details.Should().NotBeNull();
            details!.CountryCode.Should().Be("KW");
            details.BankCode.Should().Be(bban[..4]);
        }
    }

    [Theory]
    [InlineData("DOHB000012345678901234567", true)]
    [InlineData("DOHB00001234567890123456", false)] // Too short
    [InlineData("DOHB0000123456789012345678", false)] // Too long
    public void QatarBbanValidator_Tests(string bban, bool expectedValid)
    {
        var validator = new QatarBbanValidator();
        var result = validator.Validate(bban);
        result.IsValid.Should().Be(expectedValid);
        if (expectedValid)
        {
            var details = validator.ParseDetails(bban);
            details.Should().NotBeNull();
            details!.CountryCode.Should().Be("QA");
            details.BankCode.Should().Be(bban[..4]);
        }
    }

    [Theory]
    [InlineData("3800190005000000002631800", true)]
    [InlineData("380019000500000000263180", false)] // Too short
    [InlineData("38001900050000000026318000", false)] // Too long
    [InlineData("380019000500000000263180A", false)] // Digits only
    public void EgyptBbanValidator_Tests(string bban, bool expectedValid)
    {
        var validator = new EgyptBbanValidator();
        var result = validator.Validate(bban);
        result.IsValid.Should().Be(expectedValid);
        if (expectedValid)
        {
            var details = validator.ParseDetails(bban);
            details.Should().NotBeNull();
            details!.CountryCode.Should().Be("EG");
            details.BankCode.Should().Be(bban[..4]);
        }
    }
}
