using AutoFixture.Xunit2;
using FluentAssertions;
using NSubstitute;
using SoccerManager.Application.Interfaces;
using SoccerManager.Application.Services;
using Xunit;

namespace SoccerManager.Application.Tests.Services;

public class SoccerPlayerServiceTests
{
    private readonly INameGenerator _nameGenerator;
    private readonly INumberGenerator _numberGenerator;

    private readonly SoccerPlayerService _sut;

    public SoccerPlayerServiceTests()
    {
        _nameGenerator = Substitute.For<INameGenerator>();
        _numberGenerator = Substitute.For<INumberGenerator>();
        var applicationDbContext = Substitute.For<IApplicationDbContext>();
        var userIdAccessor = Substitute.For<IUserIdAccessor>();
        _sut = new SoccerPlayerService(_nameGenerator, _numberGenerator, userIdAccessor, applicationDbContext);
    }

    [Theory]
    [AutoData]
    public void Generate_Should_CreateValidPlayer(string firstName, string lastName, string countryName, string position)
    {
        // Arrange
        const int soccerTeamId = 1;
        const int playerAge = 30;
        
        _nameGenerator.GenerateFirstName().Returns(firstName);
        _nameGenerator.GenerateLastName().Returns(lastName);
        _nameGenerator.GenerateCountryName().Returns(countryName);
        _numberGenerator.GenerateInt(Arg.Any<int>(), Arg.Any<int>()).Returns(playerAge);

        // Act
        var player = _sut.Generate(soccerTeamId, position);

        // Assert
        player.Should().NotBeNull();
        player.Position.Should().Be(position);
        player.SoccerTeamId.Should().Be(soccerTeamId);

        player.FirstName.Should().Be(firstName);
        player.LastName.Should().Be(lastName);
        player.Country.Should().Be(countryName);
        player.Age.Should().Be(playerAge);
        player.MarketValue.Should().Be(1_000_000);
    }
}