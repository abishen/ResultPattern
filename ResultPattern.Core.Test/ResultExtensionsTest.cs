using Xunit;
using ResultPattern.Core;
using ResultPattern.Core.Extension;

namespace ResultPattern.Core.Test;

public class ResultExtensionsTest
{
    [Fact]
    public void Map_Success_ShouldApplyMapperAndReturnSuccess()
    {
        // Arrange
        var originalResult = Result<int>.Success(5);
        Func<int, int> mapper = x => (x * 2);

        // Act
        var mappedResult = originalResult.Map(mapper);

        // Assert
        Assert.True(mappedResult.IsSuccess);
        Assert.Equal(10, mappedResult.Value);
        Assert.Null(mappedResult.Error);
    }

    [Fact]
    public void Map_Failure_ShouldPropagateErrorAndReturnFailure()
    {
        // Arrange
        var originalResult = Result<int>.Failure("Original error");
        Func<int, string> mapper = x => (x * 2).ToString();

        // Act
        var mappedResult = originalResult.Map(mapper);

        // Assert
        Assert.False(mappedResult.IsSuccess);
        Assert.Null(mappedResult.Value); // Default for string
        Assert.Equal("Original error", mappedResult.Error);
    }

    [Fact]
    public void Map_ExceptionInMapper_ShouldReturnFailureWithExceptionMessage()
    {
        // Arrange
        var originalResult = Result<int>.Success(5);
        Func<int, string> mapper = x => throw new InvalidOperationException("Mapper failed");

        // Act
        var mappedResult = originalResult.Map(mapper);

        // Assert
        Assert.False(mappedResult.IsSuccess);
        Assert.Null(mappedResult.Value);
        Assert.Equal("Mapper failed", mappedResult.Error);
    }

    [Fact]
    public void Map_NullResult_ShouldThrowArgumentNullException()
    {
        // Arrange
        Result<int> originalResult = null;
        Func<int, string> mapper = x => x.ToString();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => originalResult.Map(mapper));
    }

    [Fact]
    public void Match_Success_ShouldApplyOnSuccessAndReturnSuccess()
    {
        // Arrange
        var originalResult = Result<int>.Success(5);
        Func<int, int> onSuccess = x => x * 2;
        Func<string, int> onFailure = err => -1;

        // Act
        var matchedResult = originalResult.Match(onSuccess, onFailure);

        // Assert
        Assert.True(matchedResult.IsSuccess);
        Assert.Equal(10, matchedResult.Value);
        Assert.Null(matchedResult.Error);
    }

    [Fact]
    public void Match_Failure_ShouldApplyOnFailureAndReturnSuccess()
    {
        // Arrange
        var originalResult = Result<int>.Failure("Error occurred");
        Func<int, int> onSuccess = x => x * 2;
        Func<string, int> onFailure = err => err.Length;

        // Act
        var matchedResult = originalResult.Match(onSuccess, onFailure);

        // Assert
        Assert.True(matchedResult.IsSuccess);
        Assert.Equal(14, matchedResult.Value); // Length of "Error occurred"
        Assert.Null(matchedResult.Error);
    }

    [Fact]
    public void Match_ExceptionInOnSuccess_ShouldReturnFailureWithExceptionMessage()
    {
        // Arrange
        var originalResult = Result<int>.Success(5);
        Func<int, int> onSuccess = x => throw new InvalidOperationException("OnSuccess failed");
        Func<string, int> onFailure = err => -1;

        // Act
        var matchedResult = originalResult.Match(onSuccess, onFailure);

        // Assert
        Assert.False(matchedResult.IsSuccess);
        Assert.Equal(0, matchedResult.Value); // Default for int
        Assert.Equal("OnSuccess failed", matchedResult.Error);
    }

    [Fact]
    public void Match_ExceptionInOnFailure_ShouldReturnFailureWithExceptionMessage()
    {
        // Arrange
        var originalResult = Result<int>.Failure("Error");
        Func<int, int> onSuccess = x => x * 2;
        Func<string, int> onFailure = err => throw new InvalidOperationException("OnFailure failed");

        // Act
        var matchedResult = originalResult.Match(onSuccess, onFailure);

        // Assert
        Assert.False(matchedResult.IsSuccess);
        Assert.Equal(0, matchedResult.Value);
        Assert.Equal("OnFailure failed", matchedResult.Error);
    }

    [Fact]
    public void Match_NullResult_ShouldThrowArgumentNullException()
    {
        // Arrange
        Result<int> originalResult = null;
        Func<int, int> onSuccess = x => x;
        Func<string, int> onFailure = err => -1;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => originalResult.Match(onSuccess, onFailure));
    }
}