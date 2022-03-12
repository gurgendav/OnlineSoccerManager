namespace SoccerManager.WebAPI.Models;

public class SoccerTeamResponse
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public string Country { get; set; }
    
    public int Budget { get; set; }

    public List<SoccerPlayerResponse> Players { get; set; }
    
    public int TeamValue => Players.Sum(p => p.MarketValue);
}