using Xunit;
using MovieReservation.Domain;

namespace MovieReservation.Domain.Tests;

public class MovieShould
{
    [Theory]
    [InlineData("Inception")]
    [InlineData("9")]
    [InlineData("The Lion Kings")]
    [InlineData("  The Lion Kings          ")]
    public void ValidateAllowNames(string name)
    {
        //Act
        Movie movie = new Movie(name);

        //Assert
        Assert.Equal(name.Trim(), movie.Name);
    }

    [Theory]
    [InlineData("     ")]
    [InlineData("")]
    [InlineData(null)]
    public void ValidateUnauthorizedName(string? name)
    {
        var ex = Assert.Throws<ArgumentException>(() => new Movie(name!));
        Assert.Contains("Name cannot be empty", ex.Message);
    }


    [Theory]
    [InlineData(Movie.NameMaxLength + 1)]
    [InlineData(Movie.NameMaxLength + 50)]
    [InlineData(Movie.NameMaxLength + 100)]
    public void ThrowException_WhenNameExceedsMaxLength(int length)
    {
        var longName = new string('a', length);
        var ex = Assert.Throws<ArgumentException>(() => new Movie(longName));
        Assert.Contains($"Name cannot exceed {Movie.NameMaxLength} characters", ex.Message);
    }

   

    // [Fact]
    // public void AllowNullDescription()
    // {
    //     var movie = new Movie("Inception", null);
    //     Assert.Null(movie.Description);
    // }

    // [Fact]
    // public void ThrowException_WhenDescriptionExceedsMaxLength()
    // {
    //     var longDescription = new string('a', Movie.DescriptionMaxLength + 1);
    //     var ex = Assert.Throws<ArgumentException>(() => new Movie("Inception", longDescription));
    //     Assert.Contains($"Description cannot exceed {Movie.DescriptionMaxLength} characters", ex.Message);
    // }

}
