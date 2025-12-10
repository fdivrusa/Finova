using Finova.Core.Bic;
using Finova.Core.Iban;
using Finova.Core.PaymentCard;
using Finova.Extensions.FluentValidation;
using FluentAssertions;
using FluentValidation;
using Xunit;

namespace Finova.Tests.Integration;

/// <summary>
/// Integration tests for FluentValidation with Finova validators.
/// These tests verify that validators work correctly in real-world scenarios.
/// </summary>
public class FluentValidationIntegrationTests
{
    #region SEPA Payment Tests

    /// <summary>
    /// Represents a SEPA Credit Transfer request.
    /// </summary>
    public class SepaPaymentRequest
    {
        public string DebtorIban { get; set; } = string.Empty;
        public string DebtorBic { get; set; } = string.Empty;
        public string CreditorIban { get; set; } = string.Empty;
        public string CreditorBic { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "EUR";
        public string Reference { get; set; } = string.Empty;
    }

    public class SepaPaymentRequestValidator : AbstractValidator<SepaPaymentRequest>
    {
        public SepaPaymentRequestValidator()
        {
            RuleFor(x => x.DebtorIban)
                .NotEmpty().WithMessage("Debtor IBAN is required")
                .MustBeValidIban().WithMessage("Debtor IBAN is invalid");

            RuleFor(x => x.DebtorBic)
                .NotEmpty().WithMessage("Debtor BIC is required")
                .MustBeValidBic().WithMessage("Debtor BIC is invalid");

            RuleFor(x => x.CreditorIban)
                .NotEmpty().WithMessage("Creditor IBAN is required")
                .MustBeValidIban().WithMessage("Creditor IBAN is invalid");

            RuleFor(x => x.CreditorBic)
                .NotEmpty().WithMessage("Creditor BIC is required")
                .MustBeValidBic().WithMessage("Creditor BIC is invalid");

            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("Amount must be positive")
                .LessThanOrEqualTo(999999999.99m).WithMessage("Amount exceeds maximum allowed");

            RuleFor(x => x.Currency)
                .Equal("EUR").WithMessage("SEPA payments must be in EUR");
        }
    }

    [Fact]
    public void SepaPayment_WithValidData_ShouldPassValidation()
    {
        // Arrange
        var validator = new SepaPaymentRequestValidator();
        var request = new SepaPaymentRequest
        {
            DebtorIban = "BE68539007547034",      // Valid Belgian IBAN
            DebtorBic = "KREDBEBB",                // KBC Bank Belgium
            CreditorIban = "DE89370400440532013000", // Valid German IBAN
            CreditorBic = "COBADEFF",              // Commerzbank Germany
            Amount = 1500.00m,
            Currency = "EUR",
            Reference = "Invoice 2024-001"
        };

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void SepaPayment_WithInvalidDebtorIban_ShouldFailValidation()
    {
        // Arrange
        var validator = new SepaPaymentRequestValidator();
        var request = new SepaPaymentRequest
        {
            DebtorIban = "BE99999999999999",       // Invalid checksum
            DebtorBic = "KREDBEBB",
            CreditorIban = "DE89370400440532013000",
            CreditorBic = "COBADEFF",
            Amount = 1500.00m,
            Currency = "EUR"
        };

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "DebtorIban");
    }

    [Fact]
    public void SepaPayment_WithInvalidCreditorBic_ShouldFailValidation()
    {
        // Arrange
        var validator = new SepaPaymentRequestValidator();
        var request = new SepaPaymentRequest
        {
            DebtorIban = "BE68539007547034",
            DebtorBic = "KREDBEBB",
            CreditorIban = "DE89370400440532013000",
            CreditorBic = "INVALID",               // Invalid BIC format
            Amount = 1500.00m,
            Currency = "EUR"
        };

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "CreditorBic");
    }

    [Fact]
    public void SepaPayment_WithNonEurCurrency_ShouldFailValidation()
    {
        // Arrange
        var validator = new SepaPaymentRequestValidator();
        var request = new SepaPaymentRequest
        {
            DebtorIban = "BE68539007547034",
            DebtorBic = "KREDBEBB",
            CreditorIban = "DE89370400440532013000",
            CreditorBic = "COBADEFF",
            Amount = 1500.00m,
            Currency = "USD"                       // Not EUR
        };

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Currency");
    }

    [Fact]
    public void SepaPayment_WithZeroAmount_ShouldFailValidation()
    {
        // Arrange
        var validator = new SepaPaymentRequestValidator();
        var request = new SepaPaymentRequest
        {
            DebtorIban = "BE68539007547034",
            DebtorBic = "KREDBEBB",
            CreditorIban = "DE89370400440532013000",
            CreditorBic = "COBADEFF",
            Amount = 0m,                           // Zero amount
            Currency = "EUR"
        };

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Amount");
    }

    #endregion

    #region Card Payment Tests

    /// <summary>
    /// Represents a card payment request.
    /// </summary>
    public class CardPaymentRequest
    {
        public string CardNumber { get; set; } = string.Empty;
        public string CardholderName { get; set; } = string.Empty;
        public string ExpiryMonth { get; set; } = string.Empty;
        public string ExpiryYear { get; set; } = string.Empty;
        public string Cvv { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Currency { get; set; } = string.Empty;
    }

    public class CardPaymentRequestValidator : AbstractValidator<CardPaymentRequest>
    {
        public CardPaymentRequestValidator()
        {
            RuleFor(x => x.CardNumber)
                .NotEmpty().WithMessage("Card number is required")
                .MustBeValidPaymentCard().WithMessage("Card number is invalid");

            RuleFor(x => x.CardholderName)
                .NotEmpty().WithMessage("Cardholder name is required")
                .MaximumLength(100).WithMessage("Cardholder name is too long");

            RuleFor(x => x.ExpiryMonth)
                .NotEmpty().WithMessage("Expiry month is required")
                .Matches(@"^(0[1-9]|1[0-2])$").WithMessage("Invalid expiry month");

            RuleFor(x => x.ExpiryYear)
                .NotEmpty().WithMessage("Expiry year is required")
                .Matches(@"^\d{2}$").WithMessage("Invalid expiry year format");

            RuleFor(x => x.Cvv)
                .NotEmpty().WithMessage("CVV is required")
                .Matches(@"^\d{3,4}$").WithMessage("CVV must be 3 or 4 digits");

            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("Amount must be positive");

            RuleFor(x => x.Currency)
                .NotEmpty().WithMessage("Currency is required")
                .Length(3).WithMessage("Currency must be 3 characters");
        }
    }

    [Fact]
    public void CardPayment_WithValidVisaCard_ShouldPassValidation()
    {
        // Arrange
        var validator = new CardPaymentRequestValidator();
        var request = new CardPaymentRequest
        {
            CardNumber = "4532015112830366",      // Valid Visa test number
            CardholderName = "John Doe",
            ExpiryMonth = "12",
            ExpiryYear = "28",
            Cvv = "123",
            Amount = 99.99m,
            Currency = "EUR"
        };

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void CardPayment_WithValidMastercard_ShouldPassValidation()
    {
        // Arrange
        var validator = new CardPaymentRequestValidator();
        var request = new CardPaymentRequest
        {
            CardNumber = "5425233430109903",      // Valid Mastercard test number
            CardholderName = "Jane Smith",
            ExpiryMonth = "06",
            ExpiryYear = "27",
            Cvv = "456",
            Amount = 250.00m,
            Currency = "USD"
        };

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void CardPayment_WithValidAmex_ShouldPassValidation()
    {
        // Arrange
        var validator = new CardPaymentRequestValidator();
        var request = new CardPaymentRequest
        {
            CardNumber = "378282246310005",       // Valid Amex test number
            CardholderName = "Robert Johnson",
            ExpiryMonth = "03",
            ExpiryYear = "26",
            Cvv = "1234",                          // Amex has 4-digit CVV
            Amount = 500.00m,
            Currency = "GBP"
        };

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void CardPayment_WithInvalidLuhn_ShouldFailValidation()
    {
        // Arrange
        var validator = new CardPaymentRequestValidator();
        var request = new CardPaymentRequest
        {
            CardNumber = "4532015112830367",      // Invalid Luhn (changed last digit)
            CardholderName = "John Doe",
            ExpiryMonth = "12",
            ExpiryYear = "28",
            Cvv = "123",
            Amount = 99.99m,
            Currency = "EUR"
        };

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "CardNumber");
    }

    [Fact]
    public void CardPayment_WithEmptyCardNumber_ShouldFailValidation()
    {
        // Arrange
        var validator = new CardPaymentRequestValidator();
        var request = new CardPaymentRequest
        {
            CardNumber = "",
            CardholderName = "John Doe",
            ExpiryMonth = "12",
            ExpiryYear = "28",
            Cvv = "123",
            Amount = 99.99m,
            Currency = "EUR"
        };

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "CardNumber");
    }

    [Fact]
    public void CardPayment_WithInvalidExpiryMonth_ShouldFailValidation()
    {
        // Arrange
        var validator = new CardPaymentRequestValidator();
        var request = new CardPaymentRequest
        {
            CardNumber = "4532015112830366",
            CardholderName = "John Doe",
            ExpiryMonth = "13",                   // Invalid month
            ExpiryYear = "28",
            Cvv = "123",
            Amount = 99.99m,
            Currency = "EUR"
        };

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "ExpiryMonth");
    }

    [Fact]
    public void CardPayment_WithShortCvv_ShouldFailValidation()
    {
        // Arrange
        var validator = new CardPaymentRequestValidator();
        var request = new CardPaymentRequest
        {
            CardNumber = "4532015112830366",
            CardholderName = "John Doe",
            ExpiryMonth = "12",
            ExpiryYear = "28",
            Cvv = "12",                           // Too short
            Amount = 99.99m,
            Currency = "EUR"
        };

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Cvv");
    }

    #endregion

    #region Multi-Country Transfer Tests

    /// <summary>
    /// Represents an international money transfer request.
    /// </summary>
    public class InternationalTransferRequest
    {
        public string SenderIban { get; set; } = string.Empty;
        public string SenderBic { get; set; } = string.Empty;
        public string SenderCountry { get; set; } = string.Empty;
        public string RecipientIban { get; set; } = string.Empty;
        public string RecipientBic { get; set; } = string.Empty;
        public string RecipientCountry { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Currency { get; set; } = string.Empty;
        public string Purpose { get; set; } = string.Empty;
    }

    public class InternationalTransferRequestValidator : AbstractValidator<InternationalTransferRequest>
    {
        public InternationalTransferRequestValidator()
        {
            RuleFor(x => x.SenderIban)
                .NotEmpty().WithMessage("Sender IBAN is required")
                .MustBeValidIban().WithMessage("Sender IBAN is invalid");

            // Validate BIC format and that it matches the IBAN country
            RuleFor(x => x.SenderBic)
                .NotEmpty().WithMessage("Sender BIC is required")
                .MustBeValidBic().WithMessage("Sender BIC is invalid")
                .MustMatchIbanCountry(x => x.SenderIban).WithMessage("Sender BIC country must match sender IBAN country");

            RuleFor(x => x.SenderCountry)
                .NotEmpty().WithMessage("Sender country is required")
                .Length(2).WithMessage("Country code must be 2 characters");

            RuleFor(x => x.RecipientIban)
                .NotEmpty().WithMessage("Recipient IBAN is required")
                .MustBeValidIban().WithMessage("Recipient IBAN is invalid");

            // Validate BIC format and that it matches the IBAN country
            RuleFor(x => x.RecipientBic)
                .NotEmpty().WithMessage("Recipient BIC is required")
                .MustBeValidBic().WithMessage("Recipient BIC is invalid")
                .MustMatchIbanCountry(x => x.RecipientIban).WithMessage("Recipient BIC country must match recipient IBAN country");

            RuleFor(x => x.RecipientCountry)
                .NotEmpty().WithMessage("Recipient country is required")
                .Length(2).WithMessage("Country code must be 2 characters");

            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("Amount must be positive");

            RuleFor(x => x.Currency)
                .NotEmpty().WithMessage("Currency is required")
                .Length(3).WithMessage("Currency must be 3 characters");

            RuleFor(x => x.Purpose)
                .MaximumLength(140).WithMessage("Purpose description is too long");
        }
    }

    [Theory]
    [InlineData("BE68539007547034", "KREDBEBB", "BE", "DE89370400440532013000", "COBADEFF", "DE")]
    [InlineData("FR7630006000011234567890189", "AGRIFRPP", "FR", "IT60X0542811101000000123456", "BLOPIT22", "IT")]
    [InlineData("NL91ABNA0417164300", "ABNANL2A", "NL", "ES9121000418450200051332", "CAIXESBB", "ES")]
    [InlineData("LU280019400644750000", "BGLLLULL", "LU", "AT611904300234573201", "BKAUATWW", "AT")]
    public void InternationalTransfer_WithValidData_ShouldPassValidation(
        string senderIban, string senderBic, string senderCountry,
        string recipientIban, string recipientBic, string recipientCountry)
    {
        // Arrange
        var validator = new InternationalTransferRequestValidator();
        var request = new InternationalTransferRequest
        {
            SenderIban = senderIban,
            SenderBic = senderBic,
            SenderCountry = senderCountry,
            RecipientIban = recipientIban,
            RecipientBic = recipientBic,
            RecipientCountry = recipientCountry,
            Amount = 5000.00m,
            Currency = "EUR",
            Purpose = "Payment for services"
        };

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void InternationalTransfer_WithMismatchedSenderBic_ShouldFailValidation()
    {
        // Arrange
        var validator = new InternationalTransferRequestValidator();
        var request = new InternationalTransferRequest
        {
            SenderIban = "BE68539007547034",      // Belgian IBAN
            SenderBic = "COBADEFF",                // German BIC - mismatch with Belgian IBAN!
            SenderCountry = "BE",
            RecipientIban = "DE89370400440532013000",
            RecipientBic = "COBADEFF",
            RecipientCountry = "DE",
            Amount = 5000.00m,
            Currency = "EUR"
        };

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "SenderBic" && e.ErrorMessage.Contains("country"));
    }

    [Fact]
    public void InternationalTransfer_WithMismatchedRecipientBic_ShouldFailValidation()
    {
        // Arrange
        var validator = new InternationalTransferRequestValidator();
        var request = new InternationalTransferRequest
        {
            SenderIban = "BE68539007547034",
            SenderBic = "KREDBEBB",
            SenderCountry = "BE",
            RecipientIban = "DE89370400440532013000", // German IBAN
            RecipientBic = "KREDBEBB",                 // Belgian BIC - mismatch with German IBAN!
            RecipientCountry = "DE",
            Amount = 5000.00m,
            Currency = "EUR"
        };

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "RecipientBic" && e.ErrorMessage.Contains("country"));
    }

    #endregion

    #region Complex Validation Scenarios

    /// <summary>
    /// Represents a merchant onboarding request with multiple bank accounts.
    /// </summary>
    public class MerchantOnboardingRequest
    {
        public string MerchantName { get; set; } = string.Empty;
        public string PrimaryIban { get; set; } = string.Empty;
        public string PrimaryBic { get; set; } = string.Empty;
        public string? SecondaryIban { get; set; }
        public string? SecondaryBic { get; set; }
        public List<string> AcceptedCardBrands { get; set; } = new();
        public string Country { get; set; } = string.Empty;
    }

    public class MerchantOnboardingRequestValidator : AbstractValidator<MerchantOnboardingRequest>
    {
        public MerchantOnboardingRequestValidator()
        {
            RuleFor(x => x.MerchantName)
                .NotEmpty().WithMessage("Merchant name is required")
                .MaximumLength(200).WithMessage("Merchant name is too long");

            RuleFor(x => x.PrimaryIban)
                .NotEmpty().WithMessage("Primary IBAN is required")
                .MustBeValidIban().WithMessage("Primary IBAN is invalid");

            // Validate BIC format and that it matches the primary IBAN country
            RuleFor(x => x.PrimaryBic)
                .NotEmpty().WithMessage("Primary BIC is required")
                .MustBeValidBic().WithMessage("Primary BIC is invalid")
                .MustMatchIbanCountry(x => x.PrimaryIban).WithMessage("Primary BIC must match primary IBAN country");

            When(x => !string.IsNullOrEmpty(x.SecondaryIban), () =>
            {
                RuleFor(x => x.SecondaryIban)
                    .MustBeValidIban().WithMessage("Secondary IBAN is invalid");

                // Validate secondary BIC matches secondary IBAN country
                RuleFor(x => x.SecondaryBic)
                    .NotEmpty().WithMessage("Secondary BIC is required when secondary IBAN is provided")
                    .MustBeValidBic().WithMessage("Secondary BIC is invalid")
                    .MustMatchIbanCountry(x => x.SecondaryIban).WithMessage("Secondary BIC must match secondary IBAN country");
            });

            RuleFor(x => x.Country)
                .NotEmpty().WithMessage("Country is required")
                .Length(2).WithMessage("Country code must be 2 characters");

            RuleFor(x => x.AcceptedCardBrands)
                .NotEmpty().WithMessage("At least one card brand must be accepted");
        }
    }

    [Fact]
    public void MerchantOnboarding_WithSingleAccount_ShouldPassValidation()
    {
        // Arrange
        var validator = new MerchantOnboardingRequestValidator();
        var request = new MerchantOnboardingRequest
        {
            MerchantName = "Acme Corporation",
            PrimaryIban = "BE68539007547034",
            PrimaryBic = "KREDBEBB",
            Country = "BE",
            AcceptedCardBrands = new List<string> { "Visa", "Mastercard" }
        };

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void MerchantOnboarding_WithDualAccounts_ShouldPassValidation()
    {
        // Arrange
        var validator = new MerchantOnboardingRequestValidator();
        var request = new MerchantOnboardingRequest
        {
            MerchantName = "Acme Corporation",
            PrimaryIban = "BE68539007547034",
            PrimaryBic = "KREDBEBB",
            SecondaryIban = "BE71096123456769",
            SecondaryBic = "GKCCBEBB",
            Country = "BE",
            AcceptedCardBrands = new List<string> { "Visa", "Mastercard", "Amex" }
        };

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void MerchantOnboarding_WithSecondaryIbanButNoBic_ShouldFailValidation()
    {
        // Arrange
        var validator = new MerchantOnboardingRequestValidator();
        var request = new MerchantOnboardingRequest
        {
            MerchantName = "Acme Corporation",
            PrimaryIban = "BE68539007547034",
            PrimaryBic = "KREDBEBB",
            SecondaryIban = "BE71096123456769",
            SecondaryBic = "",                    // Missing BIC
            Country = "BE",
            AcceptedCardBrands = new List<string> { "Visa" }
        };

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "SecondaryBic");
    }

    [Fact]
    public void MerchantOnboarding_WithNoCardBrands_ShouldFailValidation()
    {
        // Arrange
        var validator = new MerchantOnboardingRequestValidator();
        var request = new MerchantOnboardingRequest
        {
            MerchantName = "Acme Corporation",
            PrimaryIban = "BE68539007547034",
            PrimaryBic = "KREDBEBB",
            Country = "BE",
            AcceptedCardBrands = new List<string>()  // Empty list
        };

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "AcceptedCardBrands");
    }

    #endregion

    #region Edge Cases and Error Aggregation

    [Fact]
    public void SepaPayment_WithMultipleErrors_ShouldReportAllErrors()
    {
        // Arrange
        var validator = new SepaPaymentRequestValidator();
        var request = new SepaPaymentRequest
        {
            DebtorIban = "",                       // Empty
            DebtorBic = "INVALID",                 // Invalid format
            CreditorIban = "XXXXXXXX",             // Invalid IBAN
            CreditorBic = "",                      // Empty
            Amount = -100m,                        // Negative
            Currency = "GBP"                       // Not EUR
        };

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCountGreaterThanOrEqualTo(6);
    }

    [Fact]
    public void CardPayment_WithAllFieldsEmpty_ShouldReportAllErrors()
    {
        // Arrange
        var validator = new CardPaymentRequestValidator();
        var request = new CardPaymentRequest
        {
            CardNumber = "",
            CardholderName = "",
            ExpiryMonth = "",
            ExpiryYear = "",
            Cvv = "",
            Amount = 0,
            Currency = ""
        };

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCountGreaterThanOrEqualTo(7);
    }

    [Theory]
    [InlineData("BE68539007547034", true)]   // Valid Belgian IBAN
    [InlineData("be68539007547034", true)]   // Lowercase - should still be valid
    [InlineData("BE68 5390 0754 7034", true)] // With spaces - should still be valid
    [InlineData("", false)]                   // Empty
    [InlineData("INVALID", false)]            // Not an IBAN
    [InlineData("BE99999999999999", false)]   // Invalid checksum
    public void IbanValidation_WithVariousFormats_ShouldValidateCorrectly(string iban, bool expectedValid)
    {
        // Arrange
        var validator = new InlineValidator<string>();
        validator.RuleFor(x => x).MustBeValidIban();

        // Act
        var result = validator.Validate(iban);

        // Assert
        result.IsValid.Should().Be(expectedValid);
    }

    [Theory]
    [InlineData("KREDBEBB", true)]     // Valid 8-char BIC
    [InlineData("KREDBEBB123", true)]  // Valid 11-char BIC
    [InlineData("kredbebb", true)]     // Lowercase - should still be valid
    [InlineData("", false)]             // Empty
    [InlineData("KREDB", false)]        // Too short
    [InlineData("KREDBEBB12", false)]   // 10 chars - invalid
    [InlineData("123DBEBB", false)]     // Numbers at start - invalid
    public void BicValidation_WithVariousFormats_ShouldValidateCorrectly(string bic, bool expectedValid)
    {
        // Arrange
        var validator = new InlineValidator<string>();
        validator.RuleFor(x => x).MustBeValidBic();

        // Act
        var result = validator.Validate(bic);

        // Assert
        result.IsValid.Should().Be(expectedValid);
    }

    [Theory]
    [InlineData("4532015112830366", true)]   // Valid Visa
    [InlineData("5425233430109903", true)]   // Valid Mastercard
    [InlineData("378282246310005", true)]    // Valid Amex
    [InlineData("4532 0151 1283 0366", true)] // With spaces
    [InlineData("4532-0151-1283-0366", true)] // With dashes
    [InlineData("", false)]                   // Empty
    [InlineData("1234567890123456", false)]   // Invalid Luhn
    [InlineData("123", false)]                // Too short
    public void CardValidation_WithVariousFormats_ShouldValidateCorrectly(string cardNumber, bool expectedValid)
    {
        // Arrange
        var validator = new InlineValidator<string>();
        validator.RuleFor(x => x).MustBeValidPaymentCard();

        // Act
        var result = validator.Validate(cardNumber);

        // Assert
        result.IsValid.Should().Be(expectedValid);
    }

    #endregion
}
