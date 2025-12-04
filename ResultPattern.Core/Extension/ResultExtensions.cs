namespace ResultPattern.Core.Extension;

public static class ResultExtensions
{
    /// <summary>
    /// Transforms a <see cref="Result{T}"/> to a <see cref="Result{TResult}"/> by applying the given mapping function if the result is successful.
    /// If the result is a failure, propagates the error.
    /// </summary>
    /// <typeparam name="T">The type of the value in the original result.</typeparam>
    /// <typeparam name="TResult">The type of the value in the resulting result.</typeparam>
    /// <param name="result">The original result to be transformed.</param>
    /// <param name="mapper">The function to transform the value of the original result.</param>
    /// <returns>
    /// A new <see cref="Result{TResult}"/> that contains the transformed value if the original result is successful, or the propagated error if the original result is a failure.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown if the <paramref name="result"/> argument is null.</exception>
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

    /// <summary>
    /// Matches a <see cref="Result{T}"/> and processes it using the provided functions based on its state.
    /// </summary>
    /// <typeparam name="T">The type of the value contained in the result.</typeparam>
    /// <param name="result">The result to be matched and processed.</param>
    /// <param name="onSuccess">The function to process the successful result value.</param>
    /// <param name="onFailure">The function to process the failure result error message.</param>
    /// <returns>
    /// A new <see cref="Result{T}"/> representing either the processed success value or a new result indicating failure if an exception occurs.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown if the <paramref name="result"/> is null.</exception>
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