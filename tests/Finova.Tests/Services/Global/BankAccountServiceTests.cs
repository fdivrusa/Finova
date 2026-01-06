using Finova.Core.Common;
using Finova.Core.Identifiers;
using Finova.Services;
using Xunit;

namespace Finova.Tests.Services.Global;

/// <summary>
/// Tests for BankAccountService to ensure Parse method iterates through parsers
/// similar to how Validate iterates through validators.
/// </summary>
public class BankAccountServiceTests
{
    /// <summary>
    /// Mock validator that validates account numbers starting with "AAA"
    /// </summary>
    private class MockAccountValidator1 : IBankAccountValidator
    {
        public string CountryCode => "XX";

        public ValidationResult Validate(string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidInput, "Input cannot be empty");
            }

            if (input.StartsWith("AAA"))
            {
                return ValidationResult.Success();
            }

            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Invalid format for validator 1");
        }

        public string? Parse(string? input)
        {
            if (Validate(input).IsValid)
            {
                return input;
            }
            return null;
        }
    }

    /// <summary>
    /// Mock validator that validates account numbers starting with "BBB"
    /// </summary>
    private class MockAccountValidator2 : IBankAccountValidator
    {
        public string CountryCode => "XX";

        public ValidationResult Validate(string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidInput, "Input cannot be empty");
            }

            if (input.StartsWith("BBB"))
            {
                return ValidationResult.Success();
            }

            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Invalid format for validator 2");
        }

        public string? Parse(string? input)
        {
            if (Validate(input).IsValid)
            {
                return input;
            }
            return null;
        }
    }

    /// <summary>
    /// Mock parser that parses account numbers starting with "AAA"
    /// </summary>
    private class MockAccountParser1 : IBankAccountParser
    {
        public string CountryCode => "XX";

        public BankAccountDetails? ParseBankAccount(string? accountNumber)
        {
            if (string.IsNullOrWhiteSpace(accountNumber))
            {
                return null;
            }

            if (accountNumber.StartsWith("AAA"))
            {
                return new BankAccountDetails
                {
                    AccountNumber = accountNumber,
                    CountryCode = "XX",
                    AccountType = "Parser1"
                };
            }

            return null;
        }
    }

    /// <summary>
    /// Mock parser that parses account numbers starting with "BBB"
    /// </summary>
    private class MockAccountParser2 : IBankAccountParser
    {
        public string CountryCode => "XX";

        public BankAccountDetails? ParseBankAccount(string? accountNumber)
        {
            if (string.IsNullOrWhiteSpace(accountNumber))
            {
                return null;
            }

            if (accountNumber.StartsWith("BBB"))
            {
                return new BankAccountDetails
                {
                    AccountNumber = accountNumber,
                    CountryCode = "XX",
                    AccountType = "Parser2"
                };
            }

            return null;
        }
    }

    [Fact]
    public void Validate_WithMultipleValidators_ReturnsFirstValidResult()
    {
        // Arrange
        var validators = new IBankAccountValidator[]
        {
            new MockAccountValidator1(),
            new MockAccountValidator2()
        };
        var parsers = new IBankAccountParser[] { };
        var service = new BankAccountService(validators, parsers);

        // Act - account starts with "BBB", should match second validator
        var result = service.Validate("XX", "BBB123456");

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validate_WithMultipleValidators_FirstValidatorMatch_ReturnsSuccess()
    {
        // Arrange
        var validators = new IBankAccountValidator[]
        {
            new MockAccountValidator1(),
            new MockAccountValidator2()
        };
        var parsers = new IBankAccountParser[] { };
        var service = new BankAccountService(validators, parsers);

        // Act - account starts with "AAA", should match first validator
        var result = service.Validate("XX", "AAA123456");

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public void Parse_WithMultipleParsers_ReturnsFirstSuccessfulParse()
    {
        // Arrange
        var validators = new IBankAccountValidator[] { };
        var parsers = new IBankAccountParser[]
        {
            new MockAccountParser1(),
            new MockAccountParser2()
        };
        var service = new BankAccountService(validators, parsers);

        // Act - account starts with "BBB", should be parsed by second parser
        var result = service.Parse("XX", "BBB123456");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Parser2", result.AccountType);
        Assert.Equal("BBB123456", result.AccountNumber);
    }

    [Fact]
    public void Parse_WithMultipleParsers_FirstParserMatch_ReturnsCorrectResult()
    {
        // Arrange
        var validators = new IBankAccountValidator[] { };
        var parsers = new IBankAccountParser[]
        {
            new MockAccountParser1(),
            new MockAccountParser2()
        };
        var service = new BankAccountService(validators, parsers);

        // Act - account starts with "AAA", should be parsed by first parser
        var result = service.Parse("XX", "AAA123456");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Parser1", result.AccountType);
        Assert.Equal("AAA123456", result.AccountNumber);
    }

    [Fact]
    public void Parse_WithNonMatchingInput_ReturnsNull()
    {
        // Arrange
        var validators = new IBankAccountValidator[] { };
        var parsers = new IBankAccountParser[]
        {
            new MockAccountParser1(),
            new MockAccountParser2()
        };
        var service = new BankAccountService(validators, parsers);

        // Act - account starts with "CCC", doesn't match any parser
        var result = service.Parse("XX", "CCC123456");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void Parse_WithEmptyCountryCode_ReturnsNull()
    {
        // Arrange
        var validators = new IBankAccountValidator[] { };
        var parsers = new IBankAccountParser[] { new MockAccountParser1() };
        var service = new BankAccountService(validators, parsers);

        // Act
        var result = service.Parse("", "AAA123456");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void Validate_WithNoValidators_ReturnsUnsupportedCountry()
    {
        // Arrange
        var validators = new IBankAccountValidator[] { };
        var parsers = new IBankAccountParser[] { };
        var service = new BankAccountService(validators, parsers);

        // Act
        var result = service.Validate("XX", "AAA123456");

        // Assert
        Assert.False(result.IsValid);
        Assert.Equal(ValidationErrorCode.UnsupportedCountry, result.Errors[0].Code);
    }
}
