namespace GameStore.Common;

public static class Ensure
{
    public static Result<string> NotNullOrEmpty(string value, string fieldName)
    {
        return string.IsNullOrWhiteSpace(value)
            ? Result<string>.Failure($"{fieldName} cannot be null or empty")
            : Result<string>.Success(value);
    }

    public static Result<T> GreaterThan<T>(T value, T minimum, string fieldName) where T : IComparable<T>
    {
        return value.CompareTo(minimum) <= 0
            ? Result<T>.Failure($"{fieldName} must be greater than {minimum}")
            : Result<T>.Success(value);
    }

    public static Result<T> InRange<T>(T value, T minimum, T maximum, string fieldName) where T : IComparable<T>
    {
        if (value.CompareTo(minimum) < 0 || value.CompareTo(maximum) > 0)
            return Result<T>.Failure($"{fieldName} must be between {minimum} and {maximum}");

        return Result<T>.Success(value);
    }

    public static Result<IEnumerable<T>> NotEmpty<T>(IEnumerable<T>? collection, string fieldName)
    {
        if (collection is null || !collection.Any())
            return Result<IEnumerable<T>>.Failure($"{fieldName} cannot be null or empty");

        return Result<IEnumerable<T>>.Success(collection);
    }

    public static Result<string> Matches(string value, string pattern, string fieldName)
    {
        return System.Text.RegularExpressions.Regex.IsMatch(value, pattern)
            ? Result<string>.Success(value)
            : Result<string>.Failure($"{fieldName} format is invalid");
    }

    public static Result<T> Combine<T>(T value, params Result[] validations)
    {
        var failure = validations.FirstOrDefault(r => r.IsFailure);
        return failure is not null
            ? Result<T>.Failure(failure.Error)
            : Result<T>.Success(value);
    }
}
