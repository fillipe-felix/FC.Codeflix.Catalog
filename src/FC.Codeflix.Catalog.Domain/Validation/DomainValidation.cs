using FC.Codeflix.Catalog.Domain.Exceptions;

namespace FC.Codeflix.Catalog.Domain.Validation;

public static class DomainValidation
{

    public static void NotNull(object? target, string fieldName)
    {
        if (target is null)
        {
            throw new EntityValidationException($"{fieldName} should not be null.");
        }
    }

    public static void NotNullOrEmpty(string? target, string fieldName)
    {
        if (String.IsNullOrWhiteSpace(target))
        {
            throw new EntityValidationException($"{fieldName} should not be null or empty.");
        }
    }

    public static void MinLength(string target, int minLength, string fieldname)
    {
        if (target.Length < minLength)
        {
            throw new EntityValidationException($"{fieldname} should be at leats {minLength} characters long.");
        }
    }

    public static void MaxLength(string target, int maxLength, string fieldname)
    {
        if (target.Length > maxLength)
        {
            throw new EntityValidationException($"{fieldname} should be less or equal {maxLength} characters long.");
        }
    }
}
