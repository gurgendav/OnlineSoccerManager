using SoccerManager.Application.Entities;

namespace SoccerManager.Application.Interfaces;

public interface ISoccerTeamService
{
    Task<SoccerTeam> GetById(int id);
    Task<SoccerTeam> Create(string userId);
    Task<SoccerTeam> FindByUserId(string userId);
    Task<SoccerTeam> Update(int teamId, string newName, string newCountry);
}