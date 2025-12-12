using Finova.Extensions.FluentValidation;
using FluentAssertions;
using FluentValidation;
using Xunit;

namespace Finova.Tests.FluentValidation;

public class FinovaValidatorsTests
{
    #region Test Models

    private class PaymentModel
    {
        public string? Iban { get; set; }
        public string? Bic { get; set; }
        public string? CardNumber { get; set; }
    }

    private class BankTransferModel
    {
        public string? SourceIban { get; set; }
        public string? SourceBic { get; set; }
        public string? DestinationIban { get; set; }
        public string? DestinationBic { get; set; }
    }

    private class VatModel
    {
        public string? VatNumber { get; set; }
    }

    private class PaymentReferenceModel
    {
        public string? Reference { get; set; }
    }



    #endregion

    #region Test Validators

    private class PaymentModelValidator : AbstractValidator<PaymentModel>
    {
        public PaymentModelValidator()
        {
            RuleFor(x => x.Iban).MustBeValidIban();
            RuleFor(x => x.Bic).MustBeValidBic();
            RuleFor(x => x.CardNumber).MustBeValidPaymentCard();
        }
    }

    private class BankTransferValidator : AbstractValidator<BankTransferModel>
    {
        public BankTransferValidator()
        {
            RuleFor(x => x.SourceIban).MustBeValidIban();
            RuleFor(x => x.SourceBic)
                .MustBeValidBic()
                .MustMatchIbanCountry(x => x.SourceIban);

            RuleFor(x => x.DestinationIban).MustBeValidIban();
            RuleFor(x => x.DestinationBic)
                .MustBeValidBic()
                .MustMatchIbanCountry(x => x.DestinationIban);
        }
    }

    private class VatModelValidator : AbstractValidator<VatModel>
    {
        public VatModelValidator()
        {
            RuleFor(x => x.VatNumber).MustBeValidVat();
        }
    }

    private class PaymentReferenceModelValidator : AbstractValidator<PaymentReferenceModel>
    {
        public PaymentReferenceModelValidator(Finova.Core.PaymentReference.PaymentReferenceFormat format = Finova.Core.PaymentReference.PaymentReferenceFormat.IsoRf)
        {
            RuleFor(x => x.Reference).MustBeValidPaymentReference(format);
        }
    }

    #endregion

    #region MustBeValidIban Tests

    [Theory]
    // Belgium
    [InlineData("BE68539007547034")]
    [InlineData("BE71096123456769")]
    // France
    [InlineData("FR1420041010050500013M02606")]
    [InlineData("FR7630006000011234567890189")]
    // Germany
    [InlineData("DE89370400440532013000")]
    [InlineData("DE75512108001245126199")]
    // Italy
    [InlineData("IT60X0542811101000000123456")]
    // Spain
    [InlineData("ES9121000418450200051332")]
    [InlineData("ES7921000813610123456789")]
    // Netherlands
    [InlineData("NL91ABNA0417164300")]
    // Luxembourg
    [InlineData("LU280019400644750000")]
    [InlineData("LU120010001234567891")]
    // United Kingdom
    [InlineData("GB29NWBK60161331926819")]
    [InlineData("GB82WEST12345698765432")]
    public void MustBeValidIban_WithValidIbans_PassesValidation(string iban)
    {
        // Arrange
        var validator = new PaymentModelValidator();
        var model = new PaymentModel { Iban = iban, Bic = "DEUTDEFF", CardNumber = "4111111111111111" };

        // Act
        var result = validator.Validate(model);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().NotContain(e => e.PropertyName == "Iban");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("INVALID")]
    [InlineData("DE89370400440532013001")] // Invalid checksum
    [InlineData("XX89370400440532013000")] // Invalid country code
    [InlineData("BE68539007547035")] // Invalid Belgian checksum
    [InlineData("IT60X0542811101000000123457")] // Invalid Italian checksum
    [InlineData("FR14")] // Too short
    [InlineData("12345678901234567890")] // No country code
    public void MustBeValidIban_WithInvalidIbans_FailsValidation(string? iban)
    {
        // Arrange
        var validator = new PaymentModelValidator();
        var model = new PaymentModel { Iban = iban, Bic = "DEUTDEFF", CardNumber = "4111111111111111" };

        // Act
        var result = validator.Validate(model);

        // Assert
        result.Errors.Should().Contain(e => e.PropertyName == "Iban");
    }

    [Fact]
    public void MustBeValidIban_WithInvalidIban_HasCorrectErrorMessage()
    {
        // Arrange
        var validator = new PaymentModelValidator();
        var model = new PaymentModel { Iban = "INVALID", Bic = "DEUTDEFF", CardNumber = "4111111111111111" };

        // Act
        var result = validator.Validate(model);

        // Assert
        var ibanError = result.Errors.First(e => e.PropertyName == "Iban");
        ibanError.ErrorMessage.Should().Be("'Iban' is not a valid IBAN.");
    }

    #endregion

    #region MustBeValidBic Tests

    [Theory]
    // German banks
    [InlineData("DEUTDEFF")] // Deutsche Bank, Frankfurt (8 chars)
    [InlineData("DEUTDEFFXXX")] // Deutsche Bank with branch (11 chars)
    [InlineData("COBADEFFXXX")] // Commerzbank
    // French banks
    [InlineData("BNPAFRPP")] // BNP Paribas, Paris
    [InlineData("BNPAFRPPXXX")] // BNP Paribas with branch
    [InlineData("SOGEFRPP")] // Société Générale
    // Belgian banks
    [InlineData("GEBABEBB")] // BNP Paribas Fortis
    [InlineData("KREDBEBB")] // KBC Bank
    [InlineData("ARSPBE22")] // Argenta (8 chars)
    // Dutch banks
    [InlineData("ABNANL2A")] // ABN AMRO
    [InlineData("INGBNL2A")] // ING Bank
    [InlineData("RABONL2U")] // Rabobank
    // UK banks
    [InlineData("HSBCGB2L")] // HSBC, London
    [InlineData("NWBKGB2L")] // NatWest
    // US banks
    [InlineData("CHASUS33")] // JP Morgan Chase, USA
    [InlineData("BOFAUS3N")] // Bank of America
    public void MustBeValidBic_WithValidBics_PassesValidation(string bic)
    {
        // Arrange
        var validator = new PaymentModelValidator();
        var model = new PaymentModel { Iban = "DE89370400440532013000", Bic = bic, CardNumber = "4111111111111111" };

        // Act
        var result = validator.Validate(model);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().NotContain(e => e.PropertyName == "Bic");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("DEUT")] // Too short (4 chars)
    [InlineData("DEUTDE")] // Too short (6 chars)
    [InlineData("DEUTDEF")] // Too short (7 chars)
    [InlineData("DEUTDEFF1")] // Invalid length (9 chars)
    [InlineData("DEUTDEFF12")] // Invalid length (10 chars)
    [InlineData("DEUTDEFF1234")] // Too long (12 chars)
    [InlineData("1EUTDEFF")] // Bank code starts with digit
    [InlineData("DEUT1EFF")] // Country code contains digit
    [InlineData("DEUTDE@F")] // Location code contains special char
    public void MustBeValidBic_WithInvalidBics_FailsValidation(string? bic)
    {
        // Arrange
        var validator = new PaymentModelValidator();
        var model = new PaymentModel { Iban = "DE89370400440532013000", Bic = bic, CardNumber = "4111111111111111" };

        // Act
        var result = validator.Validate(model);

        // Assert
        result.Errors.Should().Contain(e => e.PropertyName == "Bic");
    }

    [Fact]
    public void MustBeValidBic_WithInvalidBic_HasCorrectErrorMessage()
    {
        // Arrange
        var validator = new PaymentModelValidator();
        var model = new PaymentModel { Iban = "DE89370400440532013000", Bic = "INVALID", CardNumber = "4111111111111111" };

        // Act
        var result = validator.Validate(model);

        // Assert
        var bicError = result.Errors.First(e => e.PropertyName == "Bic");
        bicError.ErrorMessage.Should().Be("'Bic' is not a valid BIC/SWIFT code.");
    }

    #endregion

    #region MustBeValidPaymentCard Tests

    [Theory]
    // Visa
    [InlineData("4532015112830366")]
    [InlineData("4111111111111111")]
    [InlineData("4000056655665556")]
    // Mastercard
    [InlineData("5425233430109903")]
    [InlineData("5555555555554444")]
    [InlineData("5105105105105100")]
    // American Express
    [InlineData("374245455400126")]
    [InlineData("378282246310005")]
    [InlineData("371449635398431")]
    // Discover
    [InlineData("6011111111111117")]
    [InlineData("6011000990139424")]
    // JCB
    [InlineData("3530111333300000")]
    [InlineData("3566002020360505")]
    // Maestro
    [InlineData("6304000000000000")]
    public void MustBeValidPaymentCard_WithValidCards_PassesValidation(string cardNumber)
    {
        // Arrange
        var validator = new PaymentModelValidator();
        var model = new PaymentModel { Iban = "DE89370400440532013000", Bic = "DEUTDEFF", CardNumber = cardNumber };

        // Act
        var result = validator.Validate(model);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().NotContain(e => e.PropertyName == "CardNumber");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("4532015112830367")] // Invalid check digit
    [InlineData("5425233430109904")] // Invalid check digit
    [InlineData("1234567890123456")] // Random invalid number
    [InlineData("abcd1234567890")] // Contains letters
    [InlineData("4532@151#128$036")] // Contains special chars
    public void MustBeValidPaymentCard_WithInvalidCards_FailsValidation(string? cardNumber)
    {
        // Arrange
        var validator = new PaymentModelValidator();
        var model = new PaymentModel { Iban = "DE89370400440532013000", Bic = "DEUTDEFF", CardNumber = cardNumber };

        // Act
        var result = validator.Validate(model);

        // Assert
        result.Errors.Should().Contain(e => e.PropertyName == "CardNumber");
    }

    [Fact]
    public void MustBeValidPaymentCard_WithInvalidCard_HasCorrectErrorMessage()
    {
        // Arrange
        var validator = new PaymentModelValidator();
        var model = new PaymentModel { Iban = "DE89370400440532013000", Bic = "DEUTDEFF", CardNumber = "INVALID" };

        // Act
        var result = validator.Validate(model);

        // Assert
        var cardError = result.Errors.First(e => e.PropertyName == "CardNumber");
        cardError.ErrorMessage.Should().Be("'Card Number' is not a valid card number.");
    }

    [Theory]
    [InlineData("4532 0151 1283 0366")] // Visa with spaces
    [InlineData("5425-2334-3010-9903")] // Mastercard with dashes
    [InlineData("3742 4545 5400 126")] // Amex with spaces
    public void MustBeValidPaymentCard_WithFormattedCards_PassesValidation(string cardNumber)
    {
        // Arrange
        var validator = new PaymentModelValidator();
        var model = new PaymentModel { Iban = "DE89370400440532013000", Bic = "DEUTDEFF", CardNumber = cardNumber };

        // Act
        var result = validator.Validate(model);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    #endregion

    #region MustMatchIbanCountry Tests

    [Theory]
    // German IBAN with German BIC
    [InlineData("DE89370400440532013000", "DEUTDEFF", true)]
    [InlineData("DE89370400440532013000", "COBADEFFXXX", true)]
    // French IBAN with French BIC
    [InlineData("FR7630006000011234567890189", "BNPAFRPP", true)]
    [InlineData("FR7630006000011234567890189", "SOGEFRPPXXX", true)]
    // Belgian IBAN with Belgian BIC
    [InlineData("BE68539007547034", "GEBABEBB", true)]
    [InlineData("BE68539007547034", "KREDBEBB036", true)]
    // Dutch IBAN with Dutch BIC
    [InlineData("NL91ABNA0417164300", "ABNANL2A", true)]
    [InlineData("NL91ABNA0417164300", "INGBNL2AXXX", true)]
    // UK IBAN with UK BIC
    [InlineData("GB29NWBK60161331926819", "HSBCGB2L", true)]
    [InlineData("GB29NWBK60161331926819", "NWBKGB2LXXX", true)]
    // Mismatched countries
    [InlineData("DE89370400440532013000", "BNPAFRPP", false)] // German IBAN, French BIC
    [InlineData("FR7630006000011234567890189", "DEUTDEFF", false)] // French IBAN, German BIC
    [InlineData("BE68539007547034", "HSBCGB2L", false)] // Belgian IBAN, UK BIC
    public void MustMatchIbanCountry_WithVariousCombinations_ValidatesCorrectly(string iban, string bic, bool shouldPass)
    {
        // Arrange
        var validator = new BankTransferValidator();
        var model = new BankTransferModel
        {
            SourceIban = iban,
            SourceBic = bic,
            DestinationIban = "DE89370400440532013000",
            DestinationBic = "DEUTDEFF"
        };

        // Act
        var result = validator.Validate(model);

        // Assert
        if (shouldPass)
        {
            result.Errors.Should().NotContain(e =>
                e.PropertyName == "SourceBic" &&
                e.ErrorMessage.Contains("country"));
        }
        else
        {
            result.Errors.Should().Contain(e =>
                e.PropertyName == "SourceBic" &&
                e.ErrorMessage.Contains("country"));
        }
    }

    [Fact]
    public void MustMatchIbanCountry_WithNullIban_PassesValidation()
    {
        // Arrange - When IBAN is null, the BIC country check should pass (handled by MustBeValidIban rule)
        var validator = new BankTransferValidator();
        var model = new BankTransferModel
        {
            SourceIban = null,
            SourceBic = "DEUTDEFF",
            DestinationIban = "DE89370400440532013000",
            DestinationBic = "DEUTDEFF"
        };

        // Act
        var result = validator.Validate(model);

        // Assert - Should not have country mismatch error (IBAN validation will fail separately)
        result.Errors.Should().NotContain(e =>
            e.PropertyName == "SourceBic" &&
            e.ErrorMessage.Contains("country"));
    }

    [Fact]
    public void MustMatchIbanCountry_WithEmptyIban_PassesValidation()
    {
        // Arrange
        var validator = new BankTransferValidator();
        var model = new BankTransferModel
        {
            SourceIban = "",
            SourceBic = "DEUTDEFF",
            DestinationIban = "DE89370400440532013000",
            DestinationBic = "DEUTDEFF"
        };

        // Act
        var result = validator.Validate(model);

        // Assert
        result.Errors.Should().NotContain(e =>
            e.PropertyName == "SourceBic" &&
            e.ErrorMessage.Contains("country"));
    }

    [Fact]
    public void MustMatchIbanCountry_WithMismatch_HasCorrectErrorMessage()
    {
        // Arrange
        var validator = new BankTransferValidator();
        var model = new BankTransferModel
        {
            SourceIban = "DE89370400440532013000",
            SourceBic = "BNPAFRPP", // French BIC with German IBAN
            DestinationIban = "DE89370400440532013000",
            DestinationBic = "DEUTDEFF"
        };

        // Act
        var result = validator.Validate(model);

        // Assert
        var countryError = result.Errors.First(e =>
            e.PropertyName == "SourceBic" &&
            e.ErrorMessage.Contains("country"));
        countryError.ErrorMessage.Should().Be("The BIC country does not match the IBAN country.");
    }

    #endregion

    #region Combined Validation Tests

    [Fact]
    public void Validator_WithAllValidFields_PassesValidation()
    {
        // Arrange
        var validator = new PaymentModelValidator();
        var model = new PaymentModel
        {
            Iban = "DE89370400440532013000",
            Bic = "DEUTDEFF",
            CardNumber = "4111111111111111"
        };

        // Act
        var result = validator.Validate(model);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public void Validator_WithAllInvalidFields_ReturnsMultipleErrors()
    {
        // Arrange
        var validator = new PaymentModelValidator();
        var model = new PaymentModel
        {
            Iban = "INVALID_IBAN",
            Bic = "BAD",
            CardNumber = "1234"
        };

        // Act
        var result = validator.Validate(model);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(3);
        result.Errors.Should().Contain(e => e.PropertyName == "Iban");
        result.Errors.Should().Contain(e => e.PropertyName == "Bic");
        result.Errors.Should().Contain(e => e.PropertyName == "CardNumber");
    }

    [Fact]
    public void BankTransferValidator_WithValidTransfer_PassesValidation()
    {
        // Arrange
        var validator = new BankTransferValidator();
        var model = new BankTransferModel
        {
            SourceIban = "DE89370400440532013000",
            SourceBic = "DEUTDEFF",
            DestinationIban = "FR7630006000011234567890189",
            DestinationBic = "BNPAFRPP"
        };

        // Act
        var result = validator.Validate(model);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void BankTransferValidator_WithMismatchedCountries_FailsValidation()
    {
        // Arrange
        var validator = new BankTransferValidator();
        var model = new BankTransferModel
        {
            SourceIban = "DE89370400440532013000",
            SourceBic = "BNPAFRPP", // Wrong country - FR instead of DE
            DestinationIban = "FR7630006000011234567890189",
            DestinationBic = "DEUTDEFF" // Wrong country - DE instead of FR
        };

        // Act
        var result = validator.Validate(model);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "SourceBic");
        result.Errors.Should().Contain(e => e.PropertyName == "DestinationBic");
    }

    #endregion

    #region MustBeValidVat Tests

    [Theory]
    [InlineData("FR44732829320")] // Valid French VAT
    [InlineData("MC44732829320")] // Valid Monaco VAT (mapped to FR)
    [InlineData("BE0123456749")] // Valid Belgian VAT (using valid format)
    public void MustBeValidVat_WithValidVat_PassesValidation(string vat)
    {
        // Arrange
        var validator = new VatModelValidator();
        var model = new VatModel { VatNumber = vat };

        // Act
        var result = validator.Validate(model);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("INVALID")]
    [InlineData("FR14")] // Too short
    public void MustBeValidVat_WithInvalidVat_FailsValidation(string? vat)
    {
        // Arrange
        var validator = new VatModelValidator();
        var model = new VatModel { VatNumber = vat };

        // Act
        var result = validator.Validate(model);

        // Assert
        result.Errors.Should().Contain(e => e.PropertyName == "VatNumber");
        result.Errors.First(e => e.PropertyName == "VatNumber").ErrorMessage.Should().Be("'Vat Number' is not a valid VAT number.");
    }

    #endregion

    #region MustBeValidPaymentReference Tests

    [Theory]
    [InlineData("RF18539007547034")] // Valid ISO RF
    public void MustBeValidPaymentReference_WithValidIsoRf_PassesValidation(string reference)
    {
        // Arrange
        var validator = new PaymentReferenceModelValidator();
        var model = new PaymentReferenceModel { Reference = reference };

        // Act
        var result = validator.Validate(model);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("INVALID")]
    [InlineData("RF00")] // Invalid RF check digits
    public void MustBeValidPaymentReference_WithInvalidIsoRf_FailsValidation(string reference)
    {
        // Arrange
        var validator = new PaymentReferenceModelValidator();
        var model = new PaymentReferenceModel { Reference = reference };

        // Act
        var result = validator.Validate(model);

        // Assert
        result.Errors.Should().Contain(e => e.PropertyName == "Reference");
        result.Errors.First(e => e.PropertyName == "Reference").ErrorMessage.Should().Be("'Reference' is not a valid payment reference.");
    }

    [Fact]
    public void MustBeValidPaymentReference_WithLocalFormat_PassesValidation()
    {
        // Arrange - Belgian format (+++123/4567/89012+++)
        var validator = new PaymentReferenceModelValidator(Finova.Core.PaymentReference.PaymentReferenceFormat.LocalBelgian);
        var model = new PaymentReferenceModel { Reference = "+++090/9337/55493+++" };

        // Act
        var result = validator.Validate(model);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    #endregion
}
