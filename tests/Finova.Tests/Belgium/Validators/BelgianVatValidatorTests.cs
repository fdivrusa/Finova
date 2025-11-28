using Finova.Belgium.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Belgium.Validators
{
    public class BelgianVatValidatorTests
    {
        #region IsValid Tests

        [Theory]
        [InlineData("BE0123456749")]
        [InlineData("BE0403170701")]
        public void IsValid_WithValidVatNumbers_ReturnsTrue(string vat)
        {
            // Act
            var result = BelgiumVatValidator.IsValid(vat);

            // Assert
            result.Should().BeTrue();
        }

        [Theory]
        [InlineData("BE 0123.456.749")] // With spaces and dots
        [InlineData("be0123456749")] // Lowercase
        [InlineData("Be0123456749")] // Mixed case
        [InlineData(" BE0123456749 ")] // With whitespace
        public void IsValid_WithFormattedVatNumbers_ReturnsTrue(string vat)
        {
            // Act
            var result = BelgiumVatValidator.IsValid(vat);

            // Assert
            result.Should().BeTrue();
        }

        [Theory]
        [InlineData("0123456749")] // Without BE prefix (should still work - delegates to KBO)
        public void IsValid_WithoutBEPrefix_ReturnsTrue(string vat)
        {
            // Act
            var result = BelgiumVatValidator.IsValid(vat);

            // Assert
            result.Should().BeTrue();
        }

        [Theory]
        [InlineData("BE0123456700")] // Wrong check digits
        [InlineData("BE0000000000")] // All zeros
        [InlineData("BE2123456749")] // Doesn't start with 0 or 1
        [InlineData("BE012345674")] // Too short
        [InlineData("BE01234567490")] // Too long
        [InlineData("BE012345674X")] // Contains letter in number
        [InlineData("")] // Empty
        [InlineData("   ")] // Whitespace
        [InlineData(null)] // Null
        public void IsValid_WithInvalidVatNumbers_ReturnsFalse(string? vat)
        {
            // Act
            var result = BelgiumVatValidator.IsValid(vat);

            // Assert
            result.Should().BeFalse();
        }

        #endregion

        #region Format Tests

        [Theory]
        [InlineData("BE0123456749", "BE 0123.456.749")]
        [InlineData("BE0403170701", "BE 0403.170.701")]
        public void Format_WithValidVatNumbers_ReturnsFormattedString(string vat, string expected)
        {
            // Act
            var result = BelgiumVatValidator.Format(vat);

            // Assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData("0123456749", "BE 0123.456.749")] // Without BE prefix
        [InlineData("BE 0123.456.749", "BE 0123.456.749")] // Already formatted
        [InlineData("be0123456749", "BE 0123.456.749")] // Lowercase
        [InlineData(" BE0123456749 ", "BE 0123.456.749")] // With whitespace
        public void Format_WithVariousInputs_ReturnsConsistentFormat(string vat, string expected)
        {
            // Act
            var result = BelgiumVatValidator.Format(vat);

            // Assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData("BE0123456700")] // Invalid check digits
        [InlineData("BE012345674")] // Too short
        [InlineData("")] // Empty
        [InlineData(null)] // Null
        public void Format_WithInvalidVatNumbers_ThrowsArgumentException(string? vat)
        {
            // Act
            Action act = () => BelgiumVatValidator.Format(vat);

            // Assert
            act.Should().Throw<ArgumentException>();
        }

        #endregion

        #region Normalize Tests

        [Theory]
        [InlineData("BE 0123.456.749", "BE0123456749")]
        [InlineData("BE0123456749", "BE0123456749")]
        [InlineData("be 0123.456.749", "BE0123456749")]
        [InlineData(" BE0123456749 ", "BE0123456749")]
        [InlineData("0123456749", "BE0123456749")] // Adds BE prefix
        public void Normalize_ConvertsToStandardFormat(string input, string expected)
        {
            // Act
            var result = BelgiumVatValidator.Normalize(input);

            // Assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void Normalize_WithEmptyInput_ReturnsEmpty(string? input)
        {
            // Act
            var result = BelgiumVatValidator.Normalize(input);

            // Assert
            result.Should().BeEmpty();
        }

        #endregion

        #region GetEnterpriseNumber Tests

        [Theory]
        [InlineData("BE0123456749", "0123456749")]
        [InlineData("BE 0123.456.749", "0123456749")]
        [InlineData("0123456749", "0123456749")] // Already without BE
        [InlineData("be0123456749", "0123456749")] // Lowercase
        public void GetEnterpriseNumber_ExtractsKboNumber(string vat, string expected)
        {
            // Act
            var result = BelgiumVatValidator.GetEnterpriseNumber(vat);

            // Assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData("BE0123456700")] // Invalid check digits
        [InlineData("")] // Empty
        [InlineData(null)] // Null
        public void GetEnterpriseNumber_WithInvalidVat_ReturnsNull(string? vat)
        {
            // Act
            var result = BelgiumVatValidator.GetEnterpriseNumber(vat);

            // Assert
            result.Should().BeNull();
        }

        #endregion

        #region Real-World VAT Numbers

        [Theory]
        [InlineData("BE0403170701")] // Microsoft Belgium
        public void IsValid_WithRealVatNumbers_ReturnsTrue(string vat)
        {
            // Act
            var result = BelgiumVatValidator.IsValid(vat);

            // Assert
            result.Should().BeTrue();
        }

        [Theory]
        [InlineData("BE0403170701", "BE 0403.170.701")] // Microsoft Belgium
        public void Format_WithRealVatNumbers_ReturnsFormattedString(string vat, string expected)
        {
            // Act
            var result = BelgiumVatValidator.Format(vat);

            // Assert
            result.Should().Be(expected);
        }

        #endregion

        #region VAT and KBO Equivalence Tests

        [Theory]
        [InlineData("BE0123456749", "0123456749")]
        public void VatNumber_IsEquivalentToKboNumber(string vat, string kbo)
        {
            // Act
            var vatValid = BelgiumVatValidator.IsValid(vat);
            var kboValid = BelgiumEnterpriseValidator.IsValid(kbo);
            var extractedKbo = BelgiumVatValidator.GetEnterpriseNumber(vat);

            // Assert
            vatValid.Should().BeTrue();
            kboValid.Should().BeTrue();
            extractedKbo.Should().Be(kbo);
        }

        [Fact]
        public void VatValidator_DelegatesToEnterpriseValidator()
        {
            // Arrange
            var kbo = "0123456749";
            var vat = "BE" + kbo;

            // Act
            var kboResult = BelgiumEnterpriseValidator.IsValid(kbo);
            var vatResult = BelgiumVatValidator.IsValid(vat);

            // Assert
            kboResult.Should().Be(vatResult);
        }

        #endregion

        #region Edge Cases

        [Fact]
        public void IsValid_WithOnlyBEPrefix_ReturnsFalse()
        {
            // Arrange
            var vat = "BE";

            // Act
            var result = BelgiumVatValidator.IsValid(vat);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void Format_PreservesLeadingZeros()
        {
            // Arrange - Use valid VAT
            var vat = "BE0123456749";

            // Act
            var result = BelgiumVatValidator.Format(vat);

            // Assert
            result.Should().Be("BE 0123.456.749");
            result.Should().Contain("0123");
        }

        [Fact]
        public void GetEnterpriseNumber_ReturnsKboWithoutBEPrefix()
        {
            // Arrange
            var vat = "BE0123456749";

            // Act
            var kbo = BelgiumVatValidator.GetEnterpriseNumber(vat);

            // Assert
            kbo.Should().NotContain("BE");
            kbo.Should().HaveLength(10);
        }

        #endregion
    }
}
