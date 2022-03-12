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

    private readonly SoccerPlayerService _sut;

    public SoccerPlayerServiceTests()
    {
        _nameGenerator = Substitute.For<INameGenerator>();
        var applicationDbContext = Substitute.For<IApplicationDbContext>();
        var userIdAccessor = Substitute.For<IUserIdAccessor>();
        _sut = new SoccerPlayerService(_nameGenerator, userIdAccessor, applicationDbContext);
    }

    [Theory]
    [AutoData]
    public void Generate_Should_CreateValidPlayer(string firstName, string lastName, string countryName, string position)
    {
        // Arrange
        const int soccerTeamId = 1;
        _nameGenerator.GenerateFirstName().Returns(firstName);
        _nameGenerator.GenerateLastName().Returns(lastName);
        _nameGenerator.GenerateCountryName().Returns(countryName);

        // Act
        var player = _sut.Generate(soccerTeamId, position);

        // Assert
        player.Should().NotBeNull();
        player.Position.Should().Be(position);
        player.SoccerTeamId.Should().Be(soccerTeamId);

        player.FirstName.Should().Be(firstName);
        player.LastName.Should().Be(lastName);
        player.Country.Should().Be(countryName);
        player.Age.Should().BeGreaterOrEqualTo(18).And.BeLessOrEqualTo(40);
        player.MarketValue.Should().Be(1_000_000);
    }
}