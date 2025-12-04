using ResultPattern.Core;

namespace ResultPattern.Core.Extension;

public static class ResultExtension
{
    public static Result<TResult> Map<T, TResult>(this Result<T> result, Func<T, TResult> mapper)
    {
        if (result == null) 
            throw new ArgumentNullException(nameof(result));
        try
        {
            return result.IsSuccess ? Result<TResult>.Success(mapper(result.Value)) : Result<TResult>.Failure(result.Error);
        }
        catch (Exception ex)
        {
            return Result<TResult>.Failure(ex.Message);
        }
    }

    public static Result<T> Match<T>(this Result<T> result, Func<T, T> onSuccess, Func<string, T> onFailure)
    {
        if (result == null) throw new ArgumentNullException(nameof(result));
        try
        {
            return result.IsSuccess
                ? Result<T>.Success(onSuccess(result.Value))
                : Result<T>.Success(onFailure(result.Error));
        }
        catch (Exception ex)
        {
            return Result<T>.Failure(ex.Message);
        }
        
    }
}