using Finova.Core.Common;

namespace Finova.Tests;

public static class ValidationResultExtensions
{
    public static ValidationErrorCode? ErrorCode(this ValidationResult result)
    {
        return result.Errors.FirstOrDefault()?.Code;
    }
}
