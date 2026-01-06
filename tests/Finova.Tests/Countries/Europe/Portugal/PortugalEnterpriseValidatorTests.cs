using Finova.Countries.Europe.Portugal.Validators;
using Xunit;

namespace Finova.Tests.Countries.Europe.Portugal;

public class PortugalEnterpriseValidatorTests
{
    [Theory]
    [InlineData("501964843", true)] // Valid NIF (Company)
    [InlineData("PT501964843", true)] // Valid NIF (Prefix)
    [InlineData("123456789", true)] // Valid NIF (Individual - hypothetical valid checksum)
    [InlineData("501964842", false)] // Invalid Checksum
    [InlineData("12345678", false)] // Too short
    [InlineData("1234567890", false)] // Too long
    [InlineData("ABC456789", false)] // Invalid chars
    public void Validate_ValidNif_ReturnsExpectedResult(string? input, bool expectedIsValid)
    {
        var result = PortugalNifValidator.ValidateNif(input);
        Assert.Equal(expectedIsValid, result.IsValid);
    }

    [Fact]
    public void Nif_Format_ReturnsNormalizedString()
    {
        Assert.Equal("501964843", PortugalNifValidator.Format("PT 501 964 843"));
    }
}
