using Finova.Core.Validators;
using Finova.Core.Models;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Core.Validators
{
    public class BicValidatorTests
    {
        private readonly BicValidator _validator;

        public BicValidatorTests()
        {
            _validator = new BicValidator();
        }

        #region IsValid Tests

        [Theory]
        [InlineData("DEUTDEFF")] // Deutsche Bank, Frankfurt (8 chars)
        [InlineData("DEUTDEFFXXX")] // Deutsche Bank, Frankfurt with branch (11 chars)
        [InlineData("BNPAFRPP")] // BNP Paribas, Paris
        [InlineData("BNPAFRPPXXX")] // BNP Paribas with branch
        [InlineData("CHASUS33")] // JP Morgan Chase, USA
        [InlineData("CHASUS33XXX")] // JP Morgan Chase with branch
        [InlineData("HSBCGB2L")] // HSBC, London
        [InlineData("HSBCGB2LXXX")] // HSBC with branch
        [InlineData("ABNANL2A")] // ABN AMRO, Netherlands
        [InlineData("GEBABEBB")] // BNP Paribas Fortis, Belgium
        [InlineData("BBVABEBB")] // BBVA, Belgium
        [InlineData("AAAABB11")] // Generic valid format (digits in location code OK)
        [InlineData("BBBBCCDD123")] // Generic valid format with branch
        public void IsValid_WithValidBics_ReturnsTrue(string bic)
        {
            BicValidator.IsValid(bic).Should().BeTrue();
            ((Finova.Core.Interfaces.IBicValidator)_validator).IsValid(bic).Should().BeTrue();
        }

        [Theory]
        [InlineData("")] // Empty
        [InlineData(null)] // Null
        [InlineData("   ")] // Whitespace
        [InlineData("DEUT")] // Too short (4 chars)
        [InlineData("DEUTDE")] // Too short (6 chars)
        [InlineData("DEUTDEF")] // Too short (7 chars)
        [InlineData("DEUTDEFF1")] // Invalid length (9 chars)
        [InlineData("DEUTDEFF12")] // Invalid length (10 chars)
        [InlineData("DEUTDEFF1234")] // Too long (12 chars)
        [InlineData("1EUTDEFF")] // Bank code starts with digit
        [InlineData("D2UTDEFF")] // Bank code contains digit
        [InlineData("DE1TDEFF")] // Bank code contains digit
        [InlineData("DEU2DEFF")] // Bank code contains digit
        [InlineData("DEUT1EFF")] // Country code contains digit
        [InlineData("DEUTD2FF")] // Country code contains digit
        [InlineData("DEUTDE@F")] // Location code contains special char
        [InlineData("DEUTDEF@")] // Location code contains special char
        [InlineData("DEUTDEFF@XX")] // Branch code contains special char
        [InlineData("DEUTDEFFX@X")] // Branch code contains special char
        [InlineData("DEUTDEFFXX@")] // Branch code contains special char
        public void IsValid_WithInvalidBics_ReturnsFalse(string? bic)
        {
            BicValidator.IsValid(bic).Should().BeFalse();
            ((Finova.Core.Interfaces.IBicValidator)_validator).IsValid(bic).Should().BeFalse();
        }

        [Fact]
        public void IsValid_WithLowercaseBic_ReturnsTrue()
        {
            // IsValid accepts lowercase letters
            var bic = "deutdeff";
            BicValidator.IsValid(bic).Should().BeTrue();
        }

        [Theory]
        [InlineData("AAAA")]
        [InlineData("AAAABB")]
        [InlineData("AAAABB1")]
        [InlineData("AAAABB11A")]
        public void IsValid_WithIncompleteBics_ReturnsFalse(string bic)
        {
            BicValidator.IsValid(bic).Should().BeFalse();
        }

        #endregion

        #region Parse Tests

        [Fact]
        public void Parse_WithValid8CharBic_ReturnsCorrectDetails()
        {
            var bic = "DEUTDEFF";
            var result = BicValidator.Parse(bic);

            result.Should().NotBeNull();
            result!.Bic.Should().Be("DEUTDEFFXXX"); // Normalized to 11 chars
            result.BankCode.Should().Be("DEUT");
            result.CountryCode.Should().Be("DE");
            result.LocationCode.Should().Be("FF");
            result.BranchCode.Should().Be("XXX"); // Default branch
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Parse_WithValid11CharBic_ReturnsCorrectDetails()
        {
            var bic = "DEUTDEFF500";
            var result = BicValidator.Parse(bic);

            result.Should().NotBeNull();
            result!.Bic.Should().Be("DEUTDEFF500");
            result.BankCode.Should().Be("DEUT");
            result.CountryCode.Should().Be("DE");
            result.LocationCode.Should().Be("FF");
            result.BranchCode.Should().Be("500");
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Parse_WithLowercaseBic_NormalizesToUppercase()
        {
            var bic = "deutdeff";
            var result = BicValidator.Parse(bic);

            result.Should().NotBeNull();
            result!.Bic.Should().Be("DEUTDEFFXXX");
            result.BankCode.Should().Be("DEUT");
            result.CountryCode.Should().Be("DE");
        }

        [Fact]
        public void Parse_WithMixedCaseBic_NormalizesToUppercase()
        {
            var bic = "DeUtDeFf";
            var result = BicValidator.Parse(bic);

            result.Should().NotBeNull();
            result!.Bic.Should().Be("DEUTDEFFXXX");
        }

        [Theory]
        [InlineData("BNPAFRPP", "BNPA", "FR", "PP", "XXX")]
        [InlineData("CHASUS33", "CHAS", "US", "33", "XXX")]
        [InlineData("HSBCGB2L", "HSBC", "GB", "2L", "XXX")]
        [InlineData("GEBABEBB036", "GEBA", "BE", "BB", "036")]
        public void Parse_WithVariousBics_ExtractsComponentsCorrectly(
            string bic, string expectedBank, string expectedCountry,
            string expectedLocation, string expectedBranch)
        {
            var result = BicValidator.Parse(bic);

            result.Should().NotBeNull();
            result!.BankCode.Should().Be(expectedBank);
            result.CountryCode.Should().Be(expectedCountry);
            result.LocationCode.Should().Be(expectedLocation);
            result.BranchCode.Should().Be(expectedBranch);
            result.IsValid.Should().BeTrue();
        }

        [Theory]
        [InlineData("")] // Empty
        [InlineData(null)] // Null
        [InlineData("DEUTDEF")] // Too short
        [InlineData("DEUTDEFF1")] // Invalid length
        [InlineData("1EUTDEFF")] // Invalid bank code
        [InlineData("DEUT1EFF")] // Invalid country code
        public void Parse_WithInvalidBics_ReturnsNull(string? bic)
        {
            BicValidator.Parse(bic).Should().BeNull();
            ((Finova.Core.Interfaces.IBicValidator)_validator).Parse(bic).Should().BeNull();
        }

        [Fact]
        public void Parse_CalledMultipleTimes_ReturnsConsistentResults()
        {
            var bic = "DEUTDEFF500";
            var result1 = BicValidator.Parse(bic);
            var result2 = BicValidator.Parse(bic);

            result1.Should().NotBeNull();
            result2.Should().NotBeNull();
            result1.Should().BeEquivalentTo(result2);
        }

        #endregion

        #region IsConsistentWithIban Tests

        [Theory]
        [InlineData("DEUTDEFF", "DE")] // German BIC with German IBAN
        [InlineData("BNPAFRPP", "FR")] // French BIC with French IBAN
        [InlineData("CHASUS33", "US")] // US BIC with US code
        [InlineData("HSBCGB2L", "GB")] // UK BIC with UK IBAN
        [InlineData("GEBABEBB", "BE")] // Belgian BIC with Belgian IBAN
        [InlineData("ABNANL2A", "NL")] // Dutch BIC with Dutch IBAN
        [InlineData("BCIPIT2B", "IT")] // Italian BIC with Italian IBAN
        [InlineData("CAIXESBB", "ES")] // Spanish BIC with Spanish IBAN
        [InlineData("DEUTDEFF500", "DE")] // With branch code
        public void IsConsistentWithIban_WithMatchingCountryCodes_ReturnsTrue(string bic, string ibanCountryCode)
        {
            BicValidator.IsConsistentWithIban(bic, ibanCountryCode).Should().BeTrue();
            ((Finova.Core.Interfaces.IBicValidator)_validator).IsConsistentWithIban(bic, ibanCountryCode).Should().BeTrue();
        }

        [Theory]
        [InlineData("DEUTDEFF", "FR")] // German BIC with French IBAN
        [InlineData("BNPAFRPP", "DE")] // French BIC with German IBAN
        [InlineData("HSBCGB2L", "US")] // UK BIC with US code
        [InlineData("GEBABEBB", "NL")] // Belgian BIC with Dutch IBAN
        public void IsConsistentWithIban_WithMismatchedCountryCodes_ReturnsFalse(string bic, string ibanCountryCode)
        {
            BicValidator.IsConsistentWithIban(bic, ibanCountryCode).Should().BeFalse();
        }

        [Theory]
        [InlineData("deutdeff", "de")] // Lowercase
        [InlineData("DEUTDEFF", "de")] // Mixed case
        [InlineData("deutdeff", "DE")] // Mixed case
        public void IsConsistentWithIban_IsCaseInsensitive(string bic, string ibanCountryCode)
        {
            BicValidator.IsConsistentWithIban(bic, ibanCountryCode).Should().BeTrue();
        }

        [Theory]
        [InlineData(null, "DE")]
        [InlineData("DEUTDEFF", null)]
        [InlineData(null, null)]
        [InlineData("", "DE")]
        [InlineData("DEUTDEFF", "")]
        [InlineData("   ", "DE")]
        [InlineData("DEUTDEFF", "   ")]
        public void IsConsistentWithIban_WithNullOrEmptyValues_ReturnsFalse(string? bic, string? ibanCountryCode)
        {
            BicValidator.IsConsistentWithIban(bic, ibanCountryCode).Should().BeFalse();
        }

        [Fact]
        public void IsConsistentWithIban_WithTooShortBic_ReturnsFalse()
        {
            var bic = "DEUT"; // Only 4 chars
            var ibanCountryCode = "DE";
            BicValidator.IsConsistentWithIban(bic, ibanCountryCode).Should().BeFalse();
        }

        #endregion

        #region IBicValidator Interface Tests

        [Fact]
        public void IBicValidator_IsValid_DelegatesToStaticMethod()
        {
            var validator = (Finova.Core.Interfaces.IBicValidator)_validator;
            var bic = "DEUTDEFF";

            var staticResult = BicValidator.IsValid(bic);
            var instanceResult = validator.IsValid(bic);

            instanceResult.Should().Be(staticResult);
        }

        [Fact]
        public void IBicValidator_Parse_DelegatesToStaticMethod()
        {
            var validator = (Finova.Core.Interfaces.IBicValidator)_validator;
            var bic = "DEUTDEFF";

            var staticResult = BicValidator.Parse(bic);
            var instanceResult = validator.Parse(bic);

            instanceResult.Should().BeEquivalentTo(staticResult);
        }

        [Fact]
        public void IBicValidator_IsConsistentWithIban_DelegatesToStaticMethod()
        {
            var validator = (Finova.Core.Interfaces.IBicValidator)_validator;
            var bic = "DEUTDEFF";
            var country = "DE";

            var staticResult = BicValidator.IsConsistentWithIban(bic, country);
            var instanceResult = validator.IsConsistentWithIban(bic, country);

            instanceResult.Should().Be(staticResult);
        }

        #endregion

        #region Edge Cases and Special Scenarios

        [Fact]
        public void Parse_WithAlphanumericLocationCode_ParsesCorrectly()
        {
            var bic = "HSBC GB 2L XXX".Replace(" ", "");
            var result = BicValidator.Parse(bic);

            result.Should().NotBeNull();
            result!.LocationCode.Should().Be("2L");
        }

        [Fact]
        public void Parse_WithNumericLocationCode_ParsesCorrectly()
        {
            var bic = "CHAS US 33 XXX".Replace(" ", "");
            var result = BicValidator.Parse(bic);

            result.Should().NotBeNull();
            result!.LocationCode.Should().Be("33");
        }

        [Fact]
        public void IsValid_WithAllLetterLocationCode_ReturnsTrue()
        {
            var bic = "DEUTDEFF";
            BicValidator.IsValid(bic).Should().BeTrue();
        }

        [Fact]
        public void IsValid_WithAllDigitLocationCode_ReturnsTrue()
        {
            var bic = "DEUT DE 11".Replace(" ", "");
            BicValidator.IsValid(bic).Should().BeTrue();
        }

        [Theory]
        [InlineData("AAAABBCC")]
        [InlineData("ZZZZYYXX")]
        [InlineData("TESTAA11")]
        public void IsValid_WithEdgeCaseBankAndCountryCodes_ReturnsTrue(string bic)
        {
            BicValidator.IsValid(bic).Should().BeTrue();
        }

        #endregion
    }
}
