using SoccerManager.Application.Entities;

namespace SoccerManager.Application.Interfaces;

public interface ISoccerPlayerService
{
    SoccerPlayer Create(int soccerTeamId, string position);
}