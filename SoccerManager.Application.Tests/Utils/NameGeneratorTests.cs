using FluentAssertions;
using SoccerManager.Application.Utils;
using Xunit;

namespace SoccerManager.Application.Tests.Utils;

public class NameGeneratorTests
{
    private readonly NameGenerator _sut;

    public NameGeneratorTests()
    {
        _sut = new NameGenerator();
    }

    [Fact]
    public void GenerateFirstName_Returns_DifferentNonEmptyString()
    {
        // Arrange
        
        // Act
        var result1 = _sut.GenerateFirstName();
        var result2 = _sut.GenerateFirstName();
        
        // Assert
        result1.Should().NotBeNullOrEmpty().And.NotBeEquivalentTo(result2);
    }
    
    [Fact]
    public void GenerateLastName_Returns_DifferentNonEmptyString()
    {
        // Arrange
        
        // Act
        var result1 = _sut.GenerateLastName();
        var result2 = _sut.GenerateLastName();
        
        // Assert
        result1.Should().NotBeNullOrEmpty().And.NotBeEquivalentTo(result2);
    }
    
    [Fact]
    public void GenerateCountryName_Returns_DifferentNonEmptyString()
    {
        // Arrange
        
        // Act
        var result1 = _sut.GenerateCountryName();
        var result2 = _sut.GenerateCountryName();
        
        // Assert
        result1.Should().NotBeNullOrEmpty().And.NotBeEquivalentTo(result2);
    }
}