using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    
    private readonly SoccerTeamService _sut;
    
    public SoccerTeamServiceTests()
    {
        _applicationDbContext = Substitute.For<IApplicationDbContext>();

        _sut = new SoccerTeamService(_applicationDbContext);
    }
    
    [Fact]
    public async Task Create_Returns_TeamWith20Players()
    {
        // Arrange
        
        // Act
        var result = await _sut.Create();

        // Assert
        result.Should().NotBeNull();
        result.Players.Should().HaveCount(20);
        result.Players.Where(p => p.Position == SoccerPlayerPosition.Attackers).Should().HaveCount(5);
        result.Players.Where(p => p.Position == SoccerPlayerPosition.Midfielder).Should().HaveCount(6);
        result.Players.Where(p => p.Position == SoccerPlayerPosition.Defender).Should().HaveCount(6);
        result.Players.Where(p => p.Position == SoccerPlayerPosition.Goalkeeper).Should().HaveCount(3);
        await _applicationDbContext.Received(1).SaveChangesAsync();
    }
}