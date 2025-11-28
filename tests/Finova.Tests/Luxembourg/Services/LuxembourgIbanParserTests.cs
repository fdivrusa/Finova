using Finova.Luxembourg.Models;
using Finova.Luxembourg.Services;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Luxembourg.Services
{
    public class LuxembourgIbanParserTests
    {
        private readonly LuxembourgIbanParser _parser = LuxembourgIbanParser.Create();

        [Fact]
        public void CountryCode_ReturnsLU() => _parser.CountryCode.Should().Be("LU");

        [Fact]
        public void ParseIban_ExtractsAllComponents()
        {
            var result = _parser.ParseIban("LU280019400644750000") as LuxembourgIbanDetails;

            result.Should().NotBeNull();
            result!.Iban.Should().Be("LU280019400644750000");
            result.CountryCode.Should().Be("LU");
            result.CheckDigits.Should().Be("28");
            result.BankCodeLu.Should().Be("001");
            result.AccountNumberLu.Should().Be("9400644750000");
            result.IsValid.Should().BeTrue();
        }

        [Theory]
        [InlineData("LU000019400644750000")] // Invalid
        [InlineData("")] // Empty
        [InlineData(null)] // Null
        public void ParseIban_WithInvalidIban_ReturnsNull(string? iban)
            => _parser.ParseIban(iban).Should().BeNull();

        [Fact]
        public void Create_ReturnsValidParser()
            => LuxembourgIbanParser.Create().CountryCode.Should().Be("LU");
    }
}
