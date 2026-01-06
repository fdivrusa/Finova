using Finova.Countries.Africa.Algeria.Validators;
using Finova.Countries.Africa.Egypt.Validators;
using Finova.Countries.Africa.Morocco.Validators;
using Finova.Countries.Africa.Nigeria.Validators;
using Finova.Countries.Africa.Tunisia.Validators;
using Finova.Countries.Asia.Kazakhstan.Validators;
using Finova.Countries.Europe.Russia.Validators;
using Finova.Countries.SoutheastAsia.Vietnam.Validators;
using Xunit;

namespace Finova.Tests.Validators;

public class MissingValidatorsTests
{
    [Fact]
    public void RussiaInnValidator_ValidatesCorrectly()
    {
        var validator = new RussiaInnValidator();
        Assert.True(validator.Validate("7707083893").IsValid); // Sberbank
        Assert.False(validator.Validate("7707083890").IsValid); // Invalid checksum
    }

    [Fact]
    public void MoroccoIceValidator_ValidatesCorrectly()
    {
        var validator = new MoroccoIceValidator();
        Assert.True(validator.Validate("001525487000088").IsValid);
        Assert.False(validator.Validate("123").IsValid); // Length
    }

    [Fact]
    public void AlgeriaNifValidator_ValidatesCorrectly()
    {
        var validator = new AlgeriaNifValidator();
        Assert.True(validator.Validate("000016001275946").IsValid); // Sonatrach (approx)
        Assert.False(validator.Validate("ABC").IsValid);
    }

    [Fact]
    public void TunisiaMatriculeFiscalValidator_ValidatesCorrectly()
    {
        var validator = new TunisiaMatriculeFiscalValidator();
        Assert.True(validator.Validate("1234567A/B/M/000").IsValid);
        Assert.True(validator.Validate("1234567ABM000").IsValid); // Without slashes
        Assert.False(validator.Validate("123").IsValid);
    }

    [Fact]
    public void EgyptTaxRegistrationNumberValidator_ValidatesCorrectly()
    {
        var validator = new EgyptTaxRegistrationNumberValidator();
        Assert.True(validator.Validate("100-200-300").IsValid);
        Assert.True(validator.Validate("100200300").IsValid);
        Assert.False(validator.Validate("12345").IsValid);
    }

    [Fact]
    public void KazakhstanBinValidator_ValidatesCorrectly()
    {
        var validator = new KazakhstanBinValidator();
        Assert.True(validator.Validate("980540003232").IsValid); // Kaspi Bank
        Assert.False(validator.Validate("980540003230").IsValid); // Bad checksum
    }

    [Fact]
    public void VietnamTaxIdValidator_ValidatesCorrectly()
    {
        var validator = new VietnamTaxIdValidator();
        Assert.True(validator.Validate("0100109106").IsValid); // Vietcombank
        Assert.False(validator.Validate("0100109100").IsValid); // Bad checksum
    }

    [Fact]
    public void NigeriaTinValidator_ValidatesCorrectly()
    {
        var validator = new NigeriaTinValidator();
        Assert.True(validator.Validate("12345678").IsValid);
        Assert.False(validator.Validate("123").IsValid);
    }
}
