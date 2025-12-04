namespace ResultPattern.Core;

public class Result<T>
{
    /// <summary>
    /// Indicates whether the operation represented by the result was successful.
    /// </summary>
    /// <remarks>
    /// If <c>true</c>, it signifies that the result contains a valid value and no errors occurred.
    /// If <c>false</c>, it signifies that the operation failed and there is an associated error message.
    /// </remarks>
    public bool IsSuccess { get; }

    /// <summary>
    /// Represents the value encapsulated within the result.
    /// </summary>
    /// <remarks>
    /// This property holds the value produced by a successful operation.
    /// If the result indicates a failure, this property will contain the default value for the type <c>T</c>.
    /// It is recommended to check the <c>IsSuccess</c> property before accessing this value to ensure the operation was successful.
    /// </remarks>
    public T Value { get; }

    /// <summary>
    /// Provides the error message that describes the failure of the operation.
    /// </summary>
    /// <remarks>
    /// This property is populated when the operation represented by the result fails.
    /// If the operation is successful, this property will be <c>null</c>.
    /// </remarks>
    public string Error { get; }

    /// <summary>
    /// Represents the outcome of an operation, encapsulating a success state,
    /// a value if the operation was successful, and an error message if it failed.
    /// </summary>
    /// <typeparam name="T">The type of the result value in case of a successful operation.</typeparam>
    private Result(bool isSuccess, T value, string error)
    {
        IsSuccess = isSuccess;
        Value = value;
        Error = error;
    }

    /// <summary>
    /// Creates a successful result containing the provided value.
    /// </summary>
    /// <param name="value">The value to associate with the successful result.</param>
    /// <returns>A successful result containing the provided value.</returns>
    public static Result<T> Success(T value) => new(true, value, null);

    /// <summary>
    /// Creates a failed result with the provided error message and a default value for the result type.
    /// </summary>
    /// <param name="error">The error message describing the reason for failure.</param>
    /// <returns>A failed result containing the provided error message.</returns>
    public static Result<T> Failure(string error) => new(false, default, error);
}