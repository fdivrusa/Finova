namespace Finova.Core.Common;

/// <summary>
/// Represents the result of a validation operation.
/// </summary>
public class ValidationResult
{
    public bool IsValid => Errors.Count == 0;
    public List<ValidationError> Errors { get; } = [];

    private ValidationResult() { }

    public static ValidationResult Success()
    {
        return new ValidationResult();
    }

    public static ValidationResult Failure(ValidationErrorCode code, string message)
    {
        var result = new ValidationResult();
        result.Errors.Add(new ValidationError(code, message));
        return result;
    }

    public static ValidationResult Failure(List<ValidationError> errors)
    {
        var result = new ValidationResult();
        result.Errors.AddRange(errors);
        return result;
    }

    public void AddError(ValidationErrorCode code, string message)
    {
        Errors.Add(new ValidationError(code, message));
    }
}
