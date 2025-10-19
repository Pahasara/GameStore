namespace GameStore.Common;

public class Result<T>
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    
    public T? Value { get; }
    public string Error { get; }
    public ErrorType? ErrorType { get; }

    private Result(bool isSuccess, T? value, string error, ErrorType? errorType)
    {
        switch (isSuccess)
        {
            case true when !string.IsNullOrWhiteSpace(error):
                throw new ArgumentException("Success result cannot have an error", nameof(error));
            case false when string.IsNullOrWhiteSpace(error):
                throw new ArgumentException("Failure result must have an error", nameof(error));
        }

        IsSuccess = isSuccess;
        Value = value;
        Error = error;
        ErrorType = errorType;
    }

    public static Result<T> Success(T value) =>
        new(true, value, string.Empty, null);
    public static Result<T> Failure(string error, ErrorType errorType = Common.ErrorType.BadRequest) =>
        new(false, default, error, errorType);

    // Allows: Result<Game> result = game; instead of Result<Game>.Success(game);
    public static implicit operator Result<T>(T value) => Success(value);

    public static Result<T> SuccessIf(bool condition, T value, string error) =>
       condition ? Success(value) : Failure(error);
    public static Result<T> FailureIf(bool condition, T value, string error) =>
        condition ? Failure(error) : Success(value);
}

public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public string Error { get; }
    public ErrorType? ErrorType { get; }

    private Result(bool isSuccess, string error, ErrorType? errorType)
    {
        if (!isSuccess && string.IsNullOrWhiteSpace(error))
            throw new ArgumentException("Failure result must have an error", nameof(error));

        IsSuccess = isSuccess;
        Error = error;
        ErrorType = errorType;
    }

    public static Result Success() => 
        new(true, string.Empty, null);
    public static Result Failure(string error, ErrorType errorType = Common.ErrorType.BadRequest) => 
        new(false, error, errorType);

    public static Result SuccessIf(bool condition, string error, ErrorType errorType) =>
        condition ? Success() : Failure(error, errorType);
    public static Result FailureIf(bool condition, string error, ErrorType errorType) =>
        condition ? Failure(error, errorType) : Success();
}
