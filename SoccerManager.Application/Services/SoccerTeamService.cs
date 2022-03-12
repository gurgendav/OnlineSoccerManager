using Microsoft.EntityFrameworkCore;
using SoccerManager.Application.Entities;
using SoccerManager.Application.Exceptions;
using SoccerManager.Application.Interfaces;

namespace SoccerManager.Application.Services;

public class SoccerTeamService : ISoccerTeamService
{
    private const int TeamInitialBudget = 5_000_000;
    private const int InitialGoalkeepers = 3;
    private const int InitialDefenders = 6;
    private const int InitialMidfielders = 6;
    private const int InitialAttackers = 5;

    private readonly IApplicationDbContext _applicationDbContext;
    private readonly IUserIdAccessor _userIdAccessor;
    private readonly ISoccerPlayerService _soccerPlayerService;
    private readonly INameGenerator _nameGenerator;

    public SoccerTeamService(IApplicationDbContext applicationDbContext,
        IUserIdAccessor userIdAccessor,
        ISoccerPlayerService soccerPlayerService,
        INameGenerator nameGenerator)
    {
        _applicationDbContext = applicationDbContext;
        _userIdAccessor = userIdAccessor;
        _soccerPlayerService = soccerPlayerService;
        _nameGenerator = nameGenerator;
    }

    public async Task<SoccerTeam> Create(string userId)
    {
        var team = new SoccerTeam
        {
            Name = "Default",
            Country = _nameGenerator.GenerateCountryName(),
            Budget = TeamInitialBudget,
            UserId = userId,
            Players = new List<SoccerPlayer>(InitialGoalkeepers + InitialDefenders + InitialMidfielders +
                                             InitialAttackers)
        };

        for (var i = 0; i < InitialGoalkeepers; i++)
        {
            team.Players.Add(_soccerPlayerService.Create(team.Id, SoccerPlayerPosition.Goalkeeper));
        }

        for (var i = 0; i < InitialAttackers; i++)
        {
            team.Players.Add(_soccerPlayerService.Create(team.Id, SoccerPlayerPosition.Attacker));
        }

        for (var i = 0; i < InitialDefenders; i++)
        {
            team.Players.Add(_soccerPlayerService.Create(team.Id, SoccerPlayerPosition.Defender));
        }

        for (var i = 0; i < InitialMidfielders; i++)
        {
            team.Players.Add(_soccerPlayerService.Create(team.Id, SoccerPlayerPosition.Midfielder));
        }

        await _applicationDbContext.SoccerTeams.AddAsync(team);
        await _applicationDbContext.SaveChangesAsync();

        return team;
    }

    public async Task<SoccerTeam> FindByUserId(string userId)
    {
        var team = await _applicationDbContext.SoccerTeams
            .Include(t => t.Players)
            .FirstAsync(t => t.UserId == userId);

        return team;
    }

    public async Task<SoccerTeam> Update(int teamId, string newName, string newCountry)
    {
        var team = await _applicationDbContext.SoccerTeams
            .Include(t => t.Players)
            .FirstAsync(t => t.Id == teamId);

        if (team.UserId != _userIdAccessor.GetCurrentUserId())
        {
            throw new UnauthorizedException("Not your team");
        }
        
        team.Name = newName;
        team.Country = newCountry;
        
        await _applicationDbContext.SaveChangesAsync();
        
        return team;
    }
}