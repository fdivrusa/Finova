using Finova.France.Models;
using Finova.France.Services;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.France.Services
{
    public class FranceIbanParserTests
    {
        private readonly FranceIbanParser _parser;

        public FranceIbanParserTests()
        {
            _parser = FranceIbanParser.Create();
        }

        [Fact]
        public void CountryCode_ReturnsFR()
        {
            _parser.CountryCode.Should().Be("FR");
        }

        [Fact]
        public void ParseIban_WithValidFrenchIban_ReturnsDetails()
        {
            var iban = "FR1420041010050500013M02606";
            var result = _parser.ParseIban(iban);

            result.Should().NotBeNull();
            result.Should().BeOfType<FranceIbanDetails>();
        }

        [Fact]
        public void ParseIban_ExtractsAllComponents()
        {
            var iban = "FR1420041010050500013M02606";
            var result = _parser.ParseIban(iban) as FranceIbanDetails;

            result.Should().NotBeNull();
            result!.Iban.Should().Be("FR1420041010050500013M02606");
            result.CountryCode.Should().Be("FR");
            result.CheckDigits.Should().Be("14");
            result.BankCode.Should().Be("20041");
            result.BankCodeFr.Should().Be("20041");
            result.BranchCode.Should().Be("01005");
            result.BranchCodeFr.Should().Be("01005");
            result.AccountNumber.Should().Be("0500013M026");
            result.AccountNumberFr.Should().Be("0500013M026");
            result.RibKey.Should().Be("06");
            result.NationalCheckKey.Should().Be("06");
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void ParseIban_FormatsRibCorrectly()
        {
            var iban = "FR1420041010050500013M02606";
            var result = _parser.ParseIban(iban) as FranceIbanDetails;

            result.Should().NotBeNull();
            result!.FormattedRib.Should().Be("20041 01005 0500013M026 06");
        }

        [Theory]
        [InlineData("FR14 2004 1010 0505 0001 3M02 606")] // With spaces
        [InlineData("fr1420041010050500013m02606")] // Lowercase
        public void ParseIban_WithFormattedInput_NormalizesAndParses(string iban)
        {
            var result = _parser.ParseIban(iban) as FranceIbanDetails;

            result.Should().NotBeNull();
            result!.Iban.Should().Be("FR1420041010050500013M02606");
        }

        [Theory]
        [InlineData("FR0020041010050500013M02606")] // Wrong check digits
        [InlineData("BE68539007547034")] // Wrong country
        [InlineData("")] // Empty
        [InlineData(null)] // Null
        public void ParseIban_WithInvalidIban_ReturnsNull(string? iban)
        {
            _parser.ParseIban(iban).Should().BeNull();
        }

        [Fact]
        public void Create_ReturnsValidParser()
        {
            var parser = FranceIbanParser.Create();
            parser.Should().NotBeNull();
            parser.CountryCode.Should().Be("FR");
        }
    }
}
