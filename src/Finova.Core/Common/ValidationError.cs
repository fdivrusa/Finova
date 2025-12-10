namespace Finova.Core.Common;

/// <summary>
/// Represents a validation error.
/// </summary>
public record ValidationError(ValidationErrorCode Code, string Message);
