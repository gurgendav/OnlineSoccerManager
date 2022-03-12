namespace SoccerManager.Application.Entities;

public class TransferItem
{
    public int Id { get; set; }
    
    public int SoccerPlayerId { get; set; }
    public SoccerPlayer SoccerPlayer { get; set; }
    
    public int Price { get; set; }
}