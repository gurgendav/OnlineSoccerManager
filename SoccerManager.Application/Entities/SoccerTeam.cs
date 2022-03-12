namespace SoccerManager.Application.Entities;

public class SoccerTeam
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public string Country { get; set; }
    
    public int Budget { get; set; }
    
    public List<SoccerPlayer> Players { get; set; }
    
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }
}