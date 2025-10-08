using GameStore.Common;

namespace GameStore.Extensions;

public static class ResultExtensions
{
    public static async Task<Result<TOut>> OnSuccess<TIn, TOut>(
        this Result<TIn> result,
        Func<TIn, Task<Result<TOut>>> onSuccess)
    {
        return result.IsFailure
            ? Result<TOut>.Failure(result.Error)
            : await onSuccess(result.Value!);
    }

    public static Result<T> OnSuccess<T>(this Result<T> result, Action<T> onSuccess)
    {
        if (result.IsSuccess)
            onSuccess(result.Value!);

        return result;
    }

    public static Result<T> OnFailure<T>(this Result<T> result, Action<string> onFailure)
    {
        if (result.IsFailure)
            onFailure(result.Error);

        return result;
    }

    public static Result<TOut> Map<TIn, TOut>(
        this Result<TIn> result,
        Func<TIn, TOut> mapper)
    {
        return result.IsFailure
            ? Result<TOut>.Failure(result.Error)
            : Result<TOut>.Success(mapper(result.Value!));
    }
}
