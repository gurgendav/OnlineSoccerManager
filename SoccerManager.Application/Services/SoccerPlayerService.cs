using Microsoft.EntityFrameworkCore;
using SoccerManager.Application.Entities;
using SoccerManager.Application.Exceptions;
using SoccerManager.Application.Interfaces;

namespace SoccerManager.Application.Services;

public class SoccerPlayerService : ISoccerPlayerService
{
    private const int MinPlayerAge = 18;
    private const int MaxPlayerAge = 40;

    private const int InitialPlayerMarketValue = 1_000_000;

    private readonly INameGenerator _nameGenerator;
    private readonly IUserIdAccessor _userIdAccessor;
    private readonly IApplicationDbContext _applicationDbContext;

    public SoccerPlayerService(INameGenerator nameGenerator, 
        IUserIdAccessor userIdAccessor,
        IApplicationDbContext applicationDbContext)
    {
        _nameGenerator = nameGenerator;
        _userIdAccessor = userIdAccessor;
        _applicationDbContext = applicationDbContext;
    }
    
    public SoccerPlayer Generate(int soccerTeamId, string position)
    {
        return new SoccerPlayer
        {
            Position = position,
            SoccerTeamId = soccerTeamId,
            Age = Random.Shared.Next(MinPlayerAge, MaxPlayerAge + 1),
            FirstName = _nameGenerator.GenerateFirstName(),
            LastName = _nameGenerator.GenerateLastName(),
            Country = _nameGenerator.GenerateCountryName(),
            MarketValue = InitialPlayerMarketValue
        };
    }

    public async Task<SoccerPlayer> GetById(int id)
    {
        return await _applicationDbContext.SoccerPlayers.FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<SoccerPlayer> Update(int id, string newFirstName, string newLastName, string newCountry)
    {
        var player = await _applicationDbContext.SoccerPlayers
            .Include(p => p.SoccerTeam)
            .FirstAsync(p => p.Id == id);

        if (player.SoccerTeam.UserId != _userIdAccessor.GetCurrentUserId())
        {
            throw new UnauthorizedException("Player is not in your team");
        }

        player.FirstName = newFirstName;
        player.LastName = newLastName;
        player.Country = newCountry;

        await _applicationDbContext.SaveChangesAsync();

        return player;
    }
}