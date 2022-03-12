using SoccerManager.Application.Entities;
using SoccerManager.Application.Interfaces;

namespace SoccerManager.Application.Services;

public class SoccerTeamService : ISoccerTeamService
{
    private const int TeamInitialBudget = 1_000_000;
    
    private readonly IApplicationDbContext _applicationDbContext;

    public SoccerTeamService(IApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }
    
    public async Task<SoccerTeam> Create()
    {
        var team = new SoccerTeam
        {
            Name = "Default",
            Country = "Default",
            Budget = TeamInitialBudget
        };
        
        await _applicationDbContext.SoccerTeams.AddAsync(team);
        await _applicationDbContext.SaveChangesAsync();
        
        return team;
    }
}