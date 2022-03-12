namespace SoccerManager.Application.Entities;

public static class SoccerPlayerPosition
{
    public const string Goalkeeper = "Goalkeeper";
    public const string Defender = "Defender";
    public const string Midfielder = "Midfielder";
    public const string Attackers = "Attackers";
}

public class SoccerPlayer
{
    public int Id { get; set; }
    
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Country { get; set; }
    
    public int Age { get; set; }
    
    public string Position { get; set; }
    
    public int MarketValue { get; set; }
    
    public int SoccerTeamId { get; set; }
    public SoccerTeam SoccerTeam { get; set; }
}