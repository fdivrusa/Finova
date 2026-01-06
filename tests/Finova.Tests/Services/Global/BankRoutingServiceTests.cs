using Finova.Core.Common;
using Finova.Core.Identifiers;
using Finova.Services;
using Xunit;

namespace Finova.Tests.Services.Global;

/// <summary>
/// Tests for BankRoutingService to ensure Parse method iterates through parsers
/// similar to how Validate iterates through validators.
/// </summary>
public class BankRoutingServiceTests
{
    /// <summary>
    /// Mock validator that validates routing numbers starting with "111"
    /// </summary>
    private class MockRoutingValidator1 : IBankRoutingValidator
    {
        public string CountryCode => "US";

        public ValidationResult Validate(string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidInput, "Input cannot be empty");
            }

            var normalized = input.Replace("-", "").Replace(" ", "");
            if (normalized.StartsWith("111"))
            {
                return ValidationResult.Success();
            }

            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Invalid format for validator 1");
        }

        public string? Parse(string? input)
        {
            if (Validate(input).IsValid)
            {
                return input?.Replace("-", "").Replace(" ", "");
            }
            return null;
        }
    }

    /// <summary>
    /// Mock validator that validates routing numbers starting with "222"
    /// </summary>
    private class MockRoutingValidator2 : IBankRoutingValidator
    {
        public string CountryCode => "US";

        public ValidationResult Validate(string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidInput, "Input cannot be empty");
            }

            var normalized = input.Replace("-", "").Replace(" ", "");
            if (normalized.StartsWith("222"))
            {
                return ValidationResult.Success();
            }

            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Invalid format for validator 2");
        }

        public string? Parse(string? input)
        {
            if (Validate(input).IsValid)
            {
                return input?.Replace("-", "").Replace(" ", "");
            }
            return null;
        }
    }

    /// <summary>
    /// Mock parser that parses routing numbers starting with "111"
    /// </summary>
    private class MockRoutingParser1 : IBankRoutingParser
    {
        public string CountryCode => "US";

        public BankRoutingDetails? ParseRoutingNumber(string? routingNumber)
        {
            if (string.IsNullOrWhiteSpace(routingNumber))
            {
                return null;
            }

            var normalized = routingNumber.Replace("-", "").Replace(" ", "");
            if (normalized.StartsWith("111"))
            {
                return new BankRoutingDetails
                {
                    RoutingNumber = normalized,
                    CountryCode = "US",
                    BankCode = "Parser1"
                };
            }

            return null;
        }
    }

    /// <summary>
    /// Mock parser that parses routing numbers starting with "222"
    /// </summary>
    private class MockRoutingParser2 : IBankRoutingParser
    {
        public string CountryCode => "US";

        public BankRoutingDetails? ParseRoutingNumber(string? routingNumber)
        {
            if (string.IsNullOrWhiteSpace(routingNumber))
            {
                return null;
            }

            var normalized = routingNumber.Replace("-", "").Replace(" ", "");
            if (normalized.StartsWith("222"))
            {
                return new BankRoutingDetails
                {
                    RoutingNumber = normalized,
                    CountryCode = "US",
                    BankCode = "Parser2"
                };
            }

            return null;
        }
    }

    [Fact]
    public void Validate_WithMultipleValidators_ReturnsFirstValidResult()
    {
        // Arrange
        var validators = new IBankRoutingValidator[]
        {
            new MockRoutingValidator1(),
            new MockRoutingValidator2()
        };
        var parsers = new IBankRoutingParser[] { };
        var service = new BankRoutingService(validators, parsers);

        // Act - routing number starts with "222", should match second validator
        var result = service.Validate("US", "222123456");

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validate_WithMultipleValidators_FirstValidatorMatch_ReturnsSuccess()
    {
        // Arrange
        var validators = new IBankRoutingValidator[]
        {
            new MockRoutingValidator1(),
            new MockRoutingValidator2()
        };
        var parsers = new IBankRoutingParser[] { };
        var service = new BankRoutingService(validators, parsers);

        // Act - routing number starts with "111", should match first validator
        var result = service.Validate("US", "111123456");

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public void Parse_WithMultipleParsers_ReturnsFirstSuccessfulParse()
    {
        // Arrange
        var validators = new IBankRoutingValidator[] { };
        var parsers = new IBankRoutingParser[]
        {
            new MockRoutingParser1(),
            new MockRoutingParser2()
        };
        var service = new BankRoutingService(validators, parsers);

        // Act - routing number starts with "222", should be parsed by second parser
        var result = service.Parse("US", "222123456");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Parser2", result.BankCode);
        Assert.Equal("222123456", result.RoutingNumber);
    }

    [Fact]
    public void Parse_WithMultipleParsers_FirstParserMatch_ReturnsCorrectResult()
    {
        // Arrange
        var validators = new IBankRoutingValidator[] { };
        var parsers = new IBankRoutingParser[]
        {
            new MockRoutingParser1(),
            new MockRoutingParser2()
        };
        var service = new BankRoutingService(validators, parsers);

        // Act - routing number starts with "111", should be parsed by first parser
        var result = service.Parse("US", "111123456");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Parser1", result.BankCode);
        Assert.Equal("111123456", result.RoutingNumber);
    }

    [Fact]
    public void Parse_WithNonMatchingInput_ReturnsNull()
    {
        // Arrange
        var validators = new IBankRoutingValidator[] { };
        var parsers = new IBankRoutingParser[]
        {
            new MockRoutingParser1(),
            new MockRoutingParser2()
        };
        var service = new BankRoutingService(validators, parsers);

        // Act - routing number starts with "333", doesn't match any parser
        var result = service.Parse("US", "333123456");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void Parse_WithEmptyCountryCode_ReturnsNull()
    {
        // Arrange
        var validators = new IBankRoutingValidator[] { };
        var parsers = new IBankRoutingParser[] { new MockRoutingParser1() };
        var service = new BankRoutingService(validators, parsers);

        // Act
        var result = service.Parse("", "111123456");

        // Assert
        Assert.Null(result);
    }
}
