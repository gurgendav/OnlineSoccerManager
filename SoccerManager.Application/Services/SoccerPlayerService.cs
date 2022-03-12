using SoccerManager.Application.Entities;
using SoccerManager.Application.Interfaces;

namespace SoccerManager.Application.Services;

public class SoccerPlayerService : ISoccerPlayerService
{
    private const int MinPlayerAge = 18;
    private const int MaxPlayerAge = 40;

    private const int InitialPlayerMarketValue = 1_000_000;

    private readonly INameGenerator _nameGenerator;

    public SoccerPlayerService(INameGenerator nameGenerator)
    {
        _nameGenerator = nameGenerator;
    }
    
    public SoccerPlayer Create(int soccerTeamId, string position)
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
}