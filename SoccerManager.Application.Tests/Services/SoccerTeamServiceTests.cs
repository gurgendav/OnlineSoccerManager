using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using NSubstitute;
using SoccerManager.Application.Entities;
using SoccerManager.Application.Interfaces;
using SoccerManager.Application.Services;
using Xunit;

namespace SoccerManager.Application.Tests.Services;

public class SoccerTeamServiceTests
{
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly ISoccerPlayerService _soccerPlayerService;
    private readonly INameGenerator _nameGenerator;

    private readonly SoccerTeamService _sut;
    
    public SoccerTeamServiceTests()
    {
        _applicationDbContext = Substitute.For<IApplicationDbContext>();
        _nameGenerator = Substitute.For<INameGenerator>();
        _soccerPlayerService = Substitute.For<ISoccerPlayerService>();
        var userIdAccessor = Substitute.For<IUserIdAccessor>();

        _sut = new SoccerTeamService(_applicationDbContext, userIdAccessor, _soccerPlayerService, _nameGenerator);
    }
    
    [Fact]
    public async Task Create_Returns_ValidTeamWith20Players()
    {
        // Arrange
        const string userId = "aaaaa";
        
        _nameGenerator.GenerateCountryName().Returns(new Fixture().Create<string>());
        _soccerPlayerService.Generate(Arg.Any<int>(), Arg.Any<string>())
            .Returns(callInfo => new Fixture()
                .Build<SoccerPlayer>()
                .With(p => p.SoccerTeam, (SoccerTeam)null)
                .With(p => p.TransferItem, (TransferItem)null)
                .With(p => p.SoccerTeamId, callInfo.Arg<int>())
                .With(p => p.Position, callInfo.Arg<string>())
                .Create());
        
        // Act
        var result = await _sut.Create(userId);

        // Assert
        result.Should().NotBeNull();
        result.UserId.Should().Be(userId);
        result.Players.Should().HaveCount(20);
        result.Players.Where(p => p.Position == SoccerPlayerPosition.Attacker).Should().HaveCount(5);
        result.Players.Where(p => p.Position == SoccerPlayerPosition.Midfielder).Should().HaveCount(6);
        result.Players.Where(p => p.Position == SoccerPlayerPosition.Defender).Should().HaveCount(6);
        result.Players.Where(p => p.Position == SoccerPlayerPosition.Goalkeeper).Should().HaveCount(3);
        await _applicationDbContext.Received(1).SaveChangesAsync();
    }
}