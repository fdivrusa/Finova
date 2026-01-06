using Finova.Countries.Europe.Switzerland.Validators;
using Xunit;

namespace Finova.Tests.Countries.Europe.Switzerland;

public class SwitzerlandEnterpriseValidatorTests
{
    [Theory]
    [InlineData("CHE-123.456.788", true)] // Valid UID
    [InlineData("123.456.788", true)] // Valid UID (no prefix)
    [InlineData("123456788", true)] // Valid UID (raw)
    [InlineData("CHE-123.456.780", false)] // Invalid Checksum
    [InlineData("12345678", false)] // Too short
    [InlineData("1234567890", false)] // Too long
    [InlineData("ABC.456.789", false)] // Invalid chars
    public void Uid_Validate_ReturnsExpectedResult(string? input, bool expectedIsValid)
    {
        var result = SwitzerlandUidValidator.ValidateUid(input);
        Assert.Equal(expectedIsValid, result.IsValid);
    }

    [Fact]
    public void Uid_Format_ReturnsFormattedString()
    {
        Assert.Equal("CHE-123.456.788", SwitzerlandUidValidator.Format("123456788"));
    }
}
