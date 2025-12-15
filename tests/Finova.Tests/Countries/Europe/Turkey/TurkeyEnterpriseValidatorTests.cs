using Finova.Countries.Europe.Turkey.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Turkey;

public class TurkeyEnterpriseValidatorTests
{
    [Theory]
    // Valid VKN: 0010054532 (Example found online or calculated)
    // Let's calculate one to be sure.
    // 001005453
    // i=0, d=0. tmp=(0+9)%10=9. val=9.
    // i=1, d=0. tmp=(0+8)%10=8. val=(8*(2^8))%9 = (8*256)%9 = 2048%9 = 5.
    // i=2, d=1. tmp=(1+7)%10=8. val=(8*(2^7))%9 = (8*128)%9 = 1024%9 = 7.
    // i=3, d=0. tmp=(0+6)%10=6. val=(6*(2^6))%9 = (6*64)%9 = 384%9 = 6.
    // i=4, d=0. tmp=(0+5)%10=5. val=(5*(2^5))%9 = (5*32)%9 = 160%9 = 7.
    // i=5, d=5. tmp=(5+4)%10=9. val=9.
    // i=6, d=4. tmp=(4+3)%10=7. val=(7*(2^3))%9 = (7*8)%9 = 56%9 = 2.
    // i=7, d=5. tmp=(5+2)%10=7. val=(7*(2^2))%9 = (7*4)%9 = 28%9 = 1.
    // i=8, d=3. tmp=(3+1)%10=4. val=(4*(2^1))%9 = (4*2)%9 = 8.
    // Sum = 9+5+7+6+7+9+2+1+8 = 54.
    // CheckDigit = (10 - (54%10)) % 10 = (10 - 4) % 10 = 6.
    // So 0010054536 is valid.
    [InlineData("0010054536", true)]
    [InlineData("TR 0010054536", true)] // Valid with prefix
    [InlineData("0010054537", false)] // Invalid checksum
    [InlineData("123", false)] // Invalid length
    public void Validate_ShouldReturnExpectedResult(string number, bool expectedIsValid)
    {
        var result = TurkeyVknValidator.ValidateVkn(number);
        result.IsValid.Should().Be(expectedIsValid);
    }

    [Fact]
    public void Normalize_ShouldReturnCleanedNumber()
    {
        var result = TurkeyVknValidator.Normalize("TR 0010054536");
        result.Should().Be("0010054536");
    }

    [Fact]
    public void Normalize_WithInvalidFormat_ShouldReturnNull()
    {
        var result = TurkeyVknValidator.Normalize("INVALID");
        result.Should().BeNull();
    }
}
