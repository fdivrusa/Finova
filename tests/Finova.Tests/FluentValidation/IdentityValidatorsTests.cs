using Finova.Core.Enterprise;
using Finova.Extensions.FluentValidation;
using FluentValidation;
using FluentValidation.TestHelper;
using Xunit;

namespace Finova.Tests.FluentValidation;

public class IdentityValidatorsTests
{
    public class TestModel
    {
        public string Country { get; set; } = string.Empty;
        public string? NationalId { get; set; }
        public string? TaxId { get; set; }
        public string? EnterpriseNumber { get; set; }
    }

    public class NationalIdValidator : AbstractValidator<TestModel>
    {
        public NationalIdValidator()
        {
            RuleFor(x => x.NationalId)
                .MustBeValidNationalId(x => x.Country);
        }
    }

    public class StaticNationalIdValidator : AbstractValidator<TestModel>
    {
        public StaticNationalIdValidator(string countryCode)
        {
            RuleFor(x => x.NationalId)
                .MustBeValidNationalId(countryCode);
        }
    }

    public class NorthAmericaTaxIdValidator : AbstractValidator<TestModel>
    {
        public NorthAmericaTaxIdValidator()
        {
            RuleFor(x => x.TaxId)
                .MustBeValidNorthAmericaTaxId(x => x.Country);
        }
    }

    public class StaticNorthAmericaTaxIdValidator : AbstractValidator<TestModel>
    {
        public StaticNorthAmericaTaxIdValidator(string? countryCode = null)
        {
            RuleFor(x => x.TaxId)
                .MustBeValidNorthAmericaTaxId(countryCode);
        }
    }

    public class SouthAmericaTaxIdValidator : AbstractValidator<TestModel>
    {
        public SouthAmericaTaxIdValidator()
        {
            RuleFor(x => x.TaxId)
                .MustBeValidSouthAmericaTaxId(x => x.Country);
        }
    }

    public class AsiaTaxIdValidator : AbstractValidator<TestModel>
    {
        public AsiaTaxIdValidator()
        {
            RuleFor(x => x.TaxId)
                .MustBeValidAsiaTaxId(x => x.Country);
        }
    }

    public class OceaniaTaxIdValidator : AbstractValidator<TestModel>
    {
        public OceaniaTaxIdValidator()
        {
            RuleFor(x => x.TaxId)
                .MustBeValidOceaniaTaxId(x => x.Country);
        }
    }

    public class AfricaTaxIdValidator : AbstractValidator<TestModel>
    {
        public AfricaTaxIdValidator()
        {
            RuleFor(x => x.TaxId)
                .MustBeValidAfricaTaxId(x => x.Country);
        }
    }

    public class EuropeTaxIdValidator : AbstractValidator<TestModel>
    {
        public EuropeTaxIdValidator()
        {
            RuleFor(x => x.TaxId)
                .MustBeValidEuropeTaxId(x => x.Country);
        }
    }

    public class EnterpriseNumberValidator : AbstractValidator<TestModel>
    {
        public EnterpriseNumberValidator()
        {
            RuleFor(x => x.EnterpriseNumber)
                .MustBeValidEnterpriseNumber(x => x.Country);
        }
    }

    public class StaticEnterpriseNumberValidator : AbstractValidator<TestModel>
    {
        public StaticEnterpriseNumberValidator(string countryCode)
        {
            RuleFor(x => x.EnterpriseNumber)
                .MustBeValidEnterpriseNumber(countryCode);
        }
    }

    public class TypedEnterpriseNumberValidator : AbstractValidator<TestModel>
    {
        public TypedEnterpriseNumberValidator(EnterpriseNumberType type)
        {
            RuleFor(x => x.EnterpriseNumber)
                .MustBeValidEnterpriseNumber(type);
        }
    }

    [Theory]
    [InlineData("BE", "72020290081")] // Belgium
    [InlineData("FR", "1 80 01 45 000 000 69")] // France
    [InlineData("CN", "11010519491231002X")] // China
    [InlineData("BR", "111.444.777-35")] // Brazil
    public void MustBeValidNationalId_Valid_ShouldNotHaveError(string country, string id)
    {
        var model = new TestModel { Country = country, NationalId = id };
        var validator = new NationalIdValidator();
        var result = validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.NationalId);
    }

    [Fact]
    public void MustBeValidNationalId_Static_Valid_ShouldNotHaveError()
    {
        var model = new TestModel { NationalId = "72020290081" };
        var validator = new StaticNationalIdValidator("BE");
        var result = validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.NationalId);
    }

    [Theory]
    [InlineData("US", "12-3456789")] // EIN
    [InlineData("CA", "123456782RT0001")] // BN
    public void MustBeValidNorthAmericaTaxId_Valid_ShouldNotHaveError(string country, string id)
    {
        var model = new TestModel { Country = country, TaxId = id };
        var validator = new NorthAmericaTaxIdValidator();
        var result = validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.TaxId);
    }

    [Fact]
    public void MustBeValidNorthAmericaTaxId_Static_Valid_ShouldNotHaveError()
    {
        var model = new TestModel { TaxId = "12-3456789" };
        var validator = new StaticNorthAmericaTaxIdValidator("US");
        var result = validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.TaxId);
    }

    [Theory]
    [InlineData("BR", "11.222.333/0001-81")] // CNPJ
    [InlineData("MX", "XAXX010101000")] // RFC
    public void MustBeValidSouthAmericaTaxId_Valid_ShouldNotHaveError(string country, string id)
    {
        var model = new TestModel { Country = country, TaxId = id };
        var validator = new SouthAmericaTaxIdValidator();
        var result = validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.TaxId);
    }

    [Theory]
    [InlineData("CN", "911100001000000033")] // USCC
    [InlineData("IN", "ABCPE1234A")] // PAN
    public void MustBeValidAsiaTaxId_Valid_ShouldNotHaveError(string country, string id)
    {
        var model = new TestModel { Country = country, TaxId = id };
        var validator = new AsiaTaxIdValidator();
        var result = validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.TaxId);
    }

    [Theory]
    [InlineData("AU", "51 824 753 556")] // ABN
    public void MustBeValidOceaniaTaxId_Valid_ShouldNotHaveError(string country, string id)
    {
        var model = new TestModel { Country = country, TaxId = id };
        var validator = new OceaniaTaxIdValidator();
        var result = validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.TaxId);
    }

    [Theory]
    [InlineData("EG", "100-200-300")] // TRN
    public void MustBeValidAfricaTaxId_Valid_ShouldNotHaveError(string country, string id)
    {
        var model = new TestModel { Country = country, TaxId = id };
        var validator = new AfricaTaxIdValidator();
        var result = validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.TaxId);
    }

    [Theory]
    [InlineData("RU", "7707083893")] // INN
    public void MustBeValidEuropeTaxId_Valid_ShouldNotHaveError(string country, string id)
    {
        var model = new TestModel { Country = country, TaxId = id };
        var validator = new EuropeTaxIdValidator();
        var result = validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.TaxId);
    }

    [Theory]
    [InlineData("BE", "0123456749")]
    [InlineData("FR", "732829320")]
    public void MustBeValidEnterpriseNumber_Valid_ShouldNotHaveError(string country, string id)
    {
        var model = new TestModel { Country = country, EnterpriseNumber = id };
        var validator = new EnterpriseNumberValidator();
        var result = validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.EnterpriseNumber);
    }

    [Fact]
    public void MustBeValidEnterpriseNumber_Static_Valid_ShouldNotHaveError()
    {
        var model = new TestModel { EnterpriseNumber = "0123456749" };
        var validator = new StaticEnterpriseNumberValidator("BE");
        var result = validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.EnterpriseNumber);
    }

    [Fact]
    public void MustBeValidEnterpriseNumber_Typed_Valid_ShouldNotHaveError()
    {
        var model = new TestModel { EnterpriseNumber = "732829320" };
        var validator = new TypedEnterpriseNumberValidator(EnterpriseNumberType.FranceSiren);
        var result = validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.EnterpriseNumber);
    }

    [Fact]
    public void MustBeValidNationalId_Invalid_ShouldHaveError()
    {
        var model = new TestModel { Country = "BE", NationalId = "INVALID" };
        var validator = new NationalIdValidator();
        var result = validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.NationalId);
    }

    [Fact]
    public void MustBeValidNationalId_UnsupportedCountry_ShouldHaveError()
    {
        var model = new TestModel { Country = "XX", NationalId = "123" };
        var validator = new NationalIdValidator();
        var result = validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.NationalId);
    }
}
