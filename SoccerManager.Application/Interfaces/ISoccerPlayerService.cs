using SoccerManager.Application.Entities;

namespace SoccerManager.Application.Interfaces;

public interface ISoccerPlayerService
{
    SoccerPlayer Generate(int soccerTeamId, string position);
    Task<SoccerPlayer> GetById(int id);
    Task<SoccerPlayer> Update(int id, string newFirstName, string newLastName, string newCountry);
}