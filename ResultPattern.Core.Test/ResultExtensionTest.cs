using ResultPattern.Core;
namespace ResultPattern.Core.Test;

public class ResultExtensionTest
{
    [Fact]
    public void Success_ShouldCreateSuccessfulResult_WithValueAndNoError()
    {
        // Arrange
        int expectedValue = 42;

        // Act
        var result = Result<int>.Success(expectedValue);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(expectedValue, result.Value);
        Assert.Null(result.Error);
    }

    [Fact]
    public void Failure_ShouldCreateFailedResult_WithErrorAndDefaultValue()
    {
        // Arrange
        string expectedError = "Something went wrong";

        // Act
        var result = Result<int>.Failure(expectedError);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(default(int), result.Value); // Default for int is 0
        Assert.Equal(expectedError, result.Error);
    }

    [Fact]
    public void Success_WithReferenceType_ShouldHandleNullValueCorrectly()
    {
        // Arrange
        string expectedValue = "Success!";

        // Act
        var result = Result<string>.Success(expectedValue);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(expectedValue, result.Value);
        Assert.Null(result.Error);
    }

    [Fact]
    public void Failure_WithReferenceType_ShouldHaveNullValue()
    {
        // Arrange
        string expectedError = "Failure message";

        // Act
        var result = Result<string>.Failure(expectedError);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Null(result.Value); // Default for string is null
        Assert.Equal(expectedError, result.Error);
    }
}