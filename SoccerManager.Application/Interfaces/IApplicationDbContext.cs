using Microsoft.EntityFrameworkCore;
using SoccerManager.Application.Entities;

namespace SoccerManager.Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<SoccerTeam> SoccerTeams { get; set; }
    DbSet<SoccerPlayer> SoccerPlayers { get; set; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}