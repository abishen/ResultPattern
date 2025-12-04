using ResultPattern.Core.Extension;
using ResultPattern.Core.Test.MockObject;

namespace ResultPattern.Core.Test;

/// <summary>
/// Provides unit tests for extensions methods on the Result class to validate their functionality.
/// </summary>
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
        Func<int, string> mapper = _ => throw new InvalidOperationException("Mapper failed");

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
        Result<int> originalResult = null!;
        Func<int, string> mapper = x => x.ToString();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => originalResult.Map(mapper));
    }

    [Fact]
    public void Match_Success_ShouldApplyOnSuccessAndReturnSuccess()
    {
        // Arrange
        var originalResult = Result<int>.Success(5);
        int OnSuccess(int x) => x * 2;
        int OnFailure(string _) => -1;

        // Act
        var matchedResult = originalResult.Match(OnSuccess, OnFailure);

        // Assert
        Assert.True(matchedResult.IsSuccess);
        Assert.Equal(10, matchedResult.Value);
        Assert.Null(matchedResult.Error);
    }
    
    [Fact]
    public void Match_Success_ShouldApplyOnSuccessAndReturnSuccessPerson()
    {
        // Arrange
  
        var originalResult = Result<string>.Success("Test,20,test@gmail.com");
        Person Mapper(string x) => new Person()
        {
            Name = x.Split(",")[0],
            Age = int.Parse(x.Split(",")[1]),
            Email = x.Split(",")[2]
        };

        // Act
        var mappedResult = originalResult.Map(Mapper);

        // Assert
        Assert.True(mappedResult.IsSuccess);
        Assert.Equal("Test", mappedResult.Value.Name);
        Assert.Equal(20, mappedResult.Value.Age);
        Assert.Equal("test@gmail.com", mappedResult.Value.Email);
        Assert.Null(mappedResult.Error);
    }

    [Fact]
    public void Match_Failure_ShouldApplyOnFailureAndReturnSuccess()
    {
        // Arrange
        var originalResult = Result<int>.Failure("Error occurred");
        int OnSuccess(int x) => x * 2;
        int OnFailure(string err) => err.Length;

        // Act
        var matchedResult = originalResult.Match(OnSuccess, OnFailure);

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
        Func<int, int> onSuccess = _ => throw new InvalidOperationException("OnSuccess failed");
        Func<string, int> onFailure = _ => -1;

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
        Func<string, int> onFailure = _ => throw new InvalidOperationException("OnFailure failed");

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
        Result<int> originalResult = null!;
        Func<int, int> onSuccess = x => x;
        Func<string, int> onFailure = _ => -1;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => originalResult.Match(onSuccess, onFailure));
    }
}