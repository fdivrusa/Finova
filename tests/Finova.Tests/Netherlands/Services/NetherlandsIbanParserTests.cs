using Finova.Netherlands.Models;
using Finova.Netherlands.Services;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Netherlands.Services
{
    public class NetherlandsIbanParserTests
    {
        private readonly NetherlandsIbanParser _parser = NetherlandsIbanParser.Create();

        [Fact]
        public void CountryCode_ReturnsNL() => _parser.CountryCode.Should().Be("NL");

        [Fact]
        public void ParseIban_ExtractsAllComponents()
        {
            var result = _parser.ParseIban("NL91ABNA0417164300") as NetherlandsIbanDetails;

            result.Should().NotBeNull();
            result!.Iban.Should().Be("NL91ABNA0417164300");
            result.CountryCode.Should().Be("NL");
            result.CheckDigits.Should().Be("91");
            result.BankCodeNl.Should().Be("ABNA");
            result.AccountNumberNl.Should().Be("0417164300");
            result.IsValid.Should().BeTrue();
        }

        [Theory]
        [InlineData("NL00ABNA0417164300")] // Invalid
        [InlineData("")] // Empty
        [InlineData(null)] // Null
        public void ParseIban_WithInvalidIban_ReturnsNull(string? iban)
            => _parser.ParseIban(iban).Should().BeNull();

        [Fact]
        public void Create_ReturnsValidParser()
            => NetherlandsIbanParser.Create().CountryCode.Should().Be("NL");
    }
}
