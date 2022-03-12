using SoccerManager.Application.Entities;

namespace SoccerManager.Application.Interfaces;

public interface ISoccerTeamService
{
    Task<SoccerTeam> Create();
}