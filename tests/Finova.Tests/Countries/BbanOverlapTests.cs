using Finova.Countries.Europe.Azerbaijan.Validators;
using Finova.Countries.Europe.Belarus.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries;

public class BbanOverlapTests
{
    [Fact]
    public void Verify_Belarus_BBAN_Validates_In_Azerbaijan()
    {
        // Arrange
        var belarusBban = "NBRB3600900000002Z00AB00";

        // Act & Assert
        // 1. Verify it is valid for Belarus
        var byResult = BelarusBbanValidator.Validate(belarusBban);
        byResult.IsValid.Should().BeTrue("should be valid for Belarus");

        // 2. Verify it is valid for Azerbaijan (reproducing the user's observation)
        var azResult = AzerbaijanBbanValidator.Validate(belarusBban);
        azResult.IsValid.Should().BeTrue("currently valid for Azerbaijan due to structural overlap");
    }

    [Fact]
    public void Verify_Belarus_Stricter_Rules_Differentiate_From_Azerbaijan()
    {
        // Arrange
        // "NBRB" (Bank) + "ABCD" (Balance - Invalid for BY, Valid for AZ) + "9000000002Z00AB00"
        var original = "NBRB3600900000002Z00AB00";
        var tail = original.Substring(8);
        var ambiguousBban = "NBRBABCD" + tail;

        // Act
        var byResult = BelarusBbanValidator.Validate(ambiguousBban);
        var azResult = AzerbaijanBbanValidator.Validate(ambiguousBban);

        // Assert
        byResult.IsValid.Should().BeFalse("Belarus now requires digits at pos 4-8 (Balance Account)");
        azResult.IsValid.Should().BeTrue("Azerbaijan accepts alphanumeric at pos 4-8");
    }
}
